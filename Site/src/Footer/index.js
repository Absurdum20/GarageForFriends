import React from 'react'
import showValue from '../Utils/showValue'
import showImgSource from '../Utils/showImgSource'
  
export class Footer extends React.Component {
   render() {
       return (
        <footer className="page-footer font-small mdb-color">
        <div className="container text-center text-md-left"> 
            <div className="row text-center pt-4">   
            <div className="col-md-3 col-lg-3 col-xl-3 mx-auto mt-3 footer-navi">
                <h6 className="text-center text-uppercase mb-4 font-weight-bold">Навигация</h6>                        
                {this.props.nav} 
            </div>    
            <hr className="w-100 clearfix d-md-none" />   
            <div className="col-md-6 col-lg-6 col-xl-6 mx-auto mt-3">
                <h6 className="text-center text-uppercase mb-4 font-weight-bold">Карта проезда</h6>
                <iframe src={showValue(this.props.header, 'MapHref')} width="100%" height="320" frameBorder="0">
                </iframe>
            </div>     
            <hr className="w-100 clearfix d-md-none" />
            <div className="col-md-3 col-lg-3 col-xl-3 mx-auto mt-3">
                <h6 className="text-center text-uppercase mb-4 font-weight-bold">Контакты</h6>
                <p><i className="material-icons">home</i>{showValue(this.props.header, 'Adress')}</p>
                <p><i className="material-icons">email</i>{showValue(this.props.header, 'Email')}</p>
                <p><i className="material-icons">phone</i>{showValue(this.props.header, 'Phone')}</p>
                <p><i className="material-icons">phone</i>{showValue(this.props.header, 'AltPhone')}</p>
                <h6 className="text-center text-uppercase pt-2 mb-4 font-weight-bold">Социальные сети</h6>
                <ul className="soc">
                    <li>
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
            <hr />
        </div>
        <div className="container-fluid">
        <div className="row d-flex align-items-center darker-bg">
            <div className="col-md-12 col-lg-12">
                        <img className="rounded-circle center-block mb-2" src={showImgSource(this.props.header)} alt="" width="50" height="50" />
                        <small className="text-center d-block mb-3 text-muted">© 2018</small>
            </div>
            </div>
        </div>
        </footer>
            )        
        }
}