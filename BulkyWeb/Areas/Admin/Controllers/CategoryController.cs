
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly /*ApplicationDbContext*/ IUnitOfWork _unitOfWork; //2- Creating private readonly field
        public CategoryController(IUnitOfWork unitOfWork) //1- Implementing ApplicationDbContext and calling it db .
        {
            _unitOfWork = unitOfWork; //3- Assigning implementation to local variable so we can use it in any other action method.
        }
        public IActionResult Index()
        {
            // Retrieving all the categories

            //3- var objCategoryList = _db.Categories.ToList();
            //4- Using list explicitly 
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList(); //Successfully retrieved list of categories,confirmed by debugging.

            //5- Go to View to fetch and display list of Category
            return View(objCategoryList); // Passing to view .
        }
        public IActionResult Create() //10- Creating Action Method and also making Create View-------head to create view
        {
            return View();
        }
        [HttpPost]                    // whenever something is posted this end point will be invoked to add Category
        public IActionResult Create(Category obj) //12- Actio method with same name to post request,adding Category obj
        {
            if (obj.Name == obj.DisplayOrder.ToString())            // 16- Adding error message if the name and display order is inputted as same.
            {
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the name"); // "Name" is key
            }
            //if (obj.Name!=null && obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("","test is an invalid value ");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);              // This obj has the value of Category that needs to be added
                _unitOfWork.Save();                    // For executing changes
                TempData["success"] = "Category created successfully";   //22- For showing message on successful execution
                return RedirectToAction("Index");      //  <= If you are in the same action ,
                                                       //  For redirecting to another action need to assign controller name ("Index","Category")                         
            }
            return View(obj);                     // If model state is not valid stay on create page .
        }


        // Head to Category.cs

        public IActionResult Edit(int? id)                // 19 - Action For Edit Button
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }                                                     //      Three ways to retrieve Category
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);        //19.1- using Find mthd which can work on primry keys. 
                                                                                         // Category? CategoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);   //19.2 We can also use this method to retrieve data
                                                                                         // Category? CategoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();        //Head to Edit View 

            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated successfully";   //22.1- For showing message on successful execution
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)                // 20 - Edit btn 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }                                                     //      Three ways to retrieve Category
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);        //19.1- using Find mthd which can work on primry keys. 
                                                                                         // Category? CategoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);   //19.2 We can also use this method to retrieve data
                                                                                         // Category? CategoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();        //Head to Edit View 

            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }
        [HttpPost, ActionName("Delete")]             // Setting same Name 

        public IActionResult DeletePOST(int? id)   // 
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);  //To delete we need to first find that Category from database
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);            // using remove method to delete 
            _unitOfWork.Save();                     //saving changes
            TempData["success"] = "Category deleted successfully";   //22.2- For showing message on successful execution
            return RedirectToAction("Index");

        }
    }
}
