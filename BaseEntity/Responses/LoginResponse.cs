namespace BaseEntity.Responses
{
    public record struct LoginResponse(bool Flag, string Message = null!, string Token = null!);
}
