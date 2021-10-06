using System;
using System.Collections.Generic;
using System.Linq;
using Streamish.Models;
using Streamish.Repositories;

namespace Streamish.Tests.Mocks
{
    // Mock repository implementing IVideoRepository to use for unit testing the VideoController without directly interacting with database:
    // Example: Steps to test the Get() method: Create an instance of the VideoController,
    // Call the Get() method, Check the object returned by the Get() method to see if it contains the list of videos.
    // The VideoController constructor shows us that we need to pass in instance of IVideoRepository when we instantiate a VideoController.
    class InMemoryVideoRepository : IVideoRepository
    {
        private readonly List<Video> _data;

        // This mock repository stores data inside the _data list.
        // Since the list is resides within the C# program, the data is stored in memory
        public List<Video> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryVideoRepository(List<Video> startingData)
        {
            _data = startingData;
        }

        public void Add(Video video)
        {
            var lastVideo = _data.Last();
            video.Id = lastVideo.Id + 1;
            _data.Add(video);
        }

        public void Delete(int id)
        {
            var videoToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (videoToDelete == null)
            {
                return;
            }

            _data.Remove(videoToDelete);
        }

        public List<Video> GetAll()
        {
            return _data;
        }

        public Video GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Video video)
        {
            var currentVideo = _data.FirstOrDefault(p => p.Id == video.Id);
            if (currentVideo == null)
            {
                return;
            }

            currentVideo.Description = video.Description;
            currentVideo.Title = video.Title;
            currentVideo.DateCreated = video.DateCreated;
            currentVideo.Url = video.Url;
            currentVideo.UserProfileId = video.UserProfileId;
        }

        public List<Video> Search(string criterion, bool sortDescending)
        {
            //throw new NotImplementedException();
            return _data;
        }

        public List<Video> GetAllWithComments()
        {
            //throw new NotImplementedException();
            return _data;
        }

        public Video GetVideoByIdWithComments(int id)
        {
            //throw new NotImplementedException();
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public List<Video> GetHottestVideos(DateTime since, bool sortDescending)
        {
            //throw new NotImplementedException();
            return _data;
        }
    }
}

