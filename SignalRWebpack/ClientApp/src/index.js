"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const signalR = require("@microsoft/signalr");
require("./css/main.css");
//connection.on("messageReceived", (username: string, message: string) =>
//{
//    const m = document.createElement("div");
//    m.innerHTML = `<div class="message-author">${username}</div><div>${message}</div>`;
//    divMessages.appendChild(m);
//    divMessages.scrollTop = divMessages.scrollHeight;
//});
//tbMessage.addEventListener("keyup", (e: KeyboardEvent) =>
//{
//    if (e.key === "Enter")
//    {
//        send();
//    }
//});
//btnSend.addEventListener("click", send);
//function send()
//{
//    connection.send("newMessage", username, tbMessage.value)
//        .then(() => (tbMessage.value = ""));
//}
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
        this.txtMessage = document.querySelector("#txtMessage");
        this.btnSend = document.querySelector("#btnSend");
        /** 로그 출력위치 */
        this.divLog = document.querySelector("#divLog");
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
                .then(() => { objThis.LogAdd("연결 완료!"); })
                .catch((err) => { objThis.LogAdd("ConnectClick : " + err); });
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
                To: ""
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
        this.LogAdd("준비 완료");
    }
}
exports.default = App;
window.app = new App();
//# sourceMappingURL=index.js.map