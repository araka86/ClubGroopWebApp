using Microsoft.AspNetCore.Mvc;
using ClubGroopWebApp.Models;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.ViewModels;
using System.ComponentModel;
using ClubGroopWebApp.Helpers;

namespace ClubGroopWebApp.Controllers 
{
    public class ClubController : Controller
    {
       
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController( IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor )
        {
         
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }


        [HttpGet]
        [Route("RunningClubs/{state}")]
        public async Task<IActionResult> ListClubsByState(string state)
        {
            var clubs = await _clubRepository.GetClubsByState(StateConverter.GetStateByName(state).ToString());
            var clubVM = new ListClubByStateViewModel()
            {
                Clubs = clubs
            };
            if (clubs.Count() == 0)
            {
                clubVM.NoClubWarning = true;
            }
            else
            {
                clubVM.State = state;
            }
            return View(clubVM);
        }





        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
        
            return View(club);
        }

        public async Task<IActionResult> Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId(); //ClaimsPrincipalExtemsions.cs
            var createClubNiewModel = new CreateClubViewModel { AppUserId = curUserId };
            
            return  View(createClubNiewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid) 
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload falied");
            }
              
           return View(clubVM);
        }

        public async Task<IActionResult>Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if(club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                Url = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Falied to edit club");
                return View("Edit",clubVM);
            }
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if(userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                };
                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
               
        }
        public async Task<IActionResult> Delete(int id) 
        {
            var  clubDetails = await _clubRepository.GetByIdAsync(id);
            if(clubDetails == null) return View("Error");

            return View(clubDetails);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id) 
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            _clubRepository.Delete(clubDetails);

            return RedirectToAction("Index");   
        }



    }
}
