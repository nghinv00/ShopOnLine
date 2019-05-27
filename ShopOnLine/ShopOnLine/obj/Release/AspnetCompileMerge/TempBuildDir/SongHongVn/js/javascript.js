var isIE = navigator.userAgent.toLowerCase().indexOf("msie") == -1 ? false : true;
var isIE6 = navigator.userAgent.toLowerCase().indexOf("msie 6") == -1 ? false : true;
var isIE7 = navigator.userAgent.toLowerCase().indexOf("msie 7") == -1 ? false : true;
var ie45, ns6, ns4, dom;
if (navigator.appName == "Microsoft Internet Explorer")
    ie45 = parseInt(navigator.appVersion) >= 4;
else if (navigator.appName == "Netscape")
{
    ns6 = parseInt(navigator.appVersion) >= 5;
    ns4 = parseInt(navigator.appVersion) < 5
}
dom = ie45 || ns6;
function getobj(id)
{
    el = document.getElementById(id);
    return el
}
function showobj(id)
{
    obj = getobj(id); els = dom ? obj.style : obj;
    if (dom) {
        els.display = ""
    }
    else if (ns4) {
        els.display = "show"
    }
}

function hideobj(id) {
    obj = getobj(id); els = dom ? obj.style : obj;
    if (dom) {
        els.display = "none"
    } else if (ns4) {
        els.display = "hide"
    }
}

function openPopUp(url, windowName, w, h, scrollbar) {
    var winl = (screen.width - w) / 2;
    var wint = (screen.height - h) / 2;
    winprops = "height=" + h + ",width=" + w + ",top=" + wint + ",left=" + winl + ",scrollbars=" + scrollbar;
    win = window.open(url, windowName, winprops); if (parseInt(navigator.appVersion) >= 4) {
        win.window.focus()
    }
}

var win = null;

function NewWindow(mypage, myname, w, h, scroll, pos) {
    if (pos == "random") {
        LeftPosition = screen.width ? Math.floor(Math.random() * (screen.width - w)) : 100;
        TopPosition = screen.height ? Math.floor(Math.random() * (screen.height - h - 75)) : 100
    } if (pos == "center") {
        LeftPosition = screen.width ? (screen.width - w) / 2 : 100;
        TopPosition = screen.height ? (screen.height - h) / 2 : 100
    } else if (pos != "center" && pos != "random" || pos == null) {
        LeftPosition = 0; TopPosition = 20
    }

    settings = "width=" + w + ",height=" + h + ",top=" + TopPosition +
        ",left=" + LeftPosition + ",scrollbars=" + scroll +
        ",location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=yes";

    win = window.open(mypage, myname, settings)

} function is_num(event, f) {
    if (event.srcElement) {
        kc = event.keyCode
    } else {
        kc = event.which
    } if ((kc < 47 || kc > 57) && kc != 8 && kc != 0)
        return false;
    return true
}


function bookmarksite(title, url) {
    if (window.sidebar)
        window.sidebar.addPanel(title, url, "");
    else if (window.opera && window.print) {
        var elem = document.createElement("a");
        elem.setAttribute("href", url);
        elem.setAttribute("title", title);
        elem.setAttribute("rel", "sidebar");
        elem.click()
    } else if (document.all)
        window.external.AddFavorite(url, title)

}


function setHomepage(url) {
    if (document.all) {
        document.body.style.behavior = "url(#default#homepage)";
        document.body.setHomePage(url)
    } else if (window.sidebar) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect")
            } catch (e) {
                alert("this action was aviod by your browser if you want to enable please enter about:config in your address line,and change the value of signed.applets.codebase_principal_support to true")
            }
        } var prefs = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefBranch); prefs.setCharPref("browser.startup.homepage", url)
    }
}

window.rwt = function (obj, type, id) {
    try {
        if (obj === window) {
            obj = window.event.srcElement; while (obj) {
                if (obj.href) break;
                obj = obj.parentNode
            }
        }
        obj.href = ROOT + "?" + cmd + "=mod:advertise|type:" + type + "|lid:" + id; obj.onmousedown = ""
    }
    catch (o) {
    }

    return true
};

function load_Statistics() {
    //$.ajax({
    //    async: true,
    //    dataType: "json",
    //    url: ROOT + "load_ajax.php?do=statistics",
    //    type: "POST",
    //    success: function (data) {
    //        $("#stats_online").html(data.online);
    //        $("#stats_totals").html(data.totals);
    //        $("#stats_member").html(data.mem_online)
    //    }
    //})
}

function show_popupBanner() {
    var mydata = "lang=vn";
    //$.ajax({
    //    async: true,
    //    dataType: "json",
    //    url: ROOT + "load_ajax.php?do=popupBanner",
    //    type: "POST",
    //    data: mydata,
    //    success: function (data) {
    //        if (data.show == 1) {
    //            vnTRUST.show_overlay_popup("popupBanner", "", data.html, {
    //                background: { "background-color": "transparent" },
    //                border: { "background-color": "transparent", padding: "0px" },
    //                title: { display: "none" },
    //                content: { padding: "0px", width: data.width + "px" },
    //                pos_type: "fixed", position: "center-center"
    //            })
    //        }
    //    }
    //})
}

