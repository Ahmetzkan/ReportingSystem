namespace Business.Dtos.Responses.ProjectResponses
{
    public class CreatedProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}