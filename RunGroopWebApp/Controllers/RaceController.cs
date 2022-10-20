using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Models;
using ClubGroopWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp;

namespace ClubGroopWebApp.Controllers
{
    public class RaceController : Controller
    {

        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
           
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> clubs =  await _raceRepository.GetAll();
            return View(clubs);
        }


        public async Task<IActionResult> Detail(int id)
        {
            var rece = await _raceRepository.GetByIdAcync(id);
            return View(rece);
        }


        public async Task<IActionResult> Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId(); //ClaimsPrincipalExtemsions.cs
            var createRaceViewModel = new CreateRaceViewModel { AppUserId = curUserId };
            return View(createRaceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceVM.AppUserId,
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload falied");
            }

            return View(raceVM);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAcync(id);
            if (race == null) return View("Error");
            var clubVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                Url = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(clubVM);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Falied to edit club");
                return View("Edit", raceVM);
            }
            var userClub = await _raceRepository.GetByIdAcyncNotracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address,
                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            var receDetails = await _raceRepository.GetByIdAcync(id);
            if (receDetails == null) return View("Error");

            return View(receDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var receDetails = await _raceRepository.GetByIdAcync(id);
            if (receDetails == null) return View("Error");

            _raceRepository.Delete(receDetails);

            return RedirectToAction("Index");
        }



    }
}
