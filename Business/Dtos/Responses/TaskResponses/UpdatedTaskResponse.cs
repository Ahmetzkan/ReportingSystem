﻿namespace Business.Dtos.Responses.TaskResponses
{
    public class UpdatedTaskResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}