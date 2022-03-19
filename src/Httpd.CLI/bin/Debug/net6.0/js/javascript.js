function readfile(page, pageName) {
    fetch(page)
    .then(response => response.text(page))
    .then(text => document.getElementById('MainContent').innerHTML = text);
    document.getElementById('pageTitle').innerHTML = pageName;
}

let g = 0;
const timer = ms => new Promise(res => setTimeout(res, ms))
async function load () {
  while(g < 100) {
    const letters = ['A ', 'l ', 'i ', 'a ', 's ', 'e ', 'd <br>', 'C ', 'l ', 'u ', 'b']
    await timer(500);
    if (g === 11){
        await timer(2500);
        g = 0
        document.getElementById('shufflingTitle').innerHTML = '';
    } else {
        document.getElementById('shufflingTitle').innerHTML += letters[g];
        g++
    }
  }
}
load();

function youtube() {
  window.open("https://www.youtube.com/channel/UCCsP8X6XPCmhIbEtjlf0tBg");
}

let id;
document.querySelectorAll('.buttonAction').forEach(item => {
    item.addEventListener('click', event => {
        id = document.getElementById(item.dataset.value.toString());
        slideInOrOut(id);
        hideeverything();
    })
});

function slideInOrOut(id) {
  
    try {
        hideOtherModals();
    }
    catch(err) {}

    if(id.style.display === 'block') {
        closeModal();
    } else {
        id.style.display = 'block';
        slideInEffect(id);
    }
    
}

function slideInEffect(id) {
    let pos = 0;
    let id2 = setInterval(frame, 2);
    function frame() {
        if (pos === 50) {
            clearInterval(id2);
        } else {
            pos++;
            id.style.top = pos + '%';
        }
    }
}

function slideOutEffect(id) {
    let pos = 0;
    let id2 = setInterval(frame, 2);
    function frame() {
        if (pos === -50) {
            clearInterval(id2);
        } else {
            pos--;
            id.style.top = pos + '%';
        }
    }
}

function hideeverything() {
  document.getElementById('wholePage').style.filter = 'blur(10px)';
}

function unhideeverything() {
  document.getElementById('wholePage').style.filter = 'blur(0px)';
}

function hideOtherModals() {
    let divs = document.querySelectorAll('.modal');
    let i = 0;
    [].forEach.call(divs, function(div) {
        let idTemp = document.getElementById(i+'');
        idTemp.style.display = 'none';
        i++;
    });
}

let modal = document.getElementById('test');
let modal2 = document.getElementById('test2');
window.onclick = function(event) {
    if (event.target === modal || event.target === modal2) {
        id.style.display = "none";
    }
}

function closeModal() {
    slideOutEffect(id);
    id.style.display = 'none';
    unhideeverything();
}

document.querySelectorAll('.exitButton').forEach(item => {
    item.addEventListener('click', event => {
        closeModal();
    })
});

window.addEventListener("keydown", function(event) {
    if (event.keyCode === 27) {
        closeModal();
    }
}, true);




// let element = document.getElementById('youtubeVideo');
// let positionInfo = element.getBoundingClientRect();
// let width = positionInfo.width;

// let actualHeight = (width / 16) * 9;
// console.log(actualHeight);
// document.getElementById('youtubeVideo').style.height = actualHeight;
