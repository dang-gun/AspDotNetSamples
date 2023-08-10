/*
 * 이 프로젝트에서 자주쓰는 아작스 호출 형식을 미리 정의 한다.
 * 
 * 응답에 대기하려면 await를 사용한다.(jsonOption.await을 true 로 줘야 한다.)
 */

export class AjaxAssist2
{
    constructor()
    {

    }

    OptionDefult: AjaxAssist2Option =
        {
            /** await 사용여부 */
            await: false,
            /** 컨탠츠 받기 타입. 
             * AjaxAssist.ContentGetType 사용.
             * 컨탠츠를 리턴받을때 어떤 타입으로 처리해서 받을지를 설정한다.*/
            contentGetType: ContentGetType.Text,

            /** fetch를 호출할때 강제로 전달하고 싶은 데이터가 있다면 여기에 입력한다.
             * 이 옵션이 가장 우선 된다.*/
            fetchOption: {
                /** no-cors, cors, *same-origin */
                mode: 'cors',
                /** // *default, no-cache, reload, force-cache, only-if-cached */
                cache: 'no-cache',
                /** include, *same-origin, omit */
                credentials: 'same-origin',
                headers: {
                    'Accept': 'application/json',
                    /** */
                    'Content-Type': 'application/json;charset=utf-8',
                    //'Content-Type': 'text/plain',
                    //'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                },
                /** manual, *follow, error */
                redirect: 'follow',
                /** no-referrer, *client */
                referrer: 'no-referrer',
            }
        };

    /**
     * get로 아작스 요청을 한다.
     * @param typeToken 헤더에 토큰을 넣을지 여부
     * @param jsonOption 아작스 요청에 사용할 옵션 데이터. 지정하지 않은 옵션은기본 옵션을 사용한다.
     * @returns
     */
    public Get = async (typeToken, jsonOption) =>
    {
        jsonOption.method = AjaxType.Get;
        if (true === jsonOption.await)
        {//응답 대기
            return await this.Call(typeToken, jsonOption);
        }
        else
        {
            return this.Call(typeToken, jsonOption);
        }
    };

    /**
     * post로 아작스 요청을 한다.
     * @param typeToken typeToken 헤더에 토큰을 넣을지 여부
     * @param jsonOption jsonOption 아작스 요청에 사용할 옵션 데이터. 지정하지 않은 옵션은기본 옵션을 사용한다.
     */
    public Post = async (typeToken, jsonOption) =>
    {
        jsonOption.method = AjaxType.Post;
        if (true === jsonOption.await)
        {//응답 대기
            return await this.Call(typeToken, jsonOption);
        }
        else
        {
            return this.Call(typeToken, jsonOption);
        }
    };

    /**
     * put로 아작스 요청을 한다.
     * @param {AjaxAssist.TokenRelayType} typeToken typeToken 헤더에 토큰을 넣을지 여부
     * @param {json} jsonOption jsonOption 아작스 요청에 사용할 옵션 데이터. 지정하지 않은 옵션은기본 옵션을 사용한다.
     */
    public Put = async (typeToken, jsonOption) =>
    {
        jsonOption.method = AjaxType.Put;
        if (true === jsonOption.await)
        {//응답 대기
            return await this.Call(typeToken, jsonOption);
        }
        else
        {
            return this.Call(typeToken, jsonOption);
        }
    };

    Call = async (typeToken, jsonOption) =>
    {
        let objThis = this;

        //옵션 저장
        let jsonOpt = Object.assign({}, this.OptionDefult, jsonOption);

        if ((undefined === jsonOpt.method)
            || (null === jsonOpt.method)
            || (null === jsonOpt.method))
        {//jsonOpt.method가 없다.

            //jquery 구버전 스타일 사용.
            jsonOpt.method = jsonOpt.type;
        }

        //url을 개체로 변경
        jsonOpt.UrlObj = new URL(jsonOpt.url, location.origin);

        if (AjaxType.Get === jsonOpt.method
            || AjaxType.Head === jsonOpt.method)
        {//메소드가 Get 이거나
            //메소드가 Head 이다.

            //Failed to execute 'fetch' on 'Window': Request with GET/HEAD method cannot have body.
            //'창'에서 '가져오기' 실행 실패: GET/HEAD 메서드를 사용한 요청은 본문을 가질 수 없습니다.

            //바디를 제거한다.
            delete jsonOpt["body"];

            //url쿼리를 만든다.
            jsonOpt.UrlObj.search
                = new URLSearchParams(jsonOpt.data);
        }
        else
        {//이외의 메소드

            let bJson = false;
            let sContentType = jsonOpt.fetchOption.headers["Content-Type"];

            debugger;
            if ("string" === (typeof sContentType))
            {
                if (-1 < sContentType.indexOf("application/x-www-form-urlencoded"))
                {
                    //"application/x-www-form-urlencoded"인경우
                    //json 형식이 아니고 url 쿼리모양으로 바꿔야 한다.
                    //예> nData=1000&sData="test111"
                    jsonOpt.body = (new URLSearchParams(jsonOpt.data));
                }
                else
                {//예외
                    bJson = true;
                }
            }
            else
            {//예외
                bJson = true;
            }

            if (true === bJson)
            {//json 처리 필요
                //예외는 모두 json으로 처리한다.
                jsonOpt.body = JSON.stringify(jsonOpt.data);
            }

        }


        //fetch에 사용될 옵션 정리 *****************
        let jsonFetch = {
            method: jsonOpt.method,
            body: jsonOpt.body,
        };

        let jsonFetchComplete
            = Object.assign({}, jsonFetch, jsonOpt.fetchOption);


        //완성된 리스폰스
        let responseAjaxResult = null;
        //리스폰스 처리
        if (true === jsonOpt.await)
        {//응답 대기

            responseAjaxResult
                = await fetch(jsonOpt.UrlObj, jsonFetchComplete);
            let responseCheckResult
                = await this.ResponseCheck(responseAjaxResult, jsonOpt);
            if (true === responseCheckResult.ok)
            {//성공
                jsonOpt.success(
                    await responseAjaxResult.text()
                    , responseAjaxResult.status
                    , responseAjaxResult);
            }
            else
            {//실패
                let errorAA2 = new ErrorAjaxAssist2(responseAjaxResult);
                jsonOpt.error(
                    errorAA2.response
                    , errorAA2.statusText
                    , errorAA2
                );
            }
        }
        else
        {
            responseAjaxResult
                = fetch(jsonOpt.UrlObj, jsonOpt)
                    .then(function (response)
                    {
                        return objThis.ResponseCheck(response, jsonOpt);
                    })
                    .then(function (sData)
                    {//정상 처리
                        jsonOpt.success(
                            sData
                            , responseAjaxResult.status
                            , responseAjaxResult);
                    })
                    .catch(function (errorAA2)
                    {
                        jsonOpt.error(
                            errorAA2.response
                            , errorAA2.statusText
                            , errorAA2
                        );
                    });
        }



        return responseAjaxResult;
    };

    ResponseCheck = async  (
        response
        , jsonOption) =>
    {
        if (true === response.ok)
        {//성공
            let objReturn = null;
            switch (jsonOption.contentType)
            {
                case ContentGetType.Response:
                    objReturn = response;
                    break;
                case ContentGetType.Json:
                    objReturn = response.json();
                    break;
                case ContentGetType.Binary:
                    objReturn = response.arrayBuffer();
                    break;

                case ContentGetType.Text:
                default:
                    objReturn = response.text();
                    break;
            }

            return objReturn;
        }
        else
        {
            throw new ErrorAjaxAssist2(response);
        }
    };


    public FileLoad = async (
        sFileUrl
        , funSuccess
        , jsonOption) =>
    {
        let typeToken = TokenRelayType.None;
        jsonOption.method = "GET";
        jsonOption.url = sFileUrl;
        jsonOption.success = funSuccess;

        if (true === jsonOption.await)
        {//대기
            await this.Call(typeToken, jsonOption);
        }
        else
        {
            this.Call(typeToken, jsonOption);
        }
    };
}

export class ErrorAjaxAssist2
{
    public response: any;
    public status: any;
    public statusText: any;
    public stack: any;

