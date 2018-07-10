import React from 'react'
import { Header } from '../Header'
import { SuperPromo } from '../SuperPromo'
import { NewsList } from '../News/newslist'
import showValue from '../Utils/showValue'
import isValid from '../Utils/isValid'
import { PromoList } from '../Promo/promolist'
import { YoutubeList } from '../Youtube/youtubelist'
import { ServiceList } from '../Service/servicelist'
import { CommentList } from '../Comment/commentlist'
import { SliderList } from '../Slider/sliderlist';
import scrollToComponent from 'react-scroll-to-component';
import ScrollAnimation from 'react-animate-on-scroll';
import { Navi } from '../Navi';
import {Footer} from '../Footer'

var resizeTimeout = null;

export class Main extends React.Component {
    constructor()  {
        super()
        this.state = {
            header: null,
            logo: null,
            mainpromo: null,
            mainpromo_height: 0,
            news: null,
            promo: null,            
            slider: null,
            service: null,
            youtube: null,
            comment: null,
            youtube_poz: 0,
            youtube_slice: null,
            youtube_animate: "",
            
        }
        this.getData("Data/Json/header.json", (json) => { this.setState({header: json}); })
        this.getData("Data/Json/logo.json", (json) => { this.setState({logo: json}); })
        this.getData("Data/Json/news.json", (json) => { this.setState({news: json}); })
        this.getData("Data/Json/mainpromo.json", (json) => { this.setState({mainpromo: json}); })
        this.getData("Data/Json/promo.json", (json) => { this.setState({promo: json}); })
        this.getData("Data/Json/slider.json", (json) => { this.setState({slider: json}); })
        this.getData("Data/Json/service.json", (json) => { this.setState({service: json}); })
        this.getData("Data/Json/comment.json", (json) => { this.setState({comment: json}); })
        this.getData("Data/Json/youtube.json", (json) => {         
            this.setState({youtube: json});
            this.setState({youtube_poz: 0});
            this.setStateOfYoutube(0);
        })
        
        
        this.next = this.next.bind(this);
        this.prev = this.prev.bind(this);
        this.resizeOfWindow = this.resizeOfWindow.bind(this);
        this.onScroll = this.onScroll.bind(this);
        this.resizeThrottler = this.resizeThrottler.bind(this);
        this.setResizeTimeout = this.setResizeTimeout.bind(this);
        this.getResizeTimeout = this.getResizeTimeout.bind(this);
    }

    getResizeTimeout() {
        return this.state.resizeTimeout;
    }
    setResizeTimeout(property) {
        this.setState({
            resizeTimeout: property
        });
    }


    resizeThrottler() {
            var that = this;            
            if ( !resizeTimeout ) {
                resizeTimeout = setTimeout(function() {
                    resizeTimeout = null;                   
                    that.resizeOfWindow(); 
                    that.onScroll();       
                }, 66);
            }
    } 

      componentDidMount() {
 
        window.addEventListener('load',this.resizeOfWindow, false);
        window.addEventListener('resize', this.resizeThrottler, false);       
        window.addEventListener('scroll', this.onScroll, false);   
      }
      componentWillUnmount() {

        window.removeEventListener('load',this.resizeOfWindow, false);
        window.removeEventListener('resize', this.resizeThrottler, false);
        window.removeEventListener('scroll', this.onScroll, false);
       // document.removeEventListener('DOMContentLoaded',this.onScroll);
      }

    onScroll() {
       //var scrolled = window.pageYOffset || document.documentElement.scrollTop;
        var els = document.getElementsByClassName("bg-parallax");
        

        [].forEach.call(els, function(el) {
            let containerHeight = el.parentNode.clientHeight > 0 ? el.parentNode.clientHeight : 625;           
            let imgHeight = el.clientHeight == 0 ? 1000 : el.clientHeight;            
            let parallaxDist = imgHeight - containerHeight;                 
            let top = el.offsetTop;
            let scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            let windowHeight = window.innerHeight;
            let windowBottom = scrollTop + windowHeight;
            let percentScrolled = (windowBottom - top) / (containerHeight + windowHeight);
            let parallax = parallaxDist * percentScrolled;

            el.style.transform = 'translate3D(-50%, ' + parallax + 'px, 0)';
               //console.log('translate3D(-50%, ' + parallax + 'px, 0)'); 
            });

        this.fixNavOnScroll();
    }

    fixNavOnScroll() {
        if(isValid(document.getElementById("nav"))) {
            var scrolled = window.pageYOffset || document.documentElement.scrollTop;
            let intHeaderHeight = showValue(document.getElementById("header"), 'clientHeight');
            let intNavHeight = showValue(document.getElementById("nav"), 'clientHeight');
            let element = document.getElementById("nav");
            if( scrolled >  intHeaderHeight + intNavHeight) {
                element.classList.add("fixed-top");
                document.body.style.marginTop = intNavHeight + 'px';
            } else {
                element.classList.remove("fixed-top");
                document.body.style.marginTop = 0 + 'px';
            }
            
        }

    }

