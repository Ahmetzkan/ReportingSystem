namespace Business.Dtos.Requests.ReportRequests
{
    public class UpdateReportRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }

    }
}