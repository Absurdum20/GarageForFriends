import React from 'react'
import showImgSource from '../Utils/showImgSource';
import showValue from '../Utils/showValue';
import showArray from '../Utils/showArray';
import { News } from '../News/news'
    
export class NewsList extends React.Component {

   render() {
       return (
            <section id={showValue(this.props.news, 'IdElement')} className="container">
                
                <h2 className="text-center">{showValue(this.props.news,'Header')}</h2>
              
                <div className="row mt-4">
                    {showArray(this.props.news, 'Slides', (onenews, index) => <News key={index} onenews={onenews} />)}
                </div>
            </section>
        )
    }
}