using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.DbStuff.Model;
using WebMaze.DbStuff.Model.Medicine;
using WebMaze.DbStuff.Repository.MedicineRepo;
using WebMaze.Models.HDManager;

namespace WebMaze.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HDManagerPageController : Controller
    {
        private IMapper mapper;
        private MedicineCertificateRepository certificateRepository;

        public HDManagerPageController(IMapper mapper, MedicineCertificateRepository certificateRepository)
        {
            this.mapper = mapper;
            this.certificateRepository = certificateRepository;
        }

        [HttpGet]
        public IActionResult ManagerPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddMedicineCertificate()
        {
            return View(new MedicineCertificateViewModel());
        }

        [HttpPost]
        public IActionResult AddMedicineCertificate(MedicineCertificateViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            
            var certificate = mapper.Map<MedicineCertificate>(viewModel);
            certificateRepository.Save(certificate);
                       
            return View(new MedicineCertificateViewModel());
        }

        [HttpGet]
        public IActionResult GetAllCertificate(string position, DateTime? dateOfIssue)
        {

            if (!string.IsNullOrWhiteSpace(position))
            {
                var byPosition = certificateRepository.GetCertificateByPosition(position);
                var viewModel = mapper.Map<List<MedicineCertificateViewModel>>(byPosition);

                return View(viewModel);
            }
            else if (dateOfIssue != null) 
            {
                var byDate = certificateRepository.GetCertificateByDate(dateOfIssue);
                var viewModel = mapper.Map<List<MedicineCertificateViewModel>>(byDate);

                return View(viewModel);
            }
            else
            {
                var certificate = certificateRepository.GetAll();
                var viewModel = mapper.Map<List<MedicineCertificateViewModel>>(certificate);

                return View(viewModel);
            }

        }

        
        
    }
}
