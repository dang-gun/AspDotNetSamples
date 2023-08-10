import { AjaxAssist2 } from "./AjaxAssist2";


export default class App
{
    private AA2: AjaxAssist2 = new AjaxAssist2();

    /** 로그 출력위치 */
    divLog: HTMLDivElement = document.querySelector("#divLog");

    constructor()
    {
        document.getElementById("btnApiCall1")
            .onclick = (event) =>
            {
                this.AA2.Get(0, {
                    url: "/api/Test/SuccessCall"
                    , success: function (sData, status, response)
                    {
                        console.log("btnApiCall1 success : " + sData);
                    }
                    , error: function (response, statusText, objError)
                    {
                        console.log("btnApiCall1 error : " + statusText);
                        console.log(objError);
                    }
                });
            };

        document.getElementById("btnApiCall2")
            .onclick = (event) =>
            {
                this.AA2.Put(0, {
                    fetchOption:
                    {
                        headers:
                        {
                            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                        }
                    }
                    , url: "/api/Test/ErrorCall"
                    , data: { nType : 1 }
                    , success: function (sData, status, response)
                    {
                        console.log("btnApiCall2 success : " + sData);
                    }
                    , error: function (response, statusText, objError)
                    {
                        console.log("btnApiCall2 error : " + statusText);
                        console.log(objError);
                    }
                });
            };

        document.getElementById("btnFileDownload")
            .onclick = (event) =>
            {
                this.AA2.FileLoad(
                    "/index.html"
                    , function (sData, status, response)
                    {
                        console.log("btnFileDownload success : " + sData);
                    }
                    ,
                    {
                        error: function (response, statusText, objError)
                        {
                            console.log("btnFileDownload error : " + statusText);
                            console.log(objError);
                        }
                    });
            };
    }

    
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

}

(window as any).app = new App();
