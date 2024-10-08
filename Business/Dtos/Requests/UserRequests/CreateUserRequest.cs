﻿namespace Business.Dtos.Requests.UserRequests
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string TcNo { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

