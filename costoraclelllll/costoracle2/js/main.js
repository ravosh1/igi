(function ($) {
"use strict";

// meanmenu
$('#mobile-menu').meanmenu({
	meanMenuContainer: '.mobile-menu',
	meanScreenWidth: "768"
});
$('#mobile-menu2').meanmenu({
	meanMenuContainer: '.mobile-menu',
	meanScreenWidth: "992"
});


// mainSlider
function mainSlider() {
	var BasicSlider = $('.slider-active');
	BasicSlider.on('init', function (e, slick) {
		var $firstAnimatingElements = $('.single-slider:first-child').find('[data-animation]');
		doAnimations($firstAnimatingElements);
	});
	BasicSlider.on('beforeChange', function (e, slick, currentSlide, nextSlide) {
		var $animatingElements = $('.single-slider[data-slick-index="' + nextSlide + '"]').find('[data-animation]');
		doAnimations($animatingElements);
	});
	BasicSlider.slick({
		autoplay: true,
		autoplaySpeed:3000,
		dots: true,
		slidesToShow: 1,
		prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-long-arrow-left"></i></button>',
		nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-long-arrow-right"></i></button>',
		arrows: true,
		responsive: [
			{ breakpoint: 767, settings: { dots: false, arrows: false } }
		]
	});

	function doAnimations(elements) {
		var animationEndEvents = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
		elements.each(function () {
			var $this = $(this);
			var $animationDelay = $this.data('delay');
			var $animationType = 'animated ' + $this.data('animation');
			$this.css({
				'animation-delay': $animationDelay,
				'-webkit-animation-delay': $animationDelay
			});
			$this.addClass($animationType).one(animationEndEvents, function () {
				$this.removeClass($animationType);
			});
		});
	}
}
mainSlider();

// insurance - pro
$('.insurance-pro').slick({
	slidesToShow: 3,
	slidesToScroll: 1,
	dots: true,
	arrows: false,
	responsive: [
		{
			breakpoint: 992,
			settings: {
				slidesToShow: 2,
				slidesToScroll: 1,
				dots: true
			}
		},
		{
			breakpoint: 768,
			settings: {
				slidesToShow: 1,
				slidesToScroll: 2
			}
		},
		{
			breakpoint: 560,
			settings: {
				slidesToShow: 1,
				slidesToScroll: 1
			}
		}
	]
});

// brand-active
$('.brand-active').slick({
	slidesToShow: 6,
	slidesToScroll: 1,
	dots: false,
	arrows: false,
	responsive: [
		{
			breakpoint: 992,
			settings: {
				slidesToShow: 5,
				slidesToScroll: 1
			}
		},
		{
			breakpoint: 768,
			settings: {
				slidesToShow: 3,
				slidesToScroll: 2
			}
		},
		{
			breakpoint: 560,
			settings: {
				slidesToShow: 2,
				slidesToScroll: 1
			}
		}
	]
});

// testimonial
$('.testimonia-item-active').slick({
	slidesToShow: 1,
	slidesToScroll: 1,
	arrows: false,
	autoplay: true,
	//fade: true,
	dots: true,
	asNavFor: '.testimonial-nav'
});
$('.testimonial-nav').slick({
	slidesToShow: 3,
	slidesToScroll: 1,
	asNavFor: '.testimonia-item-active',
	dots: false,
	arrows: false,
	prevArrow: '<button type="button" class="slick-prev"><i class="fas fa-chevron-left"></i></button>',
	nextArrow: '<button type="button" class="slick-next"><i class="fas fa-chevron-right"></i></button>',
	centerMode: true,
	focusOnSelect: true,
	centerPadding: 0
});


/* magnificPopup img view */
$('.popup-image').magnificPopup({
	type: 'image',
	gallery: {
	  enabled: true
	}
});

/* magnificPopup video view */
$('.popup-video').magnificPopup({
	type: 'iframe'
});


// portfolio active
$('.portfolio-area').imagesLoaded(function () {
	var grid = $('.grid').isotope({
		itemSelector: '.grid-item',
		percentPosition: true,
		masonry: {
			// use outer width of grid-sizer for columnWidth
			columnWidth: 1
		}
	})

	$('.portfolio-menu').on('click', 'button', function () {
		var filterValue = $(this).attr('data-filter');
		grid.isotope({ filter: filterValue });
	});

	//for portfolio menu active class
	$('.portfolio-menu button').on('click', function (event) {
		$(this).siblings('.active').removeClass('active');
		$(this).addClass('active');
		event.preventDefault();
	});

});


/* niceSelect */
//$('select').niceSelect();


// map
function basicmap() {
	// Basic options for a simple Google Map
	// For more options see: https://developers.google.com/maps/documentation/javascript/reference#MapOptions
	var mapOptions = {
		// How zoomed in you want the map to start at (always required)
		zoom: 11,
		scrollwheel: false,
		// The latitude and longitude to center the map (always required)
		center: new google.maps.LatLng(40.6700, -73.9400), // New York
		// This is where you would paste any style found on Snazzy Maps.
		styles: [{ "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] }, { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }] }, { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": -100 }, { "lightness": 45 }] }, { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }] }, { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#47b580" }, { "visibility": "on" }] }]
	};
	// Get the HTML DOM element that will contain your map
	// We are using a div with id="map" seen below in the <body>
	var mapElement = document.getElementById('contact-map');

	// Create the Google Map using our element and options defined above
	var map = new google.maps.Map(mapElement, mapOptions);

	// Let's also add a marker while we're at it
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(40.6700, -73.9400),
		map: map,
		title: 'Cryptox'
	});
}
if ($('#contact-map').length != 0) {
	google.maps.event.addDomListener(window, 'load', basicmap);
}



