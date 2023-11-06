const React = require("react");
const ReactDOM = require("react-dom/client");

import App from './App';
import Test from './pages/test/test';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    //��Ʈ����忡���� �ܼ��� 2��������.
    //https://ko.reactjs.org/docs/strict-mode.html
    <React.StrictMode>
        <div>
            <App />

            <Test />
        </div>
    </React.StrictMode>
);