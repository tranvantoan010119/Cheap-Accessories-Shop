/* SHARED VARS */
var touch = false;

// handles Animate
function dataAnimate(){
  $('[data-animate]').each(function(){
    
    var $toAnimateElement = $(this);
    
    var toAnimateDelay = $(this).attr('data-delay');
    
    var toAnimateDelayTime = 0;
    
    if( toAnimateDelay ) { toAnimateDelayTime = Number( toAnimateDelay ); } else { toAnimateDelayTime = 200; }
    
    if( !$toAnimateElement.hasClass('animated') ) {
      
      $toAnimateElement.addClass('not-animated');
      
      var elementAnimation = $toAnimateElement.attr('data-animate');
      
      $toAnimateElement.appear(function () {
        
        setTimeout(function() {
          $toAnimateElement.removeClass('not-animated').addClass( elementAnimation + ' animated');
        }, toAnimateDelayTime);
        
      },{accX: 0, accY: -80},'easeInCubic');
      
    }
    
  });
}

function getMaxHeight($elms) {
	var maxHeight = 0;
	$($elms).each(function () {
	
		var height = $(this).outerHeight();
		if (height > maxHeight) {
			maxHeight = height;
		}
	});
	return maxHeight;
};
   
jQuery(document).ready(function($) {
  
  /* DETECT PLATFORM */
  $.support.touch = 'ontouchend' in document;
  
  if ($.support.touch) {
    touch = true;
    $('body').addClass('touch');
  }
  else{
	$('body').addClass('notouch');
  }
  
	$('.selectpicker').selectpicker();
	
  // Product List
	$('#list-view').click(function() {
		$('#list-view').addClass('active');
		$('#grid-view').removeClass('active');
	});

	// Product Grid
	$('#grid-view').click(function() {
		$('#grid-view').addClass('active');
		$('#list-view').removeClass('active');
	});
  
	if (localStorage.getItem('display') == 'list') {
		$('#list-view').trigger('click');
	} else {
		$('#grid-view').trigger('click');
	}
  
  /* Handle Animate */
  if(touch == false){
    dataAnimate();
  }
  
	$(".open-panel,.close-panel").click(function(){
		$('body').toggleClass('openNav');
	});
	
	$(".icon-refine").click(function(){
		$('.category-list').toggleClass('openCate');
		$('.icon-refine').toggleClass('active');
	});
	
	
	$('.nav-pills li.parent > p').click(function(){

		if ($(this).text() == '+'){
			$(this).parent('li').children('.dropdown').slideDown(300);
			$(this).text('-');
		}else{
			$(this).parent('li').children('.dropdown').slideUp(300);
			$(this).text('+');
		}  
		
	});
	
});

