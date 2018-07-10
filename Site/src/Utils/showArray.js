import showValue from '../Utils/showValue'

export default function showArray(obj, prop, delegate) {
    let array = showValue(obj, prop);
    if (array !== '')
    {
       return array.map(delegate);
    }
}