﻿namespace Business.Dtos.Responses.UserResponses;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public string TcNo { get; set; }
    public DateTime BirthDate { get; set; }
}
