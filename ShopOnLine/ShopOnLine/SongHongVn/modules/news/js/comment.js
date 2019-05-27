var vnTcomment = new function()
{
    ROOT = ''
	 
	function ShowFormComment() {	
		
		$("#FormComment").show();
		$("#btn-comment").hide();
		
		$("#name").val("");
		$("#email").val("");
		$("#title").val("");
		$("#content").val("");
		$("#security_code").val("");
	}
	
	/*  show_comment */
	function show_comment(id,lang,p) {	
		$.ajax({
			 type:"POST",
			 url: ROOT+'modules/news/ajax/comment.php',
			 data: "do=list&id="+id+'&lang='+lang+'&p='+p,
			 success: function(html){
					$("#ext_comment").html(html);
			 }
		 });
	}
	
	/*  post_comment */
	function post_comment (f,id,lang)
	{
		var ok_post = true ;
		var mess_err='';
		
		var name = f.name.value; 
		if (name == '') {			 
			jAlert(js_lang['err_name_empty'],js_lang['error'], function() {	f.name.focus(); 	}); 			
			ok_post = false ;
			return false;
		}
		
		var re =/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/gi;
		email = f.email.value;
		if (email == '') {
			jAlert(js_lang['err_email_empty'],js_lang['error'], function() {	f.email.focus(); 	});   
			ok_post = false ;
			return false;
		}
		if (email != '' && email.match(re)==null) {
			jAlert(js_lang['err_email_invalid'],js_lang['error'], function() {	f.email.focus(); 	});  
			ok_post = false ;
			return false;
		}	 
		
		var title = f.title.value;
		if (title == '') {		
			jAlert(js_lang['err_title_empty'],js_lang['error'], function() {	f.title.focus(); 	});   
			ok_post = false ;
			return false;
		}
		 
		if ( f.security_code.value == '') {		
			jAlert(js_lang['err_security_code_empty'],js_lang['error'], function() {	f.security_code.focus(); 	});   
			ok_post = false ;
			return false;
		}
		
		if ( f.security_code.value != f.h_code.value) {		
			jAlert(js_lang['security_code_invalid'],js_lang['error'], function() {	f.security_code.focus(); 	});   
			ok_post = false ;
			return false;
		}
	
		var content= f.content.value;
		if ( content == '') {
			jAlert(js_lang['err_content_comment_empty'],js_lang['error'], function() {	f.content.focus(); 	});   
			ok_post = false ;
			return false;
		}
  	 
		if(ok_post) 
		{
			name = encodeURIComponent(name);
			email = encodeURIComponent(email);
			title = encodeURIComponent(title);
			content = encodeURIComponent(content);
 			
			var mydata =  "do=post&id="+id+"&name="+name+"&email="+email+"&title="+title+"&content=" + content+"&lang="+lang ;
			$.ajax({
				async: true,
				dataType: 'json',
				url:  ROOT+"modules/news/ajax/comment.php" ,
				type: 'POST',
				data: mydata ,
				success: function (data) {
					if(data.ok == 1)	{
						
						jAlert(js_lang['send_comment_success'],js_lang['announce'], function() {	$("#FormComment").hide(); $("#btn-comment").show();	} ); 						
					}	else {
						jAlert(js_lang['mess_error_post'],js_lang['error']);
					}	   
				}
			}) 		
		} 
		
		return false;
	}
	
	/*  del_comment */
	function del_comment(id,mem_id)
	{		
		Boxy.confirm("<div class='txtComple'>"+js_lang['confirm_del']+"</div>", function() {
			
			$.ajax(
			{
				
				url: ROOT+"modules/profile/ajax/comment.php" ,
				type:"POST",
				dataType: "json",
				data:"do=del&id="+id+"&mem_id="+mem_id,
				success:function(data, dataStatus){
					if(data.ok == 1)	{
						$('#cid_' + id).hide();
					}	else {
						 Boxy.alert(js_lang['mess_error_post'], function(){}, {autoClose: 1000}); 
					}					 
				} 
			});
	
		 }, {"title":"Xóa tin nhắn", "okTitle":"Đồng ý", "cancelTitle":"Đóng"});
		 return false;
	}
 
 	/*  onOver */
	function onOver(id, over)
	{
		if(over==1)	{
				$("#delc_" + id).css("display", "block");
		}	else	{
				$("#delc_" + id).css("display", "none");
		}
	} 
	
	
	
	this.ShowFormComment = ShowFormComment; 
	this.show_comment = show_comment; 
	this.post_comment = post_comment; 
	this.del_comment = del_comment;  
	this.onOver	= onOver;
	 


	
}();
