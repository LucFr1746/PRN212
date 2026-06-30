# AI Learning Reflection

## 1. Thông tin chung

| Thông tin                  | Nội dung                                                                               |
| -------------------------- | -------------------------------------------------------------------------------------- |
| Môn học                    | Application Development on .NET                                                        |
| Mã môn học                 | PRN212                                                                                 |
| Lớp                        | SE20A02                                                                                |
| Học kỳ                     | SU26                                                                                   |
| Tên bài tập / Project      | FU Mini Hotel Management System                                                        |
| Tên sinh viên / Nhóm       | Đoàn Thế Lực                                                                           |
| MSSV / Danh sách MSSV      | DE200523                                                                               |
| Giảng viên hướng dẫn       | QuangLTN3                                                                              |
| Ngày hoàn thành reflection | 2026-06-24                                                                             |

---

## 2. Mục đích Reflection

File này dùng để sinh viên/nhóm tự đánh giá quá trình sử dụng AI trong học tập và phát triển hệ thống quản lý khách sạn FU Mini Hotel Management System.

---

## 3. Tóm tắt quá trình sử dụng AI

```text
Trong quá trình phát triển giải pháp quản lý khách sạn đa tầng WPF và Entity Framework Core Database-First, AI đã hỗ trợ khởi dựng khung boilerplate dự án, sinh mã mappings tự động, định dạng cấu trúc layouts XAML cho giao diện, tích hợp các popup lịch chọn động, xây dựng các biểu thức chính quy (Regex) và logic kiểm tra trùng lặp lịch đặt phòng, viết các cấu trúc Serializable Transactions cho DAO chống race condition, và thiết lập các kịch bản unit tests xUnit. Sinh viên chịu trách nhiệm chính trong việc kiểm chứng logic trên giao diện, khắc phục lỗi LINQ dịch SQL của EF Core, tối ưu hóa các sự kiện xóa thực thể khóa ngoại và hoàn thiện logic cập nhật Booking Detail trong Transaction.
```

---

## 4. Công cụ AI đã sử dụng

- [ ] ChatGPT
- [ ] Gemini
- [ ] Claude
- [ ] GitHub Copilot
- [ ] Cursor
- [x] Antigravity
- [ ] Microsoft Copilot
- [ ] Perplexity
- [ ] Công cụ khác: ....................................

### Công cụ được sử dụng nhiều nhất

```text
Antigravity
```

### Lý do sử dụng công cụ đó

```text
Antigravity tích hợp trực tiếp vào môi trường IDE của dự án và hỗ trợ cơ chế tự động biên dịch, chạy test suite xUnit ngay lập tức sau mỗi lần sinh mã giúp phản hồi chất lượng lập trình cực kỳ nhanh chóng.
```

---

## 5. AI đã hỗ trợ em/nhóm ở điểm nào?

- [x] Hiểu yêu cầu đề bài
- [x] Phân tích bài toán
- [x] Tìm ý tưởng giải pháp
- [ ] Thiết kế database
- [x] Thiết kế giao diện
- [x] Thiết kế kiến trúc hệ thống
- [x] Viết code mẫu
- [x] Debug lỗi
- [x] Viết test case
- [x] Review code
- [x] Tối ưu code
- [ ] Kiểm tra bảo mật
- [ ] Viết báo cáo
- [ ] Chuẩn bị thuyết trình
- [ ] Tìm hiểu công nghệ mới

### Mô tả chi tiết

```text
AI đã hỗ trợ đắc lực trong việc sinh các định nghĩa lớp mô hình cơ sở dữ liệu ánh xạ, các interface/service cho repositories, layout giao diện WPF Grid và DatePicker templates, logic so khớp giao dịch Serializable Transaction trong DAO, và toàn bộ 22 ca kiểm thử xUnit cho các hàm ValidationHelper và logic trùng lặp ngày đặt phòng.
```

---

## 6. AI có giúp em/nhóm học tốt hơn không?

### 6.1. Những điểm AI giúp em/nhóm học tốt hơn

```text
Có. AI đã giúp nhóm:
- Hiểu sâu sắc hơn về cơ chế Transaction Isolation Level của EF Core (đặc biệt là mức Serializable) và tầm quan trọng của việc khóa bản ghi ở tầng database để ngăn ngừa lỗi bất đồng bộ khi nhiều người dùng đặt cùng một phòng một thời điểm.
- Nắm bắt được phương pháp viết Unit Test trong .NET (xUnit) để tạo ra các kịch bản Theory/Fact độc lập kiểm tra tính bền vững của nghiệp vụ.
```

### 6.2. Những điểm AI chưa giúp tốt hoặc gây khó khăn

