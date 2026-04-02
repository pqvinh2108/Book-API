# Hướng dẫn chạy dự án Book Management (Dành cho máy ở trường)

Chào bạn! Để chạy được dự án này trên máy tính khác (máy ở trường), bạn hãy làm theo các bước sau đây. Chỉ mất khoảng 2-3 phút thôi!

### Bước 1: Tải Code từ GitHub
1. Mở trình duyệt, truy cập vào link GitHub của bạn: `https://github.com/pqvinh2108/Book-API.git`
2. Nhấn vào nút xanh **Code** -> Chọn **Download ZIP**.
3. Giải nén file vừa tải về vào một thư mục bất kỳ.

### Bước 2: Mở Dự án bằng Visual Studio
1. Mở thư mục vừa giải nén.
2. Tìm và click đúp vào file **BookManagement.slnx** (hoặc file `.sln` nếu có) để mở bằng Visual Studio.

### Bước 3: Cập nhật Cơ sở dữ liệu ( Database )
**Đây là bước quan trọng nhất để có dữ liệu (Truyện, Tiểu thuyết...)**
1. Trên thanh menu của Visual Studio, vào: **Tools** -> **NuGet Package Manager** -> **Package Manager Console**.
2. Một cửa sổ dòng lệnh hiện ra bên dưới, bạn hãy gõ (hoặc dán) lệnh này vào rồi nhấn Enter:
   ```powershell
   Update-Database
   ```
   *Lưu ý: Đảm bảo rằng phần "Default project" trong cửa sổ này đang chọn đúng là `BookAPI`.*
   *Đợi nó hiện chữ "Done" là xong!*

### Bước 4: Chạy Ứng dụng
1. Để dự án chạy được cả API và Giao diện cùng lúc, bạn nhấn chuột phải vào **Solution 'BookManagement'** ở cột bên phải (Solution Explorer).
2. Chọn **Configure Startup Projects...**
3. Chọn **Multiple startup projects**.
4. Chỉnh cả **BookAPI** và **BookClient** thành hành động **Start**.
5. Nhấn **Apply** -> **OK**.
6. Bây giờ bạn chỉ cần nhấn nút **Start (F5)** trên thanh công cụ để bắt đầu chạy cả 2 dự án!

### Lưu ý nhỏ:
- Đảm bảo máy tính ở trường đã cài đặt **.NET 8.0 SDK** và **SQL Server Express/LocalDB** (thường thì các máy tại phòng máy tính của trường đã có sẵn các thứ này).
- Sau khi chạy xong, nếu thấy bảng trắng tinh, hãy nhấn nút **Tìm kiếm** trên giao diện để nó tải dữ liệu mẫu lên nhé!

Chúc bạn buổi học thuận lợi!
