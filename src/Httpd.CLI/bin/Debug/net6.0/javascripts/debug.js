document.getElementById('sessionid').innerHTML = Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);


let g = 1;
const timere = ms => new Promise(res => setTimeout(res, ms))
async function load () {
  while(g === 1) {
  const now = Date.now();
  await timere(0);
  document.getElementById('epoc').innerHTML = now;
  }
}
load();


let apiKey = 'f05357e963d64ad9af323232d7373eb3';
$.getJSON('https://api.ipgeolocation.io/ipgeo?apiKey=' + apiKey, function(data) {
  var obj = JSON.parse(JSON.stringify(data, null, 2));
  // console.log(JSON.stringify(data, null, 2));
  // console.log(obj.ip_address);
  if (obj.ip == "someIP ;)" || obj.ip == "someIP ;)"){
    document.getElementById('user').innerHTML = "admin";
  } else {
    document.getElementById('user').innerHTML = "visitor";
  }
});






