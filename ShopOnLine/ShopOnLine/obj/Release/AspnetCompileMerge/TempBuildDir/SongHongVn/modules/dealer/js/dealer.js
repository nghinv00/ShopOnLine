var ROOT = '';

String.prototype.replaceAll = function (searchStr, replaceStr) {
    var str = this;

    // escape regexp special characters in search string
    searchStr = searchStr.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');

    return str.replace(new RegExp(searchStr, 'gi'), replaceStr);
};

function LoadTown(ProvinceId) {
    $.ajax({
        url: '/Common/District',
        data: {
            ProvinceId: ProvinceId
        },
        type: 'POST',
        dataType: "html",
        success: function (data) {
            $('#District').html(data);
        }
    });
}

function changeCity(ProvinceId) {
    $('#DistrictId').val(ProvinceId);
    LoadTown(ProvinceId);
    searchStore(ProvinceId, null, null);
}

function changeTown(DistrictId) {
    var ProvinceId = $('#DistrictId').val();
    searchStore(ProvinceId, DistrictId, null);
}

function Search() {
    var ProvinceId = $('#DistrictId').val();
    var DistrictId = $('#DistrictId').val();
    var AgentAddress = $('#AgentAddress').val();

    searchStore(ProvinceId, DistrictId, AgentAddress)
    return false;
}

function searchStore(ProvinceId, DistrictId, AgentAddress) {
    //Load list sieu thi 		
    //var mydata =   "lang="+lang+"&cat_id="+cat_id+"&city="+city +'&state='+state+'&key='+key; 
    //$.ajax({
    //	async: true,
    //	dataType: 'json',
    //	url: ROOT+"modules/dealer/ajax/ajax.php?do=list_dealer",
    //	type: 'POST',
    //	data: mydata ,
    //	success: function (data) 
    //	{

    // datam = data.html.replaceAll('http://', 'https://');
    // console.log(datam);
    //		$("#ext_mess_result").html(data.mess_num);
    //		$("#list_dealer").html(datam);			 
    //	}
    //});

    $.ajax({
        url: '/Agent/Agent',
        data: {
            ProvinceId: ProvinceId,
            DistrictId: DistrictId,
            AgentAddress: AgentAddress
        },
        type: 'POST',
        dataType: "html",
        success: function (data) {
            $('#list_dealer').html(data);
        }
    });

    activeMap(0, ProvinceId, DistrictId);
}

// GEt loation in maps
function activeMap(idAgent, ProvinceId, DistrictId) {
    //$("#map-canvas").html("<div class=loading_maps >Đang tải bản đồ...</div>");
    //$.ajax({            
    //	type:"GET",                                             
    //	url:  ROOT+"modules/dealer/ajax/load_map.php?cat_id="+cat_id+"&city="+city+"&state="+state+"&key="+key+"&id="+id, 
    //	success: function( data ) { 

    //			$("#map-canvas").html(data);	
    //	}	
    //});	

    //if(id){
    //	$("#list_dealer li").removeClass('active');
    //	$("#item_"+id).addClass('active')	;
    //}
    return false;
}

function initStore(city, state, key) {
    searchStore(city, state, key);
}