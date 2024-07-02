using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyMVC.Helpers;
using CompanyMVC.ViewModels;

namespace CompanyMVC.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        // ask clr for creating object from class implements interface IEmployeeRepository
        // Ctor of controller used only for dependancy Injection
        public EmployeeController(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // BaseUrl/Employee/Index
        public async Task<IActionResult> Index(string? search)
        {
            var employees = string.IsNullOrEmpty(search)
                    ? await _unitOfWork.EmployeeRepository.GetAllAsync()
                    : _unitOfWork.EmployeeRepository.GetEmployeesByName(search);
            var mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {              
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var mappedEmployee = _mapper.Map<Employee>(employeeVM);
                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    TempData["Message"] = "Employee has been added successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employeeVM);
        }

        // baseurl/Employee/details/id
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id == null) return BadRequest(); // Status code 400: client error

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();

            var mappedEmployee = _mapper.Map<EmployeeViewModel>(employee);
            return View(viewName, mappedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EmployeeViewModel employeeVm)
        {
            if (id != employeeVm.Id) return BadRequest();
            var oldpicname = employeeVm.ImageName; 
            if (ModelState.IsValid) // Server-side validation
            {
                try
                {
                    if(employeeVm.Image is not null)
                    {
                        employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");
                    }

                    var mappedEmployee = _mapper.Map<Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    var result = await _unitOfWork.CompleteAsync();
                    if (result > 0)
                    {
                        TempData["Message"] = "Employee has been updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);// Log exception (to a logging service)
                }
            }
            return View(employeeVm);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, EmployeeViewModel employeeVm)
        {
            if (id != employeeVm.Id) return BadRequest();

            try
            {
                var mappedEmployee = _mapper.Map<Employee>(employeeVm);
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0 && employeeVm.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employeeVm.ImageName, "Images");
                    TempData["Message"] = "Employee has been deleted successfully.";
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeVm);
        }

    }
}
