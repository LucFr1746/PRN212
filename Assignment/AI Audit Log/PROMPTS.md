# Prompt Log

## 1. Thông tin chung

| Thông tin              | Nội dung                                                                               |
| ---------------------- | -------------------------------------------------------------------------------------- |
| Môn học                | Application Development on .NET                                                        |
| Mã môn học             | PRN212                                                                                 |
| Lớp                    | SE20A02                                                                                |
| Học kỳ                 | SU26                                                                                   |
| Tên bài tập / Project | FU Mini Hotel Management System                                                        |
| Tên sinh viên / Nhóm  | Đoàn Thế Lực                                                                           |
| MSSV / Danh sách MSSV | DE200523                                                                               |
| Giảng viên hướng dẫn  | QuangLTN3                                                                              |
| Ngày bắt đầu          | 2026-06-24T16:00:00.000Z                                                               |
| Ngày cập nhật gần nhất | 2026-06-24                                                                             |

---

## 2. Mục đích của file Prompt Log

File này dùng để ghi lại các prompt quan trọng đã sử dụng trong quá trình thực hiện bài tập, lab, assignment hoặc project.

---

## 3. Công cụ AI đã sử dụng

- [ ] ChatGPT
- [ ] Gemini
- [ ] Claude
- [ ] GitHub Copilot
- [ ] Cursor
- [x] Antigravity
- [ ] Microsoft Copilot
- [ ] Perplexity
- [ ] Công cụ khác: ....................................

---

## 4. Bảng tổng hợp prompt đã sử dụng

| STT | Ngày       | Công cụ AI  | Mục đích | Prompt tóm tắt | Kết quả chính | Có sử dụng vào bài không? | Minh chứng |
| --: | ---------- | ----------- | -------- | -------------- | ------------- | ------------------------- | ---------- |
|   1 | 2026-06-24 | Antigravity | Khởi dựng khung dự án đa dự án N-tier và models | tạo khung dự án WPF Hotel Management... | Khởi tạo solution, các dự án thành viên và mapping models từ db bằng EF Core Database-First. | Có | GitHub Commit |
|   2 | 2026-06-24 | Antigravity | Sửa giao diện WPF và ràng buộc xóa | chỉnh lại giao diện các nút dialog bị chồng... | Gán Grid.Column cho nút bấm XAML, tích hợp lịch chọn DatePicker, tính giá động và chặn xóa Room/Customer liên kết. | Có | GitHub Commit |
|   3 | 2026-06-24 | Antigravity | Helper validation, trùng phòng và giao dịch đồng thời | viết helper kiểm tra định dạng email và sđt... | Tạo ValidationHelper Regex, hàm kiểm tra trùng lặp ngày đặt tại Service và cấu hình giao dịch Serializable Transaction ở DAO. | Có | GitHub Commit |
|   4 | 2026-06-24 | Antigravity | Tạo dự án unit test tự động | tạo project unit test xUnit để test các helper... | Khởi tạo dự án `Assignment.Tests` và viết các hàm Fact/Theory bao phủ các logic kiểm định nghiệp vụ. | Có | GitHub Commit |

---

## 5. Prompt chi tiết

### Prompt số 1 (Initialize WPF Project Scaffold and Core Entities)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích            | Khởi dựng cấu trúc giải pháp đa phân tầng (WPF, BusinessObjects, DataAccess, Repositories, Services) và mapping entities tự động.                       |
| Phần việc liên quan | Architecture & Scaffolding                                                                                                                             |
| Mức độ sử dụng      | Hỗ trợ sinh mã khung file cấu hình, định nghĩa các model, db context và khung giao diện WPF XAML ban đầu.                                               |

#### 5.1. Prompt nguyên văn

```text
tạo khung dự án WPF Hotel Management quản lý phòng, khách hàng và đặt phòng sử dụng EF Core Database-First.
```

#### 5.2. Bối cảnh khi viết prompt

```text
- Sinh viên cần xây dựng một giải pháp quản lý khách sạn theo mô hình N-tier sạch sẽ, sử dụng cơ sở dữ liệu có sẵn thông qua Entity Framework Core Database-First trong WPF.
```

