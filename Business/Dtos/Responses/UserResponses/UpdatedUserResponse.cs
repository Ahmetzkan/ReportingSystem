﻿namespace Business.Dtos.Responses.UserResponses
{
    public class UpdatedUserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TcNo { get; set; }
        public DateTime BirthDate { get; set; }
    }
}