import React from "react";
import { Link } from "react-router-dom";
import { logout } from "../modules/authManager";

const Header = ({ isLoggedIn }) => {
    // using the <Link> component import from the react router instead of anchor tags for navigation
    // and use the "to" attribute to specify where we want the link to take the user to:
  return (
    <nav className="navbar navbar-expand navbar-dark bg-info">

      {isLoggedIn &&
        <>
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
          <Link to="/login">
            <a aria-current="page" className="nav-link"
              style={{ cursor: "pointer" }} onClick={logout}>Logout</a>
          </Link>
        </>
      }

      {!isLoggedIn &&
        <>
            <Link to="/login">
              Login
            </Link>
        
            <Link to="/register">
              Register
            </Link>
        </>
      }

      {/* <Link to="/" className="navbar-brand">
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
      </ul> */}
    </nav>
  );
};

export default Header;
