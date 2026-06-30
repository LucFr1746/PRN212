# AI Audit Log

## 1. Thông tin chung

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
| Ngày bắt đầu          | 2026-06-24T16:00:00.000Z                                                               |
| Ngày hoàn thành       | 2026-06-24T17:30:00.000Z                                                               |

---

## 2. Công cụ AI đã sử dụng

- [ ] ChatGPT
- [ ] Gemini
- [ ] Claude
- [ ] GitHub Copilot
- [ ] Cursor
- [x] Antigravity
- [ ] Perplexity
- [ ] Microsoft Copilot
- [ ] Công cụ khác: ....................................

---

## 3. Mục tiêu sử dụng AI

### Mô tả mục tiêu sử dụng AI

```text
Mục tiêu sử dụng AI để hỗ trợ khởi tạo nhanh khung dự án WPF và Entity Framework Core Database-First cho hệ thống quản lý khách sạn FU Mini Hotel Management, tinh chỉnh các thành phần giao diện hiển thị lịch (Calendar popup) và căn chỉnh các nút bấm dialog, triển khai các ràng buộc kiểm tra ràng buộc khóa ngoại (Safe Deletion Constraints) cho Customer/Room, đồng thời xây dựng logic kiểm tra trùng lặp phòng đặt lịch cả ở mức cục bộ (local UI check) lẫn mức cơ sở dữ liệu với giao dịch an toàn đồng thời (Serializable SQL Transaction) và viết bộ kiểm thử tự động xUnit để đảm bảo độ tin cậy.
```

---

## 4. Nhật ký sử dụng AI chi tiết

### Lần sử dụng AI số 1 (Initialize WPF Project Scaffold and Core Entities)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích sử dụng    | Tạo cấu trúc giải pháp đa dự án (.sln, .csproj) và các lớp thực thể EF Core từ database hiện hữu.                                                       |
| Phần việc liên quan | C# WPF Application & Class Libraries (BusinessObjects, DataAccess, Repositories, Services)                                                             |
| Mức độ sử dụng      | Hỗ trợ sinh mã khung giải pháp, các file cấu hình csproj tương thích .NET 8, context, model, repositories và khung UI WPF ban đầu.                      |

#### 4.1. Prompt đã sử dụng

```text
tạo khung dự án WPF Hotel Management quản lý phòng, khách hàng và đặt phòng sử dụng EF Core Database-First.
```

#### 4.2. Kết quả AI gợi ý

```text
AI đề xuất cấu trúc phân tầng giải pháp thành 5 dự án (WPF app, BusinessObjects, DataAccess, Repositories, Services) tách biệt rõ ràng trách nhiệm. Sinh mã cho các lớp thực thể (Customer, RoomInformation, BookingReservation, BookingDetail, RoomType) kế thừa DbContext, sinh các lớp truy xuất dữ liệu DAO, định nghĩa các Interface và Service tương ứng. Đề xuất file XAML cho LoginWindow, MainWindow và các dialog nhập liệu.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
- Cấu trúc thư mục giải pháp và cấu hình file .csproj.
- Các lớp thực thể được tạo ra tự động từ EF Core Context mapping các bảng.
- Interface và các lớp triển khai Repository/Service ban đầu.
- Bản phác thảo XAML của các cửa sổ Login, Main và Dialog.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
- Chỉnh sửa logic đọc chuỗi kết nối từ cấu hình động `appsettings.json` trong phương thức `OnConfiguring` của `FuminiHotelManagementContext` với phương án dự phòng chuỗi mặc định.
- Tự tay căn chỉnh các thuộc tính Margin, FontSize và các Resource Brush trong App.xaml để đồng bộ hóa chủ đề Dark/Light hiện đại cho giao diện.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nhãn | Nội dung |
| --------------- | ---- | -------- |
| Commit/PR       | Initial commit: WPF hotel management app | https://github.com/LucFr1746/PRN212/commit/76103b0b7d7e7467d2ca36b155fb1641af3d6070 |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Giúp tiết kiệm thời gian đáng kể trong giai đoạn khởi tạo cấu trúc boilerplate ban đầu của dự án WPF, giúp sinh viên tập trung sâu hơn vào viết các logic nghiệp vụ nghiệp vụ phức tạp ở các bước sau.
```

