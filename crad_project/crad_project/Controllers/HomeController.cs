using crad_project.Data;
using crad_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace crad_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext, ILogger<HomeController> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // التحقق مما إذا كان المستخدم مسجل الدخول بالفعل كأدمن
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                TempData["LoginMessage"] = "أنت مسجل الدخول بالفعل كأدمن.";
                return RedirectToAction("WelcomeAdmin", "Admin");
            }

            // التحقق مما إذا كان المستخدم مسجل الدخول بالفعل كطالب
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                TempData["LoginMessage"] = "أنت مسجل الدخول بالفعل كطالب.";
                return RedirectToAction("Welcome", "Users");
            }

            // إذا لم يكن مسجلاً، إرجاع صفحة تسجيل الدخول
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.login login)
        {


            if (ModelState.IsValid)
            {
                var admin = _appDbContext.Admins
                    .FirstOrDefault(e => e.Email == login.Email && e.Password == login.Password);

                if (admin != null)
                {
                    HttpContext.Session.SetInt32("AdminId", admin.adminId);

                    // تخزين كلمة المرور (ليس آمناً عادةً، يفضل استخدام التوكنات)
                    HttpContext.Session.SetString("SessionUser", login.Password);

                    return RedirectToAction("WelcomeAdmin", "Admin");
                }
                
                var user = _appDbContext.Users
                    .FirstOrDefault(e => e.Email == login.Email && e.Password == login.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("userId", user.userId);
                    HttpContext.Session.SetString("Welcome", "Users");

                    // تخزين كلمة المرور (ليس آمناً عادةً، يفضل استخدام التوكنات)
                    HttpContext.Session.SetString("SessionUser", login.Password);

                    return RedirectToAction("Welcome", "Users");
                }
            }


            // إعادة عرض صفحة تسجيل الدخول مع عرض الأخطاء
            return View(login);
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model, string confirmPassword)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.asddress))
            {
                ModelState.AddModelError("", "All fields are required.");
            }
            if (model.Password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    asddress = model.asddress
                };


            }

            _appDbContext.Users.Add(model);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("login", "Home"); // توجيه المستخدم بعد التسجيل
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
