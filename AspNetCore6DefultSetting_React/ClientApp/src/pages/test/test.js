import React, { Component } from 'react';
import $ from 'jquery';

import parse from 'html-react-parser';
import { replace } from 'lodash';


import "./test.scss";
import TestHtml1 from './test1.html';
import TestHtml2 from './test2.html';

import GlobalStatic from '@/Global/GlobalStatic.js';

export default class Test extends Component
{
    constructor()
    {
        super();
    }

    render()
    {
        let jsonData = {
            TestText: '테스트 텍스트입니다요~',
            TestInt: 124,
            TestFunc: this.TestCall
        }
        let sTestHtml1 = TestHtml1;
        console.log(sTestHtml1);
        let sTestHtml2 = TestHtml2;
        console.log(sTestHtml2);

        let reactElement
            = parse(sTestHtml1
                , {
                    replace: domNode =>
                    {
                        if (domNode.name === 'button')
                        {
                            let temp = domNode.attribs.onclick;

                            delete domNode.attribs.onclick;

                            //domNode.attribs.onClick
                            //    = domNode.attribs.onclick;
                            return (
                                <button
                                    {...domNode.attribs}
                                    onClick={() => { Function('"use strict";return (' + temp + ')')(); }}
                                >{domNode.children[0].data}</button>
                            );
                        }
                    }
                });

        let reactElement2
            = GlobalStatic.HtmlStringToReactObject(this, sTestHtml2);

        return (
            <div className="Test">
                테스트 입니다.
                <br />
                <br />
                <button onClick={this.TestCall}>백엔드 호출 1</button>
                <br />
                <br />
                <div dangerouslySetInnerHTML={{ __html: sTestHtml1 }}></div>
                <br />
                <br />
                {sTestHtml2}
                <br />
                <br />
                <div dangerouslySetInnerHTML={{ __html: sTestHtml2 }}></div>
                <br />
                <br />
                {reactElement}
                <br />
                <br />
                {reactElement2}
                <br />
                <br />

            </div>
        );
    }

    TestCall(e)
    {
        alert("'TestCall'에서 호출됨");
    }

    async TestCall_Api02(e)
    {
        const response = await fetch("https://localhost:7214/api/Test/SuccessCall");
        const data = await response.json();
        alert("'SuccessCall' : " + data);
    }

    async TestCall_Api03(e)
    {
    }

    /** API 호출 - 무조건 성공, fetch 호출*/
    async SuccessCall()
    {
        await fetch("/api/Test/SuccessCall")
            .then((data) =>
            {
                if (true === data.ok)
                {
                    console.log(data);
                    alert("'SuccessCall' : " + data.statusText);
                }
                else
                {
                    alert("알수 없는 오류가 발생했습니다. " + data.statusText);
                }
                
            })
            .catch((error) =>
            {
                console.error('Error:', error);

                alert("알수 없는 오류가 발생했습니다.");
            });
    }

    async SignInCall()
    {
        let formData = new FormData();
        formData.append("sSignName", $("#txtSignName").val());
        formData.append("sPassword", $("#txtPassword").val());

        let jsonResult
            = await fetch("/api/Sign/SignIn",
                {
                    method: 'PUT',
                    dataType: 'json',
                    body: formData
                })
                .then(response => response.json())
                .catch((error) =>
                {
                    console.error('Error:', error);
                    alert("알수 없는 오류가 발생했습니다.");
                });


        if (undefined !== jsonResult)
        {//재대로 데이터를 전달 받았다.
            alert("성공(SignInCall) : " + jsonResult.Token);
        }
        else
        {
            alert("알수 없는 오류가 발생했습니다. " + jsonResult);
        }


    }

    async SignInfoCall()
    {
        $.ajax({
            url: "/api/Sign/SignInfo",
            type: "Get",
            data: { idUser: 1 },
            success: function (data)
            {
                console.log(data);
                alert("성공 : " + data.UserInfo.SignName);
                //alert(data);
            },
            error: function (error)
            {
                console.log(error);

                alert("알수 없는 오류가 발생했습니다.");

            }
        });
    }
}

