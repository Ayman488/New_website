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
        public async Task<IActionResult> WelcomeAdmin()
        {
            if (User.IsInRole("Admin")) // تحقق من دور المستخدم
            {
                ViewBag.Layout = "_AdminLayout"; // استخدام Layout الإدارة
            }
            else
            {
                ViewBag.Layout = "_Layout"; // استخدام Layout العام
            }
            return View();
        }

        // عرض نموذج إضافة فئة
        public async Task<IActionResult> AddCategory()
        {
            // جلب القائمة من قاعدة البيانات
            var categories = await _appDbContext.Categories.ToListAsync();

            // تمرير القائمة والعنصر الجديد إلى العرض
            ViewBag.CategoriesList = categories; // القائمة
            return View(new Categories()); // العنصر الجديد
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Categories category)
        {
                // إضافة الفئة الجديدة إلى قاعدة البيانات
                _appDbContext.Categories.Add(category);
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AddCategory)); // إعادة التوجيه إلى نفس الصفحة
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _appDbContext.Categories.FindAsync(categoryId);

            if (category != null)
            {
                _appDbContext.Categories.Remove(category);
                await _appDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AddCategory)); // إعادة التوجيه إلى نفس الصفحة
        }


        public async Task<IActionResult> EditCategory(int categoryId)
        {
            var category = await _appDbContext.Categories.FindAsync(categoryId);

            if (category == null)
            {
                return NotFound(); // إذا لم يتم العثور على العنصر
            }

            var categories = await _appDbContext.Categories.ToListAsync();
            ViewBag.CategoriesList = categories; // تمرير القائمة لعرضها أسفل النموذج

            return View("AddCategory", category); // عرض النموذج الحالي مع البيانات الموجودة
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Categories category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _appDbContext.Categories.FindAsync(category.categoryId);

                if (existingCategory != null)
                {
                    // تعديل القيم
                    existingCategory.Name = category.Name;
                    existingCategory.IsActive = category.IsActive;

                    // حفظ التعديلات
                    await _appDbContext.SaveChangesAsync();
                }

                return RedirectToAction(nameof(AddCategory)); // إعادة التوجيه إلى الصفحة الرئيسية
            }

            // إذا كان هناك أخطاء، إعادة تمرير البيانات إلى العرض
            ViewBag.CategoriesList = await _appDbContext.Categories.ToListAsync();
            return View("AddCategory", category);
        }

        // عرض صفحة إدارة SubCategories
        public IActionResult SubCategory()
        {
            // جلب جميع الفئات (Categories) لتمريرها إلى الـ View
            var categories = _appDbContext.Categories
                .Where(c => c.IsActive)
                .ToList();

            // جلب جميع الفئات الفرعية (SubCategories) مع الفئات الرئيسية المرتبطة بها
            var subCategories = _appDbContext.SubCategories
                .Include(sc => sc.Category)
                .ToList();

            // تمرير البيانات إلى الـ View
            ViewBag.SubCategories = subCategories; // الفئات الفرعية
            return View(categories); // تمرير الفئات كـ Model
        }

        // إضافة SubCategory جديد
        [HttpPost]
        public IActionResult AddSubCategory(string name, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name) || categoryId <= 0)
            {
                ModelState.AddModelError("", "All fields are required.");
                return RedirectToAction("SubCategory");
            }

            var subCategory = new SubCategory
            {
                Name = name,
                CategoryId = categoryId
            };

            _appDbContext.SubCategories.Add(subCategory);
            _appDbContext.SaveChanges();

            TempData["SuccessMessage"] = "SubCategory added successfully!";
            return RedirectToAction("SubCategory");
        }
        // ✅
        //Products
        // لاضافة المنتج 
        [HttpGet]
        public IActionResult AddProduct()
        {
            var subcategories = _appDbContext.SubCategories.ToList();
            ViewBag.SubCategories = subcategories; // إرسال جميع الـ SubCategories إلى الـ View
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(string category)
        {
            // التحقق من الفئة المختارة
            
            if (category == "Mobiles")
            {

                // تصفية الـ SubCategories بناءً على فئة "Mobiles"
                ViewBag.SubCategories = _appDbContext.SubCategories
                    .Where(sc => sc.Category.Name == "Phones")
                    .ToList();

                // إعادة التوجيه إلى صفحة AddMobile
                return View("AddMobile");
            }

            // في حال كانت الفئة مختلفة (مستقبلاً يمكن إضافة المزيد من الفئات هنا)
            
            return RedirectToAction("AddProduct");
        }


        [HttpPost]
        public async Task<IActionResult> SaveMobile(Mobile mobile, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (images != null && images.Count > 0)
                {
                    foreach (var imageFile in images)
                    {
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                // قراءة البيانات وتحويلها إلى byte[]
                                await imageFile.CopyToAsync(memoryStream);
                                mobile.Images.Add(memoryStream.ToArray()); // تخزين الصورة كـ byte[] في قائمة الصور
                            }
                        }
                    }
                }

                // حفظ البيانات في قاعدة البيانات
                _appDbContext.Mobiles.Add(mobile);
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("ProductList"); // عرض قائمة المنتجات
            }

            // إذا كان هناك خطأ، إعادة عرض الصفحة مع البيانات
            ViewBag.SubCategories = _appDbContext.SubCategories
                .Where(sc => sc.Category.Name == "Mobiles")
                .ToList();

            return View("AddMobile", mobile);
        }




        //Province


        // عرض صفحة إضافة محافظة (GET)
        public IActionResult AddProvince()
        {
            return View();
        }

        // إضافة محافظة (POST)
        [HttpPost]
        public async Task<IActionResult> AddProvince(Province model)
        {
            // إضافة المحافظة إلى قاعدة البيانات
            _appDbContext.Provinces.Add(model);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("ProvinceList"); // الانتقال إلى قائمة المحافظات بعد الإضافة

        }

        // عرض قائمة المحافظات
        public IActionResult ProvinceList()
        {
            var provinces = _appDbContext.Provinces.ToList();
            return View(provinces);
        }

        // حذف محافظة
        public async Task<IActionResult> DeleteProvince(int id)
        {
            var province = await _appDbContext.Provinces.FindAsync(id);
            if (province == null) return NotFound(); // في حال لم يتم العثور على المحافظة
            return View(province); // عرض الصفحة للتأكيد
        }

        // حذف محافظة (POST)
        [HttpPost, ActionName("DeleteProvince")]
        public async Task<IActionResult> DeleteConfirmedP(int id)
        {
            var province = await _appDbContext.Provinces.FindAsync(id);
            if (province == null) return NotFound();

            _appDbContext.Provinces.Remove(province);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("ProvinceList"); // الانتقال إلى قائمة المحافظات بعد الحذف
        }















        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
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
            // التحقق إذا كانت صورة قد تم تحميلها
            if (imageFiles != null && imageFiles.Count > 0)
            {
                var imageData = new byte[0]; // إنشاء مصفوفة فارغة لتخزين الصورة

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            // نسخ محتوى الصورة إلى MemoryStream
                            await imageFile.CopyToAsync(memoryStream);

                            // إضافة الصورة المحفوظة كـ byte[] إلى متغير imageData
                            imageData = memoryStream.ToArray();
                        }
                    }
                }

                // حفظ الصورة كـ byte[] في قاعدة البيانات
                model.Image = imageData;
            }

            // تحديث المنتج في قاعدة البيانات
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
