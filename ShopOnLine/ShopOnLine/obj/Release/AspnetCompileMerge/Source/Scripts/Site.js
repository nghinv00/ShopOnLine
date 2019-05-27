
jQuery(function ($) {
    //$.datepicker.regional["vi-VN"] =
    //    {
    //        closeText: "Đóng",
    //        prevText: "Trước",
    //        nextText: "Sau",
    //        currentText: "Hôm nay",
    //        monthNames: ["Tháng một", "Tháng hai", "Tháng ba", "Tháng tư", "Tháng năm", "Tháng sáu", "Tháng bảy", "Tháng tám", "Tháng chín", "Tháng mười", "Tháng mười một", "Tháng mười hai"],
    //        monthNamesShort: ["Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười", "Mười một", "Mười hai"],
    //        dayNames: ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"],
    //        dayNamesShort: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy"],
    //        dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
    //        weekHeader: "Tuần",
    //        dateFormat: "dd/mm/yy",
    //        firstDay: 1,
    //        isRTL: false,
    //        showMonthAfterYear: false,
    //        yearSuffix: ""
    //    };

    //$.datepicker.setDefaults($.datepicker.regional["vi-VN"]);
});

$(document).ready(function () {

    ActiveMenu();
    FixMenuChild();

    function GetUrlLever2() {
        var url = document.location.toString(); //url
        var e_url = ''; //edited url
        var url_l2 = '';
        var p = 0; //position
        var p2 = 0; //position 2
        var p3 = 0;
        p = url.indexOf("//");
        e_url = url.substring(p + 2);

        var arr = e_url.split('/');
        if (arr[1] != '')
            url_l2 = arr[1];
        else url_l2 = arr[0];
        return url_l2;
    }
    function ActiveMenu() {
        var url_active = GetUrlLever2();
        var i = 0;
        jQuery.each($("#menu-lte"), function () {
            jQuery.each($("li > a", this), function () {
                var href = $(this).attr('href');
                if (href == $('#hdurl').val()) {
                    $(this).parent().addClass('active');
                    var indexname = $(this).parent().attr('index');
                    RootMenuActive(indexname);
                }
            });
        });

    }
    function FixMenuChild() {
        var navWidth = $("#nav-menu").width();
        if ($("#nav-menu li.activeMenu:first").length > 0) {
            var li = $("#nav-menu li.activeMenu:first").offset().left + 50;
            var ul = $("#nav-menu li.activeMenu:first ul");
            var left = li - parseInt($(ul).width() / 2);
            if (left < 0)
                left = 0;
            $(ul).css('left', left);
        }
    }
    function RootMenuActive(index) {
        jQuery.each($("#menu-lte > li"), function () {
            if (index == $(this).attr('index')) {
                $(this).addClass('active');
            }
        });
    }
    $('#btnSave').click(function () {
        $("#formview form").submit();
    });
    $('#btnBack').click(function () {
        window.history.back();
    });
    $("#btnGhiLai").click(function () {
        $("#formview form").submit();
    });
    $("#btnSearch").click(function () {
        $("#formview form").submit();
    });
    $('.main-view .grid tr').hover(function () {
        var val = $("> td:first", this).html();
        if (val != null && val != '&nbsp;') {
            $('.grid tr').removeClass('tr-hover');
            $(this).addClass('tr-hover');
        }
    });
    /*=============== check box ========*/
    $('.grid #cbxList').click(function () {
        checkAll_OnClick('cbxList', 'cbxItem');
    });

    function checkAll_OnClick(headerCheckboxId, remainingCheckboxesName) {
        var checked = $("#" + headerCheckboxId).prop('checked');
        if (checked == true) {
            $("input[type='checkbox']").prop("checked", true);

        } else {
            $("input[type='checkbox']").prop("checked", false);

        }
    }

    $('.grid #cbxItem').click(function () {
        checkItem_OnClick('cbxList', 'cbxItem');
    });
    function checkItem_OnClick(headerCheckboxId, remainingCheckboxesName) {

    }


    /*=============== end check box ========*/


    $('.modal').on('hide.bs.modal', function () {
        $(".modal input").val("");
        $(".modal textarea").val("");
    })
});



function ThongBao(title) {
    $(document).ready(function () {
        window.setTimeout(function () {
            $("<div>" + title + "</div>").dialog({
                title: "Thông báo",
                modal: true,
                buttons: { "Đóng": function () { $(this).dialog("close"); } }
            }).dialog('open');
        });
    });
};

