var path = require('path');
var HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');

module.exports = {
    mode: 'none',
    entry: './index.js',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist'),
    },
    devtool: "inline-source-map",
    devServer: {
        port: 9000,
    },
    plugins: [
        new webpack.SourceMapDevToolPlugin({}),
        new HtmlWebpackPlugin({
            // index.html 템플릿을 기반으로 빌드 결과물을 추가해줌
            template: 'index.html',
        }),
    ],
};