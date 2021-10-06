using System;
using System.Collections.Generic;
using System.Linq;
using Streamish.Models;
using Streamish.Repositories;

namespace Streamish.Tests.Mocks
{
    // Mock repository implementing IUserProfileRepository to use for unit testing the UserProfileController without directly interacting with database:
    // Example: Steps to test the Get() method: Create an instance of the UserProfileController,
    // Call the Get() method, Check the object returned by the Get() method to see if it contains the list of videos.
    // The UserProfileController constructor shows us that we need to pass in instance of IUserProfileRepository when we instantiate a UserProfileController.
    class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _data;

        // This mock repository stores data inside the _data list.
        // Since the list is resides within the C# program, the data is stored in memory
        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserProfileRepository(List<UserProfile> startingData)
        {
            _data = startingData;
        }

        public void Add(UserProfile userProfile)
        {
            var lastUserProfile = _data.Last();
            userProfile.Id = lastUserProfile.Id + 1;
            _data.Add(userProfile);
        }

        public void Delete(int id)
        {
            var userProfileToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (userProfileToDelete == null)
            {
                return;
            }

            _data.Remove(userProfileToDelete);
        }

        public List<UserProfile> GetAll()
        {
            return _data;
        }

        public UserProfile GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public UserProfile GetByIdWithVideos(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile userProfile)
        {
            var currentUserProfile = _data.FirstOrDefault(p => p.Id == userProfile.Id);
            if (currentUserProfile == null)
            {
                return;
            }

            currentUserProfile.Name = userProfile.Name;
            currentUserProfile.Email = userProfile.Email;
            currentUserProfile.DateCreated = userProfile.DateCreated;
            currentUserProfile.ImageUrl = userProfile.ImageUrl;
        }
    }
}


