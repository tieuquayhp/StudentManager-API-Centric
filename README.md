# StudentManager

Ứng dụng demo quản lý sinh viên với API và web MVC sử dụng .NET 8 và Entity Framework Core.

## Yêu cầu môi trường
- [.NET SDK 8.0](https://dotnet.microsoft.com/)
- SQL Server (LocalDB hoặc SQL Server Express)

## Cấu hình cơ sở dữ liệu
1. Mở `StudentManager.API/appsettings.json` và `StudentManager.MVC/appsettings.json`.
2. Cập nhật chuỗi kết nối `"DefaultConnection"` (API) và `"ApplicationDbContext"` (MVC) cho phù hợp với máy của bạn.
3. Tại thư mục gốc dự án, chạy lệnh áp dụng migration cho cơ sở dữ liệu:
   ```bash
   dotnet ef database update --project StudentManager.API
   ```

## Chạy dự án
- **API**:
  ```bash
  dotnet run --project StudentManager.API
  ```
  API mặc định chạy tại `https://localhost:7024`. Có thể mở `https://localhost:7024/swagger` để xem tài liệu.
- **MVC**:
  ```bash
  dotnet run --project StudentManager.MVC
  ```
  Ứng dụng web chạy tại `https://localhost:7111`.

## Ví dụ gọi API
Lấy danh sách sinh viên:
```bash
curl -k https://localhost:7024/api/students
```
