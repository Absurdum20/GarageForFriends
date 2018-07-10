import easeInOutQuad from '../Utils/easeInOutQuad'

export default function animateScroll(pos) {
        document.documentElement.scrollTop = 1;
        let element = (document.documentElement && document.documentElement.scrollTop) ? document.documentElement : document.body,
            start = element.scrollTop,
            change = pos - start,
            currentTime = 0,
            increment = 20,
            duration = 300;

        let animateScroll = function(){        
            currentTime += increment;
            let val = easeInOutQuad(currentTime, start, change, duration);
            element.scrollTop = val;
            if(currentTime < duration) {
                setTimeout(animateScroll, increment);
            }
        };
        animateScroll();
}