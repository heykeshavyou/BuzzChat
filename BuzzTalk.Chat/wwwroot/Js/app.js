window.getFromStorage = function (key) {
    return localStorage.getItem(key);
}
window.setToStrorage = function (key, value) {
    localStorage.setItem(key, value);
}
window.removeItem = function () { 
    localStorage.clear();
}
window.scrollPageToBottom = () => {
    window.scrollTo(0, document.body.scrollHeight);
}
function scrollToBottom(elementId) {
        const el = document.getElementById(elementId);
    if (el) {
        el.scrollTop = el.scrollHeight;
        console.log(el)
    }
}