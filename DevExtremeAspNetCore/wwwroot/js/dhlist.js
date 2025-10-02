const grid = $("#donhangGrid").dxDataGrid({
    dataSource: donHangData,
    keyExpr: "IDDH",
    showBorders: true,
    paging: { pageSize: 10 },
    pager: {
        visible: true,
        showNavigationButtons: false,
        showInfo: true,
        infoText: "Trang {0} / {1} (Tổng {2} dòng)",
    },
    filterRow: { visible: true },
    columns: [
        { dataField: "IDDH", caption: "Mã đơn hàng" },
        { dataField: "TenKhachHang", caption: "Tên đơn hàng" },
        { dataField: "TenKhachHang", caption: "Khách hàng" },
        { dataField: "NgayDat", caption: "Ngày đặt", dataType: "date", format: "dd/MM/yyyy" },
        { dataField: "TenSanPham", caption: "Sản phẩm" },
        { dataField: "TongSoLuong", caption: "Số Lượng" }
    ],
    onContentReady: function (e) {
        if ($("#customPageSize").length === 0) {
            const pageSizeContainer = $("#donhangGrid .dx-pager");
            pageSizeContainer.append(
                '<span class="custom-page-size-label">Số dòng:</span>' +
                '<div id="customPageSize" style="width:90px; display:inline-block;"></div>'
            );

            $("#customPageSize").dxSelectBox({
                items: [5, 10, 20, 50, 100],
                value: grid.option("paging.pageSize"),
                acceptCustomValue: true,
                searchEnabled: true,
                placeholder: "Nhập...",
                onCustomItemCreating: function (args) {
                    const val = parseInt(args.text, 10);
                    if (!isNaN(val) && val > 0) {
                        grid.option("paging.pageSize", val);
                        grid.refresh();
                        args.customItem = val;
                    }
                    return args.customItem;
                },
                onValueChanged: function (e) {
                    const val = parseInt(e.value, 10);
                    if (!isNaN(val) && val > 0) {
                        grid.option("paging.pageSize", val);
                        grid.refresh();
                    }
                }
            });
        }
    }
}).dxDataGrid("instance");
$(document).on("submit", "#formCreateDonHang", function (e) {
    e.preventDefault(); // ngăn submit form bình thường

    var form = $(this);

    $.ajax({
        url: form.attr("action"),
        type: form.attr("method"),
        data: form.serialize(),
        success: function (res) {
            if (res.success) {
                // Đóng modal
                var modalEl = document.getElementById('createDonHangModal');
                var modal = bootstrap.Modal.getInstance(modalEl);
                modal.hide();

                // Reload danh sách
                location.reload();
            } else {
                // Nếu validation fail, render lại form
                $("#createDonHangBody").html(res);
            }
        },
        error: function () {
            alert("Có lỗi xảy ra khi lưu đơn hàng!");
        }
    });
});