---

### Lần sử dụng AI số 2 (Enhance UI Components and Safe Deletion Checks)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích sử dụng    | Khắc phục lỗi chồng nút trên giao diện WPF, tích hợp popup hiển thị lịch vào DatePicker, tính giá phòng động và khóa hành động xóa khi có liên kết.    |
| Phần việc liên quan | WPF UI (XAML & code-behind), DataAccess (CustomerDAO, RoomInformationDAO, BookingReservationDAO)                                                       |
| Mức độ sử dụng      | Hỗ trợ sinh mã tính toán giá theo số ngày đặt, gán Grid.Column cho nút bấm XAML và bổ sung truy vấn kiểm tra sự tồn tại của lịch sử đặt phòng trước khi xóa. |

#### 4.1. Prompt đã sử dụng

```text
chỉnh lại giao diện các nút dialog bị chồng lên nhau, hiển thị lịch khi chọn DatePicker, và không cho xóa Room/Customer nếu đã có booking lịch sử
```

#### 4.2. Kết quả AI gợi ý

```text
- Đề xuất bổ sung Grid.Column="0" và Grid.Column="1" vào các nút bấm trong CustomerDialog, RoomDialog, BookingDialog để xếp thẳng hàng trong Grid.
- Đề xuất thêm thuộc tính Calendar popup vào XAML Template của DatePicker trong App.xaml.
- Viết hàm UpdateCalculatedPrice() đăng ký sự kiện SelectedDateChanged để tính `days * selectedRoom.RoomPricePerDay` tự động trong BookingDialog.
- Thêm kiểm tra `.Any()` trong CustomerDAO.Delete và RoomInformationDAO.Delete để ném Exception nếu thực thể đang liên kết với BookingReservation hay BookingDetail.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
- Grid.Column phân bổ lại layout các nút trong file XAML.
- Logics tính toán giá phòng và các đăng ký sự kiện DateChanged.
- Logic kiểm tra ràng buộc trước khi thực hiện xóa tại DAO.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
- Sửa đổi cơ chế sinh ID tự động cho BookingReservation trong DAO sang ép kiểu `(int?)` trước khi lấy `.Max()` để tránh lỗi `InvalidOperationException` khi bảng đang trống không có dữ liệu (null-safe).
- Bao bọc sự kiện xóa ở UI bằng các khối try-catch để bắt các ngoại lệ từ DAO đẩy lên, hiển thị thông báo MessageBox thân thiện cho người dùng thay vì để ứng dụng crash.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nhãn | Nội dung |
| --------------- | ---- | -------- |
| Commit/PR       | Add calendar popup, pricing calc, and DAO checks | https://github.com/LucFr1746/PRN212/commit/81d0c02ff2e2ff95724b6a06804917d2e63d2b99 |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Các cải tiến giúp cải thiện giao diện người dùng trở nên trực quan hơn và bảo vệ toàn vẹn dữ liệu cơ sở dữ liệu chống lại việc xóa mờ ám các dữ liệu đang có tham chiếu khóa ngoại.
```

---

### Lần sử dụng AI số 3 (Centralize Validations and Secure Concurrency with Serializable Transactions)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích sử dụng    | Đưa ra lớp ValidationHelper dùng chung, cài đặt logic nghiệp vụ kiểm tra trùng phòng ở service và bảo đảm an toàn đồng thời bằng Serializable Transaction.  |
| Phần việc liên quan | Services (BookingService), Shared (ValidationHelper), DataAccess (BookingReservationDAO)                                                               |
| Mức độ sử dụng      | Hỗ trợ sinh Regex cho số điện thoại/email, viết logic so sánh khoảng thời gian overlap và sử dụng giao dịch Serializable trong DAO.                    |

#### 4.1. Prompt đã sử dụng

```text
viết helper kiểm tra định dạng email và sđt, viết logic check trùng lặp phòng khi đặt cùng một khung giờ, và đảm bảo an toàn luồng đồng thời trong database.
```

#### 4.2. Kết quả AI gợi ý

