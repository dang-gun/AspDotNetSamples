const React = require("react");
const ReactDOM = require("react-dom/client");

import App from './App';
import Test from './pages/test/test';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    //스트릭모드에서는 콘솔이 2번찍힌다.
    //https://ko.reactjs.org/docs/strict-mode.html
    <React.StrictMode>
        <div>
            <App />

            <Test />
        </div>
    </React.StrictMode>
);