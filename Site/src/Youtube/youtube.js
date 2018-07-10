import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
    
export class Youtube extends React.Component {

   render() {
       return (
            
            <div className="col-lg-6 col-md-6 col-sm-6">
                <div className={"card box-shadow opacity animated " + this.props.animate} > 
                <div className="embed-responsive embed-responsive-16by9">
                    <iframe 
                    className="embed-responsive-item" 
                    src={"https://www.youtube.com/embed/" + showValue(this.props.item, 'HrefToGoogle') + '?ps=docs&controls=1'} 
                    //title={showValue(this.props.item, 'HeaderText')}
                    frameBorder="0" 
                    allowFullScreen></iframe>
                </div>                  
                     <div className="card-body">
                     <h3 className="card-header">{showValue(this.props.item, 'HeaderText')}</h3>
                    <p className="card-text pl-2">{showValue(this.props.item, 'RegularText')}</p>
                    </div>
                </div>
            </div>
          
        )
    }
}