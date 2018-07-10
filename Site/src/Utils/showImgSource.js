export default function showImgSource(obj) {
    return (obj && obj !== 'null' && obj !== 'undefined') ? "Data/Img/" + obj['ImgSource'].replace('\\','/') : ''; 
}