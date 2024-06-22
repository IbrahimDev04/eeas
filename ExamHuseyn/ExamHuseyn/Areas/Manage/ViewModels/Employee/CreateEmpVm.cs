using ExamHuseyn.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamHuseyn.Areas.Manage.ViewModels
{
    public class CreateEmpVm
    {
        [Required]
        public string FullName { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
        public string Twitter { get; set; }
        public string Fb { get; set; }
        public string Insta { get; set; }
        public string LinkIn { get; set; }
        [Required]

        public int PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
