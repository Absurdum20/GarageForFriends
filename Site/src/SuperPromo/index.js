import React from 'react'
import showValue from '../Utils/showValue'
import showImgSource from '../Utils/showImgSource';

window.onscroll = function() {
                    
      }

export class SuperPromo extends React.Component {
 
   render() {
       return (     
                              
        <section id="mainpromo" 
        style={{height: this.props.height}} 
        className="jumbotron jumbotron-fluid parallax">   
                <div className="bg-parallax" style={{backgroundImage: 'url(' + showImgSource(this.props.mainpromo) + ')'}}></div>
                <div className="container mainpromo-header-height animated fadeIn">            
                <div id="mainpromo-header" className="row">
                        <div className="col-md-12"> 
                                <h1 className="display-4 text-center">{showValue(this.props.mainpromo, 'HeaderText')}</h1>
                        </div>
                
                </div> 
                        <div className="row align-items-end row-to-bottom">
                        <div className="col-lg-8 col-md-8 col-sm-6">  
                        <h5>{showValue(this.props.mainpromo, 'RegularText')}</h5>
                </div>  
                        <div className="col-lg-4 col-md-4 col-sm-6 text-center"><a className="btn btn-primary btn-lg" href="#" role="button">
                <h5>Звоните сейчас: </h5>
                <h5><i className='material-icons'>phone</i>{showValue(this.props.header, 'Phone')}</h5>
                <h5><i className='material-icons'>phone</i>{showValue(this.props.header, 'AltPhone')}</h5>
                </a></div>
                </div> 
                </div>  
               
        </section>
        
        ) 
        }
}