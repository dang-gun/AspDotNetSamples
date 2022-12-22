using ModelsDB;

namespace AspNetCoreNpmApi.Global
{
	/// <summary>
	/// 이 프로젝트에서 사용할 전역 변수
	/// </summary>
	public static class GlobalStatic
	{
		/// <summary>
		/// 사인인 가능한 유저 정보
		/// </summary>
		public static List<User> Users;

		static GlobalStatic()
		{
			Users = new List<User>();
			Users.Add(new User() { idUser = 1, SignName = "test01", PasswordHash = "1111" });
			Users.Add(new User() { idUser = 2, SignName = "test02", PasswordHash = "1111" });
			Users.Add(new User() { idUser = 3, SignName = "test03", PasswordHash = "1111" });
		}
	}
}
