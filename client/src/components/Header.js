import React from "react";
import { Link } from "react-router-dom";

const Header = () => {
    // using the <Link> component import from the react router instead of anchor tags for navigation
    // and use the "to" attribute to specify where we want the link to take the user to:
  return (
    <nav className="navbar navbar-expand navbar-dark bg-info">
      <Link to="/" className="navbar-brand">
        StreamISH
      </Link>
      <ul className="navbar-nav mr-auto">
        <li className="nav-item">
          <Link to="/" className="nav-link">
            Feed
          </Link>
        </li>
        <li className="nav-item">
          <Link to="/videos/add" className="nav-link">
            New Video
          </Link>
        </li>
      </ul>
    </nav>
  );
};

export default Header;
