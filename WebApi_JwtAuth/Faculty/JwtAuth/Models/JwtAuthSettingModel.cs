namespace JwtAuth.Models;

public class JwtAuthSettingModel
{
	/// <summary>
	/// 인증 토큰의 시작 이름 - 실제로 사용되는 값
	/// </summary>
	public string AuthTokenStartName_Complete { get; set; } = "bearer ";

	/// <summary>
	/// 엑세스 토큰 생성에 사용될 시크릿 키
	/// </summary>
	/// <remarks>
	/// 이값이 null이거나 비어있으면 자동으로 생성된다.<br />
	/// 자동으로 생성된 값은 프로그램이 실행되는 동안만 유지되므로
	/// 웹사이트를 껏다키면 그전에 생성된 엑세스 토큰은 사용할 수 없게 된다.<br />
	/// <br />
	/// 이 값을 고정해야 웹사이트를 껏다켜는것과 관계없이 엑세스토큰이 유지된다.
	/// </remarks>
	public string? Secret { get; set; }
}

