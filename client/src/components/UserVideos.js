import React, { useEffect, useState } from "react";
import { ListGroup, ListGroupItem } from "reactstrap";
import { useParams } from "react-router-dom";
import Video from "./Video";
import { getVideosByUser } from "../modules/videoManager";

const UserVideos = () => {
  const [videoList, setVideoList] = useState([]);
  const { id } = useParams();

  useEffect(() => {
    // console.log('is this triggering');
    getVideosByUser(parseInt(id)).then(setVideoList);
    // getVideosByUser(parseInt(id)).then((data) => console.log("data", data));

  }, []);

//   const checkState = () => {
//       console.log('state', videoList);
//   }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-sm-12 col-lg-6">
          {/* <button onClick={() => {checkState()}}>check state</button> */}
          <br/><h1>Videos</h1><br/>
          <ListGroup>
            {
                videoList.map(video => {
                    return  <>          
                        <Video video={video} />
                        <p><strong>Description:</strong> {video.description}</p>
                        <strong>Comments:</strong>
                        {
                            video.comments.length !== 0 ?
                                video.comments.map(comment => {
                                    return <ListGroupItem>Commented by {comment.userProfile.name}: {comment.message}</ListGroupItem>}) 
                                : 
                                    <p>No Comments Posted</p>
                        }
                    </>
                })
            }                 
          </ListGroup>
        </div>
      </div>
    </div>
  );
};

export default UserVideos;