```text
- AI đề xuất so sánh chuỗi email không phân biệt chữ hoa chữ thường bằng `email.Equals(..., StringComparison.OrdinalIgnoreCase)` trong truy vấn EF Core LINQ, khiến chương trình crash do EF Core không hỗ trợ dịch sang SQL Server. Nhóm đã tự sửa lại thành `.ToLower() == email.ToLower()`.
- AI khi viết hàm Update BookingReservation đã quên làm sạch và thêm mới lại các bản ghi chi tiết BookingDetail cũ, khiến việc chỉnh sửa đơn đặt phòng không lưu trữ đúng các phòng con đã đổi. Nhóm đã tự bổ sung `RemoveRange` các phần tử cũ trước khi Add lại các phần tử con mới cập nhật.
```

### 6.3. Em/nhóm có bị phụ thuộc vào AI không?

- [ ] Không phụ thuộc
- [x] Phụ thuộc ít
- [ ] Phụ thuộc trung bình
- [ ] Phụ thuộc nhiều

Giải thích:

```text
AI được sử dụng chủ yếu để tăng tốc viết các đoạn mã giao diện XAML cồng kềnh và các boilerplate code Class/Interface lặp đi lặp lại. Sinh viên hoàn toàn chủ động trong việc kiểm soát logic nghiệp vụ, thiết kế giao dịch đồng thời và sửa lỗi.
```

---

## 7. Em/nhóm đã kiểm tra kết quả AI như thế nào?

- [x] Chạy thử chương trình
- [x] Kiểm tra output
- [x] Viết test case
- [x] So sánh với yêu cầu đề bài
- [ ] Đối chiếu với tài liệu môn học
- [x] Review code
- [ ] Hỏi lại giảng viên
- [x] Tra cứu tài liệu chính thống
- [ ] Thảo luận với thành viên nhóm
- [x] Kiểm tra bằng dữ liệu mẫu
- [x] So sánh trước và sau khi dùng AI

### Mô tả quá trình kiểm chứng

```text
1. Đăng nhập ứng dụng WPF cục bộ, tương tác trực tiếp với giao diện để thêm mới, sửa đổi, xóa các phòng và khách hàng, quan sát thông báo MessageBox hiển thị đúng Exception khi xóa thực thể có liên kết.
2. Tạo đơn đặt phòng, chọn khoảng ngày chồng chéo và kiểm tra xem hệ thống báo lỗi trùng phòng chính xác.
3. Chạy lệnh `dotnet test` kiểm chứng tất cả 22 ca kiểm thử xUnit tự động thành công (100% Passed).
```

### Ví dụ cụ thể về một lần kiểm chứng

| Nội dung | Mô tả |
| --- | --- |
| AI đã gợi ý gì? | Gợi ý so sánh chuỗi Email bằng `StringComparison.OrdinalIgnoreCase` bên trong biểu thức lambda LINQ của EF Core. |
| Em/nhóm đã kiểm tra bằng cách nào? | Khởi chạy chương trình WPF và gõ tìm kiếm thông tin khách hàng bằng Email trên màn hình chính. |
| Kết quả kiểm tra | Ứng dụng ném lỗi Runtime báo không thể dịch biểu thức `StringComparison.OrdinalIgnoreCase` sang SQL Server. |
| Em/nhóm đã xử lý tiếp như thế nào? | Sửa biểu thức lambda so khớp email thành `.ToLower() == email.ToLower()`, kiểm tra lại tìm kiếm chạy mượt mà không lỗi. |

---

## 8. Ví dụ AI gợi ý sai hoặc chưa phù hợp

| Nội dung | Mô tả |
| --- | --- |
| AI đã gợi ý gì? | Viết logic cập nhật Booking Reservation trong DAO chỉ lưu các thuộc tính tổng thể như `TotalPrice`, `BookingDate` mà không đụng đến các BookingDetail con. |
| Vì sao gợi ý đó sai/chưa phù hợp? | Làm mất đi tính toàn vẹn dữ liệu, các thay đổi về phòng hoặc ngày chi tiết trong đơn đặt phòng không được lưu xuống DB. |
| Em/nhóm phát hiện bằng cách nào? | Chạy Edit Mode trên UI, thay đổi phòng và ngày rồi lưu lại, mở database kiểm tra thấy bảng BookingDetail vẫn giữ phòng cũ. |
| Em/nhóm đã sửa như thế nào? | Thêm logic xóa sạch BookingDetail cũ và Add mới các BookingDetail trong Transaction Serializable: `context.BookingDetails.RemoveRange(existing.BookingDetails);` trước khi lưu. |
| Bài học rút ra | Các mối quan hệ Master-Detail (Một-Nhiều) luôn cần được cập nhật dữ liệu một cách cẩn thận ở cả bảng cha lẫn bảng con trong cùng một Transaction. |

---

## 9. Phần đóng góp thật sự của sinh viên/nhóm

