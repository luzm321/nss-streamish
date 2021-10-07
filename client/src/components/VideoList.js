import React, { useEffect, useState } from "react";
import Video from './Video';
import { getAllVideosWithComments } from "../modules/videoManager";

const VideoList = () => {
  const [videos, setVideos] = useState([]);

  const getVideosWithComments = () => {
    getAllVideosWithComments().then(videos => setVideos(videos));
  };

  useEffect(() => {
    getVideosWithComments();
  }, []);

  return (
    <div className="container">
      <div className="row justify-content-center">
        {videos.map((video) => (
          <Video video={video} key={video.id} />
        ))}
      </div>
    </div>
  );
};

export default VideoList;

