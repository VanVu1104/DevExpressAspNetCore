var NPL = (function () {
    let grid, popup, popupForm;
    let nplData = [];
    let token, uploadUrl;
    let isEditMode = false, editingId = null;

    function init(data, _token, _uploadUrl) {
        nplData = data || [];
        token = _token;
        uploadUrl = _uploadUrl;

        renderGrid();
        renderPopup();

        $("#btnAddNPL").on("click", function () {
            isEditMode = false;
            editingId = null;
            $("#popupNPL form")[0].reset();
            popup.option("title", "Thêm NPL");
            popup.show();
        });
    }

    function renderGrid() {
        grid = $("#gridContainer").dxDataGrid({
            dataSource: nplData,
            keyExpr: "IDNPL",
            columnAutoWidth: true,
            columnResizingMode: "nextColumn",
            allowColumnResizing: true,
            showBorders: true,
            paging: { pageSize: 10 },
            pager: {
                visible: true,
                showNavigationButtons: false,
                showInfo: true,
                infoText: "Trang {0} / {1} (Tổng {2} dòng)",
            },
            filterRow: { visible: true },
            searchPanel: { visible: true, placeholder: "Tìm kiếm..." },
            columns: [
                { dataField: "TenNPL", caption: "Tên NPL", width: 180 },
                { dataField: "ColorNPL", caption: "Màu", width: 120 },
                { dataField: "Loai", caption: "Loại", width: 120 },
                { dataField: "DonVi", caption: "Đơn vị", width: 80 },
                { dataField: "SoLuong", caption: "Số lượng", width: 100 },
                { dataField: "KhoVai", caption: "Khổ vải", width: 100 },
                {
                    caption: "Ảnh",
                    width: 100,
                    cellTemplate: function (container, options) {
                        $("<button>")
                            .addClass("btn-view btn-view-photo")
                            .text("Xem ảnh")
                            .on("click", function () {
                                showImagePopup(options.data.Images || []);
                            })
                            .appendTo(container);
                    }
                },
                {
                    caption: "File",
                    width: 100,
                    cellTemplate: function (container, options) {
                        $("<button>")
                            .addClass("btn-view btn-view-file")
                            .text("Xem file")
                            .on("click", function () {
                                showFilePopup(options.data.UrlFiles || []);
                            })
                            .appendTo(container);
                    }
                },
                { dataField: "GhiChu", caption: "Ghi chú", width: 200 },
                {
                    caption: "Thao tác",
                    width: 60,
                    cellTemplate: function (container, options) {
                        container.empty();
                        const $wrap = $('<div class="action-wrap"></div>').appendTo(container);

                        // nút edit
                        $('<div>').dxButton({
                            hint: 'Sửa',
                            stylingMode: 'text',
                            template: function () {
                                return $('<img>').attr('src', '/images/ic_edit.png')
                                    .css({ width: '20px', height: '20px' });
                            },
                            onClick: function () {
                                isEditMode = true;
                                editingId = options.data.IDNPL;

                                const form = $("#popupNPL form");
                                form.find("[name=TenNpl]").val(options.data.TenNPL);
                                form.find("[name=ColorNpl]").val(options.data.ColorNPL);
                                form.find("[name=Loai]").val(options.data.Loai);
                                form.find("[name=DonVi]").val(options.data.DonVi);
                                form.find("[name=SoLuong]").val(options.data.SoLuong);
                                form.find("[name=VatTu]").val(options.data.VatTu);

                                popup.option("title", "Sửa NPL");
                                popup.show();
                            }
                        }).appendTo($wrap);

                        // nút delete
                        $('<div>').dxButton({
                            hint: 'Xóa',
                            type: 'danger',
                            stylingMode: 'text',
                            template: function () {
                                return $('<img>').attr('src', '/images/ic_delete.png')
                                    .css({ width: '20px', height: '20px' });
                            },
                            onClick: function () {
                                DevExpress.ui.dialog.confirm("Bạn có chắc muốn xóa NPL này?", "Xác nhận")
                                    .done(function (confirmed) {
                                        if (confirmed) {
                                            $.ajax({
                                                url: '/NPL/Delete/' + encodeURIComponent(options.data.IDNPL),
                                                type: 'POST',
                                                data: { __RequestVerificationToken: token },
                                                success: function () {
                                                    DevExpress.ui.notify("Đã xóa", "success", 2000);
                                                    nplData = nplData.filter(x => x.IDNPL !== options.data.IDNPL);
                                                    $("#gridContainer").dxDataGrid("instance")
                                                        .option('dataSource', nplData);
                                                },
                                                error: function (xhr) {
                                                    DevExpress.ui.notify("Lỗi khi xóa: " + xhr.status, "error", 3000);
                                                }
                                            });
                                        }
                                    });
                            }
                        }).appendTo($wrap);
                    }
                }
            ],
            toolbar: {
                items: [
                    {
                        location: "before",
                        widget: "dxButton",
                        options: {
                            text: "Thêm NPL",
                            icon: "plus",
                            elementAttr: { class: "btn-add-npl" },
                            onClick: function () {
                                isEditMode = false;
                                editingId = null;
                                $("#popupNPL form")[0].reset();
                                popup.option("title", "Thêm NPL");
                                popup.show();
                            }
                        }
                    },
                    "searchPanel"
                ]
            },
            onContentReady: function () {
                if ($("#customPageSize").length === 0) {
                    const pageSizeContainer = $("#gridContainer .dx-pager");
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
    }

    function renderPopup() {
        popup = $("#popupNPL").dxPopup({
            title: "Thêm / Sửa NPL",
            width: 700,
            height: "auto",
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            toolbarItems: [
                {
                    widget: "dxButton",
                    toolbar: "bottom",
                    location: "after",
                    options: {
                        text: "Lưu",
                        type: "success",
                        onClick: saveData
                    }
                },
                {
                    widget: "dxButton",
                    toolbar: "bottom",
                    location: "after",
                    options: {
                        text: "Hủy",
                        onClick: function () { popup.hide(); }
                    }
                }
            ]
        }).dxPopup("instance");
    }

    function saveData() {
        const form = $("#popupNPL form");
        const newItem = {
            TenNpl: form.find("[name=TenNpl]").val(),
            ColorNpl: form.find("[name=ColorNpl]").val(),
            Loai: form.find("[name=Loai]").val(),
            DonVi: form.find("[name=DonVi]").val(),
            SoLuong: form.find("[name=SoLuong]").val(),
            VatTu: form.find("[name=VatTu]").val()
        };
        nplData.push(newItem);
        $("#gridContainer").dxDataGrid("instance").refresh();
        popup.hide();
    }

    function showImagePopup(images) {
        $("#imageList").empty();
        if (images.length === 0) {
            $("#imageList").append("<p>Không có ảnh</p>");
        } else {
            images.forEach(src => {
                $("<img>")
                    .attr("src", src)
                    .css({ width: "120px", height: "120px", objectFit: "cover", borderRadius: "6px" })
                    .appendTo("#imageList");
            });
        }
        $("#popupImageViewer").dxPopup({
            title: "Xem ảnh",
            width: 600,
            height: 400,
            visible: true,
            showCloseButton: true,
            hideOnOutsideClick: true
        });
    }

    function showFilePopup(files) {
        $("#fileList").empty();
        if (files.length === 0) {
            $("#fileList").append("<li>Không có file</li>");
        } else {
            files.forEach(url => {
                $("<li>")
                    .append($("<a>").attr("href", url).attr("target", "_blank").text("Tải file"))
                    .appendTo("#fileList");
            });
        }
        $("#popupFileViewer").dxPopup({
            title: "Xem file",
            width: 500,
            height: 300,
            visible: true,
            showCloseButton: true,
            hideOnOutsideClick: true
        });
    }

    return {
        init: init
    };
})();
