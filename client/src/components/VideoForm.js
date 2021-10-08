import React, {useState} from "react";
// import { Button } from "reactstrap";
import { addVideo } from "../modules/videoManager";

const VideoForm = ({getVideosWithComments}) => {

    const [videoToAdd, setVideoToAdd] = useState({
        title: "",
        url: "",
        description: ""
    });

    // method that handles adding a new video by invoking
    const handleAddNewVideo = () => {
        addVideo(videoToAdd).then(() => {
            getVideosWithComments()
            // resetting state back to initial state to clear input for fields:
            setVideoToAdd({
                title: "",
                url: "",
                description: ""
            });
        });
    };

    // method that stores user input:
    const handleInputChange = (event) => {
        // making a copy of state:
        const newVideo = {...videoToAdd}
        // dynamically creating properties and setting the values of user input in form:
        newVideo[event.target.id] = event.target.value 
        setVideoToAdd(newVideo);
    };

    return (
        <div className="row videoFormContainer">
            <h3>~Add A New Video~</h3>
            <div className="videoForm">
                <label>Title:</label><br/>
                <input id="title" type="text" defaultValue={videoToAdd.title} value={videoToAdd.title} onChange={(event) => {handleInputChange(event)}} placeholder="video title..." required />
                <br/><label>Url:</label><br/>
                <input id="url" type="text" defaultValue={videoToAdd.url} value={videoToAdd.url} onChange={(event) => {handleInputChange(event)}} placeholder="video url..." required />
                <br/><label>Description (optional):</label><br/>
                <input id="description" type="text" defaultValue={videoToAdd.description} value={videoToAdd.description} onChange={(event) => {handleInputChange(event)}} placeholder="video description..." />
                <br/><br/><input type="button" value ="Add New Video" onClick={() => {handleAddNewVideo()}}/>
                {/* <br/><br/><Button value ="Add New Video" onClick={() => {handleAddNewVideo()}}/> */}
            </div>
        </div>
    );
};

export default VideoForm;
