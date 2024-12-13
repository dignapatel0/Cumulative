# School Database - CRUD Operations for Teachers, Courses, and Students

## Description
School Application is a web application designed to manage and display information about teachers, courses, and students in a school system. It provides a simple interface for viewing and managing teacher details, course details, and student information.

The application is built using ASP.NET Core MVC and MySQL, enabling efficient CRUD operations (Create, Read, Update, Delete) across all three tables: Teachers, Courses, and Students.

## Features
- **Teacher Management**: View, add, edit, and delete teacher details such as name, hire date, and assigned courses.
- **Course Management**: Manage courses with details like course name, code, start date, end date, and assigned teachers.
- **Student Management**: Handle student records, including name, student number, enrollment date, and enrolled courses.
- **CRUD Operations**: 
  - **Create**: Add new records to the database.
  - **Read**: Display and search for existing records.
  - **Update**: Modify existing data.
  - **Delete**: Remove records from the database.
- **Responsive Design**: Ensures accessibility across devices.

## Technologies Used
- **ASP.NET Core MVC**: For building the web application using the MVC architecture.
- **MySQL**: For storing and managing data related to teachers, courses, and students.
- **HTML/CSS**: For creating the frontend to display information effectively.
- **MySQL Data Provider**: To establish database connections and perform operations within the application.

## Requirements
- **Operating System**: Windows, macOS, or Linux
- **.NET SDK**: .NET 6.0 or higher
- **Database**: MySQL 8.0 or higher
- **Tools**:
  - Visual Studio or Visual Studio Code
  - MySQL Workbench or any SQL client for database management
- **Browser**: Latest versions of Chrome, Firefox, Safari, or Edge


## Application Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/dignapatel0/Cumulative.git
   ```

2. Set up the MySQL database:
   - Create a database named `schooldb` (or your preferred name).

3. Update the connection string in `SchoolDbContext.cs` with your database credentials:
   ```csharp
   private static string User { get { return "root"; } }
   private static string Password { get { return ""; } }
   private static string Database { get { return "schooldb"; } }
   private static string Server { get { return "localhost"; } }
   private static string Port { get { return "3306"; } }
   ```

4. Run the application using Visual Studio or Visual Studio Code.

## Project Structure

### Controllers
- Handles the logic for CRUD operations.
  - `TeacherPageController`
  - `CourseAPIController`
  - `StudentPageController`

### Models
- Represents data entities:
  - `Teacher`: Contains details like name, hire date, and courses.
  - `Course`: Holds course-related data such as name, code, and dates.
  - `Student`: Includes student information such as name, enrollment date, and courses.

### Views
- Provides the user interface for:
  - Listing teachers, courses, and students.
  - Showing individual details.
  - Forms for adding or editing data.

### Database Context
- `SchoolDbContext.cs`: Manages database connections and query execution for teachers, courses, and students.

## How to Use
1. **Teachers**:
   - View all teachers.
   - Add a new teacher.
   - Edit teacher details.
   - Delete a teacher.

2. **Courses**:
   - View the list of courses.
   - Add, update, or delete course records.

3. **Students**:
   - List all students.
   - Add or edit student details.
   - Remove a student record.

