using crad_project.Data;
using crad_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace crad_project.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public AdminController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult WelcomeAdmin()
        {
            return View();
        }


        //Categories
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Categories model)
        {
                _appDbContext.Categories.Add(model);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("CategoryList"); // الانتقال إلى قائمة التصنيفات بعد الإضافة
        }

        // ✅ جلب قائمة التصنيفات وعرضها
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return View(categories);
        }

        // ✅ تعديل التصنيف (GET)
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // ✅ تعديل التصنيف (POST)
        [HttpPost]
        public async Task<IActionResult> EditCategory(Categories model)
        {
                _appDbContext.Categories.Update(model);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("CategoryList");    
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }
        // ✅ حذف التصنيف
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(Categories model)
        {
            _appDbContext.Categories.Remove(model);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("CategoryList");
        }



        //Products


        // عرض نموذج إضافة منتج
        public IActionResult AddProduct()
        {
            ViewBag.Categories = new SelectList(_appDbContext.Categories, "categoryId", "Name");
            return View();
        }

        // إضافة منتج (POST)
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model, List<IFormFile> imageFiles)
        {
            // التحقق من وجود صورة تم تحميلها
            var imageUrls = new List<string>();

            // التحقق من وجود صور تم تحميلها
            if (imageFiles != null && imageFiles.Count > 0)
            {
                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        // تحديد اسم الصورة ومسار حفظها
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        // حفظ الصورة في المجلد
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // إضافة المسار إلى قائمة الصور
                        imageUrls.Add("/images/" + fileName);
                    }
                }

                // حفظ قائمة مسارات الصور في قاعدة البيانات
                model.ImageUrls = imageUrls;
            }
            _appDbContext.Products.Add(model);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("ProductList"); // الانتقال إلى قائمة المنتجات بعد الإضافة
            
        }


        public async Task<IActionResult> ProductList()
        {
            var products = await _appDbContext.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }


        // عرض نموذج تعديل منتج
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = _appDbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            // إذا كان المنتج موجودًا، أضف التصنيفات إلى ViewBag
            ViewBag.Categories = new SelectList(_appDbContext.Categories, "categoryId", "Name", product.CategoryId);
            return View(product);


        }

        // تعديل منتج (POST)
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product model, List<IFormFile> imageFiles)
        {
            var imageUrls = new List<string>();
            if (imageFiles != null && imageFiles.Count > 0)
            {
                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        // تحديد اسم الصورة ومسار حفظها
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        // حفظ الصورة في المجلد
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // إضافة المسار إلى قائمة الصور
                        imageUrls.Add("/images/" + fileName);
                    }
                }

                // حفظ قائمة مسارات الصور في قاعدة البيانات
                model.ImageUrls = imageUrls;
            }
            _appDbContext.Products.Update(model);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("ProductList");
        }
        // عرض نموذج حذف منتج
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // حذف منتج (POST)
        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();

            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }

    }
}
