namespace Business.Dtos.Requests.TaskRequests
{
    public class UpdateTaskRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}