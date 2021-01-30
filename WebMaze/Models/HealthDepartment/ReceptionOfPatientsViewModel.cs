using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.CustomAttribute;

namespace WebMaze.Models.HealthDepartment
{
    public class ReceptionOfPatientsViewModel
    {
        public virtual long Id { get; set; }
        public virtual long EnrolledCitizenId { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public virtual string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public virtual string LastName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public virtual string PhoneNumber { get; set; }
        [CheckEndDate(ErrorMessage = "Необходимо установить дату больше сегодняшней")]
        public virtual DateTime DateTime { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public virtual string PrimarySymptoms { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public virtual string MedicineDepartment { get; set; }

    }
}