#### 5.3. Kết quả AI trả về

```text
AI cung cấp cấu trúc file sln, các file csproj cấu hình .NET 8 WPF, các lớp thực thể tương ứng các bảng cơ sở dữ liệu và cấu trúc các lớp DAO/Service cơ bản cùng giao diện ban đầu của các Form nhập liệu.
```

#### 5.4. Kết quả đã áp dụng vào bài

```text
- Tạo thành công cấu trúc giải pháp và biên dịch mượt mà dự án.
```

---

### Prompt số 2 (Enhance UI Components and Safe Deletion Checks)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích            | Căn chỉnh giao diện nút bấm WPF, tích hợp popup lịch, tính giá phòng động và khóa hành động xóa thực thể có liên kết.                                 |
| Phần việc liên quan | WPF UI & DAOs (CustomerDAO, RoomInformationDAO)                                                                                                        |
| Mức độ sử dụng      | Hỗ trợ gán Grid.Column cho nút bấm, tích hợp lịch chọn trong DatePicker template, đăng ký sự kiện SelectedDateChanged và viết hàm check Any trong DAO.  |

#### 5.1. Prompt nguyên văn

```text
chỉnh lại giao diện các nút dialog bị chồng lên nhau, hiển thị lịch khi chọn DatePicker, và không cho xóa Room/Customer nếu đã có booking lịch sử
```

#### 5.2. Bối cảnh khi viết prompt

```text
- Giao diện các Dialog có nút Save và Cancel bị hiển thị đè lên nhau do thiếu cấu hình phân bổ cột Grid.Column.
- DatePicker mặc định của WPF không hiển thị popup chọn ngày trực quan.
- Thao tác xóa khách hàng hay phòng có thể gây lỗi vi phạm ràng buộc khóa ngoại trên Database nếu chúng đã từng có đơn đặt phòng lịch sử.
```

#### 5.3. Kết quả AI trả về

```text
- Đề xuất bổ sung Grid.Column="0" và Grid.Column="1" vào các nút bấm dialog.
- Bổ sung cấu hình Calendar popup vào style DatePicker.
- Viết logic tính tiền tự động dựa trên số ngày lưu trú nhân đơn giá ngày.
- Bổ sung kiểm tra `.Any()` ném Exception trong CustomerDAO/RoomInformationDAO trước khi thực thi Remove.
```

#### 5.4. Kết quả đã áp dụng vào bài

```text
- Cập nhật giao diện Dialog gọn gàng và bổ sung kiểm tra khóa khi xóa tại DAO.
```

---

### Prompt số 3 (Centralize Validations and Secure Concurrency with Serializable Transactions)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích            | Tạo bộ kiểm định Regex email/sđt dùng chung, viết logic check trùng lặp phòng tại Service và khóa an toàn giao dịch đồng thời trong DAO.               |
| Phần việc liên quan | Service & DAO Concurrency                                                                                                                              |
| Mức độ sử dụng      | Hỗ trợ sinh Regex điện thoại (9-12 số) và email, viết logic so khớp khoảng ngày trùng phòng và viết transaction Serializable.                            |

#### 5.1. Prompt nguyên văn

```text
viết helper kiểm tra định dạng email và sđt, viết logic check trùng lặp phòng khi đặt cùng một khung giờ, và đảm bảo an toàn luồng đồng thời trong database.
```

#### 5.2. Bối cảnh khi viết prompt

```text
- Nhập liệu sđt và email cần được chuẩn hóa định dạng để đảm bảo chất lượng dữ liệu.
- Việc kiểm tra phòng trống cần được thực thi cả ở client (để báo lỗi sớm) và ở cơ sở dữ liệu để chống tình trạng race condition khi 2 luồng đồng thời cố gắng đặt cùng một phòng trong cùng một khoảng ngày.
```

#### 5.3. Kết quả AI trả về

```text
- Đề xuất tạo `ValidationHelper` với Regex.
- Đề xuất hàm `ValidateBookingDetail` trong `BookingService` thực hiện so sánh ngày bắt đầu/kết thúc chồng lấn lịch.
- Cấu hình DAO sử dụng `Database.BeginTransaction(IsolationLevel.Serializable)` của EF Core để đảm bảo an toàn ghi đè.
```

