vnTProduct = {
	load_image:function (src,src_big)
	{
		$("#divImage img").attr( "src", src );	
		$("#divImage a").attr( "href", src_big ) ;
	},
	
	check_all:function()	
	{		
		var c = $("#all").attr('checked');
		
		$("#List_Product").find('input:checkbox' ).attr( 'checked', function() {
			var item_id = 'item'+$(this).val();
			if (c){
				$('#'+item_id).addClass('item_select')	;
				return 'checked';
			}else{
				$('#'+item_id).removeClass('item_select')	;	
				return '';	
			}
		}); 
				
	},
	
	select_item:function(id)	
	{
		var item_id = 'item'+id;
		var c = $("#"+item_id+" #ch_id").attr('checked');
		if (c){
			$('#'+item_id).addClass('item_select')	;
		}else{
			$('#'+item_id).removeClass('item_select')	;	
		}
		  
	},
	
	DoAddCart:function (){ 
		var aList = new Array;
		var count=0;
		$("#List_Product :input:checkbox").each( function() {
				if( $(this).attr('checked') && $(this).attr('id')!="all" ){
					aList.push($(this).val());
					count=count+1;
				} 																																 
		} );		
		
 		if (count==0){ jAlert('Vui lòng chọn 1 sản phẩm', 'Báo lỗi') }
 		else {
			p_id = aList.join(',');	
			location.href=ROOT+'product/cart.html/?do=add&pID='+p_id;	
		}
	  
		return false;
	
	},
	
	DoCompare:function (){
		var catRoot =  $("#catRoot").val();
		var aList = new Array;
		var count=0;
		$("#List_Product :input:checkbox").each( function() {
				if( $(this).attr('checked') && $(this).attr('id')!="all" ){
					aList.push($(this).val());
					count=count+1;
				} 																																 
		} );
		
		
		if(catRoot =='') { jAlert('Không thể so sánh do không cùng chủng loại', 'Báo lỗi')}
		else if (count==0){ jAlert('Vui lòng chọn 1 sản phẩm để so sánh', 'Báo lỗi') }
		else if (count>3){ jAlert('Vui lòng chọn tối đa 3 sản phẩm để so sánh', 'Báo lỗi') }
		else {
			p_id = aList.join(',');	
			location.href=ROOT+'product/compare_product.html/?catRoot='+catRoot+'&p_id='+p_id;	
		}
	  
		return false;
	
	},

	do_WishList:function (doAction,id) {
 
 		if(mem_id>0)
		{
			var mydata =  "act="+doAction +'&id='+id; 
			$.ajax({
				async: true,
				dataType: 'json',
				url: ROOT+"modules/product/ajax/ajax.php?do=wishList",
				type: 'POST',
				data: mydata ,
				success: function (data) {
					jAlert(data.mess, 'Thông báo');					    
				}
			})
		
		}else{
			jAlert('Vui lòng đăng nhập thành viên','Báo lỗi', function() {	vnTRUST.customer.login.show(); 	});  
		}
		return false ;
	} 
	 
	
};
   

function format_number (num) {
	num = num.toString().replace(/\$|\,/g,'');
	if(isNaN(num))
	num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.round(num*100+0.50000000001);
	num = Math.round(num/100).toString();
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
	num = num.substring(0,num.length-(4*i+3))+','+ num.substring(num.length-(4*i+3));
	return (((sign)?'':'-') + num);
}


function numberFormat(nStr){
	nStr += '';
	x = nStr.split('.');
	x1 = x[0];
	x2 = x.length > 1 ? '.' + x[1] : '';
	var rgx = /(\d+)(\d{3})/;
	while (rgx.test(x1))
		x1 = x1.replace(rgx, '$1' + ',' + '$2');
	return x1 + x2;
}

function formatCurrency(div_id,str_number){
	/*Convert tu 1000->1.000*/
	/*var mynumber=1000;str_number = str_number.replace(/\./g,"");*/
	document.getElementById(div_id).innerHTML = '<font color=blue>' + numberFormat(str_number) + '<font>'; 
	document.getElementById(div_id).innerHTML = document.getElementById(div_id).innerHTML + ' <font color=red>VND</font>';
}

