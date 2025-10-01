function openModal(url, title) {
    $("#modalTitle").text(title);

    // load partial view (_Create) từ server
    $("#modalBody").load(url, function () {
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
    $("#donhangGrid").dxDataGrid("instance").refresh();
    DevExpress.ui.notify({
        message: "Đơn hàng đã được lưu thành công!",
        type: "success",
        displayTime: 2500,
        position: {
            my: "top center",
            at: "top center"
        }
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
        showError("TenChiTietDonHang", "Vui lòng nhập tên");
        isValid = false;
    }

    // Validate Ngày đặt
    var NgayGiaoHang = $("#NgayGiaoHang").val();
    if (!NgayGiaoHang) {
        showError("NgayGiaoHang", "Vui lòng chọn ngày đặt");
        isValid = false;
    }

    // Validate Product
    var productId = $("#IdProduct").val();
    if (!productId) {
        showError("Product", "Vui lòng chọn sản phẩm");
        isValid = false;
    }

    // Validate Color
    var colorId = $("#colorSelect").val();
    if (!colorId) {
        showError("Color", "Vui lòng chọn màu sắc");
        isValid = false;
    }

    return isValid;
}

// Hàm lưu chi tiết đơn hàng
$(document).on("submit", "#orderForm", function (e) {
    e.preventDefault();

    // Validate form trước khi submit
    if (!validateForm()) {
        return;
    }

    var orderId = 3; // test
    var productId = parseInt($("#IdProduct").val());
    var colorId = parseInt($("#colorSelect").val());
    var TenChiTietDonHang = $("#TenChiTietDonHang").val().trim();
    var NgayGiaoHang = $("#NgayGiaoHang").val();

    var sizes = [];
    $("#sizesContainer input[type='number']").each(function () {
        var qty = parseInt($(this).val()) || 0;
        var variantId = parseInt($(this).data("variant-id") || $(this).attr("name").match(/\[(.*?)\]/)[1]);

        if (qty > 0) {
            sizes.push({
                IdProductVariant: variantId,
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

    $.ajax({
        url: "/ChiTietDonHang/CreateMultiple",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(dto),
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

// Khi chọn Product → load màu sắc
$(document).on("change", "#IdProduct", function () {
    var productId = $(this).val();
    var $colorSelect = $("#colorSelect");
    var $sizeContainer = $("#sizesContainer");

    // Clear error khi user thay đổi
    $("#ProductError").text("");
    $("#IdProduct").removeClass("is-invalid");

    // Reset color + size khi đổi product
    $colorSelect.empty().append('<option value="">-- Chọn màu --</option>');
    $colorSelect.prop("disabled", true);
    $sizeContainer.empty();

    if (!productId) return;

    $.get("/ChiTietDonHang/GetColorsByProduct/" + productId, function (data) {
        if (data.length === 0) {
            $colorSelect.append('<option value="">(Không có màu)</option>');
            return;
        }

        data.forEach(function (c) {
            $colorSelect.append(`<option value="${c.Id}">${c.Name}</option>`);
        });
        $colorSelect.prop("disabled", false);
    });
});

// Khi chọn Color → load size
$(document).on("change", "#colorSelect", function () {
    var productId = $("#IdProduct").val();
    var colorId = $(this).val();
    var $container = $("#sizesContainer");

    // Clear error khi user thay đổi
    $("#ColorError").text("");
    $("#colorSelect").removeClass("is-invalid");

    $container.empty();

    if (!productId || !colorId) return;

    $.get("/ChiTietDonHang/GetSizesByProductAndColor/" + productId + "/" + colorId, function (data) {
        if (data.length === 0) {
            $container.append("<p>Không có size nào</p>");
            return;
        }

        data.forEach(function (s) {
            $container.append(`
                <div class="form-group d-flex align-items-center mb-2">
                    <label class="me-2" style="width: 60px;">${s.Name}</label>
                    <input type="number" name="SizeQuantities[${s.VariantId}]" 
                           class="form-control" min="0" value="0" style="width:100px;" />
                </div>
            `);
        });
    });
});

$(document).on("change", "#NgayDat", function () {
    $("#NgayDatError").text("");
    $(this).removeClass("is-invalid");
});