#### 5.4. Kết quả đã áp dụng vào bài

```text
- Cài đặt ValidationHelper và logic Serializable transaction an toàn đồng thời trong BookingReservationDAO.
```

---

### Prompt số 4 (Unit Testing for Booking Validations and Helpers)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích            | Viết các ca kiểm thử tự động xUnit cho các hàm ValidationHelper và logic trùng lặp đặt phòng.                                                           |
| Phần việc liên quan | Assignment.Tests / BookingValidationTests.cs                                                                                                           |
| Mức độ sử dụng      | Hỗ trợ sinh project test xUnit, viết các test cases sử dụng Theory/InlineData để kiểm định các biên nhập liệu điện thoại/email và logic kiểm tra ngày. |

#### 5.1. Prompt nguyên văn

```text
tạo project unit test xUnit để test các helper validation và logic booking service vừa thêm.
```

#### 5.2. Bối cảnh khi viết prompt

```text
- Cần có bộ kiểm thử tự động để kiểm định và kiểm tra nhanh chóng tính đúng đắn của logic nghiệp vụ đặt phòng tránh các lỗi phát sinh ngoài ý muốn (regressions) khi cập nhật mã nguồn sau này.
```

#### 5.3. Kết quả AI trả về

```text
AI đề xuất dự án test độc lập `Assignment.Tests.csproj` và các hàm kiểm thử xUnit mẫu kiểm tra sđt hợp lệ/không hợp lệ, email hợp lệ/không hợp lệ, ngày bắt đầu lớn hơn hoặc bằng ngày kết thúc, ngày bắt đầu trong quá khứ và logic trùng ngày đặt phòng trên DB.
```

#### 5.4. Kết quả đã áp dụng vào bài

```text
- Viết 15+ test cases hoàn chỉnh chạy thành công bằng trình chạy kiểm thử `dotnet test`.
```

---

## 8. Bài học về cách viết prompt

### 8.1. Khi viết prompt, em/nhóm cần cung cấp thông tin gì để AI trả lời tốt hơn?

```text
Cần mô tả rõ yêu cầu về mặt dữ liệu nhập và hành vi nghiệp vụ mong muốn (như hành vi đồng thời, các ràng buộc lỗi cụ thể và cấu trúc giải pháp phân lớp), giúp AI viết code khớp chuẩn với kiến trúc và hạn chế tối đa các gợi ý sơ sài thiếu an toàn.
```

### 8.2. Em/nhóm đã học được gì về cách đặt câu hỏi cho AI?

```text
Cần chia nhỏ các yêu cầu phức tạp thành các prompt tuần tự (ví dụ: tạo khung trước, tinh chỉnh UI và ràng buộc DAO sau, cài đặt logic nghiệp vụ sâu sau đó mới viết Unit Test), điều này giúp AI phản hồi tập trung và chính xác hơn rất nhiều so với một prompt gộp quá lớn.
```

---

## 9. Phân loại prompt đã sử dụng

| Loại prompt   | Số lượng | Ví dụ prompt tiêu biểu |
| ------------- | -------: | ---------------------- |
| Prompt Design |        2 | tạo khung dự án WPF Hotel Management... / tạo project unit test xUnit... |
| Prompt Fix    |        2 | chỉnh lại giao diện các nút dialog bị chồng... / viết helper kiểm tra định dạng email và sđt... |

---

## 10. Checklist chất lượng prompt

| Tiêu chí                   | Đã đạt? | Ghi chú |
| -------------------------- | :-----: | ------- |
| Prompt có mục tiêu rõ ràng |    x    |         |
| Prompt có đủ bối cảnh      |    x    |         |
| Tự kiểm tra và chỉnh sửa   |    x    |         |

---

## 11. Cam kết sử dụng prompt minh bạch

Sinh viên/nhóm cam kết sử dụng prompt minh bạch và ghi nhận đúng đóng góp của AI.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
| ----------------------- | ------------- |
| Đoàn Thế Lực            | 2026-06-24    |
