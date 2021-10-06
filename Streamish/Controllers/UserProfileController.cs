using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Streamish.Repositories;
using Streamish.Models;

namespace Streamish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {

        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        // https://localhost:5001/api/userprofile/
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userProfileRepository.GetAll());
        }

        // https://localhost:5001/api/userprofile/id
        // Route parameters enable the ability to pass in data on the URL, such as the id parameter with integer value:
        // The route parameters are specified in the HTTP verb attribute (ex. [HttpGet("{id}")])
        // and in the method parameters (ex. public IActionResult Get(int id))
        // Route parameters are not limited to ids and can get significantly more complex if the need arises.
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userProfile = _userProfileRepository.GetById(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        // https://localhost:5001/api/userprofile/getwithvideos/id
        [HttpGet("GetWithVideos/{id}")]
        public IActionResult GetWithVideos(int id)
        {
            var userProfile = _userProfileRepository.GetByIdWithVideos(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        // https://localhost:5001/api/userprofile/
        [HttpPost]
        public IActionResult Post(UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Add(userProfile);
                return CreatedAtAction("Get", new { id = userProfile.Id }, userProfile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // https://localhost:5001/api/userprofile/id
        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return BadRequest();
            }

            _userProfileRepository.Update(userProfile);
            return NoContent();
        }

        // https://localhost:5001/api/userprofile/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userProfileRepository.Delete(id);
            return NoContent();
        }
    }
}
