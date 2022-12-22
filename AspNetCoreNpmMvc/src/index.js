import _ from 'lodash';
import Test01 from "./Test01.js";

function createChild() 
{
    var element = document.createElement('div');
    let aa = _.join(['Hello', 'Webpack'], ' ');

    let tempTest01 = new Test01();
    aa = aa + " " + tempTest01.Msg();
    element.innerHTML = aa;
    return element;
}

document.body.appendChild(createChild());