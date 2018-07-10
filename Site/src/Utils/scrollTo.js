import animateScroll from '../Utils/animateScroll'

export default function scrollTo(el) {
    console.log(el);
    let link = el.getAttribute('href').replace('#', ''),
        anchor = document.getElementById(el);

        let location = 0;
    if (anchor.offsetParent) {
        do {
            location += anchor.offsetTop;
            anchor = anchor.offsetParent;
        } while (anchor);
    }
    location = location >= 0 ? location : 0;

    animateScroll(location);
    return false;
}