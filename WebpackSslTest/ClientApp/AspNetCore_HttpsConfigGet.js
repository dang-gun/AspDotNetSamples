const fs = require('fs');
const spawn = require('child_process').spawn;
const path = require('path');

const crypto = require('crypto');

const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

/** 
 * aspnetcore-https.js
 * This script sets up HTTPS for the application using the ASP.NET Core HTTPS certificate 
 * 
 * 로컬 인증서를 생성한다.
 * */
function aspnetcore_https()
{
    const baseFolder =
        process.env.APPDATA !== undefined && process.env.APPDATA !== ''
            ? `${process.env.APPDATA}/ASP.NET/https`
            : `${process.env.HOME}/.aspnet/https`;

    const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
    const certificateName = certificateArg ? certificateArg.groups.value : process.env.npm_package_name;

    if (!certificateName)
    {
        console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
        process.exit(-1);
    }

    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath))
    {
        spawn('dotnet', [
            'dev-certs',
            'https',
            '--export-path',
            certFilePath,
            '--format',
            'Pem',
            '--no-password',
        ], { stdio: 'inherit', })
            .on('exit', (code) => { });
    }
}

/**
 * 
 * aspnetcore-react.js
 * .env.development.local 파일을 생성하여 인증서의 위치를 지정한다.
 */
function aspnetcore_react()
{

    const baseFolder =
        process.env.APPDATA !== undefined && process.env.APPDATA !== ''
            ? `${process.env.APPDATA}/ASP.NET/https`
            : `${process.env.HOME}/.aspnet/https`;

    const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
    const certificateName = certificateArg ? certificateArg.groups.value : process.env.npm_package_name;

    if (!certificateName)
    {
        console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
        process.exit(-1);
    }

    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync('.env.development.local'))
    {
        fs.writeFileSync(
            '.env.development.local',
            `SSL_CRT_FILE=${certFilePath}
SSL_KEY_FILE=${keyFilePath}`
        );
    } else
    {
        let lines = fs.readFileSync('.env.development.local')
            .toString()
            .split('\n');

        let hasCert, hasCertKey = false;
        for (const line of lines)
        {
            if (/SSL_CRT_FILE=.*/i.test(line))
            {
                hasCert = true;
            }
            if (/SSL_KEY_FILE=.*/i.test(line))
            {
                hasCertKey = true;
            }
        }
        console.log("hasCert : " + hasCert);
        if (!hasCert)
        {
            fs.appendFileSync(
                '.env.development.local',
                `\nSSL_CRT_FILE=${certFilePath}`
            );
        }
        console.log("hasCertKey : " + hasCertKey);
        if (!hasCertKey)
        {
            fs.appendFileSync(
                '.env.development.local',
                `\nSSL_KEY_FILE=${keyFilePath}`
            );
        }
    }

}




/**
 * Ensure the certificate and key provided are valid and if not
 * throw an easy to debug error
 * @param {any} param0
 */
function validateKeyAndCerts({ cert, key, keyFile, crtFile })
{
    let encrypted;
    try
    {
        // publicEncrypt will throw an error with an invalid cert
        encrypted = crypto.publicEncrypt(cert, Buffer.from('test'));
    } catch (err)
    {
        throw new Error(
            `The certificate  is invalid.\n${err.message}`
        );
    }

    try
    {
        // privateDecrypt will throw an error with an invalid key
        crypto.privateDecrypt(key, encrypted);
    } catch (err)
    {
        throw new Error(
            `The certificate key  is invalid.\n${err.message
            }`
        );
    }
}

/**
 * Read file and throw an error if it doesn't exist
 * @param {any} file
 * @param {any} type
 * @returns
 */
function readEnvFile(file, type)
{
    if (!fs.existsSync(file))
    {
        throw new Error(
            `You specified ${chalk.cyan(
                type
            )} in your env, but the file  can't be found.`
        );
    }
    return fs.readFileSync(file);
}

/**
 * Get the https config
 * Return cert files if provided in env, otherwise just true or false
 * @param {any} bHttpsIs
 * @returns
 */
function HttpsConfigGet(bHttpsIs) 
{
    //const { SSL_CRT_FILE, SSL_KEY_FILE, HTTPS } = process.env;
    //const isHttps = HTTPS === 'true';

    let SSL_CRT_FILE = "";
    let SSL_KEY_FILE = "";

    if (true === bHttpsIs)
    {
        let dirEnv_D_L = path.resolve(__dirname, ".env.development.local");
        //let dirEnv_D_L = path.resolve(__dirname, ".env.development.txt");
        console.log("dirEnv_D_L : " + dirEnv_D_L);

        if (!fs.existsSync(dirEnv_D_L))
        {//지정된 파일이 없다.

            //로컬 인증서 생성
            aspnetcore_https();

            //인증서 경로 파일 생성
            aspnetcore_react();
        }

        
        {
            //인증서 읽기
            console.log("SSL 로컬 인증서 : ");
            var array = fs.readFileSync(dirEnv_D_L).toString().split("\n");

            for (let i in array)
            {

                let arrCut = array[i].split("=");
                console.log(arrCut);

                if (0 < arrCut.length)
                {
                    switch (arrCut[0])
                    {
                        case "SSL_CRT_FILE":
                            SSL_CRT_FILE = arrCut[1];
                            break;
                        case "SSL_KEY_FILE":
                            SSL_KEY_FILE = arrCut[1];
                            break;

                    }
                }
            }
        }//end if (!fs.existsSync(dirEnv_D_L))

        //SSL_CRT_FILE = "C:\\Users\\Kim\\AppData\\Roaming\\ASP.NET\\https\\aspnetreactfull.pem";
        //SSL_KEY_FILE = "C:\\Users\\Kim\\AppData\\Roaming\\ASP.NET\\https\\aspnetreactfull.key";
    }


    //console.log("getHttpsConfig0 : ");
    //console.log("SSL_CRT_FILE : " + SSL_CRT_FILE);
    //console.log("SSL_KEY_FILE : " + SSL_KEY_FILE);
    //console.log(bHttpsIs);

    if (bHttpsIs && SSL_CRT_FILE && SSL_KEY_FILE)
    {
        const crtFile = path.resolve(resolveApp('.'), SSL_CRT_FILE);
        const keyFile = path.resolve(resolveApp('.'), SSL_KEY_FILE);
        const config = {
            cert: readEnvFile(crtFile, 'SSL_CRT_FILE'),
            key: readEnvFile(keyFile, 'SSL_KEY_FILE'),
        };

        validateKeyAndCerts({ ...config, keyFile, crtFile });
        return config;
    }

    return bHttpsIs;
}

module.exports = HttpsConfigGet;