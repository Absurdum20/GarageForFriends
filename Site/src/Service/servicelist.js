import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
import showArray from '../Utils/showArray';
import { Service } from '../Service/service'
    
export class ServiceList extends React.Component {

   render() {
       return (
        <section id={showValue(this.props.service,'IdElement')} 
        style={{backgroundImage: 'url(Data/Img/bg-two.png)'}}
        className="container-fluid">  
            <div className="container">               
                <h2 className="text-center">{showValue(this.props.service,'Header')}</h2>             
                <div className="row mt-4">
                    {showArray(this.props.service, 'Slides', (item, index) => <Service key={index} item={item} />)}
                </div>
            </div>
        </section> 
        )
    }
}