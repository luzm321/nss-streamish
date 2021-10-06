using System;
using Microsoft.AspNetCore.Mvc;
using Streamish.Repositories;
using Streamish.Models;

namespace Streamish.Controllers
{
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
        [HttpPost]
        public IActionResult Post(Video video)
        {
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

