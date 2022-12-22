const React = require("react");

import parse from 'html-react-parser'

/** 전역 변수 */
export default class GlobalStatic
{
    /**
     * Html 문자열을 리액트 오브젝트로 변환한다.
     * @param {class} objThis 호출한 개체
     * @param {string} sHtml 변환할 html 문자열
     * @returns {ReactDom} 완성된 리액트 개체
     */
    static HtmlStringToReactObject(
        objThis
        , sHtml)
    {
        let sReturn
            = parse(sHtml
                , {
                    replace: domNode =>
                    {
                        if (domNode.name === 'button')
                        {
                            console.log(domNode);
                            let temp = domNode.attribs.onclick;
                            //기본 빈 함수
                            let funcCall = function (event, param) { };

                            //기존 로드의 클릭이벤트 제거
                            delete domNode.attribs.onclick;

                            if ("{" === temp.substring(0, 1)
                                && "}" === temp.substring(temp.length - 1))
                            {//앞뒤로 있는게 중괄호다 = 리액트 함수

                                //리액트 함수로 취급한다.
                                temp = temp.split(/{|}/g)[1];
                                //클래스일때
                                funcCall = objThis[temp];
                            }
                            else
                            {//자바스크립트
                                funcCall = function (event, param)
                                {
                                    Function('"use strict";return (' + temp + ')')(event, param);
                                };
                            }

                            return (
                                <button
                                    {...domNode.attribs}
                                    onClick=
                                    {(event, param) =>
                                    {
                                        if (undefined != funcCall
                                            && null != funcCall)
                                        {
                                            funcCall(event, param);
                                        }
                                    }}
                                >{domNode.children[0].data}</button>
                            );
                        }
                    }
                });

        return sReturn;
    }
}

/**
 * 문자열을 리러털로 변환
 * https://stackoverflow.com/a/41015840/6725889
 * @param {json} params 데이터로 사용할 json
 */
String.prototype.interpolate = function (params)
{
    const names = Object.keys(params);
    const vals = Object.values(params);
    return new Function(...names, `return \`${this}\`;`)(...vals);
}

/**
 * 문자열에서 리액트 문법의 변수를 찾아 변환하여 리턴한다.
 * @param {any} jsonParams 찾을 변수명: 데이터
 */
String.prototype.replaceReact = function (jsonParams)
{
    let sReturn = this;

    //처리할 대상
    let arrTarget = sReturn.match(/\{[\w]+\}/g);
    arrTarget && arrTarget.forEach((jsonItem) =>
    {
        let regex = new RegExp(jsonItem, 'g');
        let stateItem = jsonItem.split(/{|}/g)[1];
        let objTarget = jsonParams[stateItem];
        if (objTarget)
        {//대상이 있다.
            sReturn = sReturn.replace(regex, objTarget);
        }
        //대상이 아니면 그냥 둔다.
    });

    return sReturn;
}