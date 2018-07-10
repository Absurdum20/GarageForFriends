import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
    
export class Slider extends React.Component {

   render() {
       return (           
               // <div className="slider-brand-item active">
                //    {this.props.poz}
               // </div>
                    <img className={"slider-brand-item " + (this.props.animate === 'animate' ? 'active' : this.props.item.action) + ' ' + this.props.animate + ' ' + this.props.direction}                   
                    style={{ left: this.props.left }}                   
                    src={showImgSource(this.props.item.slide)} /> 
                                
        )
    }
}