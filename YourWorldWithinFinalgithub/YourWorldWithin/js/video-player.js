/*
SimpleYouTubePlayer
by John E Maddox
*/

$(document).ready(function(){

  // display video player
  $('.btn-play').on('click',function(e){

    e.preventDefault();

    // get video url
    var u = $(this).attr('href');

    // display video or go to youtube depending on window size
    // this is an attempt to load videos on mobile devices in the youtube app
    if($(window).width() > 800){

      // get video id
      var i = u.substring(u.search('=')+1,u.length);

      // build player
      $('#tnit-videoPlayer-outer .tnit-videoplay-inner').html('<iframe src="https://www.youtube.com/embed/' + i + '" frameborder="0" allowfullscreen></iframe>');

      // display player
      $('#tnit-videoPlayer-outer').fadeIn(500);

    }else{
      window.location.href = u;
    }
  }); // eof display player

  // hide video player
  $('#tnit-videoPlayer-outer').on('click',function(e){

    // hide player
    $('#tnit-videoPlayer-outer').fadeOut(500);

    // destroy player
    $('#tnit-videoPlayer-outer .tnit-videoplay-inner').empty();

  }); // eof hide player
});


