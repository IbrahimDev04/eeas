using ExamHuseyn.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamHuseyn.Areas.Manage.ViewModels
{
    public class UpdateEmpVm
    {
        public string FullName { get; set; }

        public string Description { get; set; }
    
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
        public string Twitter { get; set; }
        public string Fb { get; set; }
        public string Insta { get; set; }
        public string LinkIn { get; set; }
        public int PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