function danhsachcvId(name) {
    var checked = $("input:checked[name=" + name + "]");
    var id = checked.val();
    return id;
};
function danhsachId(name) {
    var checked = $("input:checked[name=" + name + "]");
    var id = '';
    jQuery.each($("input:checked[name=" + name + "]"), function () {

        if (id == '')
            id = $(this).val();
        else
            id += ',' + $(this).val();
    });
    return id;
};
function ClearCheck(name) {
    $("input[name='" + name + "']").removeAttr("checked");
}
/***********file dinh kem **************************/
function rcheck() {
    $("input[type='file']").each(function () {
        $(this).rules("add", {
            extension: "jpg,jpeg,png,pdf,doc,docx,xlsx,xls"
        });
    });
}
//xoa file dinh kem
$(".attachFiles .delAttach").click(function () {
    var item = $("#hdDelFile").val();
    if (item != "0")
        item = item + "," + $(this).attr('id');
    else
        item = $(this).attr('id');
    $("#hdDelFile").val(item);
    $(this).parent().hide();
});

$(".btnDeleteFile").click(function () {
    var item = $("#hdDelFile").val();
    if (item != "0")
        item = item + "," + $(this).attr('value');
    else
        item = $(this).attr('value');
    $("#hdDelFile").val(item);
    $(this).parent().hide();
});

var counter = 1;
function AddFileUpload() {
    var div = document.createElement('DIV');
    div.innerHTML = '<input id="FileAttach' + counter + '" name = "FileAttach' + counter +
        '" type="file" />' +
        '<input id="Button' + counter + '" type="button" ' +
        'value="Xóa" onclick = "RemoveFileUpload(this)" class="btn btn-default" />';
    if (document.getElementById("FileUploadContainer") != null)
        document.getElementById("FileUploadContainer").appendChild(div);
    counter++;
    rcheck();
}
function RemoveFileUpload(div) {
    if (document.getElementById("FileUploadContainer") != null)
        document.getElementById("FileUploadContainer").removeChild(div.parentNode);
}


var counter2 = 1;
function AddFileUpload2() {
    var div = document.createElement('DIV');
    div.innerHTML = '<input id="FileAttach' + counter + '" name = "FileAttach' + counter +
        '" type="file" />' +
        '<input id="Button' + counter + '" type="button" ' +
        'value="Xóa" onclick = "RemoveFileUpload2(this)" class="btn btn-default" />';
    if (document.getElementById("FileUploadContainer2") != null)
        document.getElementById("FileUploadContainer2").appendChild(div);
    counter++;
    rcheck();
}
function RemoveFileUpload2(div) {
    if (document.getElementById("FileUploadContainer2") != null)
        document.getElementById("FileUploadContainer2").removeChild(div.parentNode);
}

var counter_ = 1;
function AddFileUpload_Tag(id_tag) {
    var div = document.createElement('DIV');
    div.innerHTML = '<input id="FileAttach' + counter_ + '" name = "FileAttach' + counter_ +
        '" type="file" />' +
        '<input id="Button' + counter_ + '" type="button" ' +
        'value="Xóa" onclick = "RemoveFileUpload_Tag(this,\'' + id_tag + '\')" class="btn btn-default" />';
    if (document.getElementById(id_tag) != null)
        document.getElementById(id_tag).appendChild(div);
    counter_++;
    rcheck();
}
function RemoveFileUpload_Tag(div, id_tag) {
    if (document.getElementById(id_tag) != null)
        document.getElementById(id_tag).removeChild(div.parentNode);
}



var counterdialog = 1;
function AddFileUploadDiaLog() {
    var div = document.createElement('DIV');
    div.innerHTML = '<input id="FileAttach' + counter + '" name = "FileAttach' + counter +
        '" type="file" />' +
        '<input id="Button' + counter + '" type="button" ' +
        'value="Xóa" onclick = "RemoveFileUploadDiaLog(this)" class="btn btn-default" />';
    if (document.getElementById("FileUploadContainer") != null)
        document.getElementById("FileUploadContainer").appendChild(div);
    counter++;

}
function RemoveFileUploadDiaLog(div) {
    if (document.getElementById("FileUploadContainer") != null)
        document.getElementById("FileUploadContainer").removeChild(div.parentNode);
}
/***************end file ****************************************/



$(document).ready(function () {
    $("#txtUserName,#txtPassword").focus(function () {
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $('#formview form').submit();
            }
        });
    })
});

