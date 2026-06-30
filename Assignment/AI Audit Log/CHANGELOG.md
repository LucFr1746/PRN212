# Changelog

## 1. Quy định ghi Changelog

File này dùng để ghi lại các thay đổi quan trọng trong quá trình thực hiện bài tập, lab, assignment hoặc project.

Nguyên tắc ghi changelog:

- Chỉ ghi những gì đã hoàn thành thật sự.
- Không ghi kế hoạch nếu chưa thực hiện.
- Mỗi thay đổi nên có ngày, nội dung, người thực hiện và minh chứng.
- Nếu có AI hỗ trợ, cần ghi rõ AI đã hỗ trợ phần nào.
- Nếu có commit GitHub, cần ghi link commit.
- Nếu có lỗi đã sửa, cần ghi rõ lỗi, nguyên nhân và cách xử lý.

---

## 2. Thông tin project

| Thông tin             | Nội dung                                                                               |
| --------------------- | -------------------------------------------------------------------------------------- |
| Môn học               | Application Development on .NET                                                        |
| Mã môn học            | PRN212                                                                                 |
| Lớp                   | SE20A02                                                                                |
| Học kỳ                | SU26                                                                                   |
| Tên bài tập / Project | FU Mini Hotel Management System                                                        |
| Tên sinh viên / Nhóm  | Đoàn Thế Lực                                                                           |
| MSSV / Danh sách MSSV | DE200523                                                                               |
| Giảng viên hướng dẫn  | QuangLTN3                                                                              |
| Repository URL        | https://github.com/LucFr1746/PRN212.git                                                |
| Ngày bắt đầu          | 2026-06-24T16:00:00.000Z                                                               |
| Ngày hoàn thành       | 2026-06-24T17:30:00.000Z                                                               |

---

## 3. Tổng quan các phiên bản/giai đoạn

| Phiên bản/Giai đoạn | Thời gian               | Nội dung chính                                                                                 | Trạng thái  |
| ------------------- | ----------------------- | ---------------------------------------------------------------------------------------------- | ----------- |
| Phase 01            |                         |                                                                                                | Not Started |
| Phase 02            |                         |                                                                                                | Not Started |
| Phase 03            |                         |                                                                                                | Not Started |
| Phase 04            |                         |                                                                                                | Not Started |
| Phase 05            |                         |                                                                                                | Not Started |
| Phase 06            | 2026-05-23 ~ 2026-05-23 | Secure Authentication Refactoring & Super Admin Enhancements                                   | Completed   |
| Phase 07            | 2026-05-28 ~ 2026-05-28 | Reclaim Ownership OTP Verification & Identity Normalization                                    | Completed   |
| Phase 08            | 2026-05-29 ~ 2026-05-29 | Components System Visual Explorer & Workspace Architecture                                     | Completed   |
| Phase 09            | 2026-05-30 ~ 2026-05-30 | Secure OAuth Integration & Settings Change Password Overhaul                                   | Completed   |
| Phase 10            | 2026-05-31 ~ 2026-05-31 | Email Normalization Correction, Multi-Email Support & Password Recovery Overhaul               | Completed   |
| Phase 11            | 2026-05-31 ~ 2026-05-31 | Multi-Connection OAuth Linking, Per-Session Revocation & Pending Link Confirmation             | Completed   |
| Phase 12            | 2026-06-01 ~ 2026-06-01 | Account Deletion Lifecycle & Modular Monolith Transition                                       | Completed   |
| Phase 13            | 2026-06-02 ~ 2026-06-02 | Automatic Username System & Public Profile Routing                                             | Completed   |
| Phase 14            | 2026-06-03 ~ 2026-06-03 | Persisting Avatar Source, Re-engineering Experience/Achievements Settings & Form Consistency   | Completed   |
| Phase 15            | 2026-06-05 ~ 2026-06-06 | Repository Analysis Engine with Real-time SSE Progress Streaming                               | Completed   |
| Phase 16            | 2026-06-15 ~ 2026-06-16 | AI CV Assessment, Source-Code Provider Integrations, and Session Inactivity Management         | Completed   |
| Phase 17            | 2026-06-17 ~ 2026-06-18 | Candidate Assessment SSE Streaming, Repository Reset, and Dashboard UI Enhancements           | Completed   |
| Phase 18            | 2026-06-22 ~ 2026-06-22 | Sidebar Navigation Redesign, Accordion Groups & Bi-directional URL Tab Synchronization         | Completed   |
| Phase 19            | 2026-06-24 ~ 2026-06-24 | WPF Hotel Management scaffold, safe deletion checks, centralized validations, serializable transactions & xUnit tests | Completed |

