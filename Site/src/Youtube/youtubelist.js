import React from 'react'
import showValue from '../Utils/showValue'
import showArray from '../Utils/showArray'
import mapArray from '../Utils/mapArray'
import { Youtube } from '../Youtube/youtube'
    
export class YoutubeList extends React.Component {

   render() {
       return (
        <section id="youtube" 
            style={{backgroundImage: 'url(Data/Img/bg-one.png)'}}
            className="container-fluid">

            <div className="youtube-prev" onClick={this.props.prev}>
                <span className="youtube-prev-icon"> </span> 
            </div>
            <div className="youtube-next" onClick={this.props.next}>
                <span className="youtube-next-icon"> </span> 
            </div>
            <div className="container">          
                <h2 className="text-center">{showValue(this.props.youtube,'Header')}</h2>
                <div className="row mt-4">
                    { mapArray(this.props.slides, (item, index) => <Youtube animate={this.props.animate} key={index} item={item} />) }
                </div>
            </div>
        </section>
        )
    }
}