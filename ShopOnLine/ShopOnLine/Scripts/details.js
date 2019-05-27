var CommentUser = {

    postcomment: function (rating, name, email, content, productguid) {

        var re = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/gi;

        if (rating == '' || rating == 0) {
            alert('Vui lòng cho điểm đánh giá.');
            $("#hvote").focus();
            return false;
        }

        if (name == '') {
            alert('Vui lòng nhập họ tên.');
            $("#com_name").focus();
            return false;
        }

        if (email == '') {
            alert('Vui lòng nhập Email');
            $("#com_email").focus();
            return false;
        }

        if (email != '' && email.match(re) == null) {
            alert('Dịa chỉ Email không hợp lệ');
            $("#com_email").focus();
            return false;
        }

        if (content.length < 10) {
            alert('Vui lòng nhập nội dung tối thiểu 10 ký tự');
            $("#txtComment").focus();
            return false;
        }

        $('#loader').fadeIn('slow');

        $.ajax({
            url: '/Common/PostComment',
            data: {
                rating: rating,
                name: name,
                email: email,
                content: content,
                productguid: productguid
            },
            type: "POST",
            dataType: "html",
            success: function (data, textStatus) {
                changpagecomment(1);
            },
            complete: function (data) {
                $("#com_name").val('');
                $("#com_email").val('');
                $("#txtComment").val('');

                $('#loader').fadeOut('fast');
            }
        });
    }

}


var Cart = {

    DsProduct: function () {
        $.ajax({
            url: '/Cart/DsProduct',
            type: "POST",
            dataType: "html",
            success: function (data, textStatus) {
                $('#list-item').html(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            },
            complete: function (data) {
                $('#loader').fadeOut('fast');
            }
        });

    },

    AddItemCart: function (ProductGuid, Quantity, SectionGuid, SizeGuid) {


        if (SectionGuid == '' || SectionGuid == null) {
            alert('Bạn chưa chọn mục sản phẩm. Vui lòng chọn mục sản phẩm');
            $('#SectionGuid').focus();
            return;
        }

        if (SizeGuid == '' || SizeGuid == null) {
            alert('Bạn chưa chọn kích thước sản phẩm. Vui lòng chọn kích thước sản phẩm');
            $('#SizeGuid').focus();
            return;
        }

        var data = {
            ProductGuid: ProductGuid,
            Quantity: Quantity,
            SectionGuid: SectionGuid,
            SizeGuid: SizeGuid
        };

        $('#loader').fadeIn('slow');

        $.ajax({
            url: '/Cart/AddItem',
            data: data,
            type: "POST",
            dataType: "json",
            success: function (data, textStatus) {
                alert('Thêm giỏ vào hàng thành công')
            },
            complete: function (data) {
                $('#loader').fadeOut('fast');
                
            }
        });

    },
    DeleteItemCart: function (ProductGuid, SectionGuid, SizeGuid) {
        var data = {
            ProductGuid: ProductGuid,
            SectionGuid: SectionGuid,
            SizeGuid: SizeGuid
        }

        $.ajax({
            url: '/Cart/DeleteItem',
            data: data,
            type: "POST",
            dataType: "json",
            success: function (data, textStatus) {
                alert('Xóa trống sản phẩm thành công');

                $("#loader").fadeIn();
                Cart.DsProduct();
            },
            complete: function (data) {

                $('#loader').fadeOut('fast');
            }
        });
    },

    BuyCart: function (ProductGuid, Quantity, SectionGuid, SizeGuid) {

        if (SectionGuid == '') {
            alert('Bạn chưa chọn mục sản phẩm. Vui lòng chọn mục sản phẩm');
            $('#SectionGuid').focus();
            return;
        }

        if (SizeGuid == '') {
            alert('Bạn chưa chọn kích thước sản phẩm. Vui lòng chọn kích thước sản phẩm');
            $('#SizeGuid').focus();
            return;
        }

        var data = {
            ProductGuid: ProductGuid,
            Quantity: Quantity,
            SectionGuid: SectionGuid,
            SizeGuid: SizeGuid
        };

        $('#loader').fadeIn('slow');

        $.ajax({
            url: '/Cart/AddItem',
            data: data,
            type: "POST",
            dataType: "json",
            success: function (data, textStatus) {

            },
            complete: function (data) {
                $('#loader').fadeOut('fast');
                window.location.href = '/gio-hang';
            }
        });
    },

    DeleteAll: function () {

        $.ajax({
            url: '/Cart/DeleteAll',
            type: "POST",
            dataType: "json",
            success: function (data, textStatus) {
                alert('Xóa trống giỏ hàng thành công');
                $("#loader").fadeIn();

                Cart.DsProduct();
            },
            complete: function (data) {
                $('#loader').fadeOut('fast');
            }
        });
    }
}

 


$(document).ready(function () {


})
