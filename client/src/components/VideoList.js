import React, { useEffect, useState } from "react";
import Video from './Video';
import VideoSearch from "./VideoSearch";
import VideoForm from "./VideoForm";
import { getAllVideosWithComments} from "../modules/videoManager";

const VideoList = () => {
    // Array destructuring initializes variables and useState() hook returns an array of 2 things: the initial value of the state variable
    // that is set by what passed to the hook and a function/method that updates that state/variable
  const [videos, setVideos] = useState([]);



  const getVideosWithComments = () => {
    getAllVideosWithComments().then(videos => setVideos(videos));
  };

  useEffect(() => {
    getVideosWithComments();
  }, []);


  return (
    <div>
        <br/>
        <div className="container">
            <div className="row justify-content-center">
                <VideoSearch setVideos={setVideos}/>
            </div>
        </div>
        <br/>
        <div className="container">
            <div className="row justify-content-center">
                <VideoForm getVideosWithComments={getVideosWithComments}/>
            </div>
        </div>
        <br/>
        <div className="container">
            <div className="row justify-content-center">
                {console.log(videos)}
                {videos.map((video) => (
                    <Video video={video} key={video.id} />
                ))}
            </div>
        </div>
    </div>
  );
};

export default VideoList;

