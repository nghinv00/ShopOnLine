$(document).ready(function($) {

  $('.silder-brand .center').slick({
    centerMode: true, 
    slidesToShow: 5,   
    centerPadding: '0px',
    asNavFor: '.nav-silder-brand .center',
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 3000,
    focusOnSelect: true,
    speed: 800,
  });

  $('.nav-silder-brand .center').slick({
    centerMode: true, 
    slidesToShow: 1,   
    asNavFor: '.silder-brand .center',
    centerPadding: '0px',
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 3000,
    speed: 1500,
  });
});