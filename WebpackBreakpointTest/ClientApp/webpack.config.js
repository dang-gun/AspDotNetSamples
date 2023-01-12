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
            // index.html ���ø��� ������� ���� ������� �߰�����
            template: 'index.html',
        }),
    ],
};