namespace ExamHuseyn.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Twitter { get; set; }
        public string Fb { get; set; }
        public string Insta { get; set; }
        public string LinkIn { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
