import React from 'react'
import showValue from '../Utils/showValue';
import showImgSource from '../Utils/showImgSource';

    
export class Header extends React.Component {


   render() {
       return (
                                     
            <section id={showValue(this.props.header, 'IdElement')} className="container border-bottom box-shadow">
                <div className="row justify-content-around">
                    <div className="col-lg-2 col-md-3 col-sm-3">
                        <img className="img-responsive rounded-circle center-block" src={showImgSource(this.props.header)} width={showValue(this.props.header, 'Width')} height={showValue(this.props.header, 'Height')} alt={showValue(this.props.header, 'AltText')} />
                    </div>
          
                    <div className="col-lg-5 col-md-9 col-sm-9 text-center header-text-vertical-center">                                              
                        <h2 className="algeria-one">{showValue(this.props.header, 'Header')}</h2>                        
                    </div>
                    <div className="col-lg-3 col-md-6 col-sm-6 text-center">
                    <p className="mt-2"> <img className="img-responsive" src={'Data/Img/feniks_logo_rus.jpg'} height={30} width={50} />{showValue(this.props.header, 'Phone')}</p>
                        <p> <img className="img-responsive" src={'Data/Img/vodafone_logo.png'} height={30} width={50} />{showValue(this.props.header, 'AltPhone')}</p>
                        {/*<p>{showValue(this.props.header, 'Adress')}</p>*/}
                        
                    </div>
                    <div className="col-lg-2 col-md-6 col-sm-6 text-center">
                    <ul className="soc"><li>
                        <a href={showValue(this.props.header, 'YouTubeHref')}>
                            <div className="btn btn-danger">Youtube</div>
                        </a>
                        </li>
                        <li>
                        <a href={showValue(this.props.header, 'VKHref')}>
                            <div className="btn btn-primary">Вконтакте</div>
                        </a>
                        </li>
                    </ul>
                    </div>
          
                </div>
            </section>   
            )        
        }
}