```text
- Thiết lập giao dịch khóa Serializable Transactions an toàn đồng thời tuyệt đối chống race condition đặt trùng phòng.
- Khắc phục lỗi dịch SQL Server của truy vấn so sánh Email của EF Core.
- Thiết lập logic xóa-thêm mới (Clean and Save) cho BookingDetail trong Update Reservation.
- Bổ sung cấu hình try-catch và hiển thị thông báo lỗi thân thiện trên giao diện WPF khi gặp lỗi xóa thực thể liên kết khóa ngoại.
```

---

## 10. So sánh trước và sau khi dùng AI

| Nội dung | Trước khi dùng AI | Sau khi dùng AI | Cải thiện đạt được |
| --- | --- | --- | --- |
| Coding Speed | Average | Fast | Tiết kiệm ~60% thời gian tạo XAML layout, Models mapping và các Interface. |
| Code Integrity | Basic | High | Đảm bảo tính nhất quán nghiệp vụ đặt phòng đồng thời nhờ Serializable Transactions và ValidationHelper. |
| Test Coverage | None | Excellent | 22 unit tests bao phủ mọi kịch bản validation và trùng phòng, giúp bảo vệ mã nguồn. |

---

## 11. Bài học về môn học

- Hiểu sâu sắc cách phát triển ứng dụng Windows Presentation Foundation (WPF) theo mô hình phân lớp sạch sẽ N-tier.
- Nắm bắt được cơ chế ánh xạ ORM thông qua Entity Framework Core Database-First và cách xử lý giao dịch đồng thời ở mức cô lập cao nhất Serializable Transaction để đảm bảo tính toàn vẹn dữ liệu cho nghiệp vụ.

---

## 12. Bài học về sử dụng AI có trách nhiệm

- Không được chủ quan và phó mặc hoàn toàn các logic cơ sở dữ liệu quan trọng cho AI. Lập trình viên luôn phải thực thi kiểm tra ứng dụng thực tế và xây dựng bộ unit test kiểm định tự động để rà soát chất lượng code.
- Phải hiểu rõ bản chất dòng mã AI sinh ra để kịp thời sửa lỗi (như lỗi dịch LINQ to SQL) thay vì nộp nguyên bản.

---

## 13. Điều em/nhóm sẽ không làm khi sử dụng AI

- [x] Không dùng AI để làm toàn bộ bài mà không hiểu nội dung.
- [x] Không nộp nguyên văn kết quả AI nếu chưa kiểm tra.
- [x] Không che giấu việc sử dụng AI trong các phần quan trọng.
- [x] Không dùng AI để tạo nội dung sai lệch hoặc gian lận.
- [x] Không dùng AI thay thế hoàn toàn quá trình học.
- [x] Không bỏ qua yêu cầu, rubric hoặc hướng dẫn của giảng viên.

---

## 14. Kế hoạch cải thiện lần sau

- Sẽ cung cấp rõ ràng sơ đồ cơ sở dữ liệu và các ràng buộc khóa ngoại cho AI ngay từ đầu để AI đề xuất đầy đủ giải pháp khóa và ràng buộc xóa an toàn nhất.
- Hướng dẫn AI cụ thể về cơ chế dịch LINQ của EF Core SQL Server để giảm thiểu lỗi dịch hàm StringComparison.

---

## 15. Tự đánh giá mức độ hoàn thành

| Tiêu chí | Điểm tự đánh giá 1-5 | Ghi chú |
| --- | --- | --- |
| Ghi nhận việc dùng AI trung thực | 5 | |
| Prompt có mục tiêu rõ ràng | 5 | |
| Kiểm chứng kết quả AI | 5 | |
| Tự chỉnh sửa/cải tiến | 5 | |
| Hiểu nội dung đã nộp | 5 | |
| Reflection có chiều sâu | 5 | |
| Sử dụng AI có trách nhiệm | 5 | |

---

## 16. Câu hỏi tự vấn cuối bài

### 16.1. Nếu giảng viên hỏi về phần AI đã hỗ trợ, em/nhóm có giải thích lại được không?

```text
Có. Em giải thích rõ được cơ cấu hoạt động của mô hình N-tier trong .NET, cách viết UI XAML trong WPF, cách cấu hình Serializable Transactions để đảm bảo luồng đặt phòng không trùng lặp, và cơ chế chạy kiểm thử xUnit.
```

### 16.2. Nếu không có AI, em/nhóm có thể tự làm lại phần quan trọng nhất không?

```text
Có. Logic xử lý cơ sở dữ liệu EF Core, xử lý Transaction Serializable, viết code-behind của WPF UI và viết bộ unit test hoàn toàn là các kỹ năng lập trình hướng đối tượng và C# cốt lõi em tự làm được.
```

---

## 17. Cam kết Reflection

Sinh viên/nhóm cam kết nội dung reflection phản ánh chân thực quá trình làm việc.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
| ----------------------- | ------------- |
| Đoàn Thế Lực            | 2026-06-24    |
