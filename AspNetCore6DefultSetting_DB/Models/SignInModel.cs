namespace AspNetCore6DefultSetting_DB.Models
{
    /// <summary>
    /// 사인인이 성공하였을때 전달되는 정보(자바스크립트 전달용)
    /// </summary>
	public class SignInModel
	{
        /// <summary>
        /// 성공여부
        /// </summary>
        public bool Complete { get; set; } = false;
        /// <summary>
        /// 토큰
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