    resizeOfWindow() {
        let els = document.getElementsByClassName("bg-parallax");
        //console.log(els);
 


        let intHeaderHeight = showValue(document.getElementById("header"), 'clientHeight');
        let intNavHeight = showValue(document.getElementById("nav"), 'clientHeight');
        let screenHeight = window.innerHeight;
        let heightToBottomPage = screenHeight - intNavHeight - intHeaderHeight;
        let content = document.getElementsByClassName("mainpromo-header-height");
        let heightOfContent = showValue(content[0], 'clientHeight');
        let targetHeight = heightToBottomPage >= heightOfContent ? heightToBottomPage : heightOfContent+50;

        [].forEach.call(els, function(el) {
            let parentWidth = el.parentNode.clientWidth > 0 ? el.parentNode.clientWidth : 1000;           
            let imgWidth = parentWidth;
            el.style.width = imgWidth+'px'; 
            if(imgWidth< 560) {
                el.style.bottom = '150vh';
            } else {
                el.style.bottom = '50vh';    
            }
        });

        this.setState({mainpromo_height: targetHeight}, () => this.onScroll());

       
    }

    next()
    {
        this.setStateOfYoutube(this.state.youtube_poz + 1);
        console.log('next '+ this.state.youtube_poz);
    }

    prev() {
        this.setStateOfYoutube(this.state.youtube_poz - 1);
        console.log('prev ' + this.state.youtube_poz);
    }

    setStateOfYoutube(pozition) {
        if (isValid(this.state.youtube)) {
            var slides = this.state.youtube.Slides;
            var length = this.state.youtube.Slides.length;
            var poz = pozition;
            var end = 2;
            if(poz < 0) {
                poz = Math.floor(length/2);
            }
            if(poz * 2 >= length) {
                poz = 0;
            }
            /*setTimeout(() => , 1000);*/
            this.setState({youtube_animate: 'zoomOut'}, () => {
                setTimeout(() => {
                    this.setState({youtube_poz: poz});
                    this.setState({youtube_slice: slides.slice(poz*2, poz*2 + 2)}); 
                    this.setState({youtube_animate: 'zoomIn'}, () => {
                        setTimeout(() => {
                        this.setState({youtube_animate: ''});
                        }, 300);
                    })
                }, 300);
            });
   
        }
    }

    getData(url, state) {
        fetch(url).then((response) => response.json()).then(state)
        .catch((error) => console.log(error));
    }


    render() {


        let navH = showValue(document.getElementById("nav"), 'clientHeight');
        let headH = showValue(document.getElementById("header"), 'clientHeight');

        let nav = (
            <ul>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.Header, { offset:-(headH + navH), align: 'top', duration: 700})}>
                        {showValue(this.state.header, 'Header')}
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.SuperPromo, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.mainpromo, 'Header')}
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.PromoList, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.promo, 'Header')} 
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.ServiceList, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.service, 'Header')} 
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.NewsList, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.news, 'Header')} 
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.YoutubeList, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.youtube, 'Header')} 
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.CommentList, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.comment, 'Header')} 
                    </div> 
                </li>
                <li className="nav-li"> 
                    <div onClick={() => scrollToComponent(this.SliderList, { offset: -navH, align: 'top', duration: 700})}>
                        {showValue(this.state.slider, 'Header')} 
                    </div> 
                </li>
                </ul> );



        return (<div className="main-parallax">             
                    <Header ref={(section) => { this.Header = section; }} header={this.state.header} />
                    <Navi nav={nav} />
                    <SuperPromo ref={(section) => { this.SuperPromo = section; }} header={this.state.header} mainpromo={this.state.mainpromo} height={this.state.mainpromo_height}  />
                    <ScrollAnimation animateIn="fadeIn" duration="0.5">
                        <PromoList ref={(section) => { this.PromoList = section; }} promo={this.state.promo} />
                    </ScrollAnimation>
                    <ScrollAnimation animateIn="fadeIn" duration="0.5">
                    <ServiceList ref={(section) => { this.ServiceList = section; }} service={this.state.service} />
                    </ScrollAnimation>
                    <ScrollAnimation animateIn="fadeIn" duration="0.5">
                    <NewsList ref={(section) => { this.NewsList = section; }} news={this.state.news} />
                    </ScrollAnimation>
                    <ScrollAnimation animateIn="fadeIn" duration="0.5">
                    <YoutubeList 
                        ref={(section) => { this.YoutubeList = section; }}
                        prev={this.prev}
                        next={this.next}
                        animate={this.state.youtube_animate} 
                        slides={this.state.youtube_slice} 
                        youtube={this.state.youtube} />
                        </ScrollAnimation>
                        <ScrollAnimation animateIn="fadeIn" duration="0.5">
                    <CommentList ref={(section) => { this.CommentList = section; }} comment={this.state.comment} />
                    </ScrollAnimation>
                    <ScrollAnimation animateIn="fadeIn" duration="0.5">
                    <SliderList  ref={(section) => { this.SliderList = section; }} slider={this.state.slider} />
                    </ScrollAnimation>
                    <Footer header={this.state.header} nav={nav} />
             </div>)
    }
}