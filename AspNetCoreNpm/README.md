# AspNetCoreNpm<br />
ASP.NET Core .NET6 + NPM 프로젝트<br />
<br />
<br />
#### 프로젝트를 실행하기 전에 필요한 것
- Node.js 설치 (이 프로젝트는 v16.14.2에서 테스트 됨)
- 확장 > 확장관리 > NPM Task Runner 설치
<br />
<br />
<br />

### 프로젝트내용<br />

ASP.NET Core 프로젝트에 NPM을 설정하고 'gulp'모듈을 설치하여 npm이 잘 동작하는지 확인하는 프로젝트입니다.
<br />
<br />

#### package.json 파일 생성

1) 'package.json'파일을 생성하고 'devDependencies'에 원하는 라이브러리를 넣는다.
<br />
<br />
2) 솔루션 탐색기에서 'package.json'파일을 오른쪽 클릭하고 '패키지 복원'을 한다.(자동으로 되있을 수 있음)
- 여기서는 'gulp'으로 테스트하므로 다음과 같이 구성한다.

```
{
  "version": "1.0.0",
  "name": "asp.net",
  "private": true,
	"devDependencies": {
		"gulp": "4.0.2"
	}
}
```

<br />
<br />
3) 'gulpfile.js'파일을 생성하고 내용을 아래와 같이 넣는다.

```
var gulp = require('gulp');

gulp.task('testTask', function (done)
{
    console.log('Hello World! We finished a task!');
    done();
});
```

<br />
<br />
4) 생성한 파일에서 오른쪽 클릭하고 '작업러너 탐색기'실행
<br />
<br />
5) 작업 에서 오른쪽 클릭하고 실행
<br />
<br />
<br />
참고 : https://www.danylkoweb.com/Blog/advanced-basics-using-task-runner-in-visual-studio-2019-RJ
