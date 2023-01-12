namespace JwtAuth;

/// <summary>
/// 인증 스킵 속성
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }