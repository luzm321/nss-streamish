import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import "./App.css";
import ApplicationViews from "./components/ApplicationViews";

function App() {
  return (
    <div className="App">
      <Router>
        <ApplicationViews />
      </Router>
    </div>
  );
}

export default App;

// import VideoList from "./components/VideoList";

// function App() {
//   return (
//     <div className="App">
//       <VideoList />
//     </div>
//   );
// }

