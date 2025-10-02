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
                { dataField: "TenNPL", caption: "Tên vật tư", width: 180 },
                { dataField: "ColorNPL", caption: "Màu", width: 120 },
                { dataField: "Loai", caption: "Loại", width: 100 },
                { dataField: "DonVi", caption: "Đơn vị", width: 80 },
                { dataField: "KhoVai", caption: "Khổ vải", width: 100 },
                { dataField: "SoLuong", caption: "Số lượng", width: 100 },
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
                    {
                        location: "before",
                        widget: "dxButton",
                        options: {
                            text: "Import Excel",
                            icon: "doc",
                            elementAttr: { class: "btn-import-excel" },
                            onClick: function () {
                                // mở input file Excel
                                $("<input>")
                                    .attr({ type: "file", accept: ".xlsx,.xls" })
                                    .on("change", function (e) {
                                        let file = e.target.files[0];
                                        if (!file) return;

                                        let reader = new FileReader();
                                        reader.onload = function (ev) {
                                            let data = new Uint8Array(ev.target.result);
                                            let workbook = XLSX.read(data, { type: "array" });

                                            // lấy sheet đầu tiên
                                            let sheetName = workbook.SheetNames[0];
                                            let sheet = workbook.Sheets[sheetName];
                                            let rows = XLSX.utils.sheet_to_json(sheet);

                                            rows.forEach(r => {
                                                nplData.push({
                                                    TenNPL: r["Tên nguyên phụ liệu"] || "",
                                                    ColorNPL: r["Màu"] || "",
                                                    Loai: r["Loại"] || "",
                                                    DonVi: r["Đơn vị"] || "",
                                                    KhoVai: r["Khổ vải"] || "",
                                                    SoLuong: r["Số lượng"] || 0,
                                                    GhiChu: r["Ghi chú"] || ""
                                                });
                                            });

                                            $("#gridContainer").dxDataGrid("instance")
                                                .option("dataSource", nplData);
                                            DevExpress.ui.notify("Import thành công!", "success", 2000);
                                        };
                                        reader.readAsArrayBuffer(file);
                                    })
                                    .click();
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
            onShown: function () {
                initDropdownWithInput();
            },
            title: "Thêm / Sửa NPL",
            width: 800,
            maxHeight: 1200,  
            resizeEnabled: true, 
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
            IDNPL: editingId,
            TenNPL: form.find("[name=TenNpl]").val(),
            ColorNPL: form.find("[name=ColorNpl]").val(),
            Loai: form.find("[name=Loai]").val(),
            DonVi: form.find("[name=DonVi]").val(),
            SoLuong: form.find("[name=SoLuong]").val(),
        };

        let url = isEditMode ? "/NPL/Edit" : "/NPL/Create";

        $.ajax({
            url: url,
            type: "POST",
            data: {
                __RequestVerificationToken: token,
                ...newItem
            },
            success: function () {
                DevExpress.ui.notify(isEditMode ? "Cập nhật thành công" : "Thêm thành công", "success", 2000);

                if (isEditMode) {
                    // update mảng
                    const idx = nplData.findIndex(x => x.IDNPL === editingId);
                    if (idx !== -1) nplData[idx] = newItem;
                } else {
                    // backend trả về ID mới -> bạn có thể push
                    nplData.push(newItem);
                }

                $("#gridContainer").dxDataGrid("instance").option("dataSource", nplData);
                popup.hide();
            },
            error: function (xhr) {
                DevExpress.ui.notify("Lỗi khi lưu: " + xhr.responseText, "error", 3000);
            }
        });
    }
    function showImagePopup(images) {
        $("#imageList").empty();

        if (images.length === 0) {
            $("#imageList").append("<p>Không có ảnh</p>");
        } else {
            images.forEach(img => {
                const wrapper = $("<div>").addClass("image-item");

                $("<img>")
                    .attr("src", img.Urlimage)   // backend trả về Urlimage
                    .css({ width: "120px", height: "120px", objectFit: "cover", borderRadius: "6px" })
                    .appendTo(wrapper);

                $("<p>").text(img.Noidung || "").appendTo(wrapper); // hiện ghi chú
                $("#imageList").append(wrapper);
            });
        }

        $("#popupImageViewer").dxPopup({
            title: "Xem ảnh",
            width: 700,
            height: 500,
            visible: true,
            showCloseButton: true,
            hideOnOutsideClick: true,
            contentTemplate: function (contentElement) {
                contentElement.append($("#imageList"));

                // nút upload ảnh
                $("<button>")
                    .addClass("btn btn-success")
                    .text("📷 Upload ảnh")
                    .on("click", function () {
                        $("#fileUploadImage").click(); // trigger input file
                    })
                    .appendTo(contentElement);

                // input ẩn để chọn ảnh
                if ($("#fileUploadImage").length === 0) {
                    $("<input>")
                        .attr({ type: "file", id: "fileUploadImage", accept: "image/*", multiple: true })
                        .css("display", "none")
                        .on("change", function (e) {
                            let formData = new FormData();
                            for (let file of e.target.files) {
                                formData.append("image", file);
                            }
                            formData.append("idNPL", window.currentNPLId); // idNPL đang xem

                            $.ajax({
                                url: "/NoteNPL/Upload", // API upload ảnh
                                type: "POST",
                                data: formData,
                                processData: false,
                                contentType: false,
                                success: function () {
                                    DevExpress.ui.notify("Upload ảnh thành công!", "success", 2000);
                                    $("#popupImageViewer").dxPopup("instance").hide();
                                },
                                error: function () {
                                    DevExpress.ui.notify("Lỗi khi upload ảnh", "error", 2000);
                                }
                            });
                        })
                        .appendTo("body");
                }
            }
        });
    }

    function showFilePopup(files) {
        $("#fileList").empty();

        if (files.length === 0) {
            $("#fileList").append("<li>Không có file</li>");
        } else {
            files.forEach(f => {
                $("<li>")
                    .append($("<a>").attr("href", f.Urlfile).attr("target", "_blank").text(f.FileName || "Tải file"))
                    .append($("<span>").text(" - " + (f.Noidung || ""))) // ghi chú
                    .appendTo("#fileList");
            });
        }

        $("#popupFileViewer").dxPopup({
            title: "Xem file",
            width: 600,
            height: 400,
            visible: true,
            showCloseButton: true,
            hideOnOutsideClick: true,
            contentTemplate: function (contentElement) {
                contentElement.append($("#fileList"));

                // nút upload file
                $("<button>")
                    .addClass("btn btn-success")
                    .text("📂 Upload file")
                    .on("click", function () {
                        $("#fileUploadInput").click();
                    })
                    .appendTo(contentElement);

                // input ẩn để chọn file
                if ($("#fileUploadInput").length === 0) {
                    $("<input>")
                        .attr({ type: "file", id: "fileUploadInput", multiple: false })
                        .css("display", "none")
                        .on("change", function (e) {
                            let formData = new FormData();
                            formData.append("file", e.target.files[0]);
                            formData.append("idNPL", window.currentNPLId);

                            $.ajax({
                                url: "/NoteNPL/Upload", // API upload file
                                type: "POST",
                                data: formData,
                                processData: false,
                                contentType: false,
                                success: function () {
                                    DevExpress.ui.notify("Upload file thành công!", "success", 2000);
                                    $("#popupFileViewer").dxPopup("instance").hide();
                                },
                                error: function () {
                                    DevExpress.ui.notify("Lỗi khi upload file", "error", 2000);
                                }
                            });
                        })
                        .appendTo("body");
                }
            }
        });
    }

    //function openUploadPopup() {
    //    $("#popupUploadImage").dxPopup({
    //        title: "Upload ảnh cho NPL",
    //        width: 600,
    //        height: "auto",
    //        visible: true,
    //        showCloseButton: true,
    //        dragEnabled: true,
    //        hideOnOutsideClick: true,
    //        contentTemplate: function (contentElement) {
    //            contentElement.append(`
    //            <form id="uploadForm" enctype="multipart/form-data">
    //                <div id="imageInputs"></div>
    //                <button type="button" class="btn btn-secondary" onclick="addImageInput()">➕ Thêm ảnh</button>
    //                <br/><br/>
    //                <button type="submit" class="btn btn-success">Upload</button>
    //            </form> 
    //        `);

    //            $("#uploadForm").on("submit", function (e) {
    //                e.preventDefault();
    //                let formData = new FormData(this);

    //                $.ajax({
    //                    url: '/NPLImage/UploadTemp',
    //                    type: 'POST',
    //                    data: formData,
    //                    processData: false,
    //                    contentType: false,
    //                    success: function (res) {
    //                        DevExpress.ui.notify("Upload thành công!", "success", 2000);
    //                        $("#popupUploadImage").dxPopup("instance").hide();
    //                    },
    //                    error: function () {
    //                        DevExpress.ui.notify("Có lỗi xảy ra", "error", 2000);
    //                    }
    //                });
    //            });
    //        }
    //    });
    //}
    // ---------------- CAMERA & PREVIEW -----------------
    document.getElementById("btnOpenCamera").addEventListener("click", async function () {
        try {
            let stream = await navigator.mediaDevices.getUserMedia({ video: true });
            const video = document.getElementById("videoCam");
            video.srcObject = stream;
            video.style.display = "block";
            document.getElementById("btnCapture").style.display = "inline-block";
            document.getElementById("btnCloseCamera").style.display = "inline-block";
            window._cameraStream = stream;
        } catch (err) {
            alert("Không thể mở camera: " + err.message);
        }
    });

    document.getElementById("btnCloseCamera").addEventListener("click", function () {
        const video = document.getElementById("videoCam");
        video.style.display = "none";
        document.getElementById("btnCapture").style.display = "none";
        document.getElementById("btnCloseCamera").style.display = "none";
        if (window._cameraStream) {
            window._cameraStream.getTracks().forEach(track => track.stop());
            window._cameraStream = null;
        }
    });

    document.getElementById("btnUploadImage").addEventListener("click", function () {
        document.getElementById("fileInput").click();
    });

    document.getElementById("fileInput").addEventListener("change", function (event) {
        previewImages(event.target.files);
    });

    function previewImages(files) {
        const preview = document.getElementById("imagePreview");
        preview.innerHTML = "";
        for (let file of files) {
            const reader = new FileReader();
            reader.onload = function (e) {
                addPreviewImage(e.target.result, file);
            };
            reader.readAsDataURL(file);
        }
    }

    // Chụp ảnh
    document.getElementById("btnCapture").addEventListener("click", function () {
        const video = document.getElementById("videoCam");
        const canvas = document.getElementById("canvasPhoto");
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        const ctx = canvas.getContext("2d");
        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

        canvas.toBlob(function (blob) {
            const file = new File([blob], "photo_" + Date.now() + ".png", { type: "image/png" });
            addPreviewImage(URL.createObjectURL(file), file);
        }, "image/png");
    });

    function addPreviewImage(src, file) {
        const preview = document.getElementById("imagePreview");

        const wrapper = document.createElement("div");
        wrapper.className = "img-wrapper";

        const img = document.createElement("img");
        img.src = src;
        img.className = "preview-img";

        const del = document.createElement("button");
        del.innerHTML = "×";
        del.className = "btn-del-img";
        del.onclick = function () {
            wrapper.remove();
        };

        wrapper.appendChild(img);
        wrapper.appendChild(del);
        preview.appendChild(wrapper);
    }
    function initDropdownWithInput() {
        // Màu
        $("#ColorNpl").dxSelectBox({
            placeholder: "Chọn hoặc nhập màu...",
            dataSource: new DevExpress.data.CustomStore({
                loadMode: "raw",
                load: function () {
                    return $.getJSON("/NPL/GetColors");
                }
            }),
            searchEnabled: true,
            acceptCustomValue: true,
            onValueChanged: function (e) {
                console.log("Màu:", e.value);
            }
        });

        // Đơn vị
        $("#DonVi").dxSelectBox({
            placeholder: "Chọn hoặc nhập đơn vị...",
            dataSource: new DevExpress.data.CustomStore({
                loadMode: "raw",
                load: function () {
                    return $.getJSON("/NPL/GetUnits");
                }
            }),
            searchEnabled: true,
            acceptCustomValue: true,
            onValueChanged: function (e) {
                console.log("Đơn vị:", e.value);
            }
        });

        // Loại
        $("#Loai").dxSelectBox({
            placeholder: "Chọn hoặc nhập loại...",
            dataSource: new DevExpress.data.CustomStore({
                loadMode: "raw",
                load: function () {
                    return $.getJSON("/NPL/GetTypes");
                }
            }),
            searchEnabled: true,
            acceptCustomValue: true,
            onValueChanged: function (e) {
                console.log("Loại:", e.value);
            }
        });
    }

    return {
        init: init
    };
})();
