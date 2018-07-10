import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
import showArray from '../Utils/showArray';
import { Promo } from '../Promo/promo'
    
export class PromoList extends React.Component {

   render() {
       return (
            <section id={showValue(this.props.promo, 'IdElement')} className="container pt-4">                                           
                <div className="row">
                    {showArray(this.props.promo, 'Slides', (promo, index) => <Promo key={index} onepromo={promo} />)}
                </div>
            </section>
        )
    }
}