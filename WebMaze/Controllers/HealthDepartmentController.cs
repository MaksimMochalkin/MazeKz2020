﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMaze.DbStuff;
using WebMaze.DbStuff.Model;
using WebMaze.DbStuff.Model.Medicine;
using WebMaze.DbStuff.Repository;
using WebMaze.DbStuff.Repository.MedicineRepo;
using WebMaze.DbStuff.Repository.MedicineRepository;
using WebMaze.Infrastructure.Enums;
using WebMaze.Models.Account;
using WebMaze.Models.Department;
using WebMaze.Models.HealthDepartment;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class HealthDepartmentController : Controller
    {
        private RecordFormRepository recordFormRepository;
        private IMapper mapper;
        private CitizenUserRepository citizenRepository;
        private MedicalInsuranceRepository insuranceRepository;
        private UserService userService;
        private ReceptionOfPatientsRepository receptionRepository;


        public HealthDepartmentController(RecordFormRepository recordFormRepository,
                                          IMapper mapper, CitizenUserRepository citizenRepository,
                                          MedicalInsuranceRepository insuranceRepository, UserService userService,
                                          ReceptionOfPatientsRepository receptionRepository)
        {
            this.mapper = mapper;
            this.recordFormRepository = recordFormRepository;
            this.citizenRepository = citizenRepository;
            this.insuranceRepository = insuranceRepository;
            this.userService = userService;
            this.receptionRepository = receptionRepository;
        }


        [HttpGet]
        public IActionResult HealthDepartment()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult RecordForm()
        {
            return PartialView("RecordForm");
        }



        [HttpPost]
        public IActionResult RecordForm(RecordFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var citizen = citizenRepository.Get(viewModel.CitizenId);
            var recordForm = mapper.Map<RecordForm>(viewModel);
            recordForm.Citizen = citizen;
            recordFormRepository.Save(recordForm);

            return RedirectToAction("HealthDepartment");
        }

        [HttpGet]
        public IActionResult GetAllForm()
        {
            var record = recordFormRepository.GetAll();
            var viewModel = mapper.Map<List<ListRecordFormViewModel>>(record);


            return View(viewModel);
        }

        [HttpGet]
        public IActionResult MedicalInsurance()
        {
            return View(new MedicalInsuranceViewModel());
        }

        [HttpPost]
        public IActionResult MedicalInsurance(MedicalInsuranceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var id = viewModel.OwnerId;
            var coast = viewModel.Coast;
            var person = citizenRepository.Get(id);
            var solvency = person.Balance.CompareTo(coast);

            if (solvency >= 0)
            {
                var user = mapper.Map<MedicalInsurance>(viewModel);
                insuranceRepository.Save(user);
            }

            return View();

        }


        [HttpGet]
        public IActionResult GetListHospital()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetListMorgue()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetListSchool()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contacts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HotLine()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MedicineNews()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new ForDHRegistrationViewModel());
        }

        [HttpPost]
        public IActionResult Registration(ForDHRegistrationViewModel registrView)
        {
            if (!ModelState.IsValid)
            {
                return View(registrView);
            }

            var user = mapper.Map<CitizenUser>(registrView);
            userService.Save(user);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var viewvModel = new ForDHLoginViewModel();
            viewvModel.ReturnUrl = Request.Query["ReturnUrl"];

            return View(viewvModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(ForDHLoginViewModel loginView)
        {
            var user = citizenRepository.GetUserByNameAndPassword(loginView.Login, loginView.Password);
            if(user == null)
            {
                return View(loginView);
            }

            
            await userService.SignInAsync(loginView.Login, loginView.Password, isPersistent: false);

            if (string.IsNullOrEmpty(loginView.ReturnUrl))
            {
                return RedirectToAction("HealthDepartment", "HealthDepartment");
            }
            else
            {
                return Redirect(loginView.ReturnUrl);
            }
            
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("HealthDepartment", "HealthDepartment");
        }

        
        [HttpGet]
        public IActionResult Hospital1()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Hospital2()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Hospital3()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Hospital4()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Hospital5()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Hospital6()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Hospital7()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ReceptionPatient()
        {
            return PartialView("ReceptionPatient");
        }

        [HttpPost]
        public IActionResult ReceptionPatient(ReceptionOfPatientsViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var citizen = citizenRepository.Get(viewModel.EnrolledCitizenId);
            var reception = mapper.Map<ReceptionOfPatients>(viewModel);
            reception.EnrolledCitizen = citizen;
            receptionRepository.Save(reception);


            return RedirectToAction("HealthDepartment");
        }


    }
}
