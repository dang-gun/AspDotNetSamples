namespace SignalRWebpack.Models;

/// <summary>
/// 시그널r에서 데이터 주고 받기용 모델
/// </summary>
/// <remarks>
/// 타입스크립트와 동일해야 한다.
/// </remarks>
public class SignalRSendModel
{
    /// <summary>
    /// 보내는 사람
    /// </summary>
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// 특정 유저한테 메시지를 보낼때 대상 아이디(없으면 전체)
    /// </summary>
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// 전달할 명령어
    /// </summary>
    public string Command { get; set; } = string.Empty;

    /// <summary>
    /// 보내는 메시지
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
