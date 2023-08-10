"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const signalR = require("@microsoft/signalr");
require("./css/main.css");
class App {
    constructor() {
        /** 서버 주소 */
        this.txtServerUrl = document.querySelector("#txtServerUrl");
        /** 연결 버튼 */
        this.btnConnect = document.querySelector("#btnConnect");
        /** 연결 끊기 버튼 */
        this.btnDisconnect = document.querySelector("#btnDisconnect");
        /** id입력창 */
        this.txtId = document.querySelector("#txtId");
        /** 로그인 버튼 */
        this.btnLogin = document.querySelector("#btnLogin");
        this.txtTo = document.querySelector("#txtTo");
        this.txtMessage = document.querySelector("#txtMessage");
        this.btnSend = document.querySelector("#btnSend");
        /** 로그 출력위치 */
        this.divLog = document.querySelector("#divLog");
        // #region UI 관련
        /** 연결이 되지 않은 상태*/
        this.UI_Disconnect = () => {
            this.txtServerUrl.disabled = false;
            this.btnConnect.disabled = false;
            this.btnDisconnect.disabled = true;
            this.txtId.disabled = true;
            this.btnLogin.disabled = true;
            this.txtTo.disabled = true;
            this.txtMessage.disabled = true;
            this.btnSend.disabled = true;
        };
        /** 연결만 된상태 */
        this.UI_Connect = () => {
            this.txtServerUrl.disabled = true;
            this.btnConnect.disabled = true;
            this.btnDisconnect.disabled = false;
            this.txtId.disabled = false;
            this.btnLogin.disabled = false;
            this.txtTo.disabled = true;
            this.txtMessage.disabled = true;
            this.btnSend.disabled = true;
        };
        /** 로그인 까지 완료 */
        this.UI_Login = () => {
            this.txtServerUrl.disabled = true;
            this.btnConnect.disabled = true;
            this.btnDisconnect.disabled = false;
            this.txtId.disabled = true;
            this.btnLogin.disabled = true;
            this.txtTo.disabled = false;
            this.txtMessage.disabled = false;
            this.btnSend.disabled = false;
        };
        // #endregion
        /**
         * 로그 출력
         * @param sMsg
         */
        this.LogAdd = (sMsg) => {
            //요청 시간
            let dtNow = new Date();
            //출력할 최종 메시지
            let sMsgLast = sMsg;
            //로그개체 생성
            let divItem = document.createElement("div");
            //출력 내용 지정
            divItem.innerHTML = `<label>[${dtNow.getHours()}:${dtNow.getMinutes()}:${dtNow.getSeconds()}]</label> <label>${sMsgLast}</label>`;
            //내용 출력
            this.divLog.appendChild(divItem);
        };
        // #region 연결 관련
        /**
         * 연결 클릭
         * @param event
         */
        this.ConnectClick = (event) => {
            let objThis = this;
            this.connection.start()
                .then(() => {
                objThis.LogAdd("연결 완료!");
                objThis.UI_Connect();
            })
                .catch((err) => {
                objThis.LogAdd("ConnectClick : " + err);
                objThis.UI_Disconnect();
            });
        };
        /**
         * 연결 클릭
         * @param event
         */
        this.DisconnectClick = (event) => {
            this.Disconnect();
        };
        /** 시그널r 끊기 시도*/
        this.Disconnect = () => {
            let objThis = this;
            this.connection.stop()
                .then(() => { objThis.LogAdd("연결 끊김"); })
                .catch((err) => { objThis.LogAdd("DisconnectClick : " + err); });
            objThis.UI_Disconnect();
        };
        // #endregion
        this.SendModel = (sendModel) => {
            let sSendModel = JSON.stringify(sendModel);
            this.connection
                .send("SendMessageAsync", sSendModel)
                .then(() => { });
        };
        // #region 로그인 관련
        /**
         * 로그인 시도
         * @param event
         */
        this.LoginClick = (event) => {
            this.SendModel({
                Sender: "",
                Command: "Login",
                Message: this.txtId.value,
                To: ""
            });
        };
        // #endregion
        this.ReceivedMessage = (sSendModel) => {
            //전달받은 모델을 파싱한다.
            let sendModel = JSON.parse(sSendModel);
            //debugger;
            switch (sendModel.Command) {
                case "LoginSuccess":
                    this.LogAdd("로그인 성공 : " + sendModel.Message);
                    this.UI_Login();
                    break;
                case "LoginError_Duplication":
                    this.LogAdd("이미 사용중인 아이디 입니다.");
                    this.UI_Disconnect();
                    break;
                case "LoginError_Reconnect":
                    this.LogAdd("다시 접속해 주세요.");
                    this.UI_Disconnect();
                    break;
                case "MsgSend": //메시지 전달받음
                    this.LogAdd(sendModel.Sender + " : " + sendModel.Message);
                    break;
            }
        };
        this.SendClick = (event) => {
            this.SendModel({
                Sender: "",
                Command: "MsgSend",
                Message: this.txtMessage.value,
                To: this.txtTo.value
            });
        };
        this.btnConnect.onclick = this.ConnectClick;
        this.btnLogin.onclick = this.LoginClick;
        this.btnSend.onclick = this.SendClick;
        //시그널r 연결
        this.connection
            = new signalR.HubConnectionBuilder()
                .withUrl(this.txtServerUrl.value)
                .build();
        //메시지 처리 연결
        this.connection.on("ReceiveMessage", this.ReceivedMessage);
        //서버 끊김 처리
        this.connection.onclose(error => {
            this.LogAdd("서버와 연결이 끊겼습니다.");
            this.UI_Disconnect();
        });
        this.LogAdd("준비 완료");
        this.UI_Disconnect();
    }
}
exports.default = App;
window.app = new App();
//# sourceMappingURL=index.js.map