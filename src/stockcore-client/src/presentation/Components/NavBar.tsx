import React from 'react';
import { Link } from 'react-router-dom';
import './NavBar.css';

const NavBar = () =>{


    return(
        <nav className="sidebar">
            <div className = "logo">
                <h1>EParafia</h1>
            </div>
            <ul>
                <li>
                    <Link to="/">Strona główna</Link>
                </li>
                <li> 
                <Link to="/login">Not implemented</Link>
                </li>
                <li> 
                <Link to="/register">Not implemented</Link>
                </li>
                <li> 
                <Link to="/dashboard">Not implemented</Link>
                </li>
                <li> 
                                    <Link to="/dashboard">Not implemented</Link>

                </li>


            </ul>
        </nav>


    );
};
export default NavBar;