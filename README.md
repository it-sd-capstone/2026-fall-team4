# Event Ticket Flow

Event Ticket Flow is a web-based ticketing application that lets customers browse events and buy tickets online. Tickets are general admission, with optional fast-track and VIP upgrades.

After purchase, customers receive an emailed receipt and ticket with check-in instructions. Staff accounts with the proper permissions create and edit events. Check-in accounts can check guests in at the venue, while customers can only browse and purchase.

The application is built in Visual Studio with C#, HTML, and CSS. Customer and transaction data is stored in a SQL database, the application is hosted in Microsoft Azure, and reCAPTCHA protects checkout from bots.

## Installation

- Visual Studio 2026
- .NET 9.0
- Node.js and npm

Event admin styling requires Node.js and npm. From the `EventTicketFlow` directory, run:

```bash
npm install
npm run css:build
```

Run `npm run css:watch` while editing the event views to regenerate the stylesheet automatically.

## Email Configuration

Set these variables in the local `EventTicketFlow/.env` file or the deployment environment:

```dotenv
SMTP_HOST=smtp.example.com
SMTP_PORT=587
SMTP_USERNAME=your-smtp-username
SMTP_PASSWORD=your-smtp-password
SMTP_FROM_EMAIL=tickets@example.com
SMTP_FROM_NAME=Event Ticket Flow
PUBLIC_BASE_URL=https://tickets.example.com
```

Use port `587` for required STARTTLS or port `465` for implicit TLS. `PUBLIC_BASE_URL` must be the public HTTPS origin with no path; HTTP localhost is permitted only in development.

## NuGet Packages

Install the following packages:

- `DotNetEnv` v3.2.0
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` v9.0.22
- `Microsoft.AspNetCore.Identity.UI` v9.0.22
- `Microsoft.Data.SqlClient` v7.1.1
- `Microsoft.EntityFrameworkCore.SqlServer` v9.0.22
- `Microsoft.EntityFrameworkCore.Tools` v9.0.22
- `Microsoft.VisualStudio.Web.CodeGeneration.Design` v9.0.12
- `MailKit` v4.16.0


## Database Testing

The screenshot below shows the SQL database connection for the project:

![SQL database connection](https://github.com/user-attachments/assets/4e12d216-ac79-47d8-8847-6656adfe1db4)

### Add the SQL Server to the Project

1. Open the **View** menu and select **SQL Server Object Explorer**.
2. Right-click **SQL Servers** and select **Add SQL Server**.
3. Enter `event-ticket-flow.database.windows.net` as the server name.
4. Enter the server information and credentials shown in the screenshot below.
5. Click **Connect**.

![SQL Server connection settings](https://github.com/user-attachments/assets/c3137082-c169-46e1-975b-35c538cd823f)

### Test the Database Connection

Open the [database test endpoint](https://eventticketflow-bsgph3hjerc2e6dq.westus-01.azurewebsites.net/db-test). A successful connection displays:

```text
Database connection successful.
```

## Usage

Access the application at [Event Ticket Flow](https://eventticketflow-bsgph3hjerc2e6dq.westus-01.azurewebsites.net/).