// Js smartresize
(function($,sr){
  // debouncing function from John Hann
  // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
  var debounce = function (func, threshold, execAsap) {
      var timeout;

      return function debounced () {
          var obj = this, args = arguments;
          function delayed () {
              if (!execAsap)
                  func.apply(obj, args);
              timeout = null; 
          };

          if (timeout)
              clearTimeout(timeout);
          else if (execAsap)
              func.apply(obj, args);

          timeout = setTimeout(delayed, threshold || 100); 
      };
  }
// smartresize 
 jQuery.fn[sr] = function(fn){  return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery,'smartresize');

var TO = false;
$(window).smartresize(function(){
if(TO !== false)
    clearTimeout(TO);
 TO = setTimeout(resizeWidth, 400); //400 is time in miliseconds
});

function handleMenu(){
  // Listener for header
  var scrollTop = $(this).scrollTop();
  var heightHeader = $('#header').outerHeight();
  var heightNav = $('#bt_mainmenu').outerHeight();
  var heighttotal = (heightHeader+heightNav);
  
  if(getWidthBrowser() > 1024){
    if(scrollTop > heighttotal){
      if(!$('#bt_mainmenu').hasClass('show')){
        $('<div style="min-height:'+heightNav+'px"></div>').insertBefore('#bt_mainmenu');
        $('#bt_mainmenu').addClass('show').addClass('fadeInDown animated');
      }
    }else{
      if($('#bt_mainmenu').hasClass('show')){
        $('#bt_mainmenu').prev().remove();
        $('#bt_mainmenu').removeClass('show').removeClass('fadeInDown animated');
      }
    }
  }
};
$(window).load(function(){
	resizeWidth();
});

function resizeWidth(){
	var currentWidth = $(".bt-content-menu").outerWidth();	
	$('.mega-menu ul > li.parent > div').each(function(index, element) {		
		var menu = $('.bt-content-menu').offset();
		var dropdown = $(this).parent().offset();
		
		i = (dropdown.left + $(this).outerWidth()) - (menu.left + currentWidth);
		if (i > 0) {
			$(this).css('margin-left', '-' + (i)+ 'px');
		}
		else
			$(this).css('margin-left', '0px');
	});
}


$.fn.bttabs = function() {
	var selector = this;
	
	this.each(function() {
		var obj = $(this); 
		
		$(obj.attr('href')).hide();
		
		obj.click(function() {
			$(selector).removeClass('selected');
			
			$(this).addClass('selected');
			
			$($(this).attr('href')).fadeIn();
			
			var tabmodule = $(this).attr('data-crs');
			loadslider(tabmodule);
			
			$(selector).not(this).each(function(i, element) {
				$($(element).attr('href')).hide();
			});
			
			return false;
		});
	});

	$(this).show();
	
	$(this).first().click();
};

var btadd = {
	'cart': function(product_id) {
		$.ajax({
			url: 'index.php?route=bossthemes/boss_add/cart/',
			type: 'post',
			data: 'product_id=' + product_id,
			dataType: 'json',
			success: function(json) {
				if (json['redirect']) {
					location = json['redirect'];
				}

				if (json['success']) {
					addProductNotice(json['title'], json['thumb'], json['success'], 'success');
					$('#cart-total').html(json['total']);
				
					$('#cart > ul').load('index.php?route=common/cart/info ul li');
				}
			}
		});
	},
	'wishlist': function(product_id) {
		$.ajax({
			url: 'index.php?route=bossthemes/boss_add/wishlist/',
			type: 'post',
			data: 'product_id=' + product_id,
			dataType: 'json',
			success: function(json) {
				if (json['success']) {
					addProductNotice(json['title'], json['thumb'], json['success'], 'success');
					$('#wishlist-total').prop('title', json['total']);
				}else{
					addProductNotice(json['title'],'', json['info']);
					$('#wishlist-total').prop('title', json['total']); 
				}
			}
		});
	},
	'compare': function(product_id) {
		$.ajax({
			url: 'index.php?route=bossthemes/boss_add/compare',
			type: 'post',
			data: 'product_id=' + product_id,
			dataType: 'json',
			success: function(json) {
				if (json['success']) {
					addProductNotice(json['title'], json['thumb'], json['success'], 'success');
					$('#compare-total').html(json['total']);
				}
			}
		});
	}
	
};

function addProductNotice(title, thumb, text, type) {
	$.jGrowl.defaults.closer = true;
	var tpl = thumb + '<h3>'+text+'</h3>';
	$.jGrowl(tpl, {		
		life: 3000,
		header: title,
		speed: 'slow'
	});
}
/*FAQ*/
$(document).ready(function(){
	$('.item-title').click(function(){
		$('.item-content').slideUp();
		$('.icon-toggle').removeClass('close').addClass('open');
		$('.item').removeClass('active');
		if($(this).next().is(":visible"))
		{
			$(this).find('.icon-toggle').removeClass('close').addClass('open');
			$(this).next().slideUp();
			$(this).parent().removeClass('active');
		}
		else
		{
			$(this).find('.icon-toggle').removeClass('open').addClass('close');
			$(this).next().slideDown();
			$(this).parent().addClass('active');
		}
	})
});
