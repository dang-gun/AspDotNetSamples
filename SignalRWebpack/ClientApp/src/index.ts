import * as signalR from "@microsoft/signalr";
import "./css/main.css";

import { SignalRSendModel } from "./SignalRSendModel";



export default class App
{
    /** 서버 주소 */
    txtServerUrl: HTMLInputElement = document.querySelector("#txtServerUrl");
    /** 연결 버튼 */
    btnConnect: HTMLButtonElement = document.querySelector("#btnConnect");
    /** 연결 끊기 버튼 */
    btnDisconnect: HTMLButtonElement = document.querySelector("#btnDisconnect");

    /** id입력창 */
    txtId: HTMLInputElement = document.querySelector("#txtId");
    /** 로그인 버튼 */
    btnLogin: HTMLButtonElement = document.querySelector("#btnLogin");
    

    txtTo: HTMLInputElement = document.querySelector("#txtTo");
    txtMessage: HTMLInputElement = document.querySelector("#txtMessage");
    btnSend: HTMLButtonElement = document.querySelector("#btnSend");

    /** 로그 출력위치 */
    divLog: HTMLDivElement = document.querySelector("#divLog");

    /** 시그널r 연결 개체 */
    connection: signalR.HubConnection;

    constructor()
    {
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
        this.connection.onclose(error =>
        {
            this.LogAdd("서버와 연결이 끊겼습니다.");
            this.UI_Disconnect();
        });



        this.LogAdd("준비 완료");
        this.UI_Disconnect();
    }

    // #region UI 관련

    /** 연결이 되지 않은 상태*/
    UI_Disconnect = () =>
    {
        this.txtServerUrl.disabled = false;
        this.btnConnect.disabled = false;

        this.btnDisconnect.disabled = true;

        this.txtId.disabled = true;
        this.btnLogin.disabled = true;

        this.txtTo.disabled = true;
        this.txtMessage.disabled = true;
        this.btnSend.disabled = true;
    }

    /** 연결만 된상태 */
    UI_Connect = () =>
    {
        this.txtServerUrl.disabled = true;
        this.btnConnect.disabled = true;

        this.btnDisconnect.disabled = false;

        this.txtId.disabled = false;
        this.btnLogin.disabled = false;

        this.txtTo.disabled = true;
        this.txtMessage.disabled = true;
        this.btnSend.disabled = true;
    }

    /** 로그인 까지 완료 */
    UI_Login = () =>
    {
        this.txtServerUrl.disabled = true;
        this.btnConnect.disabled = true;

        this.btnDisconnect.disabled = false;

        this.txtId.disabled = true;
        this.btnLogin.disabled = true;

        this.txtTo.disabled = false;
        this.txtMessage.disabled = false;
        this.btnSend.disabled = false;
    }

    // #endregion

    /**
     * 로그 출력
     * @param sMsg
     */
    LogAdd = (sMsg: string) =>
    {
        //요청 시간
        let dtNow = new Date();

        //출력할 최종 메시지
        let sMsgLast = sMsg;


        //로그개체 생성
        let divItem: HTMLElement = document.createElement("div");
        //출력 내용 지정
        divItem.innerHTML = `<label>[${dtNow.getHours()}:${dtNow.getMinutes()}:${dtNow.getSeconds()}]</label> <label>${sMsgLast}</label>`;
        
        //내용 출력
        this.divLog.appendChild(divItem);
    }

    // #region 연결 관련

    /**
     * 연결 클릭
     * @param event
     */
    ConnectClick = (event) =>
    {
        let objThis = this;

        this.connection.start()
            .then(() =>
            {
                objThis.LogAdd("연결 완료!");
                objThis.UI_Connect();
            })
            .catch((err) =>
            {
                objThis.LogAdd("ConnectClick : " + err);
                objThis.UI_Disconnect();
            });
    };

    /**
     * 연결 클릭
     * @param event
     */
    DisconnectClick = (event) =>
    {
        this.Disconnect();
    };

    /** 시그널r 끊기 시도*/
    Disconnect = () =>
    {
        let objThis = this;

        this.connection.stop()
            .then(() => { objThis.LogAdd("연결 끊김"); })
            .catch((err) => { objThis.LogAdd("DisconnectClick : " + err); });

        objThis.UI_Disconnect();
    }

    // #endregion

    SendModel = (sendModel: SignalRSendModel) =>
    {
        let sSendModel: string = JSON.stringify(sendModel);

        this.connection
            .send("SendMessageAsync", sSendModel)
            .then(() => { });
    }

    // #region 로그인 관련

    /**
     * 로그인 시도
     * @param event
     */
    LoginClick = (event) =>
    {
        this.SendModel({
            Sender: ""
            , Command: "Login"
            , Message: this.txtId.value
            , To: ""
        });
    }

    // #endregion

    ReceivedMessage = (sSendModel: string) =>
    {
        //전달받은 모델을 파싱한다.
        let sendModel: SignalRSendModel = JSON.parse(sSendModel);

        //debugger;
        switch (sendModel.Command)
        {
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

            case "MsgSend"://메시지 전달받음
                this.LogAdd(sendModel.Sender + " : " + sendModel.Message);
                break;

        }
    }

    SendClick = (event) =>
    {
        this.SendModel({
            Sender: ""
            , Command: "MsgSend"
            , Message: this.txtMessage.value
            , To: this.txtTo.value
        });
    }
}

(window as any).app = new App();
