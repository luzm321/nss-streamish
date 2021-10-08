//Component's responsibility is to capture the text from the user. As the user types, the searchTerms state variable must immediately be 
//updated in the parent component:


import React from "react";
import { searchVideos } from "../modules/videoManager";


const VideoSearch = ({setVideos}) => {

    return (
        <div className="searchDiv">
            <div className="searchBar">
                <div className="search"><strong>Search Video:</strong>
                    <img className="searchIcon" src="https://img.icons8.com/ultraviolet/48/4a90e2/search--v1.png" alt="search icon"/>
                </div>
                <input type="text"
                    className="input--wide"
                    onKeyUp={(event) => {
                        // setVideos(event.target.value)
                        console.log('is it triggering?', event.target.value);
                        searchVideos(event.target.value).then((videoResults) => {
                            console.log('video results', videoResults);
                            setVideos(videoResults);
                        });
                    }}
                    placeholder="Search for a video..." />
            </div>
        </div>
    )
};

export default VideoSearch;