namespace Business.Dtos.Requests.TaskRequests
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}