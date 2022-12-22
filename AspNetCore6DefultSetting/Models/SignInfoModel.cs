using ModelsDB;

namespace AspNetCore6DefultSetting.Models
{
    /// <summary>
    /// 사인인이 성공하였을때 전달되는 정보(자바스크립트 전달용)
    /// </summary>
	public class SignInfoModel
    {
        /// <summary>
        /// 성공여부
        /// </summary>
        public bool Complete { get; set; } = false;
        
        /// <summary>
        /// 검색된 유저 정보
        /// </summary>
        public User? UserInfo { get; set; }

    }
}
