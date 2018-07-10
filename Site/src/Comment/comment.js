import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
    
export class Comment extends React.Component {

   render() {
       return (
        <div className="col-lg-6 col-md-6 col-sm-12">
            <div className="card box-shadow">
                <div className="media">
                    <img className="media-left" width="150"
                        alt={showValue(this.props.item, 'AltText')} 
                        src={showImgSource(this.props.item)} />
                    <div className="media-body">
                        <h4 className="card-header">{showValue(this.props.item, 'HeaderText')}, написал:</h4>                       
                        <div className="card-body">
                            <p className="card-text pl-3">
                                <em>
                                    "{showValue(this.props.item, 'RegularText')}"    
                                    </em>                
                            </p>  
                        </div>
                     
                    </div>
                </div>
            </div>
        </div>         
        )
    }
}