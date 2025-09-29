import React from "react";
import "./Content/css/DonHangList.css";

const DonHangList = ({ data }) => {
    return (
        <div className="donhang-container">
            <div className="donhang-header">
                <h2>📦 Danh sách Đơn hàng</h2>
                <button className="btn btn-add">+ Thêm đơn hàng</button>
            </div>

            <table className="donhang-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Ngày đặt</th>
                        <th>Khách hàng</th>
                        <th>Product</th>
                        <th>Color</th>
                        <th>Ảnh</th>
                        <th>File</th>
                        <th colSpan={7}>Size</th>
                        <th>Tổng SL</th>
                        <th>Thao tác</th>
                    </tr>
                    <tr>
                        <th colSpan={7}></th>
                        <th>XXS</th>
                        <th>XS</th>
                        <th>SM</th>
                        <th>MED</th>
                        <th>LRG</th>
                        <th>XLG</th>
                        <th>XXL</th>
                        <th colSpan={2}></th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, idx) => (
                        <tr key={idx}>
                            <td>{item.id}</td>
                            <td>{item.ngayDat}</td>
                            <td>{item.khachHang}</td>
                            <td>{item.product}</td>
                            <td>{item.color}</td>
                            <td>
                                {item.image ? (
                                    <img src={item.image} alt="Ảnh" className="product-img" />
                                ) : (
                                    <span className="no-img">Không có ảnh</span>
                                )}
                            </td>
                            <td>{item.file || "Không có file"}</td>
                            <td>{item.xxs}</td>
                            <td>{item.xs}</td>
                            <td>{item.sm}</td>
                            <td>{item.med}</td>
                            <td>{item.lrg}</td>
                            <td>{item.xlg}</td>
                            <td>{item.xxl}</td>
                            <td>{item.tong}</td>
                            <td>
                                <button className="btn btn-edit">✏ Sửa</button>
                                <button className="btn btn-delete">🗑 Xóa</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default DonHangList;
