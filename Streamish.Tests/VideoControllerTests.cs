using Streamish.Controllers;
using Streamish.Models;
using Streamish.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Streamish.Tests
{
    // A unit test verifies a single method works properly:
    public class VideoControllerTests
    {
        [Fact]
        public void Get_Returns_All_Videos()
        {
            // Arrange:
            // Mocks the environment (pre-conditions necessary) to test method in Act:
            // In the arrange section we create any objects that are needed for the test.
            // In example below, we create some test Videos, a mock repository and an instance of the VideoController class.
            var videoCount = 20;
            var videos = CreateTestVideos(videoCount);

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act: Invoke method to retrieve result:
            // In the act section we execute the method that is being tested.
            // In this example, we call the VideoController.Get() method.
            var result = controller.Get();

            // Assert: 2 assertions to verify and compare if value of result/date is of type list videos
            // and if they are an identical set:
            // In the assert section we verify that the method tested did what we expected it to do. Usually use the Assert utility class (provided by xUnit) to help.
            // In this example, we verify the result of Get() method is an instance of the OkObjectResult class - this is the type returned by the Ok() method - and
            // then we verify that it contains the expected list of videos.
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualVideos = Assert.IsType<List<Video>>(okResult.Value);

            Assert.Equal(videoCount, actualVideos.Count);
            Assert.Equal(videos, actualVideos);
        }

        [Fact]
        public void Get_Returns_All_Videos_With_Comments()
        {
            // Arrange:
            var videoCount = 20;
            var videos = CreateTestVideos(videoCount);

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act: Invoke method to retrieve result:
            var result = controller.GetWithComments();

            // Assert: 2 assertions to verify and compare if value of result/date is of type list videos
            // and if they are an identical set:
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualVideos = Assert.IsType<List<Video>>(okResult.Value);

            Assert.Equal(videoCount, actualVideos.Count);
            Assert.Equal(videos, actualVideos);
        }

        [Fact]
        public void Get_By_Id_Returns_NotFound_When_Given_Unknown_id()
        {
            // Arrange 
            var videos = new List<Video>(); // no videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_By_Id_Returns_Video_With_Given_Id()
        {
            // Arrange
            var testVideoId = 99;
            var videos = CreateTestVideos(5);
            videos[0].Id = testVideoId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act
            var result = controller.Get(testVideoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualVideo = Assert.IsType<Video>(okResult.Value);

            Assert.Equal(testVideoId, actualVideo.Id);
        }

        [Fact]
        public void Get_By_Id_With_Comments_Returns_NotFound_When_Given_Unknown_id()
        {
            // Arrange 
            var videos = new List<Video>(); // no videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act
            var result = controller.GetVideoWithComments(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_By_Id_With_Comments_Returns_Video_With_Given_Id()
        {
            // Arrange
            var testVideoId = 99;
            var videos = CreateTestVideos(5);
            videos[0].Id = testVideoId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act
            var result = controller.GetVideoWithComments(testVideoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualVideo = Assert.IsType<Video>(okResult.Value);

            Assert.Equal(testVideoId, actualVideo.Id);
        }

        [Fact]
        public void Post_Method_Adds_A_New_Video()
        {
            // Arrange 
            var videoCount = 20;
            var videos = CreateTestVideos(videoCount);

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act
            var newVideo = new Video()
            {
                Description = "Description",
                Title = "Title",
                Url = "http://youtube.url?v=1234",
                DateCreated = DateTime.Today,
                UserProfileId = 999,
                UserProfile = CreateTestUserProfile(999),
            };

            controller.Post(newVideo);

            // Assert
            Assert.Equal(videoCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Put_Method_Returns_BadRequest_When_Ids_Do_Not_Match()
        {
            // Arrange
            var testVideoId = 99;
            var videos = CreateTestVideos(5);
            videos[0].Id = testVideoId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            var videoToUpdate = new Video()
            {
                Id = testVideoId,
                Description = "Updated!",
                Title = "Updated!",
                UserProfileId = 99,
                DateCreated = DateTime.Today,
                Url = "http://some.url",
            };
            var someOtherVideoId = testVideoId + 1; // make sure they aren't the same

            // Act
            var result = controller.Put(someOtherVideoId, videoToUpdate);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_Method_Updates_A_Video()
        {
            // Arrange
            var testVideoId = 99;
            var videos = CreateTestVideos(5);
            videos[0].Id = testVideoId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            var videoToUpdate = new Video()
            {
                Id = testVideoId,
                Description = "Updated!",
                Title = "Updated!",
                UserProfileId = 99,
                DateCreated = DateTime.Today,
                Url = "http://some.url",
            };

            // Act
            controller.Put(testVideoId, videoToUpdate);

            // Assert
            var videoFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testVideoId);
            Assert.NotNull(videoFromDb);

            Assert.Equal(videoToUpdate.Description, videoFromDb.Description);
            Assert.Equal(videoToUpdate.Title, videoFromDb.Title);
            Assert.Equal(videoToUpdate.UserProfileId, videoFromDb.UserProfileId);
            Assert.Equal(videoToUpdate.DateCreated, videoFromDb.DateCreated);
            Assert.Equal(videoToUpdate.Url, videoFromDb.Url);
        }

        [Fact]
        public void Delete_Method_Removes_A_Video()
        {
            // Arrange
            var testVideoId = 99;
            var videos = CreateTestVideos(5);
            videos[0].Id = testVideoId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act
            controller.Delete(testVideoId);

            // Assert
            var videoFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testVideoId);
            Assert.Null(videoFromDb);
        }

        [Fact]
        public void Get_Returns_All_Videos_From_Search()
        {
            // Arrange:
            // Mocks the environment (pre-conditions necessary) to test method in Act:         
            var videoCount = 20;
            var videos = CreateTestVideos(videoCount);
            var search = "Early";
            var isDesc = false;

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act: Invoke method to retrieve result:
            var result = controller.Search(search, isDesc);

            // Assert: 2 assertions to verify and compare if value of result/date is of type list videos
            // and if they are an identical set:
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualVideos = Assert.IsType<List<Video>>(okResult.Value);

            Assert.Equal(videoCount, actualVideos.Count);
            Assert.Equal(videos, actualVideos);
        }

        [Fact]
        public void Get_Returns_All_Videos_From_Hottest_Date()
        {
            // Arrange:
            // Mocks the environment (pre-conditions necessary) to test method in Act:         
            var videoCount = 20;
            var videos = CreateTestVideos(videoCount);
            var since = "01/02/2020";
            var isDesc = false;

            var repo = new InMemoryVideoRepository(videos);
            var controller = new VideoController(repo);

            // Act: Invoke method to retrieve result:
            var result = controller.HottestVideos(DateTime.Parse(since), isDesc);

            // Assert: 2 assertions to verify and compare if value of result/date is of type list videos
            // and if they are an identical set:
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualVideos = Assert.IsType<List<Video>>(okResult.Value);

            Assert.Equal(videoCount, actualVideos.Count);
            Assert.Equal(videos, actualVideos);
        }




        private List<Video> CreateTestVideos(int count)
        {
            var videos = new List<Video>();
            for (var i = 1; i <= count; i++)
            {
                videos.Add(new Video()
                {
                    Id = i,
                    Description = $"Description {i}",
                    Title = $"Title {i}",
                    Url = $"http://youtube.url/{i}?v=1234",
                    DateCreated = DateTime.Today.AddDays(-i),
                    UserProfileId = i,
                    UserProfile = CreateTestUserProfile(i),
                });
            }
            return videos;
        }

        private UserProfile CreateTestUserProfile(int id)
        {
            return new UserProfile()
            {
                Id = id,
                Name = $"User {id}",
                Email = $"user{id}@example.com",
                DateCreated = DateTime.Today.AddDays(-id),
                ImageUrl = $"http://user.url/{id}",
            };
        }
    }
}


