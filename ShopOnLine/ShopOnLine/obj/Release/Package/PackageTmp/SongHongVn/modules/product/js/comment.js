var ROOT = '';

var vnTcomment = {   
	//alert(ROOT)
    
	/*  show_comment */
	show_comment:function(id,lang,p) {	
		$.ajax({
			 type:"POST",
			 url: ROOT+'modules/product/ajax/comment.php?do=list',
			 data: "id="+id+'&lang='+lang+'&p='+p,
			 success: function(html){
					$("#ext_comment").html(html);
			 }
		 });
    },

	select_vote: function (num) {
		var uservote=num;
		for (i=1;i<=5;i++) {
			objname='vote_'+i;
			var imgshow=(i<=num) ? 'star2.gif':'star0.gif';
			src =  ROOT+"modules/product/images/"+imgshow;
			$("#"+objname).attr("src", src); 
		}	
		$('#hvote').val(num);
		
    },

	/*  post_comment */
	post_comment:function (id,lang)
	{

		var ok_post = true ;
		var mess_err='';
		var re =/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/gi;
		var rating =$('input[name=chon-dt]:checked').val() ;

        var js_lang = new Array();
        js_lang['announce'] = "Thông báo";
        js_lang['error'] = "Báo lỗi";
        js_lang['err_name_empty'] = "Vui lòng nhập họ tên";
        js_lang['err_email_empty'] = "Vui lòng nhập Email";
        js_lang['err_email_invalid'] = "Email không hợp lệ";
        js_lang['security_code_invalid'] = "Mã bảo vệ không đúng";
        js_lang['err_security_code_empty'] = "Vui lòng nhập mã bảo vệ";
        js_lang['err_title_empty'] = "";
        js_lang['err_content_comment_empty'] = "";
        js_lang['send_comment_success'] = "Viết đánh giá thành công . Vui lòng đợi xét duyệt của BQT";
        js_lang['mess_error_post'] = "";


		  
		var name = $("#com_name").val()
		if (name == '') {			 
			jAlert(js_lang['err_name_empty'],js_lang['error'], function() {	$("#com_name").focus(); 	}); 			
			ok_post = false ;
			return false;
		}

		email = $("#com_email").val();
		if (email == '') {
			jAlert(js_lang['err_email_empty'],js_lang['error'], function() {	$("#com_email").focus(); 	});   
			ok_post = false ;
			return false;
		}
		
		if (email != '' && email.match(re)==null) {
			jAlert(js_lang['err_email_invalid'],js_lang['error'], function() {	$("#com_email").focus(); 	});  
			ok_post = false ;
			return false;
		}	  
		if ( rating =='' || rating==0) {
			jAlert('Vui lòng cho điểm đánh giá ','Báo lỗi', function() {	 $("#hvote").focus(); 	});   
			ok_post = false ;
			return false;
		}
		 
		var content= $("#txtComment").val();		 
		if ( content.length <10) {
			jAlert('Vui lòng nhập nội dung tối thiểu 10 ký tự','Báo lỗi', function() {	 $("#txtComment").focus(); 	});   
			ok_post = false ;
			return false;
		}
		 
  	  
		if(ok_post) 
		{			
			name = encodeURIComponent(name);
			email = encodeURIComponent(email); 
			content = encodeURIComponent(content);
 			
			var mydata =  "id="+id+"&rating="+rating+"&name="+name+"&email="+email+"&content=" + content +"&lang="+lang ; 
			$.ajax({
				async: true,
				dataType: 'json',
				url:  ROOT+"modules/product/ajax/comment.php?do=post" ,
				type: 'POST',
				data: mydata ,
				success: function (data) {
					if(data.ok == 1)	{
						 vnTcomment.show_comment (id,lang,0); 		
						 $("#com_name").val('Tên của bạn')		;
						 $("#com_email").val('Email')		;
						 $("#txtComment").val('')		;
					}	else {
						jAlert('Có lỗi xảy ra .','Báo lỗi');
					}	   
				}
			}) 		
		} 
		
		return false;
	},
	
 
	/*  post_subcomment */
	post_votes:function (id)
	{
		  
		var rating = $("input[name='rating']:checked").val();
		
		var mydata =  "id="+id+ "&rating=" + rating  ;
		$.ajax({
			async: true,
			dataType: 'json',
			url:  ROOT+"modules/product/ajax/comment.php?do=votes" ,
			type: 'POST',
			data: mydata ,
			success: function (data) {
				if(data.ok == 1)	{
					$("#ext_votes").html('<div class="ajax_mess" >Cám ơn bạn đã tham gia bình chọn</div>');					 
				}	else {
					jAlert('Có lỗi xảy ra .','Báo lỗi');
				}	   
			}
		}) 		
		  
		
		return false;
	}
	
}; 