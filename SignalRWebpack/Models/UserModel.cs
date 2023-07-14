namespace SignalRWebpack.Models;

/// <summary>
/// 유저
/// </summary>
public class UserModel
{
    /// <summary>
    /// 시그널R에서 생성한 고유아이디
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 채팅으로 사용중인 이름
    /// </summary>
    public string Name { get; set; } = string.Empty;

}

public class UserList
{
    public List<UserModel> Users { get; set; } = new List<UserModel>();

    public void Add(string sConnectionId)
    {
        Users.Add(new UserModel {  ConnectionId = sConnectionId });
    }

    public UserModel? Find(string sName)
    {
        return this.Users.FirstOrDefault(x => x.Name == sName);
    }

    public UserModel? FindConnectionId(string ConnectionId)
    {
        return this.Users.FirstOrDefault(x => x.ConnectionId == ConnectionId);
    }

    public void Remove(string sConnectionId) 
    {

        UserModel? userModel 
            =this.FindConnectionId(sConnectionId);

        if (userModel != null)
        {
            Users.Remove(userModel);
        }

    }
}
