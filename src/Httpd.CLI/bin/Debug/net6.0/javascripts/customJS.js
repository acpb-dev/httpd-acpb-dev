document.addEventListener('contextmenu', event => event.preventDefault());

let i = 1;
const timer = ms => new Promise(res => setTimeout(res, ms))
async function load () {
  while(i === 1) {
  var today = new Date();
  var dd = String(today.getDate()).padStart(2, '0');
  var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
  var yyyy = today.getFullYear();
  var time = ("0" + today.getHours()).slice(-2) + ":" + ("0" + today.getMinutes()).slice(-2) + ":<a style=\"color:#94b806;\">" + ("0" + today.getSeconds()).slice(-2) + "</a>";
  today = mm + '/' + dd + '/' + '<a style=\"color:#94b806;\">' + yyyy + '</a>';
  await timer(500);
  document.getElementById('time').innerHTML = time;
  document.getElementById('date').innerHTML = today;
  }
}
load();
