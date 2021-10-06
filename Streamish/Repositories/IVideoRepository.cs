using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Streamish.Models;

namespace Streamish.Repositories
{
    public interface IVideoRepository
    {
        List<Video> GetAll();
        List<Video> GetAllWithComments();
        Video GetById(int id);
        Video GetVideoByIdWithComments(int id);
        void Add(Video video);
        void Update(Video video);
        void Delete(int id);
        List<Video> Search(string criterion, bool sortDescending);
        List<Video> GetHottestVideos(DateTime since, bool sortDescending);
    }
}
