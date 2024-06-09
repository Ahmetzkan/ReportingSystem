namespace Business.Dtos.Responses.ReportResponses
{
    public class UpdatedReportResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
    }
}