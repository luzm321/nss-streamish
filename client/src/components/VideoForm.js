import React, {useState} from "react";
import { useHistory } from "react-router-dom";
import { Button, Form, FormGroup, Label, Input, FormText } from 'reactstrap';
import { addVideo } from "../modules/videoManager";

const VideoForm = ({getVideosWithComments}) => {

    const [videoToAdd, setVideoToAdd] = useState({
        title: "",
        url: "",
        description: ""
    });

    const history = useHistory();

    // method that handles adding a new video by invoking
    const handleAddNewVideo = () => {
        addVideo(videoToAdd).then(() => {
            // getVideosWithComments()
            // resetting state back to initial state to clear input for fields:
            setVideoToAdd({
                title: "",
                url: "",
                description: ""
            });
            history.push("/");
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
                <br/><Label>Title:</Label><br/><br/>
                <input id="title" type="text" defaultValue={videoToAdd.title} value={videoToAdd.title} onChange={(event) => {handleInputChange(event)}} placeholder="video title..." required />
                <br/><br/><Label>Url:</Label><br/><br/>
                <input id="url" type="text" defaultValue={videoToAdd.url} value={videoToAdd.url} onChange={(event) => {handleInputChange(event)}} placeholder="video url..." required />
                <br/><br/><Label>Description (optional):</Label><br/><br/>
                <input id="description" type="text" defaultValue={videoToAdd.description} value={videoToAdd.description} onChange={(event) => {handleInputChange(event)}} placeholder="video description..." />
                {/* <br/><br/><input type="button" value ="Add New Video" onClick={() => {handleAddNewVideo()}}/> */}
                <br/><br/><Button className="btn btn-primary" value ="Add New Video" onClick={() => {handleAddNewVideo()}}>Add New Video</Button><br/><br/>
            </div>
        </div>
    );
};

export default VideoForm;