```text
- Tạo class ValidationHelper chứa Regex kiểm tra định dạng Telephone (9-12 chữ số) và Email tiêu chuẩn.
- Viết hàm `ValidateBookingDetail` trong `BookingService` để kiểm tra ngày bắt đầu phải trước ngày kết thúc, không được ở quá khứ, không trùng lặp cục bộ trong list đang nhập, và gọi Repository kiểm tra trùng lặp trên DB.
- Cấu hình phương thức Add/Update của BookingReservationDAO thực hiện trong một Transaction với mức cô lập `IsolationLevel.Serializable` để khóa bảng tránh tình trạng race condition khi nhiều người cùng đặt phòng một lúc.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
- Biểu thức chính quy Regex và các hàm tiện ích trong ValidationHelper.
- Cấu trúc so khớp khoảng ngày trùng lặp (StartDate < EndDate_DB && EndDate > StartDate_DB).
- Boildplate code khởi chạy Transaction trong EF Core.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
- Tự hoàn thiện logic cập nhật BookingReservation trong trường hợp chỉnh sửa (Edit Mode). Cụ thể, khi cập nhật, sinh viên đã viết thêm logic xóa bỏ các BookingDetail cũ tương ứng với Reservation đó trước khi thêm mới lại các BookingDetail đã chỉnh sửa, tất cả chạy trong transaction cô lập cao nhất để tránh rác dữ liệu.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nhãn | Nội dung |
| --------------- | ---- | -------- |
| Commit/PR       | Add validation, DB overlap checks, and unit tests | https://github.com/LucFr1746/PRN212/commit/8b3e73e6b0c36c87a89a81cf9252bb230fcdb5c6 |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Logic nghiệp vụ đặt phòng yêu cầu tính toàn vẹn rất cao. Việc sử dụng các giao dịch Serializable giúp triệt tiêu hoàn toàn nguy cơ đặt trùng lặp phòng do các yêu cầu đồng thời (race conditions) gây ra.
```

---

### Lần sử dụng AI số 4 (Unit Testing for Booking Validations and Helpers)

| Nội dung            | Thông tin                                                                                                                                              |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Ngày sử dụng        | 2026-06-24                                                                                                                                             |
| Công cụ AI          | Antigravity                                                                                                                                            |
| Mục đích sử dụng    | Tạo các ca kiểm thử tự động để bảo đảm tính đúng đắn cho các hàm validation và các kiểm tra logic nghiệp vụ đặt phòng.                               |
| Phần việc liên quan | Unit Test project (Assignment.Tests / BookingValidationTests.cs)                                                                                       |
| Mức độ sử dụng      | Hỗ trợ sinh mã khung dự án kiểm thử xUnit và viết các hàm Fact/Theory kiểm tra biên cho số điện thoại, email, và logic so sánh ngày đặt phòng.         |

#### 4.1. Prompt đã sử dụng

```text
tạo project unit test xUnit để test các helper validation và logic booking service vừa thêm.
```

#### 4.2. Kết quả AI gợi ý

```text
Đề xuất tạo dự án test xUnit độc lập, cài đặt các mock dữ liệu phòng để đưa vào phương thức `ValidateBookingDetail`, viết các ca kiểm thử kiểm chứng định dạng telephone/email đúng/sai, kiểm tra ngày bắt đầu lớn hơn ngày kết thúc sẽ ném ngoại lệ, và kiểm thử trường hợp trùng lặp ngày ở local list hoặc DB.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
- Các ca kiểm thử định dạng số điện thoại và email dùng Theory và InlineData.
- Khung cấu trúc sắp xếp Arrange-Act-Assert cho các ca test nghiệp vụ booking.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
- Tinh chỉnh các ngày kiểm thử động (sử dụng `DateOnly.FromDateTime(DateTime.Today)`) để tránh việc các ngày cố định (hardcoded date) bị trôi vào quá khứ theo thời gian làm hỏng các ca kiểm thử kiểm tra ngày quá khứ.
- Bổ sung cấu hình sln để loại trừ thư mục test khỏi việc biên dịch trong ứng dụng chính và liên kết các tham chiếu thư viện chính xác.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nhãn | Nội dung |
| --------------- | ---- | -------- |
| Commit/PR       | Add validation, DB overlap checks, and unit tests | https://github.com/LucFr1746/PRN212/commit/8b3e73e6b0c36c87a89a81cf9252bb230fcdb5c6 |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Việc thiết lập bộ kiểm thử tự động giúp kiểm soát chất lượng mã nguồn tốt, đảm bảo các sửa đổi refactor tương lai không phá vỡ logic cốt lõi.
```

