# Education Centre System – Upgrade Guide

## 1) System Architecture Overview

Hệ thống được tổ chức theo MVC mức đơn giản:

- Model: `Person`, `Student`, `Teacher`, `Admin`
- Controller: `PersonController` (CRUD + điều hướng use-case)
- View:
  - WinForms: `MainForm` (list/filter) + `PersonForm` (add/edit)
  - Console (giữ lại để so sánh): `ConsoleMenuView`
- Data/Repository:
  - `IPersonRepository` là hợp đồng lưu trữ
  - `InMemoryPersonRepository` cho demo nhanh
  - `MySqlPersonRepository` cho CRUD với MySQL (ADO.NET style)

Điểm quan trọng: UI không thao tác trực tiếp database. UI gọi Controller, Controller gọi Repository.

## 2) Before vs After

### Before (Console + in-memory)
- Toàn bộ code (models + CRUD + menu) dồn trong 1 file.
- Lưu dữ liệu bằng `List<T>`, tắt chương trình là mất dữ liệu.
- Logic CRUD bị trộn với `Console.ReadLine/WriteLine` nên khó thay UI.

### After (MVC + Repository + MySQL + WinForms)
- Tách code theo vai trò: Model/Controller/View.
- Repository che chắn cách lưu trữ (List hay MySQL).
- Có thể đổi UI từ Console sang WinForms mà không phải viết lại toàn bộ CRUD.

## 3) Thiết kế MVC (đơn giản, không over-engineering)

### Vì sao cần tách MVC?
- Khi hệ thống lớn lên, nếu logic CRUD nằm trong UI:
  - Sửa một chỗ có thể ảnh hưởng nhiều chỗ
  - Khó test logic vì bị phụ thuộc I/O
  - Đổi UI (Console → WinForms) gần như phải viết lại

### MVC trong dự án này giải quyết gì?
- Model chỉ chứa dữ liệu + OOP (kế thừa/đa hình qua `GetDetails()`).
- Controller chứa use-case (Add/View/Edit/Delete) và quy tắc (Email unique).
- View chỉ lo input/output, bấm nút, hiển thị bảng.

## 4) Database Design (MySQL)

File tạo database: `Database/schema.sql`

Thiết kế dùng 1 bảng `People` (đơn giản, dễ hiểu):
- `PersonId` (PK, auto increment)
- `Role` (Student/Teacher/Admin)
- `Name`, `Telephone`, `Email` (Email unique)
- Các cột role-specific (nullable) cho Student/Teacher/Admin

### Vì sao chuyển từ List<T> sang database?
- Dữ liệu bền vững (không mất khi tắt chương trình)
- Dễ tìm kiếm/lọc/xóa theo điều kiện
- Chuẩn hóa “một nguồn dữ liệu” thay vì nhiều list sync với nhau

### Kết nối DB
Ứng dụng lấy connection string từ biến môi trường:
- `EDU_DB_CONNECTION`

Ví dụ:
```
server=localhost;user id=root;password=YOUR_PASSWORD;database=education_centre;
```

## 5) Design Pattern đã áp dụng (1–2 pattern)

### Repository Pattern
- Problem: UI/Controller bị “dính” chặt vào cách lưu trữ (List hay DB)
- Pattern: `IPersonRepository` + 2 implementation (InMemory/MySQL)
- Benefit:
  - Dễ thay storage mà không sửa UI/Controller
  - Dễ mở rộng (thêm file storage, sqlite…) mà không thay use-case

### Factory Pattern (Person)
- Problem: tạo đúng object theo `Role` bị lặp ở nhiều nơi (controller + mapping DB)
- Pattern: `PersonFactory.Create(role)`
- Benefit:
  - Một điểm tạo object, giảm switch/if rải rác
  - Dễ mở rộng thêm role mới (nếu có)

## 6) GUI Structure (WinForms)

- `MainForm`
  - ComboBox lọc role (All/Student/Teacher/Admin)
  - DataGridView hiển thị danh sách
  - Buttons: Add, Edit, Delete, Refresh
- `PersonForm`
  - Add/Edit dùng chung
  - Panel role-specific:
    - Student: 3 subjects
    - Teacher: salary + 2 subjects
    - Admin: salary + type + working hours

## 7) Luồng hoạt động (End-to-end)

Ví dụ Add Teacher:
- View (PersonForm) thu input → tạo `CreatePersonRequest`
- Controller `PersonController.Add`:
  - validate Email
  - tạo object đúng role bằng `PersonFactory`
  - gọi `repo.Add(person)`
- Repository:
  - In-memory: add vào list
  - MySQL: `INSERT` vào bảng `People`
- View refresh DataGridView

Edit/Delete tương tự: View chỉ gửi request, Controller xử lý use-case, Repository thực thi lưu trữ.

## 8) Tư duy “đi 1 bước tính 3 bước”

- Bước 1 (Console → tách MVC):
  - Mục tiêu: tách “logic” khỏi “giao diện”
  - Kết quả: đổi UI không cần viết lại CRUD

- Bước 2 (in-memory → Repository):
  - Mục tiêu: đổi storage không đụng UI/Controller
  - Kết quả: có thể chạy demo nhanh (in-memory) hoặc chạy thật (MySQL)

- Bước 3 (Repository → Database):
  - Mục tiêu: dữ liệu bền vững + chuẩn hóa lưu trữ
  - Kết quả: CRUD thật với SQL, đúng hướng coursework nhưng vẫn dễ hiểu

- Bước 4 (Console → WinForms):
  - Mục tiêu: UI trực quan hơn, phù hợp “Upgrade UI”
  - Vì MVC + Repository đã tách sẵn, nên WinForms chỉ cần gọi Controller.
