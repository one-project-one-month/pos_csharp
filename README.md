# POS C# .NET 8

### Overview

This database schema is designed to support a sales system that manages customers, products, sales transactions, and related information. It includes various tables to store data about customers, products, sales invoices, and more. Additionally, stored procedures are provided to generate sales reports and manage sequences for invoice numbers.

### Tables

1. **Tbl_Customer**
    - Stores information about customers.
    - Fields: `CustomerId`, `CustomerCode`, `CustomerName`, `MobileNo`, `DateOfBirth`, `Gender`, `StateCode`, `TownshipCode`.

2. **Tbl_PlaceState**
    - Stores information about states.
    - Fields: `StateId`, `StateCode`, `StateName`.

3. **Tbl_PlaceTownship**
    - Stores information about townships.
    - Fields: `TownshipId`, `TownshipCode`, `TownshipName`, `StateCode`.

4. **Tbl_Product**
    - Stores information about products.
    - Fields: `ProductId`, `ProductCode`, `ProductCategoryCode`, `ProductName`, `Price`.

5. **Tbl_ProductCategory**
    - Stores information about product categories.
    - Fields: `ProductCategoryId`, `ProductCategoryCode`, `ProductCategoryName`.

6. **Tbl_SaleInvoice**
    - Stores information about sales invoices.
    - Fields: `SaleInvoiceId`, `SaleInvoiceDateTime`, `VoucherNo`, `TotalAmount`, `Discount`, `StaffCode`, `Tax`, `PaymentType`, `CustomerAccountNo`, `PaymentAmount`, `ReceiveAmount`, `Change`, `CustomerCode`.

7. **Tbl_SaleInvoiceDetail**
    - Stores details of each sales invoice.
    - Fields: `SaleInvoiceDetailId`, `VoucherNo`, `ProductCode`, `Quantity`, `Price`, `Amount`.

8. **Tbl_Sequence**
    - Stores sequence information for generating unique codes.
    - Fields: `Id`, `Field`, `Code`, `Length`, `Sequence`.

9. **Tbl_Shop**
    - Stores information about shops.
    - Fields: `ShopId`, `ShopCode`, `ShopName`, `MobileNo`, `Address`.

10. **Tbl_Staff**
    - Stores information about staff members.
    - Fields: `StaffId`, `StaffCode`, `StaffName`, `DateOfBirth`, `MobileNo`, `Address`, `Gender`, `Position`, `Password`.

11. **Tbl_Tax**
    - Stores tax information.
    - Fields: `TaxId`, `FromAmount`, `ToAmount`, `TaxType`, `Percentage`, `FixedAmount`.

### Stored Procedures

1. **sp_Dashboard**
    - Generates various sales reports for the dashboard.
    - Input: `@SaleInvoiceDate` (Date of the sales invoice).
    - Logic:
      - Creates temporary tables for weekly, daily, monthly, and yearly sales reports.
      - Populates these tables with data based on the input date.
      - Retrieves and returns data for:
        - Top 10 best-selling products.
        - Daily sales report.
        - Weekly sales report.
        - Monthly sales report.
        - Yearly sales report.
      - Cleans up temporary tables after use.

2. **Sp_GenerateSaleInvoiceNo**
    - Generates a new sale invoice number.
    - Logic:
      - Retrieves the current sequence value for sales invoices.
      - Increments the sequence.
      - Updates the sequence in the `Tbl_Sequence` table.
      - Returns the new sales invoice number formatted with leading zeros.

3. **Sp_monthly_report**
    - Generates a paginated monthly sales report.
    - Inputs: `@PageNo`, `@PageSize`, `@FromDate`, `@ToDate`.
    - Logic:
      - Creates a temporary table to store monthly sales totals.
      - Populates the table with sales data aggregated by month within the specified date range.
      - Calculates total count and page count for pagination.
      - Retrieves and returns paginated results.
      - Cleans up the temporary table after use.

---

Staff Token
```js
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjAiLCJTdGFmZk5hbWUiOiJTdSBTdSIsIlN0YWZmQ29kZSI6IlUwMDAwMSIsIlRva2VuRXhwaXJlZCI6IjIwMjQtMDQtMjJUMTY6MzY6NDMuNjE1MTc1NFoiLCJuYmYiOjE3MTM4MDI5MDMsImV4cCI6MTcxMzgwMzgwMywiaWF0IjoxNzEzODAyOTAzfQ.IA6JMyYx1yaM2K9ch38sS1Fr2eukLKjOOhh-u5oPTI4
```
EF Core Database First အနေနဲ့
အသုံးပြုချင်ပါက အောက်က Command များကို အသုံးပြုနိုင်ပါတယ်
```bash
Scaffold-DbContext "Server=.;Database=Pos;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -f
Scaffold-DbContext "Server=.;Database=Pos;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext

dotnet tool install --global dotnet-ef 7.0.17
dotnet ef dbcontext scaffold "Server=.;Database=Pos;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext -f
```
Database အတွက် လိုအပ်တဲ့ Table များကို ဒီ [Link](https://github.com/sannlynnhtun-coding/pos_backend_csharp/blob/main/pos_db_script.sql) ကနေ ကြည့်နိုင်ပါတယ်

Postman Collection ကို ဒီ [Link](https://github.com/sannlynnhtun-coding/pos_backend_csharp/blob/main/POS.postman_collection.json) ကနေ ကြည့်နိုင်ပါတယ်

[How to turn IDENTITY_INSERT on and off using SQL Server?](https://stackoverflow.com/questions/7063501/how-to-turn-identity-insert-on-and-off-using-sql-server-2008)
```sql
SET IDENTITY_INSERT Tbl_ProductCategory ON

INSERT INTO [dbo].[Tbl_ProductCategory]
           ([ProductCategoryId]
		   ,[ProductCategoryCode]
           ,[ProductCategoryName])
     VALUES
           (1
		   ,'C001'
           ,'Foods')

SET IDENTITY_INSERT Tbl_ProductCategory OFF
```
