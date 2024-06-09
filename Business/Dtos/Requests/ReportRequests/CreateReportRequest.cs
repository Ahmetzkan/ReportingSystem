namespace Business.Dtos.Requests.ReportRequests
{
    public class CreateReportRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
    }
}