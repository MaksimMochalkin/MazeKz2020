using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.DbStuff.Repository;
using WebMaze.DbStuff.Repository.MedicineRepo;

namespace WebMaze.Models.CustomAttribute
{
    public class CheckUserIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !(value is long))
            {
                throw new Exception("CheckOwnerIdAttribute must be applied only for long fields");
            }

            if (value == null)
            {
                return new ValidationResult("Поле не может быть пустым");
            }


            var userId = (long)value;

            var certRepo = validationContext.GetService(typeof(MedicineCertificateRepository))
                as MedicineCertificateRepository;
            var existingId = certRepo.GetUser(userId);
            if(existingId != null)
            {
                return new ValidationResult($" Пользователь с данным id:{userId} уже зарегестрирован.");
            }

            return ValidationResult.Success;
        }
    }
}
