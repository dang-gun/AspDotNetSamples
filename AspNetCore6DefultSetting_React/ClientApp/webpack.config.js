const path = require("path");
const fs = require("fs");
const webpack = require('webpack');
const dotenv = require('dotenv');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");

//�ҽ� ��ġ
const RootPath = path.resolve(__dirname);
const SrcPath = path.resolve(RootPath, "src");

//�������� ����ϴ� ���� �̸�
const WwwRoot = "wwwroot";
//�������� ����ϴ� ���� ��ġ
const WwwRootPath = path.resolve(__dirname, ".." , WwwRoot);
//����Ʈ ���ø� ��ġ
const React_IndexHtmlPath = path.resolve(SrcPath, "index.html");
//����� ��� ���� �̸�
let OutputFolder = "development";
//����� ��� ���� �̸� - �̹��� ����
const OutputFolder_Images = "images";
//����� ��� ��ġ
let OutputPath = path.resolve(WwwRootPath, OutputFolder);
//����� ��� ��ġ - ��� �ּ�
let OutputPath_relative = path.resolve("/", OutputFolder);


module.exports = (env, argv) =>
{
    //������(���δ���)���� ����
    const EnvPrductionIs = argv.mode === "production";
    if (true === EnvPrductionIs)
    {
        //������ ��� ���� ����
        OutputFolder = "production";
        OutputPath = path.resolve(WwwRootPath, OutputFolder);
        OutputPath_relative = path.resolve("/", OutputFolder);
    }

    //ȯ�溯�� �ҷ�����
    dotenv.config();

    return {
        /** ���� ��� */
        mode: EnvPrductionIs ? "production" : "development",
        //devtool: "eval",
        devtool: "inline-source-map",
        resolve: {
            extensions: [".js", ".jsx"],
            alias: { "@": SrcPath }
        },
        entry: { // ������ ���� ��ҵ� �Է�
            app: [path.resolve(SrcPath, "index.js")],
        },
        output: {// ���������� ������� js
            /** ���� ��ġ */
            path: OutputPath,
            /** ���� ���� �� ���������� ������� ���� */
            filename: "app.js"
        },
        module: {
            rules: [
                {//�ҽ� ����
                    test: /\.(js|jsx)$/,
                    exclude: /node_module/,
                    use:
                        [
                            { loader: "babel-loader" },
                        ]
                },
                {//��Ÿ�� ����
                    test: /\.(sa|sc|c)ss$/i,
                    exclude: /node_module/,
                    use:
                        [
                            {
                                //���� ���������� style-loader ���
                                loader: MiniCssExtractPlugin.loader,
                                options: { esModule: false }
                            },
                            { loader: 'css-loader' },
                            { loader: 'sass-loader' },
                            { loader: 'postcss-loader' },
                        ]
                },
                {//html ����
                    test: /\.html$/i,
                    loader: "html-loader",
                },
                {//�̹��� ����
                    rules: [
                        {
                            test: /\.(png|jpg|gif|svg|webp)$/,
                            loader: "file-loader",
                            options: {
                                outputPath: OutputFolder_Images,
                                name: "[name].[ext]?[hash]",
                            }
                        },
                    ],
                }
            ]
        },
        optimization: {
            minimizer: [
                new CssMinimizerPlugin(),
            ],
        },
        plugins: [
            new webpack.ProvidePlugin({ $: "jquery", jQuery: "jquery" }),
            new webpack.SourceMapDevToolPlugin({}),
            // ������ �����(��>��������)�� HTML�� �������ִ� �÷�����
            new HtmlWebpackPlugin({ template: React_IndexHtmlPath }),
            // ��������� ����ִ� �÷�����
            new CleanWebpackPlugin({
                cleanOnceBeforeBuildPatterns: [
                    '**/*',
                    "!robots.txt",
                    "!Upload"
                ]
            }),
            // ������ css ������ ���� �����ϴ� �÷�����
            new MiniCssExtractPlugin({
                filename: "app.css"
            })
        ],
        devServer: {
            https: {
                type: "https",
                options: {
                    //ca: fs.readFileSync("C:\Users\Kim\AppData\Roaming\ASP.NET\https\reacttest2.pem"),
                    //ca: fs.readFileSync(path.resolve(RootPath,"reacttest2.pem")),
                    //key: fs.readFileSync("C:\Users\Kim\AppData\Roaming\ASP.NET\https\reacttest2.key"),
                    //key: fs.readFileSync(path.resolve(RootPath, "reacttest2.key")),
                    passphrase: 'webpack-dev-server',
                    requestCert: true,
                }
            },
            /** ���� ��Ʈ */
            port: "44414",
            /** ��������� ��ġ */
            static: [path.resolve("./", WwwRoot)],
            /** ������ ���� ���� */
            open: false,
            /** �ָ��ε� ��뿩�� */
            hot: true,
            /** ���̺� ���ε� ��뿩�� */
            liveReload: true,

            proxy: {
                "/api/":
                {
                    //target: "https://localhost:7214/api/",
                    target: "https://localhost:7273",
                    logLevel: "debug",
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
            }
        },
    };
}