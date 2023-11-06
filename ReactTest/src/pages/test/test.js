const React = require("react");

import parse from 'html-react-parser'
import { replace } from 'lodash';

import "./test.scss";
import TestHtml1 from './test1.html';
import TestHtml2 from './test2.html';

function Test()
{
    function TestCall(e)
    {
        alert("'TestCall'에서 호출됨");
    }

    let jsonData = {
        TestText: '테스트 텍스트입니다요~',
        TestInt: 124,
        TestFunc: TestCall
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
                    if ( domNode.name === 'button')
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
    

    return (
        <div className="Test">
            테스트 입니다.
            <br />
            <br />
            <button onClick={TestCall}>백엔드 호출 1</button>
            <br />
            <br />
            <div dangerouslySetInnerHTML={{ __html: sTestHtml1 }}></div>
            <br />
            <br />
            {sTestHtml2}
            <br />
            <br />
            <div dangerouslySetInnerHTML={{ __html: sTestHtml2}}></div>
            
        </div>
    );
}

export default Test;