---

## 5. Bảng tổng hợp mức độ sử dụng AI

| Hạng mục                    | Không dùng AI | AI hỗ trợ ít | AI hỗ trợ nhiều | AI sinh chính | Ghi chú                                                                                     |
| --------------------------- | :-----------: | :----------: | :-------------: | :-----------: | ------------------------------------------------------------------------------------------- |
| Phân tích yêu cầu           |               |      x       |                 |               | Nghiên cứu yêu cầu thiết kế hệ thống và định dạng các bảng dữ liệu.                         |
| Viết user story/use case    |       x       |              |                 |               |                                                                                             |
| Thiết kế database           |       x       |              |                 |               | Cơ sở dữ liệu có sẵn theo đề bài.                                                           |
| Thiết kế kiến trúc hệ thống |               |              |        x        |               | Thiết kế cấu trúc phân lớp giải pháp (DataAccess, Services, Repositories, Tests).           |
| Thiết kế giao diện          |               |              |        x        |               | Tạo layout và style giao diện Dark/Light hiện đại cho WPF trong App.xaml.                  |
| Code frontend               |               |              |                 |       x       | Sinh mã XAML cho các Form Windows và các Dialog nghiệp vụ.                                   |
| Code backend                |               |              |                 |       x       | Sinh các lớp DAO, Service, và đặc biệt là logic xử lý Transaction nâng cao.                |
| Debug lỗi                   |               |              |        x        |               | Phát hiện và khắc phục các lỗi dịch LINQ to SQL của EF Core.                                 |
| Viết test case              |               |              |                 |       x       | Thiết lập dự án xUnit và các test cases tự động kiểm định logic nghiệp vụ.                   |
| Kiểm thử sản phẩm           |               |      x       |                 |               | Chạy kiểm thử thủ công tích hợp trên UI và chạy kiểm thử tự động trên IDE.                  |
| Tối ưu code                 |               |              |        x        |               | Cải tiến cơ chế cập nhật đồng thời trong transaction và làm sạch luồng sinh ID tự động.     |
| Viết báo cáo                |       x       |              |                 |               |                                                                                             |
| Làm slide thuyết trình      |       x       |              |                 |               |                                                                                             |

---

## 6. Các lỗi hoặc hạn chế từ AI

| STT | Lỗi/hạn chế từ AI | Cách phát hiện | Cách xử lý/cải tiến |
| --: | ----------------- | -------------- | ------------------- |
|   1 | AI sử dụng phương thức so sánh chuỗi `email.Equals(..., StringComparison.OrdinalIgnoreCase)` trong truy vấn EF Core LINQ. | Khi chạy ứng dụng thực tế và thực hiện tìm kiếm khách hàng bằng Email, chương trình báo lỗi Runtime: "The LINQ expression ... could not be translated." do EF Core không hỗ trợ biên dịch hàm này sang SQL tương ứng. | Sửa lại logic so sánh trong CustomerDAO thành lower case thủ công: `c.EmailAddress.ToLower() == email.ToLower()` để tương thích tốt với trình biên dịch SQL. |
|   2 | Khi cập nhật BookingReservation ở chế độ Edit, AI chỉ ghi đè thuộc tính tổng thể mà quên xử lý các phần tử BookingDetail liên đới trong database. | Khi thực hiện sửa đổi phòng hoặc ngày của một đơn đặt phòng đã lưu, các thay đổi chi tiết không được ghi nhận lưu trữ lại. | Bổ sung logic xóa các bản ghi con cũ bằng `RemoveRange` và thêm mới lại các bản ghi con đã cập nhật trong Transaction an toàn đồng thời. |

---

## 7. Kiểm chứng kết quả AI

### Nội dung kiểm chứng

