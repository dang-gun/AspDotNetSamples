using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ModelsDB
{
	public class User
	{
		/// <summary>
		/// 유저 고유키
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int idUser { get; set; }

		/// <summary>
		/// 사인인에 사용되는 이름
		/// </summary>
		/// <remarks>프로젝트에따라 이것이 이름, 이메일 등의 다양한 값이 될 수 있으므로
		/// 네이밍을 이렇게 한다.</remarks>
		public string SignName { get; set; } = string.Empty;
		/// <summary>
		/// 단방향 암호화가된 비밀번호
		/// </summary>
		/// <remarks>이 프로젝트는 최소한으로 구현되기 때문에 암호화를 하지 않는다.</remarks>
		public string PasswordHash { get; set; } = string.Empty;
	}
}
