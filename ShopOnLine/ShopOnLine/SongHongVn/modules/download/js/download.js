function show_comment(id,p,lang) {
		
	obj=getobj('ext_comment');
	els = dom ? obj.style : obj;
 	if (dom){
	   els.display = "";
	} else if (ns4){
			els.display = "show";
	}
	
	var mydata =  "do=showComment&id="+id+'&p='+p+'&lang='+lang;
	$.ajax({
		 type: "GET",
		 url: ROOT+"modules/download/ajax_download.php",
		 data: mydata,
		 success: function(html){
				$("#ext_comment").html(html);
		 }
	 });	  
	
	return false;
}
$(function() {
   //  $('#slidre_price').slick({
   //   slidesToShow: 4,
   //   slidesToScroll: 4,
   //   autoplay: true,
   //   autoplaySpeed: 5000,
   //   dots: true,
   // });
});