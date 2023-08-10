const path = require("path");
const webpack = require('webpack');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");


const HttpsConfigGet = require('./AspNetCore_HttpsConfigGet');

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

            //그대로 출력폴더에 복사할 파일 지정
            new CopyPlugin({
                patterns: [
                    {
                        //모든 html파일 복사
                        from: "./src/**/*.html",
                        to({ context, absoluteFilename })
                        {
                            //'src/'를 제거
                            let sOutDir = path.relative(context, absoluteFilename).substring(4);
                            //index.html은 리액트가 생성해주므로 여기선 스킵한다.
                            if ("index.html" === sOutDir)
                            {
                                //sOutDir = "index_Temp.html";
                                sOutDir = "";
                            }
                            //console.log("sOutDir : " + sOutDir);
                            return `${sOutDir}`;
                        },
                    },
                ],
                options: {
                    concurrency: 100,
                },
            }),

            new MiniCssExtractPlugin({
                filename: "css/[name].[chunkhash].css",
            }),
        ],

        devServer: {
            /** 서비스 포트 */
            port: "9002",
            https: HttpsConfigGet(true),
            proxy: {
                "/api/":
                {
                    target: "https://localhost:7061",
                    logLevel: "debug",
                    //호스트 헤더 변경 허용
                    changeOrigin: true,
                    secure: false,
                    onProxyReq: function (proxyReq, req, res)
                    {
                        console.log(`[HPM] [${req.method}] ${req.url}`);
                        //console.log(" ~~~~ proxyReq ~~~~");
                        //console.log(proxyReq);
                        //console.log(" ~~~~ res ~~~~");
                        //console.log(res);
                    },
                },
            },
            /** 출력파일의 위치 */
            static: [path.resolve("./", "build/development/")],
            /** 브라우저 열지 여부 */
            open: false,
            /** 핫리로드 사용여부 */
            hot: true,
            /** 라이브 리로드 사용여부 */
            liveReload: true,
            historyApiFallback: true,
        },
    };
    
}