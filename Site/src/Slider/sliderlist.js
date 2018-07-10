import React from 'react'
import ReactDOM from 'react-dom'
import showImgSource from '../Utils/showImgSource'
import showValue from '../Utils/showValue'
import showArray from '../Utils/showArray'
import mapArray from '../Utils/mapArray'
import isValid from '../Utils/isValid'
import { Slider } from '../Slider/slider'
    
export class SliderList extends React.Component {
    constructor() {
        super()
        this.state = {
            poz: 0,
            animate: 'none',
            slice: null,
            width: 0,
            isNeedToRender: true,
            isNeedToAnimate: true,
            left: '',
            direction: 'none',
        
        }

        this.resizeTimeout = null;
        this.prev = this.prev.bind(this);
        this.next = this.next.bind(this);
        this.setSliderSlice = this.setSliderSlice.bind(this);
        this.resizeSlider = this.resizeSlider.bind(this);
        this.resizeThrottler = this.resizeThrottler.bind(this);
        
        
    } 

    componentDidMount() {
        this.setSliderSlice();
        window.addEventListener('resize', this.resizeThrottler);
        window.addEventListener('DOMContentLoaded',this.setSliderSlice);
    }

    resizeThrottler() { 
        var that = this;           
        if ( !this.resizeTimeout ) {
                this.resizeTimeout = setTimeout(function() {
                    this.resizeTimeout = null;               
                    that.setSliderSlice();                      
            }, 66);
        }
    } 

    componentDidUpdate() {
       this.setSliderSlice();
    }

    shouldComponentUpdate(nextProp, nextState) {
        return this.state.isNeedToRender ||
        nextState.width !== this.state.width ||
        nextState.left !== this.state.left ||
        nextState.animate !== this.state.animate ||
        nextState.direction !== this.state.direction ||
        nextState.slice !== this.state.slice;
    }

    componentWillUnmount() {
        window.removeEventListener('resize', this.resizeThrottler);
        window.removeEventListener('DOMContentLoaded', this.setSliderSlice);
    }

    prev() {
        if(!this.state.isNeedToAnimate) {
            this.setCyclePoz(-1);
            this.setState({direction: 'right'});
            this.setState({isNeedToAnimate: true});  
        }
  
    }

    next() {
        if(!this.state.isNeedToAnimate) {
            this.setCyclePoz(1);
            this.setState({direction: 'left'});
            this.setState({isNeedToAnimate: true});
        }
    }
    setCyclePoz(sumPoz) {
        let index_slice = this.state.poz + sumPoz;
        if(isValid(this.props.slider)) {
            if(index_slice < 0) {
                index_slice = this.props.slider.Slides.length - 1;
            }
            if(index_slice > this.props.slider.Slides.length - 1)
            {
                index_slice = 0;
            }
            this.setState({poz: index_slice});
        }
    }



    resizeSlider() {
        if(isValid(this.props.slider)) {
            let container_width = document.getElementById('slider-container').clientWidth;
            this.setState({isNeedToRender: true});
            this.setState({width: container_width});
        }
    }

    setSliderSlice() {

        if(isValid(this.props.slider)) {

            let index_slice = this.state.poz-1;

            let container_width = document.getElementById('slider-container').clientWidth;
            //console.log('width: ', container_width);
            let offset = 50;
            let legth_of_visible = Math.floor((container_width) / 120);  
            //console.log('length: ', legth_of_visible); 
            let center = (container_width - offset - Math.floor(((legth_of_visible) * 120)))/2 - 120;
            let center_in_px = center + 'px';
           // console.log('left: ', center_in_px); 
            let sliced = [];
            
            for(let i = 0; i < legth_of_visible + 2; i++) {
                if(index_slice < 0) {
                    index_slice = this.props.slider.Slides.length-1;
                }
                if(index_slice > this.props.slider.Slides.length-1)
                {
                    index_slice = 0;
                }

                switch(i) {
                    case 0 : 
                        sliced.push({ action: 'invisible', slide: this.props.slider.Slides[index_slice]});
                        
                    break;
                    case legth_of_visible + 1: 
                        sliced.push({ action: 'invisible', slide: this.props.slider.Slides[index_slice]});
                        
                    break;
                    default: 
                        sliced.push({ action: 'active', slide: this.props.slider.Slides[index_slice]});
                        
                    break;
                }
                
                index_slice++;
            }
           // console.log('index_slice: ', this.state.poz - 1); 
            
            this.setState({left: center_in_px});
            this.setState({isNeedToRender: false});

            if(this.state.isNeedToAnimate) {
                
                this.setState({animate: 'animate'}, () => {
                   
                    setTimeout(() => {
                        this.setState({isNeedToAnimate: false});
                        this.setState({animate: ''});
                        this.setState({direction: 'none'});                                            
                        this.setState({slice: sliced});
                    }, 300);
                });
            }
        }
    }
    

   render() { 

       return (
            <section id={showValue(this.props.slider, 'IdElement')} className="container-fluid"> 
            <div className="youtube-prev" onClick={this.prev}>
                <span className="youtube-prev-icon"> </span> 
            </div>
            <div className="youtube-next" onClick={this.next}>
                <span className="youtube-next-icon"> </span> 
            </div>    
                    <div id="slider-container" className="container slider-brand">                                      
                    {mapArray(this.state.slice, (item, index) => <Slider 
                    direction={this.state.direction} 
                    animate={this.state.animate} 
                    left={this.state.left} 
                    key={item.slide.AltText} 
                    item={item} 
                    poz={this.state.poz+index} />)}
                    </div>
            </section>
        )
    }
}