function openModal(url, title) {
    $("#modalTitle").text(title);
    // load partial view (_Create) từ server 
    $("#modalBody").load(url, function ()
    {
        $("#formModal").show();
    });
}
function closeModal() {
    $("#formModal").hide();
    $("#modalBody").empty();
}
// Sau khi submit form thành công 
function onFormSuccess() {
    closeModal();
    $("#chiTietDonHangGrid").dxDataGrid("instance").refresh();
    DevExpress.ui.notify(
        {
            message: "Đơn hàng đã được lưu thành công!", type: "success", displayTime: 2500,
            position: { my: "top center", at: "top center" }
        });
}
// Hàm xóa tất cả error messages 
function clearErrors() {
    $(".text-danger[id$='Error']").text("");
    $(".form-control").removeClass("is-invalid");
}
// Hàm hiển thị error cho một field 
function showError(fieldId, message) {
    $("#" + fieldId + "Error").text(message);
    $("#" + fieldId).addClass("is-invalid");
}
// Hàm validate form 
function validateForm() {
    clearErrors();
    var isValid = true;
    // Validate Tên đơn hàng 
    var tenChiTietDonHang = $("#TenChiTietDonHang").val().trim();
    if (!tenChiTietDonHang) {
        showError("TenChiTietDonHang", "Vui lòng nhập tên"); isValid = false;
    }
    // Validate Ngày giao hàng
    var NgayGiaoHang = $("#NgayGiaoHang").val();
    if (!NgayGiaoHang) {
        showError("NgayGiaoHang", "Vui lòng chọn ngày giao hàng");
        isValid = false;
    } else {
        // Chuyển string thành Date object
        var today = new Date();
        today.setHours(0, 0, 0, 0); // reset giờ về 00:00 để so sánh chính xác
        var selectedDate = new Date(NgayGiaoHang);

        if (selectedDate < today) {
            showError("NgayGiaoHang", "Ngày giao hàng phải lớn hơn hoặc bằng ngày hiện tại");
            isValid = false;
        }
    }
    // Validate Product 
    var productId = $("#IdProduct").val();
    if (!productId) {
        showError("Product", "Vui lòng chọn sản phẩm"); isValid = false;
    }
    // Validate Color 
    var colorId = $("#IdColor").val();
    if (!colorId) {
        showError("Color", "Vui lòng chọn màu sắc"); isValid = false;
    }
    if (!isValid) {
        scrollToFirstError();
    }
    return isValid;
}
// Hàm lưu chi tiết đơn hàng 
$(document).on("submit", "#orderForm", function (e) {
    e.preventDefault();
    // Validate form trước khi submit 
    if (!validateForm()) { return; }
    var orderId = 3;
    // test 
    var productId = parseInt($("#IdProduct").val());
    var colorId = parseInt($("#IdColor").val());
    var TenChiTietDonHang = $("#TenChiTietDonHang").val().trim();
    var NgayGiaoHang = $("#NgayGiaoHang").val();
    var sizes = [];
    $("#sizesContainer input[type='number']").each(function () {
        var qty = parseInt($(this).val()) || 0;
        var sizeId = parseInt($(this).data("size-id") ||
            $(this).attr("name").match(/\[(.*?)\]/)[1]);
        if (qty > 0) {
            sizes.push({
                IdSize: sizeId,
                SoLuong: qty
            });
        }
    });
    // Size không bắt buộc, cho phép sizes.length === 0 
    var dto = {
        IdDonHang: orderId,
        TenChiTietDonHang: TenChiTietDonHang,
        NgayGiaoHang: NgayGiaoHang,
        IdProduct: productId,
        IdColor: colorId,
        Sizes: sizes
    };
    console.log(dto);

    $.ajax(
        {
            url: "/ChiTietDonHang/CreateMultiple", type: "POST", contentType: "application/json", data: JSON.stringify(dto),
            success: function (res) {
                if (res.success) {
                    onFormSuccess();
                } else {
                    alert(res.message || "Có lỗi xảy ra khi lưu!");
                }
            },
            error: function (xhr) {
                console.error(xhr.responseText);
                alert("Lỗi khi gọi API!");
            }
        });
});
$(document).on("change", "#NgayDat", function () {
    $("#NgayDatError").text(""); $(this).removeClass("is-invalid");
});
// Hàm scroll đến field lỗi đầu tiên
function scrollToFirstError() {
    var firstError = $(".is-invalid").first();
    if (firstError.length > 0) {
        // Scroll đến element với offset để không bị che bởi header (nếu có)
        $('html, body').animate({
            scrollTop: firstError.offset().top - 100 // 100px offset từ top
        }, 300); // 300ms animation

        // Focus vào field để người dùng có thể sửa luôn
        firstError.focus();
    }
}