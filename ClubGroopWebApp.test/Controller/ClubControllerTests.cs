using ClubGroopWebApp.Controllers;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClubGroopWebApp.test.Controller
{
    public class ClubControllerTests
    {
        private readonly ClubController _clubController;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClubControllerTests()
        {
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _clubController = new ClubController(_clubRepository, _photoService, _httpContextAccessor); 

        }

        [Fact]
        public void ClubController_Index_RerunSuccess()
        {
            //Arrange
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
            //Act
            var result  = _clubController.Index();

            //Asert

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ClubController_Detail_RerunSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
            //Act
            var result = _clubController.Detail(id);

            //Asert

            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
