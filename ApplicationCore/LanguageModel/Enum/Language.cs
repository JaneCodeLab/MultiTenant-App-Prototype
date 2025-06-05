
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel
{
    public enum Language
    {
        [Display(Name = "English")]
        English = 1,
        [Display(Name = "Français")]
        French = 2,
    }
}
