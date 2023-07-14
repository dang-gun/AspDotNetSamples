export interface SignalRSendModel
{
    /** 보내는 사람 */
    Sender: string;
    /** 특정 유저한테 메시지를 보낼때 대상 아이디 */
    To: string;

    /** 전달할 명령어 */
    Command: string;
    /** 보내는 메시지 */
    Message: string;
}