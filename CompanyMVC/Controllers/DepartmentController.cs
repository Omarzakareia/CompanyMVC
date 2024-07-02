using AutoMapper;
using BLL.Interfaces;
using CompanyMVC.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCProjectPL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Constructor for dependency injection
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) // ask clr for creating object from class implements interface IDepartmentRepository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var mappedDepartment = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);

            return View(mappedDepartment);
        }

        // GET: /Department/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentViewModel)
        {
            //check model state
            if (ModelState.IsValid) // Server-side validation
            {
                var mappedDepartment = _mapper.Map<Department>(departmentViewModel);
                await _unitOfWork.DepartmentRepository.AddAsync(mappedDepartment);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    TempData["Message"] = "Department has been created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(departmentViewModel);
        }

        // GET: /Department/Details/id
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id == null) return BadRequest(); // Status code 400: client error

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department == null) return NotFound();

            var mappedDepartment = _mapper.Map<DepartmentViewModel>(department);

            return View(viewName, mappedDepartment);
        }

        // GET: /Department/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id,"Edit");
        }

        // POST: /Department/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel departmentViewModel)
        {
            if(id != departmentViewModel.Id) return BadRequest();
            if (ModelState.IsValid) //server side validation
            {
                try
                {
                    var mappedDepartment = _mapper.Map<Department>(departmentViewModel);
                    _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                    var Result = await _unitOfWork.CompleteAsync();
                    if(Result > 0 )
                    {
                        TempData["Message"] = "Department has been updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (System.Exception ex)
                {
                    // Log exception (e.g., to a logging service)
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentViewModel);
        }

        // GET: /Department/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        // POST: /Department/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id, DepartmentViewModel departmentViewModel)
        {
            if (id != departmentViewModel.Id) return BadRequest();

            try
            {
                var mappedDepartment = _mapper.Map<Department>(departmentViewModel);
                _unitOfWork.DepartmentRepository.Delete(mappedDepartment);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    TempData["Message"] = "Department has been deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(departmentViewModel);
        }


    }
}
