jQuery(document).ready(function($) {
    "use strict";
    
    /*
    Banner Slider*/
    if ($('#tnit-banner-slider').length) {
        var outerCarousel = $('#tnit-banner-slider');
        outerCarousel.owlCarousel({
            loop: false,
            dots: false,
            nav: false,
            navText: '',
            items: 1,
            autoplay: false,
            smartSpeed: 4000,
            animateIn: 'zoomIn',
            animateOut: 'fadeOut',
            touchDrag: false,
            mouseDrag: false,
        });
    }
 
    /*
    Owl Slide For Blog Page*/
    if ($('#tnit-blog-slider').length) {
        $('#tnit-blog-slider').owlCarousel({
            loop: true,
            dots: false,
            nav: true,
            navText: '',
            items: 2,
            margin: 30,
            autoplay: false,
            smartSpeed: 2000,
            animateIn: 'fadeIn',
            animateOut: 'fadeOut',
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                768: {
                    items: 1,
                },
                992: {
                    items: 2,
                },
                1199: {
                    items: 2,
                }
            }
        });
    }

    /*
    Owl Slide For Testimonial*/
    if ($('#tnit-testimonial-slider').length) {
        $('#tnit-testimonial-slider').owlCarousel({
            loop: true,
            dots: false,
            nav: true,
            navText: '',
            items: 3,
            margin: 30,
            autoplay: false,
            smartSpeed: 2000,
            animateIn: 'fadeIn',
            animateOut: 'fadeOut',
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                768: {
                    items: 2,
                },
                992: {
                    items: 3,
                },
                1199: {
                    items: 3,
                }
            }
        });
    }

    /*
    Owl Slide For Partners*/
    if ($('#tnit-partners-slider').length) {
        $('#tnit-partners-slider').owlCarousel({
            loop: true,
            dots: false,
            nav: false,
            navText: '',
            items: 8,
            margin: 60,
            autoplay: false,
            smartSpeed: 2000,
            animateIn: 'fadeIn',
            animateOut: 'fadeOut',
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                },
                640: {
                    items: 2,
                },
                768: {
                    items: 4,
                },
                992: {
                    items: 6,
                },
                1199: {
                    items: 8,
                }
            }
        });
    }

    /*
   Jquery AddClass onClick*/
    if ($('.btn-play').length) {
        $(".btn-play").on('click', function(e){
            $('#tnit-videoPlayer-outer').removeClass('tnit-videoOpen')
            $('#tnit-videoPlayer-outer').addClass('tnit-videoOpen');
        });
    }


    /* 
    Search Outer Jquery Code*/
    if($('#trigger-tnit-search').length){
        $('#trigger-tnit-search').on('click', function(event){
            // $('.tnit-search-form1').slideToggle(500);
            $('.tnit-search-form1').css({
                width: '263px',
                opacity: '1',
                visibility: 'visible',
            });;
            // event.preventDefault();
            event.stopPropagation();
        });
        $(document).on("click", function(event){
        var $trigger = $(".tnit-search-form1");
            if($trigger !== event.target && !$trigger.has(event.target).length){
                // $(".tnit-search-form1").slideUp("500");
                $('.tnit-search-form1').css({
                    width: '0',
                    opacity: '0',
                    visibility: 'hidden',
                });;
            }            
        });

    }

    /* 
    Social Links Code*/
    if($('.share-holder').length){
        $('.share-holder').on('click', function(event){
            $('#tnit-social-overlay').slideToggle(500);
            // event.preventDefault();
            event.stopPropagation();
        });
        $(document).on("click", function(event){
        var $trigger = $("#tnit-social-overlay");
            if($trigger !== event.target && !$trigger.has(event.target).length){
                $("#tnit-social-overlay").slideUp("500");
            }            
        });

    }
   
    /*
    jQuery Scrollbar Code*/
    if ($('.scrollbar-macosx').length) {
        jQuery('.scrollbar-macosx').scrollbar();
    }

    /* 
    Banner Social Jquery Code*/
    if($('.tnit-social-counter').length){
        $('.tnit-social-counter').counterUp({
            delay: 30,
            time: 1500
        });
     }

      /* 
    Facts Counter Jquery Code*/
    if($('.tnit-counter').length){
        $('.tnit-counter').counterUp({
            delay: 20,
            time: 1000
        });
     }


    /* 
    Skill Circle JQuery Code*/
    if($('.tnit-progress-bar').length){
        $(".tnit-progress-bar").loading();
    }


    /* 
    Product Spinner jQuery Code*/
    (function($) {
        $.fn.spinner = function() {
            this.each(function() {
                var el = $(this);

                // add elements
                el.wrap('<span class="spinner"></span>');     
                el.before('<span class="sub">-</span>');
                el.after('<span class="add">+</span>');

                // substract
                el.parent().on('click', '.sub', function () {
                    if (el.val() > parseInt(el.attr('min')))
                        el.val( function(i, oldval) { return --oldval; });
                });

                // increment
                el.parent().on('click', '.add', function () {
                    if (el.val() < parseInt(el.attr('max')))
                        el.val( function(i, oldval) { return ++oldval; });
                });
            });
        };
    })(jQuery);

    $('input[type=number]').spinner();
 

});



  