Description:  
Event Ticket Flow is a web-based ticketing application that lets customers browse events and buy tickets online. Tickets are general admission, with optional fast track and VIP upgrades, and after a purchase the customer is emailed a receipt, and their ticket with check-in instructions. Staff accounts with the proper permissions create and edit events. A check-in account can check guests in at the venue, while customers can only browse and purchase. We are building it in Visual Studio using C# with HTML and CSS, storing all customer and transaction data in a SQL database, and hosting everything on Microsoft Azure, with reCAPTCHA protecting checkout from bots. Our goal is a smooth, secure flow from finding an event to checking in at the door.

Installation:  
Visual Studio 2026 .NET 9.0 Framework

NuGet packages to install:
DotNetEnv V3.2.0  
Microsoft.AspNetCore.Identity.EntityFrameworkCore  v9.0.22
Microsoft.AspNetCore.Identity.UI  v9.0.22
Microsoft.Data.SqIClient  v7.1.1
Microsoft.EntityFrameworkCore.SqlServer  v9.0.22
Microsoft.EntityFrameworkCore.Tools  v9.0.22
Microsoft.VisualStudio.Web.CodeGeneration.Design  v9.0.12


Testing:  
Screenshot below shows connection to SQL database on project  
<img width="321" height="438" alt="image" src="https://github.com/user-attachments/assets/4e12d216-ac79-47d8-8847-6656adfe1db4" />

Steps needed to get sever added to project:
  1. Go to the view menu and open SQL Server Object Explorer
  2. Right click on SQL Server and select Add SQL Server
  3. Enter server name of "event-ticket-flow.database.windows.net
  4. Enter server information shown in screenshot below and credentials
  5. Click connect
<img width="508" height="715" alt="image" src="https://github.com/user-attachments/assets/c3137082-c169-46e1-975b-35c538cd823f" />

Database test:
Use this link to verify database connection. Will display "Database connection successful." if connected.
[https://eventticketflow-bsgph3hjerc2e6dq.westus-01.azurewebsites.net/db-test ](https://eventticketflow-bsgph3hjerc2e6dq.westus-01.azurewebsites.net/db-test)


Usage:  
Access webpage via the URL [eventticketflow-bsgph3hjerc2e6dq.westus-01.azurewebsites.net ](https://eventticketflow-bsgph3hjerc2e6dq.westus-01.azurewebsites.net/)