---

# [Phase 19]

## Thông tin giai đoạn

- **Thời gian thực hiện:** 2026-06-24 ~ 2026-06-24
- **Mô tả giai đoạn:** WPF Hotel Management scaffold, safe deletion checks, centralized validations, serializable transactions & xUnit tests
- **Trạng thái hiện tại:** Completed

## Thay đổi chi tiết

### Added

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
| --: | ----------------- | --------------- | --------------------- | ---------- |
|   1 | Khởi tạo cấu trúc dự án WPF quản lý khách sạn và giải pháp đa phân tầng lớp (.sln, .csproj). | Đoàn Thế Lực | Assignment/ | GitHub Commit |
|   2 | Tạo các business models và DbContext ánh xạ các bảng bằng EF Core Database-First. | Đoàn Thế Lực | Assignment/BusinessObjects/ | GitHub Commit |
|   3 | Xây dựng các lớp truy xuất dữ liệu DAO, repositories và service cơ bản. | Đoàn Thế Lực | Assignment/DataAccess/, Repositories/, Services/ | GitHub Commit |
|   4 | Cài đặt các cửa sổ WPF: LoginWindow, MainWindow và Customer/Room/Booking dialogs. | Đoàn Thế Lực | Assignment/*.xaml | GitHub Commit |
|   5 | Tạo helper dùng chung `ValidationHelper` kiểm định định dạng sđt và email bằng Regex. | Đoàn Thế Lực | Assignment/Shared/ValidationHelper.cs | GitHub Commit |
|   6 | Tạo dự án unit test `Assignment.Tests` sử dụng xUnit để tự động hóa kiểm thử logic nghiệp vụ. | Đoàn Thế Lực | Assignment.Tests/ | GitHub Commit |

### Changed

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
| --: | ----------------- | --------------- | --------------------- | ---------- |
|   1 | Căn chỉnh các nút dialog hành động bằng Grid.Column tránh chồng đè layout trong WPF. | Đoàn Thế Lực | CustomerDialog, RoomDialog, BookingDialog | GitHub Commit |
|   2 | Tích hợp Popup lịch hiển thị động vào DatePicker template trong App.xaml. | Đoàn Thế Lực | Assignment/App.xaml | GitHub Commit |
|   3 | Đăng ký sự kiện DateChanged để tự động tính toán tổng giá phòng thực tế trong BookingDialog. | Đoàn Thế Lực | BookingDialog.xaml.cs | GitHub Commit |
|   4 | Cải tiến phương thức sinh ID tự động cho BookingReservation trong DAO để bảo đảm null-safe. | Đoàn Thế Lực | BookingReservationDAO.cs | GitHub Commit |
|   5 | Tiêu chuẩn hóa đối chiếu Email khách hàng case-insensitive sang SQL tương thích EF Core. | Đoàn Thế Lực | CustomerDAO.cs | GitHub Commit |
|   6 | Triển khai cơ chế giao dịch đồng thời an toàn Serializable Transaction khi đặt phòng. | Đoàn Thế Lực | BookingReservationDAO.cs | GitHub Commit |

### Removed

| STT | Nội dung xóa bỏ | Người thực hiện | File/Module liên quan | Minh chứng |
| --: | --------------- | --------------- | --------------------- | ---------- |
|   1 | Loại bỏ cơ chế sửa đổi trạng thái phòng đơn giản cũ trong DAO khi xóa phòng có liên kết lịch sử để chuyển sang ném Exception an toàn. | Đoàn Thế Lực | RoomInformationDAO.cs | GitHub Commit |

## AI có hỗ trợ không?

- [x] Có
- [ ] Không

## Minh chứng liên quan

| Loại minh chứng | Nhãn | Nội dung |
| --------------- | ---- | -------- |
| Commit/PR       | Initial commit: WPF hotel management app | https://github.com/LucFr1746/PRN212/commit/76103b0b7d7e7467d2ca36b155fb1641af3d6070 |
| Commit/PR       | Add calendar popup, pricing calc, and DAO checks | https://github.com/LucFr1746/PRN212/commit/81d0c02ff2e2ff95724b6a06804917d2e63d2b99 |
| Commit/PR       | Add validation, DB overlap checks, and unit tests | https://github.com/LucFr1746/PRN212/commit/8b3e73e6b0c36c87a89a81cf9252bb230fcdb5c6 |

---

## 4. Tổng kết thay đổi cuối project

### 4.1. Các chức năng đã hoàn thành

```text
- Khởi dựng thành công ứng dụng quản lý khách sạn WPF Hotel Management kết nối Database bằng EF Core.
- Thiết kế giao diện Dark/Light gọn gàng, nút bấm dialog phân bổ cột chuẩn, tích hợp popup chọn lịch.
- Thực thi thành công cơ chế khóa an toàn dữ liệu, ném ngoại lệ khi cố xóa Phòng/Khách hàng có lịch sử đặt phòng.
- Hoàn tất logic kiểm định định dạng email/sđt và logic kiểm tra trùng lịch phòng (Overlap) ở mức nghiệp vụ.
- Thiết lập cơ chế Serializable SQL Transactions chống ghi đè/đặt trùng phòng đồng thời từ nhiều yêu cầu.
- Xây dựng 15+ ca kiểm thử tự động xUnit Passed hoàn chỉnh bao phủ các logic Helper và Booking validation.
```

---

### 4.2. Các chức năng chưa hoàn thành

```text
- Chưa triển khai đầy đủ cơ chế phân quyền chi tiết (RBAC) mà mới chỉ phân biệt tài khoản admin tĩnh và khách hàng qua cơ sở dữ liệu.
```

---

### 4.3. Cải thiện chính

```text
- Tối ưu hóa độ an toàn giao dịch đồng thời và tính toàn vẹn dữ liệu cơ sở dữ liệu của ứng dụng quản lý khách sạn.
- Tăng cường trải nghiệm người dùng trên các màn hình dialog nhập liệu và chọn lịch.
```

---

### 4.4. Tổng kết project

```text
Giai đoạn này mang lại nền móng vững chắc cho dự án FU Mini Hotel Management System về cả mặt cấu trúc ứng dụng N-Tier, giao diện người dùng WPF thân thiện lẫn hệ thống nghiệp vụ đặt phòng bảo mật đồng thời và kiểm định dữ liệu nghiêm ngặt.
```

---

### 4.5. Hướng cải thiện tiếp theo

```text
1. Triển khai phân quyền nâng cao động dựa trên cơ sở dữ liệu thay vì tài khoản admin cấu hình tĩnh trong appsettings.json.
2. Thêm biểu đồ thống kê doanh thu và báo cáo trực quan dạng dashboard trong MainWindow.
```

---

## 5. Cam kết cập nhật Changelog

Sinh viên/nhóm cam kết rằng nội dung changelog phản ánh đúng các thay đổi đã thực hiện trong quá trình làm bài tập/project.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
| ----------------------- | ------------- |
| Đoàn Thế Lực            | 2026-06-24    |