//tu dong can chinh do cao form them moi
$(document).ready(function () {
    var wrapper = $('.content-wrapper').outerHeight();
    $(".form-input").css('min-height', wrapper);
    var form_input = $('.form-input').outerHeight() || 0;
    if (form_input > 0) {
        $(".content").css('min-height', wrapper - 40);
    }

});
//fix slect2 to dialog
//if ($.ui && $.ui.dialog && $.ui.dialog.prototype._allowInteraction) {
//    var ui_dialog_interaction = $.ui.dialog.prototype._allowInteraction;
//    $.ui.dialog.prototype._allowInteraction = function (e) {
//        if ($(e.target).closest('.select2-dropdown').length) return true;
//        return ui_dialog_interaction.apply(this, arguments);
//    };
//}
/*************************accordion***************************/
$(document).ready(function () {
    $('.accordion').click(function () {
        $(this).toggleClass("active");
        $(this).next('.panel-accordion').toggleClass("show");
    });
});
/*************************end accordion***************************/
//ModalDialog
(function () {
    window.spawn = window.spawn || function (gen) {
        function continuer(verb, arg) {
            var result;
            try {
                result = generator[verb](arg);
            } catch (err) {
                return Promise.reject(err);
            }
            if (result.done) {
                return result.value;
            } else {
                return Promise.resolve(result.value).then(onFulfilled, onRejected);
            }
        }
        var generator = gen();
        var onFulfilled = continuer.bind(continuer, 'next');
        var onRejected = continuer.bind(continuer, 'throw');
        return onFulfilled();
    };
    window.showModalDialog = window.showModalDialog || function (url, arg, opt) {
        url = url || ''; //URL of a dialog
        arg = arg || null; //arguments to a dialog
        opt = opt || 'dialogWidth:300px;dialogHeight:200px'; //options: dialogTop;dialogLeft;dialogWidth;dialogHeight or CSS styles
        var caller = showModalDialog.caller.toString();
        var dialog = document.body.appendChild(document.createElement('dialog'));
        dialog.setAttribute('style', opt.replace(/dialog/gi, ''));
        dialog.innerHTML = '<a href="#" id="dialog-close" style="position: absolute; top: 0; right: 4px; font-size: 20px; color: #000; text-decoration: none; outline: none;">&times;</a><iframe id="dialog-body" src="' + url + '" style="border: 0; width: 100%; height: 100%;"></iframe>';
        document.getElementById('dialog-body').contentWindow.dialogArguments = arg;
        document.getElementById('dialog-close').addEventListener('click', function (e) {
            e.preventDefault();
            dialog.close();
        });
        dialog.showModal();
        //if using yield
        if (caller.indexOf('yield') >= 0) {
            return new Promise(function (resolve, reject) {
                dialog.addEventListener('close', function () {
                    var returnValue = document.getElementById('dialog-body').contentWindow.returnValue;
                    document.body.removeChild(dialog);
                    resolve(returnValue);
                });
            });
        }
        //if using eval
        var isNext = false;
        var nextStmts = caller.split('\n').filter(function (stmt) {
            if (isNext || stmt.indexOf('showModalDialog(') >= 0)
                return isNext = true;
            return false;
        });
        dialog.addEventListener('close', function () {
            var returnValue = document.getElementById('dialog-body').contentWindow.returnValue;
            document.body.removeChild(dialog);
            nextStmts[0] = nextStmts[0].replace(/(window\.)?showModalDialog\(.*\)/g, JSON.stringify(returnValue));
            eval('{\n' + nextStmts.join('\n'));
        });
        throw 'Execution stopped until showModalDialog is closed';
    };
})();

/*************************tree view by toggle***************************/
function toggle() {
    for (var i = 0; i < arguments.length; i++) {
        with (document.getElementById(arguments[i])) {
            if (className.indexOf('removed') > -1) {
                className = className.replace('removed');
            }
            else {
                className += ' removed';
            }
        }
    }
}
/*************************end tree view by toggle***************************/

$.fn.extend({
    treeview: function (o) {

        var openedClass = 'glyphicon-minus-sign';
        var closedClass = 'glyphicon-plus-sign';

        if (typeof o != 'undefined') {
            if (typeof o.openedClass != 'undefined') {
                openedClass = o.openedClass;
            }
            if (typeof o.closedClass != 'undefined') {
                closedClass = o.closedClass;
            }
        };

        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        tree.find('li').each(function () {
            if ($(this).has("ul").length > 0) {
                var branch = $(this); //li with children ul
                branch.prepend("<i class='indicator glyphicon " + closedClass + "'></i>");
                branch.addClass('branch');
                branch.on('click', function (e) {
                    if (this == e.target) {
                        var icon = $(this).children('i:first');
                        icon.toggleClass(openedClass + " " + closedClass);
                        $(this).children().children().toggle();
                    }
                })
                branch.children().children().toggle();
            }
            else {
                var branch = $(this); //li with children ul
                branch.prepend("<i class='glyphicon'></i>");
            }
        });
        //fire event from the dynamically added icon
        tree.find('.branch .indicator').each(function () {
            $(this).on('click', function () {
                $(this).closest('li').click();
            });
        });
        //fire event to open branch if the li contains an anchor instead of text
        tree.find('.branch>a').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
        //fire event to open branch if the li contains a button instead of text
        tree.find('.branch>button').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
    }
});

function DisableButton(id) {
    if ($("#" + id).attr('disabled') == 'disabled')
        return true;
    return false;
}

function ParseStringToDateTime(ngay) {

    var thongtin = ngay.split('/');

    return new Date(thongtin[2], thongtin[1], thongtin[0])

}

