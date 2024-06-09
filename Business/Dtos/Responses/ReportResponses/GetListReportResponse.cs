namespace Business.Dtos.Responses.ReportResponses
{
    public class GetListReportResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
        public DateTime? CreatedDate{ get; set; }
        public DateTime? UpdatedDate{ get; set; }
    }
}