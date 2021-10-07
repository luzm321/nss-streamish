import React from "react";
import { Card, CardBody } from "reactstrap";

const Video = ({ video }) => {
  return (
    <Card >
      <p className="text-left px-2"><strong>Posted by:</strong> {video.userProfile.name}</p>
      <CardBody>
        <iframe className="video"
          src={video.url}
          title="YouTube video player"
          frameBorder="0"
          allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
          allowFullScreen />

        <p><strong>Video Title:</strong> {video.title}</p>
        <p><strong>Description:</strong> {video.description}</p>
        <strong>Comments:</strong>
        {/* {video.comments.map(comment => {
             return <p>Commented by {comment.userProfile.name}: {comment.message}</p>
        })} */}
        {
          video.comments.length !== 0 ?
            video.comments.map(comment => {
                return <p>Commented by {comment.userProfile.name}: {comment.message}</p>}) 
            : 
                <p>N/A</p>
        }


      </CardBody>
    </Card>
  );
};

export default Video;
