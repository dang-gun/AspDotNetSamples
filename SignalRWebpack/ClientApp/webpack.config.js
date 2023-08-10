const path = require("path");
const webpack = require('webpack');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = (env, argv) => 
{
    //릴리즈(프로덕션)인지 여부
    const EnvPrductionIs = argv.mode === "production";
    console.log("*** Mode  = " + argv.mode);

    return {
        /** 서비스 모드 */
        mode: EnvPrductionIs ? "production" : "development",
        devtool: "inline-source-map",

        entry: "./src/index.ts",
        output: {
            path: path.resolve(__dirname, "../wwwroot"),
            filename: "[name].[chunkhash].js",
            publicPath: "/",
        },

        resolve: {
            extensions: [".js", ".ts"],
        },
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    use: "ts-loader",
                },
                {
                    test: /\.css$/,
                    use: [MiniCssExtractPlugin.loader, "css-loader"],
                },
            ],
        },
        plugins: [
            new webpack.SourceMapDevToolPlugin({}),
            new CleanWebpackPlugin(),
            new HtmlWebpackPlugin({
                template: "./src/index.html",
            }),
            new MiniCssExtractPlugin({
                filename: "css/[name].[chunkhash].css",
            }),
        ],

        devServer: {
            /** 서비스 포트 */
            port: "9500",
            https: true,
            /** 출력파일의 위치 */
            static: [path.resolve("./", "build/development/")],
            /** 브라우저 열지 여부 */
            open: true,
            /** 핫리로드 사용여부 */
            hot: true,
            /** 라이브 리로드 사용여부 */
            liveReload: true
        },
    };
    
}