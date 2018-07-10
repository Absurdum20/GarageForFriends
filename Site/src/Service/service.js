import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
    
export class Service extends React.Component {

   render() {
       return (
            
            <div className="col-lg-3 col-md-4 col-sm-6">
                <div className="card box-shadow service-card-hover">
                    <h4 className="card-header">{showValue(this.props.item, 'HeaderText')}</h4>
                    <div className="service-card">
                        <img className="card-img-top w-100"                    
                        src={showImgSource(this.props.item)} />
                     </div>
                     <div className="card-body">
                        <p className="card-text pl-2">{showValue(this.props.item, 'RegularText')}</p>
                    </div>
                    <div className="card-footer">
                        <h5 className="price card-text pl-2">От {showValue(this.props.item, 'Price')} руб.</h5>
                     </div>
                </div>
            </div>
          
        )
    }
}