function initLoaded() {
    load_Statistics()
}

function ActiveMenu() {
    console.log('active menu')
    var hdurl = $('#hdurl').val();
    $('#vnt-header .vnt-menutop ul li a').removeClass('current')
    $('#vnt-header .vnt-menutop ul li a').each(function (e) {
        var href = $(this).attr('href');
        if (hdurl == href) {
            $(this).addClass('current');
        }
    })

}

$(document).ready(function () {
    vnTRUST.goTopStart();
    $(window).scroll(function (event) {
        if ($(window).scrollTop() > 0) {
            $("#vnt-header").addClass("divfixed")
        } else {
            $("#vnt-header").removeClass("divfixed")
        }
    })
});

$(document).ready(function () {
    $(".iconSearch .divIcon").click(function (event) {
        if (!$(this).parents(".iconSearch").hasClass("show")) {
            $(this).parents(".iconSearch").addClass("show");
            $("#maskmenu").addClass("show")
        } else {
            $(this).parents(".iconSearch").removeClass("show");
            $("#maskmenu").removeClass("show")
        }
    }); $(".iconSupport .divIcon").click(function (event) {
        if (!$(this).parents(".iconSupport").hasClass("show")) {
            $(this).parents(".iconSupport").addClass("show");
            $("#maskmenu").addClass("show")
        } else {
            $(this).parents(".iconSupport").removeClass("show");
            $("#maskmenu").removeClass("show")
        }
    });

    $(".iconSocial .divIcon").click(function (event) {
        if (!$(this).parents(".iconSocial").hasClass("show")) {
            $(this).parents(".iconSocial").addClass("show");
            $("#maskmenu").addClass("show")
        } else {
            $(this).parents(".iconSupport").removeClass("show");
            $("#maskmenu").removeClass("show")
        }
    });

    $(window).bind("click", function (event) {
        var $clicked = $(event.target);
        if (!$clicked.parents().hasClass("iconSearch")) {
            $(".iconSearch").removeClass("show")
        }

        if (!$clicked.parents().hasClass("iconSupport")) {
            $(".iconSupport").removeClass("show")
        }

        if (!$clicked.parents().hasClass("iconSocial")) {
            $(".iconSocial").removeClass("show")
        }

        if (!$(".iconSocial").hasClass("show") &&
            !$(".iconSocial").hasClass("show") &&
            !$(".iconSearch").hasClass("show") &&
            !$(".iconSupport").hasClass("show") &&
            !$clicked.parents().hasClass("submenu")
            && !$clicked.hasClass("submenu")) {
            $("#maskmenu").removeClass("show")
        }
    })
});

$(document).ready(function () {
    $(window).load(function () {
        $(".wrapSubmenu").masonry(
            {
                itemSelector: ".subItem",
                columnWidth: 318,
                gutter: 30
            })
    })
});



$(document).ready(function () {
    $(".vnt-menutop > ul > li").hover(function () {
        if ($(this).find(".submenu").hasClass("submenu") ||
            $(this).find(".submenu1").hasClass("submenu1")) { 
            $(".iconSearch").removeClass("show");
            $(".iconSupport").removeClass("show");
            $(".iconSocial").removeClass("show");
            $("#maskmenu").addClass("show")
        }
    }, function () {
        if ($(this).find(".submenu").hasClass("submenu") ||
            $(this).find(".submenu1").hasClass("submenu1")) {
            $("#maskmenu").removeClass("show")
        }
    })
});

$(document).ready(function () {
    $(".menuCategory ul li").each(function () {
        var countsize = $(this).find("ul li").size();
        if (countsize > 0) {
            $(this).addClass("hassSub");
            var $heightA = $(this).find("a:first").innerHeight();
            $(this).append("<div class='button-submenu' style='height:" + $heightA + "px'></div>")
        }
        console.log($(this).find("a:first") + +$(this).find("a:first").innerHeight())

    });

    $(".menuCategory ul li").find("ul:first").css({ display: "none" });
    $(".menuCategory ul li a.current").parent().addClass("show");
    $(".menuCategory ul li a.current").parent().find("ul:first").css({ display: "block" });
    $(".menuCategory ul li .button-submenu").click(function () {
        if (!$(this).parent().hasClass("show")) {
            $(this).parent().find("ul:first").stop().slideToggle(700);
            $(this).parent().addClass("show"); $(this).parent().siblings("li").each(function (e) {
                if ($(this).hasClass("show")) {
                    $(this).find("ul:first").stop().slideToggle(700);
                    $(this).removeClass("show")
                }
            })
        } else {
            $(this).parent().find("ul:first").stop().slideToggle(700); $(this).parent().removeClass("show")
        }
    })
});