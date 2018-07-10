import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
    
export class News extends React.Component {

   render() {
       return (
            
            <div className="col-lg-3 col-md-4 col-sm-6">
                <div className="card box-shadow">
                    <h4 className="card-header">{showValue(this.props.onenews, 'HeaderText')}</h4>
                    <img className="card-img-top w-100"                    
                    src={showImgSource(this.props.onenews)} />
                     <div className="card-body">
                    <p className="card-text pl-2">{showValue(this.props.onenews, 'RegularText')}</p>
                    </div>
                </div>
            </div>
          
        )
    }
}