// scrollToTop
$.scrollUp({
	scrollName: 'scrollUp', // Element ID
	topDistance: '300', // Distance from top before showing element (px)
	topSpeed: 300, // Speed back to top (ms)
	animation: 'fade', // Fade, slide, none
	animationInSpeed: 200, // Animation in speed (ms)
	animationOutSpeed: 200, // Animation out speed (ms)
	scrollText: '<i class="icofont icofont-long-arrow-up"></i>', // Text for element
	activeOverlay: false, // Set CSS color to display scrollUp active point, e.g '#00FFFF'
});


// slider - range
$(".slider-range").slider({
	range: true,
	min: 0,
	max: 500,
	values: [75, 300],
	slide: function (event, ui) {
		$(".amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
	}
});
$(".amount").val("$" + $(".slider-range").slider("values", 0) +
	" - $" + $(".slider-range").slider("values", 1));


	
	
	
	
(function ($) {
    "use strict";


    /*==================================================================
    [ Focus Contact2 ]*/
    $('.input3').each(function(){
        $(this).on('blur', function(){
            if($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })    
    })
            

    /*==================================================================
    [ Chose Radio ]*/
    $("#radio1").on('change', function(){
        if ($(this).is(":checked")) {
            $('.input3-select').slideUp(300);
        }
    });

    $("#radio2").on('change', function(){
        if ($(this).is(":checked")) {
            $('.input3-select').slideDown(300);
        }
    });
        
  
    
    /*==================================================================
    [ Validate ]*/
    var name = $('.validate-input input[name="name"]');
    var email = $('.validate-input input[name="email"]');
    var message = $('.validate-input textarea[name="message"]');


    $('.validate-form').on('submit',function(){
        var check = true;

        if($(name).val().trim() == ''){
            showValidate(name);
            check=false;
        }


        if($(email).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
            showValidate(email);
            check=false;
        }

        if($(message).val().trim() == ''){
            showValidate(message);
            check=false;
        }

        return check;
    });


    $('.validate-form .input3').each(function(){
        $(this).focus(function(){
           hideValidate(this);
       });
    });

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
    
    

})(jQuery);
	
	
	
	
	
	
	
})(jQuery);