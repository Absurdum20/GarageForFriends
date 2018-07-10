import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
    
export class Promo extends React.Component {

   render() {
       return (
            
            <div className="col-lg-4 col-md-4 col-sm-4">
                <div className="card box-shadow">                   
                    <img className="card-img-top w-100"                    
                    src={showImgSource(this.props.onepromo)} />
                     <div className="card-body">
                     <h3 className="card-header">{showValue(this.props.onepromo, 'HeaderText')}</h3>
                    <p className="card-text pl-2">{showValue(this.props.onepromo, 'RegularText')}</p>
                    </div>
                </div>
            </div>
          
        )
    }
}