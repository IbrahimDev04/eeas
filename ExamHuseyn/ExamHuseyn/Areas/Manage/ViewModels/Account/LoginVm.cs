using System.ComponentModel.DataAnnotations;

namespace ExamHuseyn.Areas.Manage.ViewModels
{
    public class LoginVm
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }

    }
}