    constructor(responseAjaxResult: any)
    {
        this.response = responseAjaxResult;
        this.status = this.response.status;
        this.statusText = this.response.statusText;;
        this.stack = (new Error()).stack;
    }
}

export interface AjaxAssist2Option
{
    /** await 사용여부 */
    await: boolean,
    /** 컨탠츠 받기 타입. 
     * AjaxAssist.ContentGetType 사용.
     * 컨탠츠를 리턴받을때 어떤 타입으로 처리해서 받을지를 설정한다.*/
    contentGetType: ContentGetType,

    /** fetch를 호출할때 강제로 전달하고 싶은 데이터가 있다면 여기에 입력한다.
     * 이 옵션이 가장 우선 된다.*/
    fetchOption: any
}


/** 아작스 요청 타입 */
export const AjaxType = {
    /** 검색 */
    Get: "GET",
    /** 생성 */
    Post: "POST",
    /** 수정(전체) */
    Put: "PUT",
    /** 수정(일부) */
    Patch: "PATCH",
    /** 삭제 */
    Delete: "DELETE",
    /** 검색(바디 없음) */
    Head: "HEAD",
};

/** 
 *  컨탠츠 타입.
 *  여기에 정의되지 않은 타입은 처리가 없다.
 * */
export const enum ContentGetType
{
    /** (기본값)Text, Html 등등 텍스트 처리가 가능한 데이터 */
    Text = 0,
    /** 전달된 리스폰스를 그대로 전달한다. */
    Response = 1,
    /** Json */
    Json = 2,
    /** 바이너리 데이터 */
    Binary = 3,
};


/** 아작스 요청시 토큰을 어떻게 처리할지 여부 */
export const enum TokenRelayType
{
    /** 전달하지 않음 */
    None = 0,

    /** 
     *  무조건 전달 
     *  기존 토큰이 죽어 있으면 갱신후 전달.
     */
    HeadAdd = 1,

    /** 
     *  기존 토큰이 없으면 없이 전달.
     *  기존 토큰이 있으면 있이 전달.
     *  기존 토큰이 죽어 있으면 갱신후 전달.
     * */
    CaseByCase = 2,
};