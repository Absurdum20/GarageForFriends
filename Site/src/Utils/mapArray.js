import isValid from '../Utils/isValid'

export default function showArray(array, delegate) {
    let valid = isValid(array);
    if (valid === true)
    {
       return array.map(delegate);
    } 
}