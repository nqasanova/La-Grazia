// Header Area Start Here

const accountIcon = document.querySelector('.header-fixed .fa-user')
const accountBox = document.querySelector('.account-box-main')
const cartIcon = document.querySelector('.header-fixed .fa-shopping-cart')
const cartBox = document.querySelector('.cart-box')

accountIcon.onmouseover=function(){
    accountBox.classList.add('active')
}

accountIcon.onmouseout=function(){
    accountBox.classList.remove('active')
}

accountBox.onmouseover=function(){
    accountBox.classList.add('active')
}

accountBox.onmouseout=function(){
    accountBox.classList.remove('active')
}

cartIcon.onmouseover=function(){
    cartBox.classList.add('active')
}

cartIcon.onmouseout=function(){
    cartBox.classList.remove('active')
}

cartBox.onmouseover=function(){
    cartBox.classList.add('active')
}

cartBox.onmouseout=function(){
    cartBox.classList.remove('active')
}

// Header Area End Here

// Scroll Top Area Start Here 

const scrollTop = document.querySelector('.scroll-top');

scrollTop.onclick = function(e){
    e.preventDefault();
    window.scrollTo({
        top: 0,
        left: 0,
        behavior: 'smooth'
      });
}

window.addEventListener("scroll", () => {
    if (window.pageYOffset > 200) {
        scrollTop.classList.add("active");
    } 
    
    else 
    {
        scrollTop.classList.remove("active");
    }
})

// Scroll Top Area End Here

// Wishlist Area Start Here

const wishlistButtons = Array.from(document.querySelectorAll('.wishlist-button'))
const heart = document.querySelector('.wishlist-button .far.fa-heart')

for (let i = 0; i < wishlistButtons.length; i++) {
    wishlistButtons[i].onclick=function(e){
        e.preventDefault();

        if(wishlistButtons[i].firstElementChild.className=="far fa-heart"){
            wishlistButtons[i].firstElementChild.className="fas fa-heart"
        }

        else
        {
            wishlistButtons[i].firstElementChild.className="far fa-heart"
        }
    }
}

// Wishlist Area End Here

// Clock Area Start Here

function getTimeRemaining(endtime) {
  var t = Date.parse(endtime) - Date.parse(new Date());
  var seconds = Math.floor((t / 1000) % 60);
  var minutes = Math.floor((t / 1000 / 60) % 60);
  var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
  var days = Math.floor(t / (1000 * 60 * 60 * 24));
  return {
    'total': t,
    'days': days,
    'hours': hours,
    'minutes': minutes,
    'seconds': seconds
  };
}
  
function initializeClock(id, endtime) {
  var clock = document.getElementById(id);
  var daysSpan = clock.querySelector('.days');
  var hoursSpan = clock.querySelector('.hours');
  var minutesSpan = clock.querySelector('.minutes');
  var secondsSpan = clock.querySelector('.seconds');

  function updateClock() {
    var t = getTimeRemaining(endtime);

    daysSpan.innerHTML = t.days;
    hoursSpan.innerHTML = ('0' + t.hours).slice(-2);
    minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
    secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

    if (t.total <= 0) {
      clearInterval(timeinterval);
    }
  }
  updateClock();
  var timeinterval = setInterval(updateClock, 1000);
}

var deadline = new Date(Date.parse(new Date()) + 01 * 24 * 60 * 60 * 1000);
initializeClock('clockdiv-p', deadline);

// Clock Area End Here