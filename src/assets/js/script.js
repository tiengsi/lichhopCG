//event
$('[data-href]').on('click', event => {
  let href = $(event.target).closest('[data-href]').attr('data-href')
  if (!href.length) return
  location.href = href
})

$('#select-language').change(e => {
  let lang = e.target.value;
  let srcImgByLang = {
    vn: './img/icon-lang-vn.png',
    en: './img/icon-lang-en.png'
  }[lang]
  $('#select-language').css('background-image', 'url("' + srcImgByLang + '")')
  $('#select-language').val('')
})

$('#select-language').trigger('change');

$('.owl-carousel').owlCarousel({
  loop: true,
  margin: 10,
  dots: false,
  items: 1,
  nav: true,
  navText: ["<img width='64px' src='../img/Frame%2039.png'>", "<img width='64px' src='../img/Frame%2040.png'>"]
})

$('.tab-item').click(e => {
  $(e.target).addClass('active')
  $(e.target).siblings().removeClass('active')
  let blockId = $(e.target).attr('data-id')
  $('#' + blockId).addClass('active')
  $('#' + blockId).siblings().removeClass('active')
})


$('.nav-item').click(e => {
  $(e.target).addClass('active')
  $(e.target).siblings().removeClass('active')
})


//menu
$('.icon-mobile-menu, .icon-mobile-close-menu').click(e => {
  $('.nav-mobile-wrapper').toggleClass('active')
})

//menu
$('.toggle-menu-mobile-1').click(e => {
  let $menu = $($(e.target)).closest('.nav-mobile-1__item').find('.nav-mobile-2')
  let $icon = $($(e.target)).closest('.nav-mobile-1__item').find('.toggle-menu-mobile-1 img')
  $icon.toggleClass('active')
  $menu.toggleClass('active')
})

//menu
$('.toggle-menu-mobile-2').click(e => {
  let $menu = $($(e.target)).closest('.nav-mobile-2__item').find('.nav-mobile-3')
  let $icon = $($(e.target)).closest('.nav-mobile-2__item').find('.toggle-menu-mobile-2 img')
  $icon.toggleClass('active')
  $menu.toggleClass('active')
})

$('#date-range-picker').length && $('#date-range-picker').daterangepicker({}, function (start, end, label) {
  console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
}).click();


$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})

tinymce.init({
  selector: 'textarea#modal-new-note-content',
  menubar: false,
});
