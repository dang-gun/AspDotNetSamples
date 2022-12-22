# ASP.NET Core 6 Defult Setting - DB
ASP.NET Core 6 기본 세팅에 엔트리 프레임웍 6 (Entity Framework 6 )을 세팅할 프로젝트 입니다.

<br />

## API 구성
- API 파스칼 케이스 유지하도록 구성됨
- wwwroot 사용
- 기본 파일  html파일(index.html) 지정함

<br />

## 엔트리 프레임웍 6
- Sqlite 구성
- 다른 DB도 추가 가능
- 다른 DB를 추가할 때는 'ModelsDbContext'에서 해당 db옵션을 지정해야 합니다.