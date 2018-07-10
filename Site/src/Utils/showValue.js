export default function showValue(obj, property) {
    return (obj && obj !== 'null' && obj !== 'undefined') ? obj[property] : '';       
}