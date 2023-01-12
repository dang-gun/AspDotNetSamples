namespace WebApi_JwtAuthTest.Models
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
        /// 성공시 검색된 유저의 고유번호
        /// </summary>
        public long idUser { get; set; } = 0;

        /// <summary>
        /// 엑세스 토큰
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
        /// <summary>
        /// 라플레시 토큰
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

    }
}
