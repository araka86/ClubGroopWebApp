using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ClubGroopWebApp.Models;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using ClubGroopWebApp.ViewModels;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Data;
using ClubGroopWebApp.Helpers;

namespace ClubGroopWebApp.Controllers
{
    public class HomeController : Controller
    {
         private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILocationService _locationService;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILocationService locationService)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            try
            {
                string url = "https://ipinfo.io?token=6a613f66ff98c9";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo regionInfo = new RegionInfo(ipInfo.Country);
                ipInfo.Country = regionInfo.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;
                if (ipInfo.Region != null)
                {
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                }
                else
                {
                    homeViewModel.Clubs = null;
                }

            }
            catch (Exception ex)
            {
                homeViewModel.Clubs = null;
                throw;
            }
            return View(homeViewModel);
        }


        public IActionResult Register()
        {
            var response = new HomeUserCreateViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel homeVM)
        {
            var createVM = homeVM.Register;
            if (!ModelState.IsValid) return View(homeVM);

            var user = await _userManager.FindByEmailAsync(createVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", "This email address is already in use");
                return View(homeVM);
            }

            var userLocation = await _locationService.GetCityByZipCode(createVM.ZipCode ?? 0);

            if (userLocation == null)
            {
                ModelState.AddModelError("Register.ZipCode", "Could not find zip code!");
                return View(homeVM);
            }

            var newUser = new AppUser
            {
                UserName = createVM.UserName,
                Email = createVM.Email,
                Address = new Address()
                {
                    State = userLocation.StateCode,
                    City = userLocation.CityName,
                    ZipCode = createVM.ZipCode ?? 0,
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Club");
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