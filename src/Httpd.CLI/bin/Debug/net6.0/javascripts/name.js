let j = 1;
let letters = ['B ', 'e ', 'r ', 'g ', 'y']
let name = '';
let num = 0;
let k = 0;
let account = '<a class="row linksCheats altSteam accounts" href="http://steamcommunity.com/profiles/76561199155047417" target="_blank">Xeno</a><a class="row linksCheats altSteam accounts" href="http://steamcommunity.com/profiles/76561198887922595" target="_blank">StoneWarrior</a><a class="row linksCheats altSteam accounts" href="https://gamesense.pub/forums/profile.php?id=5967" target="_blank">Stone Rock</a>';

$(".temp").hide();

const timer2 = ms => new Promise(res => setTimeout(res, ms))
async function loader () {
  while(j === 1) {
    if(num <= 4) {
      name += letters[num];
    }
  await timer2(500);
  num++;
  if(num > 8){
    num = 0;
    name = '';
  }
  document.getElementById('name').innerHTML = name;
  }
}

loader();

$(document).ready(function(){
  $("#temper").click(function(){
    $(".temp").fadeIn();
    $(".temp").delay(2500).fadeOut();
  });
});