```text
Kiểm chứng kết quả qua các hình thức sau:
1. Chạy thành công ứng dụng WPF, đăng nhập bằng tài khoản quản trị viên để vào MainWindow.
2. Thực hiện thêm mới một Khách hàng với định dạng email sai hoặc số điện thoại thiếu số, hệ thống báo lỗi Validation chặn lại chính xác. Nhập đúng định dạng sẽ lưu thành công.
3. Thực hiện xóa một Khách hàng hoặc Phòng đã có lịch sử đặt phòng, hệ thống hiển thị thông báo lỗi chặn xóa đúng theo logic DAO Exception.
4. Đơn đặt phòng mới: Khi thay đổi phòng hoặc ngày, giá tạm tính cập nhật tức thì. Khi cố đặt phòng vào ngày đã bị đặt trước đó trong DB, hệ thống báo lỗi trùng phòng và rollback thành công.
5. Chạy bộ unit tests với lệnh `dotnet test`: Toàn bộ các ca test validation helpers, date range và overlaps đều vượt qua thành công (100% Passed).
```

---

## 8. Đóng góp cá nhân hoặc đóng góp nhóm

### 8.1. Đối với bài cá nhân

```text
- Thiết lập cơ chế Serializable Transactions an toàn cao nhất trong EF Core để loại bỏ tình trạng race condition đặt trùng phòng.
- Trực tiếp sửa đổi các biểu thức LINQ bị lỗi dịch SQL để đảm bảo ứng dụng vận hành trơn tru.
- Phát triển và bổ sung logic làm sạch và ghi đè BookingDetail khi chỉnh sửa Reservation.
```

### 8.2. Đối với bài nhóm

```text
Không áp dụng (Bài tập cá nhân).
```

---

## 9. Reflection cuối bài

### 9.1. AI đã hỗ trợ em/nhóm ở điểm nào?

```text
AI hỗ trợ đắc lực trong việc sinh boilerplate code cho các lớp Entity mappings, các phương thức DAO CRUD cơ bản, phác thảo cấu trúc UI XAML giúp tiết kiệm rất nhiều công sức gõ phím lặp lại.
```

### 9.2. Phần nào em/nhóm không sử dụng theo gợi ý của AI? Vì sao?

```text
Không sử dụng hoàn toàn cơ chế update đơn giản của AI cho Booking Reservation mà phải viết lại logic đồng bộ chi tiết phòng con, do AI chưa bao quát hết việc cập nhật các bản ghi con trong quan hệ một-nhiều.
```

### 9.3. Em/nhóm đã kiểm tra tính đúng đắn của kết quả AI như thế nào?

```text
Kiểm thử thủ công từng trường hợp biên trên UI ứng dụng và chạy bộ test tự động xUnit bao phủ mọi kịch bản nghiệp vụ để đảm bảo kết quả chính xác 100%.
```

### 9.4. Nếu không có AI, phần nào sẽ khó khăn nhất?

```text
Phần thiết kế và viết các mã cấu trúc giao diện WPF (XAML styles, Templates, Layout Grid) sẽ mất cực kỳ nhiều thời gian căn chỉnh tọa độ và màu sắc thủ công.
```

### 9.5. Sau bài tập/project này, em/nhóm học được gì về môn học?

```text
Hiểu rõ kiến trúc ứng dụng phân lớp (N-Tier) trong .NET, cách kết nối cơ sở dữ liệu bằng EF Core và sự quan trọng của việc quản trị các giao dịch (Transactions) đồng thời để duy trì tính nhất quán dữ liệu của hệ thống.
```

### 9.6. Sau bài tập/project này, em/nhóm học được gì về cách sử dụng AI có trách nhiệm?

```text
Không bao giờ tin tưởng mù quáng vào mã nguồn do AI sinh ra, đặc biệt là các phần truy vấn cơ sở dữ liệu phức tạp hoặc logic xử lý đồng thời, lập trình viên bắt buộc phải kiểm chứng và viết test case bao phủ.
```

---

## 10. Cam kết học thuật

Sinh viên/nhóm cam kết rằng:

- Nội dung AI hỗ trợ đã được ghi nhận trung thực.
- Không nộp nguyên văn kết quả AI mà không kiểm tra.
- Có khả năng giải thích các phần đã nộp.
- Chịu trách nhiệm về tính đúng đắn của sản phẩm cuối cùng.
- Hiểu rằng việc sử dụng AI không khai báo có thể ảnh hưởng đến kết quả đánh giá.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
| ----------------------- | ------------- |
| Đoàn Thế Lực            | 2026-06-24    |
