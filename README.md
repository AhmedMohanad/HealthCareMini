# Healthcare Mini API

A comprehensive healthcare management system API for managing healthcare centers, doctors, patients, appointments, and medical records.

## 🚀 Features

- **Multi-role Authentication** (Admin, Doctor, Receptionist, Staff, Patient, Healthcare Center)
- **Healthcare Center Management** - CRUD operations for healthcare facilities
- **Employee Management** - Manage doctors, receptionists, and staff across centers
- **Patient Management** - Register and manage patient records
- **Appointment Scheduling** - Book, reschedule, cancel, and check-in appointments
- **Medical Records** - Secure storage of patient medical history
- **Password Encryption** - BCrypt hashing for secure authentication
- **JWT Authentication** - Token-based security with cookie support

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT with BCrypt password hashing
- **Validation**: FluentValidation
- **Architecture**: Repository Pattern, Dependency Injection



## 🔧 Installation

1. **Clone the repository**

git clone https://github.com/yourusername/HealthcareMini.git
cd HealthcareMini


/////////////////  how to start ///////////////
1- run the server 
2- make the next req ->> 
  url: POST:  https://localhost:7101/api/admin
  Body:
  {
  "firstName": "Ahmed",
  "lastName": "Admin",
   "email": "ahmed@admin.com",
   "password": "111111",
  "dateOfBirth": "1990-01-01T00:00:00",
  "contactDetails": {
    "phoneNumbers": ["+1234567890"]
  },
  "addressDetails": {
    "street": "Admin Street 123",
    "city": "Baghdad",
    "province": 0,
    "zipCode": "10001"
  }
}


**** note i have made the access on admin controller free (just for testing) by [AllowAnonymous]

3- u have to login by:
  a- POST: https://localhost:7101/api/auth/loginUser
  b- Body: 
  {
      "email": "ahmed@admin.com",
  "password": "111111"
}

///////////////////////////////////////////////////////////

now feel free 
