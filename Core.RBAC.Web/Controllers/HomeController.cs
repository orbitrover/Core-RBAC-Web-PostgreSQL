using Core.RBAC.Web.Models;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Core.RBAC.Web.Controllers
{
    [Authorize("Authorization")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataAccessService _dataAccessService;
        public HomeController(IDataAccessService dataAccessService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataAccessService = dataAccessService ?? throw new ArgumentNullException(nameof(dataAccessService));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var demoUsers = await _dataAccessService.GetDetmoUsers();
            return View(demoUsers);
        }
        public IActionResult ClientAdmin()
        {
            return View();
        }
       
        public IActionResult CustomerSupport()
        {
            return View();
        }
       
        public IActionResult ITNetwork()
        {
            return View();
        }
        
        public IActionResult Payroll()
        {
            return View();
        }
        
        public IActionResult Engineering()
        {
            return View();
        }
        
        public IActionResult Reception()
        {
            return View();
        }
        
        public IActionResult Manager()
        {
            return View();
        }
        
        public IActionResult HumanResource()
        {
            return View();
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
