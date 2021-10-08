import React, { useEffect, useState } from "react";
import { ListGroup, ListGroupItem } from "reactstrap";
import { useParams } from "react-router-dom";
import Video from "./Video";
import { getVideo } from "../modules/videoManager";

const VideoDetails = () => {
  const [video, setVideo] = useState();
  const { id } = useParams();

  useEffect(() => {
    getVideo(id).then(setVideo);
  }, []);

  if (!video) {
    return null;
  }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-sm-12 col-lg-6">
          <Video video={video} />
          <ListGroup>
            {/* {
              console.log('video from VideoDetails', video)
            } */}
            <p><strong>Description:</strong> {video.description}</p>
            <strong>Comments:</strong>
            {/* {video.comments.map((c) => (
              <ListGroupItem>Commented by {c.userProfile.name}: {c.message}</ListGroupItem>
            ))} */}
            {
            video.comments.length !== 0 ?
                video.comments.map(comment => {
                    return <ListGroupItem>Commented by {comment.userProfile.name}: {comment.message}</ListGroupItem>}) 
                : 
                    <p>No Comments Posted</p>
            }
          </ListGroup>
        </div>
      </div>
    </div>
  );
};

export default VideoDetails;
