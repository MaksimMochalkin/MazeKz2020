using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.CustomAttribute;
using WebMaze.Models.CustomAttribute.Medecine;

namespace WebMaze.Models.HealthDepartment
{
    public class RecordFormViewModel
    {
        [CheckCitizenId]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public long CitizenId { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string PhoneNumber { get; set; }
        [CheckEndDate(ErrorMessage = "Необходимо установить дату больше сегодняшней")]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public DateTime DateTime { get; set; }
    }
}
