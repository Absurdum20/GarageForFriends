import React from 'react'
import showValue from '../Utils/showValue';
import showArray from '../Utils/showArray';
import { Comment } from '../Comment/comment'
    
export class CommentList extends React.Component {

   render() {
       return (
            <section id={showValue(this.props.comment, 'IdElement')} className="container">
                
                <h2 className="text-center">{showValue(this.props.comment,'Header')}</h2>
              
                <div className="row mt-4">
                    {showArray(this.props.comment, 'Slides', (item, index) => <Comment key={index} item={item} />)}
                </div>
            </section>
        )
    }
}