let allDonHang = [];
let rowsPerPage = 10;
let currentPage = 1;

// ================== Hàm load danh sách đơn hàng ==================
function loadDonHang() {
    fetch("http://localhost:57440/DonHang/GetAll")
        .then(response => response.json())
        .then(data => {
            allDonHang = data.filter(dh => {
                const tong = dh.S + dh.M + dh.L + dh.XL;
                return tong > 0;
            });
            currentPage = 1;
            renderDonHangPage();
            renderPagination();
        })
        .catch(err => console.error(err));
}

// ================== Render bảng theo trang ==================
function renderDonHangPage() {
    const tbody = document.querySelector("#donhangTable tbody");
    tbody.innerHTML = "";

    const startIndex = (currentPage - 1) * rowsPerPage;
    const pageData = allDonHang.slice(startIndex, startIndex + rowsPerPage);

    pageData.forEach(dh => {
        const tong = dh.S + dh.M + dh.L + dh.XL;
        const ngayDat = dh.NgayDat
            ? new Date(dh.NgayDat).toLocaleString("vi-VN", {
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit"
            })
            : "";

        const row = `<tr>
            <td>${dh.IDDH}</td>
            <td></td>
            <td>${ngayDat}</td>
            <td>${dh.KhachHang}</td>
            <td>${dh.ProductName}</td>
            <td>${dh.Color}</td>            
            <td>${tong}</td>
        </tr>`;
        tbody.innerHTML += row;
    });

    const pageInput = document.getElementById("pageInput");
    if (pageInput) pageInput.value = currentPage;
}

// ================== Render phân trang ==================
function renderPagination() {
    const pagination = document.getElementById("pagination");
    pagination.innerHTML = "";

    const totalPages = Math.ceil(allDonHang.length / rowsPerPage);
    if (totalPages <= 1) return;

    // Nút ←
    const prevBtn = document.createElement("button");
    prevBtn.textContent = "←";
    prevBtn.disabled = currentPage === 1;
    prevBtn.onclick = () => {
        if (currentPage > 1) {
            currentPage--;
            renderDonHangPage();
            renderPagination();
        }
    };
    pagination.appendChild(prevBtn);

    // Các nút trang
    for (let i = 1; i <= totalPages; i++) {
        if (i === 1 || i === totalPages || Math.abs(i - currentPage) <= 1) {
            const btn = document.createElement("button");
            btn.textContent = i;
            if (i === currentPage) btn.classList.add("active");
            btn.onclick = () => {
                currentPage = i;
                renderDonHangPage();
                renderPagination();
            };
            pagination.appendChild(btn);
        } else if (
            (i === currentPage - 2 || i === currentPage + 2) &&
            totalPages > 6
        ) {
            const dots = document.createElement("span");
            dots.textContent = "...";
            dots.style.margin = "0 5px";
            pagination.appendChild(dots);
        }
    }

    // Nút →
    const nextBtn = document.createElement("button");
    nextBtn.textContent = "→";
    nextBtn.disabled = currentPage === totalPages;
    nextBtn.onclick = () => {
        if (currentPage < totalPages) {
            currentPage++;
            renderDonHangPage();
            renderPagination();
        }
    };
    pagination.appendChild(nextBtn);
}

// ================== Thay đổi số dòng/trang ==================
function changeRowsPerPage(value) {
    rowsPerPage = parseInt(value);
    currentPage = 1;
    renderDonHangPage();
    renderPagination();
}

// ================== Nhảy đến trang nhập ==================
function jumpToPage() {
    const input = document.getElementById("pageInput");
    const page = parseInt(input.value);
    const totalPages = Math.ceil(allDonHang.length / rowsPerPage);

    if (!isNaN(page) && page >= 1 && page <= totalPages) {
        currentPage = page;
        renderDonHangPage();
        renderPagination();
    } else {
        Swal.fire("Lỗi", "Số trang không hợp lệ", "error");
    }
}

// ================== Popup sửa đơn hàng ==================
function editDonHang(id) {
    fetch(`/DonHang/Edit/${id}`)
        .then(res => res.text())
        .then(html => {
            Swal.fire({
                title: 'Sửa đơn hàng',
                html: html,
                showCancelButton: true,
                confirmButtonText: 'Lưu',
                cancelButtonText: 'Hủy',
                width: '700px',
                focusConfirm: false,
                preConfirm: () => {
                    const form = Swal.getPopup().querySelector('form');
                    if (form) {
                        const formData = new FormData(form);
                        return fetch(form.action, {
                            method: form.method,
                            body: formData
                        })
                            .then(res => res.json())
                            .then(data => {
                                if (data.success) {
                                    loadDonHang();
                                    Swal.fire('Thành công', 'Đã sửa đơn hàng', 'success');
                                } else {
                                    Swal.showValidationMessage(data.message || 'Có lỗi xảy ra');
                                }
                            })
                            .catch(() => Swal.showValidationMessage('Có lỗi xảy ra'));
                    }
                }
            });
        })
        .catch(err => console.error(err));
}

// ================== Popup xóa ==================
function confirmDelete(id) {
    Swal.fire({
        title: 'Bạn có chắc muốn xóa?',
        text: "Hành động này không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy',
        background: 'linear-gradient(135deg, #EEE9DA, #BDCDD6)',
        color: '#004d40',
        confirmButtonColor: '#6096B4',
        cancelButtonColor: '#93BFCF'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/DonHang/Delete/${id}`;
        }
    });
}

// ================== Popup xem ảnh ==================
function viewImages(images) {
    if (!images || images.length === 0) {
        Swal.fire({
            title: 'Đơn hàng này chưa có ảnh',
            icon: 'info',
            background: 'linear-gradient(135deg, #EEE9DA, #BDCDD6)',
            color: '#004d40',
            confirmButtonColor: '#6096B4'
        });
        return;
    }
    const htmlContent = images.map(img => `<img src="${img}" class="popup-img">`).join('');
    Swal.fire({
        title: 'Ảnh đơn hàng',
        html: htmlContent,
        width: '650px',
        background: 'linear-gradient(135deg, #FFFDF7, #EEE9DA)',
        color: '#004d40',
        confirmButtonText: 'Đóng',
        confirmButtonColor: '#6096B4',
        showCloseButton: true
    });
}

// ================== Popup xem file ==================
function viewFiles(files) {
    if (!files || files.length === 0) {
        Swal.fire({
            title: 'Đơn hàng này chưa có file',
            icon: 'info',
            background: 'linear-gradient(135deg, #EEE9DA, #BDCDD6)',
            color: '#004d40',
            confirmButtonColor: '#6096B4'
        });
        return;
    }
    const htmlContent = files.map(f =>
        `<div class="popup-file"><a href="${f}" target="_blank">${f.split('/').pop()}</a></div>`).join('');
    Swal.fire({
        title: 'File đơn hàng',
        html: htmlContent,
        width: '650px',
        background: 'linear-gradient(135deg, #FFFDF7, #EEE9DA)',
        color: '#004d40',
        confirmButtonText: 'Đóng',
        confirmButtonColor: '#6096B4',
        showCloseButton: true
    });
}

// ================== Load khi DOM sẵn sàng ==================
document.addEventListener("DOMContentLoaded", loadDonHang);
