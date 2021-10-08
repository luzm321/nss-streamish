import React from "react";
import { Switch, Route } from "react-router-dom";
import VideoList from "./VideoList";
import VideoForm from "./VideoForm";
import VideoDetails from "./VideoDetails";

const ApplicationViews = () => {
  return (
      // The Switch component is going to look at the url and render the first route that is a match:
      // The exact attribute specifies that we only want to render this component then the url is exactly /
      // If a url matches the value of the path attribute, the children of that <Route> will be what gets rendered.
    <Switch>
      <Route path="/" exact>
        <VideoList />
      </Route>

      <Route path="/videos/add">
        <VideoForm />
      </Route>

      {/* Route below is an example of a path with a route param: /videos/:id. Using the colon, we can tell the react router that this will be some id parameter. */}
      <Route path="/videos/:id">
        <VideoDetails />
      </Route>
    </Switch>
  );
};

export default ApplicationViews;