$(document).ready(function(){
   $('#slider_prod').slick({
      slidesToShow: 4,
      slidesToScroll: 4,
      autoplay: true,
      autoplaySpeed: 5000,
    });
});
$(document).ready(function(){
    $("#txtComment").focus(function(){
        $(this).css({height:"115px"});   
        $(this).parents(".w_content").find(".content-info").stop().slideDown(700); 
    });  
});
$(document).ready(function(){
    $(".choose-evaluate ul li").hover(function(){
        $(".choose-evaluate ul li").removeClass("star-red");
        $(this).addClass("star-red");
        $(this).prevAll().addClass("star-red");
        var $text = $(this).attr("title");
        $(this).parents(".choose-evaluate").find(".show-title").text($text);
        $(this).parents(".choose-evaluate").find(".show-title").addClass("show-active"); 
    },function(){
        $(".choose-evaluate ul li").removeClass("star-red");
        $(this).parent().find("li.active").addClass("star-red");
        $(this).parent().find("li.active").prevAll().addClass("star-red");
        var $text = $(this).parent().find("li.active").attr("title");
        $(this).parents(".choose-evaluate").find(".show-title").text($text);
        $(this).parents(".choose-evaluate").find(".show-title").removeClass("show-active");
    });
    $(".choose-evaluate ul li").click(function(){
        $(".choose-evaluate ul li").removeClass("active");
        $(".choose-evaluate ul li").removeClass("star-red");
        $(this).addClass("active star-red");
        $(this).parent().find("li.active").prevAll().addClass("star-red");
        $(this).find("input:radio").prop('checked', true);  
        var $text = $(this).attr("title");
        $(this).parents(".choose-evaluate").find(".show-title").text($text);
        $(this).parents(".choose-evaluate").find(".show-title").css({opacity:1});
    });
});
$(document).ready(function(){
    tabs();
    function tabs() {
    $('.tab_content').hide();
    $('.tab_content:first').show();
    $('.tab_nav li a:first').addClass('active');
    $(".tab_rep .ulTitle").text($('.tab_nav li a:first').text());
    $('.tab_nav li a').click(function(){
          var $string = $(this).attr("class");
            $(".tab_rep .ulTitle").text($(this).text());
            $(".tab_rep .wrapREP").removeClass('show');
              if($string != "" && String($string) != "undefined" ){
                if( $string.match(/active/gi) != "" ){
                    console.log($string.match(/active/gi));
                    return false;   
                }
             }
       var  id_content = $(this).attr("href"); 
       $('.tab_content').hide();
       $(id_content).stop().fadeIn();
       $('.tab_nav li a').removeClass('active');
       $(this).addClass('active');
       return false;
    });
    var $size = $(".tab_nav li").size();
    $(".tab_nav li").each(function(e){
        $(this).css({"z-index":parseInt($size) - parseInt(e)});        
    });
  }
});
$(document).ready(function(){
     $('#slider-detail').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        asNavFor: '#slider-thumbnail',
        autoplay: false,
        fade: true,
        autoplaySpeed: 5000,
        speed: 800,
     });
     $('#slider-thumbnail').slick({
        vertical: true,
        arrows: true,
        slidesToShow: 5,
        asNavFor: '#slider-detail',
        autoplay: false,
        autoplaySpeed: 5000,
        speed: 800,
        focusOnSelect: true,
     });
    $('#slider-detail').on('afterChange', function(event, slick, currentSlide){
        var currentSlide = $('#slider-detail').slick('slickCurrentSlide');
        var img_src = $("#slider-detail .slick-active img").attr("src");
        $(".zoomContainer").remove();
        $("#slider-detail .slick-active img").elevateZoom({
            zoomWindowWidth:390,
            zoomWindowHeight:440,
            zoomWindowFadeIn: 500,
            zoomWindowFadeOut: 500,
            lensFadeIn: 500,
            lensFadeOut: 500,
        });
    });
    $("#slider-detail .slick-active img").elevateZoom({
        zoomWindowWidth:390,
        zoomWindowHeight:440,
        zoomWindowFadeIn: 500,
        zoomWindowFadeOut: 500,
        lensFadeIn: 500,
        lensFadeOut: 500,
    });
});
$(document).ready(function(){
  $(".ykien").click(function(){
      $('html,body').animate({scrollTop: $(".comment").offset().top},1000);  
  });  
});

function onchangeSet(id,lang,p_id)
{
	$.ajax({
		 type:"POST",
		 url: ROOT+'modules/product/ajax/comment.php?do=list_size',
		 data: "id="+id+'&lang='+lang+'&p_id='+p_id,
		 success: function(html){
				$("#list_size").html(html);
		 }
	 });
}