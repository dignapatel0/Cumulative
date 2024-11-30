# School Database - CRUD Operations for Teachers, Courses, and Students

## Description
School Application is a web application designed to manage and display information about teachers, courses, and students in a school system. It provides a simple interface for viewing and managing teacher details, course details, and student information.

The application is built using ASP.NET Core MVC and MySQL, enabling efficient CRUD operations (Create, Read, Update, Delete) across all three tables: Teachers, Courses, and Students.

## Features
- **Teacher Information**: Each course has associated teacher details, including their name, hire date, and assigned courses.
- **Course Management**: Displays a list of all courses with details such as course name, code, start date, end date, and assigned teachers.
- **Student Information**: Provides details like student name, student number, enrollment date, and the courses they are enrolled in.
- **Responsive Design**: Mobile-friendly interface to ensure accessibility across all devices.
- **CRUD Operations**: Supports basic Create, Read, Update, and Delete operations for managing teachers, courses, and students.

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

## Git Repo
git clone https://github.com/dignapatel0/Cumulative.git


## Set Up MySQL Database
Create a MySQL database named schooldb (or use your preferred name).

## Update the connection string in SchoolDbContext.cs with your database credentials:

private static string User { get { return "root"; } } <br>
private static string Password { get { return ""; } } <br>
private static string Database { get { return "schooldb"; } } <br>
private static string Server { get { return "localhost"; } } <br>
private static string Port { get { return "3306"; } } <br>


## Application Structure

- **Controllers**: Contains the logic for handling requests related to teachers, courses, and students. For example, `TeacherPageController`, `CourseAPIController`, and `StudentPageController`.
  
- **Models**: Contains the class definitions for `Teacher`, `Course`, and `Student`, which represent the data objects used in the application. 
  - `Teacher` model represents the details of each teacher, such as their name, hire date, and salary.
  - `Course` model contains course-related information, such as course name, code, start and finish dates, and the teacher assigned.
  - `Student` model holds details about students, including their forst name, last name, student number and enroll date.
  
- **Views**: Displays the UI for listing teachers, courses, students, and showing individual details for each of them.

- **Database Context**: `SchoolDbContext.cs` is responsible for handling MySQL database connections and executing queries. It manages the interaction between the application and the database, including the tables for teachers, courses, and students.
