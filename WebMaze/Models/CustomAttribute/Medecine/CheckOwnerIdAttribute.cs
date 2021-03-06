﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.DbStuff.Repository.MedicineRepo;

namespace WebMaze.Models.CustomAttribute.Medecine
{
    public class CheckOwnerIdAttribute : ValidationAttribute
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


            var id = (long)value;

            var medRepo = validationContext.GetService(typeof(MedicalInsuranceRepository))
                as MedicalInsuranceRepository;
            var existingId = medRepo.GetOwner(id);
            if (existingId != null)
            {
                return new ValidationResult($" Пользователь с данным id: {id} уже имеет страховку.");
            }
           
            return ValidationResult.Success;
        }
    }
}
