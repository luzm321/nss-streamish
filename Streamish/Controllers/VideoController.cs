using System;
using Microsoft.AspNetCore.Mvc;
using Streamish.Repositories;
using Streamish.Models;
using Microsoft.AspNetCore.Authorization;

namespace Streamish.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _videoRepository;
        public VideoController(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        // https://localhost:5001/api/video/
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_videoRepository.GetAll());
        }

        //https://localhost:5001/api/video/getwithcomments
        [HttpGet("GetWithComments")]
        public IActionResult GetWithComments()
        {
            var videos = _videoRepository.GetAllWithComments();
            return Ok(videos);
        }

        //https://localhost:5001/api/video/users/id
        [HttpGet("Users/{id}")]
        public IActionResult GetUserVideos(int id)
        {
            var videos = _videoRepository.GetVideosByUserProfileId(id);
            if (videos == null)
            {
                return StatusCode(404);
            }
            return Ok(videos);
        }


        // https://localhost:5001/api/video/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var video = _videoRepository.GetById(id);
            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
        }

        //https://localhost:5001/api/video/getvideowithcomments/id
        [HttpGet("GetVideoWithComments/{id}")]
        public IActionResult GetVideoWithComments(int id)
        {
            var video = _videoRepository.GetVideoByIdWithComments(id);
            if (video == null)
            {
                //return StatusCode(404);
                return NotFound();
            }
            return Ok(video);
        }

        //https://localhost:5001/api/video/search?q=e&sortDesc=false
        // URL's route contains search and the URL's query string has values for q and sortDesc keys. search corresponds to the the argument
        // passed to the [HttpGet("search")] attribute, and q and sortDesc correspond to the method's parameters.
        [HttpGet("search")]
        public IActionResult Search(string q, bool sortDesc)
        {
            return Ok(_videoRepository.Search(q, sortDesc));
        }

        //https://localhost:5001/api/video/hottest?since=<SOME_DATE> 
        [HttpGet("hottest")]
        public IActionResult HottestVideos(DateTime since, bool sortDesc)
        {
            return Ok(_videoRepository.GetHottestVideos(since, sortDesc));
        }


        // https://localhost:5001/api/video/
        //[HttpPost]
        //public IActionResult Post(Video video)
        //{
        //    _videoRepository.Add(video);
        //    return CreatedAtAction("Get", new { id = video.Id }, video);
        //}

        [HttpPost]
        public IActionResult Post(Video video)
        {
            // NOTE: This is only temporary to set the UserProfileId until we implement login
            // TODO: After we implement login, use the id of the current user
            video.UserProfileId = 1;

            video.DateCreated = DateTime.Now;
            if (string.IsNullOrWhiteSpace(video.Description))
            {
                video.Description = null;
            }

            try
            {
                // Handle the video URL

                // A valid video link might look like this:
                //  https://www.youtube.com/watch?v=sstOXCQ-EG0&list=PLdo4fOcmZ0oVGRpRwbMhUA0KAvMA2mLyN
                // 
                // Our job is to pull out the "v=XXXXX" part to get the get the "code/id" of the video
                //  So we can construct an URL that's appropriate for embedding a video

                // An embeddable Video URL looks something like this:
                //  https://www.youtube.com/embed/sstOXCQ-EG0

                // If this isn't a YouTube video, we should just give up
                if (!video.Url.Contains("youtube"))
                {
                    return BadRequest();
                }

                // If it's not already an embeddable URL, we have some work to do
                if (!video.Url.Contains("embed"))
                {
                    var videoCode = video.Url.Split("v=")[1].Split("&")[0];
                    video.Url = $"https://www.youtube.com/embed/{videoCode}";
                }
            }
            catch // Something went wrong while creating the embeddable url
            {
                return BadRequest();
            }

            _videoRepository.Add(video);

            return CreatedAtAction("Get", new { id = video.Id }, video);
        }


        // https://localhost:5001/api/video/id
        [HttpPut("{id}")]
        public IActionResult Put(int id, Video video)
        {
            if (id != video.Id)
            {
                return BadRequest();
            }

            _videoRepository.Update(video);
            return NoContent();
        }

        // https://localhost:5001/api/video/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _videoRepository.Delete(id);
            return NoContent();
        }
    }
}

