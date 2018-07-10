import React from 'react'
  
export class Navi extends React.Component {
   render() {
       return (
            <nav id="nav" className="jumbotron-fluid nav-ul">
                <div className="container">
                        <div id="navigation" className="navigation">        
                            {this.props.nav}              
                        </div>
                </div>
            </nav>
            )        
        }
}
