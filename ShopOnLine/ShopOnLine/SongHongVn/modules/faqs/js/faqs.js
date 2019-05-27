function SubmitSearch(f)
{
	var key 	= f.key.value; 
	var cat_id 	= f.cat_id.value;  
	var day 	= f.day.value;		
	var month 	= f.month.value;		
	var year 	= f.year.value;		
	
	document.location = 
	'?'+cmd +'=mod:news|act:category|catID:'+cat_id+		
	'|day:'	+ day +
	'|month:'+ month +
	'|year:'+ year +
	'|key:'+ key
	return false;
}


function show_comment(id,lang,p) {
	
	$.ajax({
		 type: "GET",
		 url: ROOT+'modules/news/ajax/_show_comment.php',
		 data: "id="+id+'&lang='+lang+'&p='+p,
		 success: function(html){
			  $("#ext_comment").html(html);
		 }
	 });
}
$(document).ready(function(){
    $(".question .item .c-answer").hide();
    $(".question .item:first .c-answer").show();
    $(".question .item:first").addClass("show");
    $(".question .item .item-question").click(function(){
        if($(this).parents(".item").hasClass("show")){
            $(this).parents(".item").removeClass("show");
            $(this).parents(".item").find(".c-answer").stop().slideToggle("700");
        }
        else{
            $(".question .item.show .c-answer").stop().slideToggle("700");
            $(".question .item").removeClass("show");
            $(this).parents(".item").addClass("show");
            $(this).parents(".item").find(".c-answer").stop().slideToggle("700");  
        }
    });  
});
$(document).ready(function(){
    $(".button-question").fancybox({
        padding     : 0,
        maxWidth    : 670,
        fitToView   : false,
        width       : '100%',
        autoSize    : false,
        closeClick  : false,
        wrapCSS     : "pQuestion",
        openEffect  : 'none',
        closeEffect : 'none'
    });
});
function changeQuestion(value,url)
{
    top.location.href=url+'/?fid='+value;
}
