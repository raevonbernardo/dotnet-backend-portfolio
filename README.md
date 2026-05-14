# .NET Backend Development Portolio
For Resume Purposes.

**Implementation Summary**
- Uses **EntityFramework Core** + Minimal API design.
- Uses **FluentValidation** for validating requests.
- Uses **PasswordHasher** for hashing password stored in databases.
- Uses **JWT Bearer** for authentication and authorization.
- Uses **Sqlite** for databases.
- Separates User accounts for determining api access from the actual Employee accounts.
- CRUD implementation for both User and Employee endpoints.

**Development API Key**
- eFKb3LEws0728alfJgGyznNhR7xMmajIRFBunLkPjZWKENykwrxveL5B9E0djatY

**Development Admin User**
- Username - admin1234
- Password - password1234
- PublicId - 05d3cd05-0d86-4647-b951-7393570f222e

**.NET SDK Version**
- 10.0

**Rider IDE**
- JetBrains Rider 2025.3.3

# How To Run

**Rider IDE:**
- If you have the installed Rider IDE version or higher, you can just hit play on the upper-right. It should auto-open your browser to the Swagger url.

<img width="391" height="51" alt="Screenshot 2026-05-11 at 6 02 18 PM" src="https://github.com/user-attachments/assets/93c59bfa-f6e8-4137-b18a-84ef23217b87" />


**.NET CLI:**
- Go to the project directory where the .csproj exists and run this in the terminal:
```
dotnet run . -c Development --launch-profile http
```
- Then go to your browser and enter this url:
```
http://localhost:5195/scalar
```


**Docker:**
- Go to the project directory where the Dockerfile is and run this in the terminal:
```
// builds the docker image
docker build -t rt-backend-api .

// deletes any running container with the name rt-backend-api
docker rm -f rt-backend-api

// runs the image on a container named "rt-backend-api"
docker run --name "rt-backend-api" -p 8080:8080 rt-backend-api
```
- Then go to your browser and enter this url:
```
http://localhost:8080/scalar
```

**Swagger UI Preview**
<img width="1727" height="996" alt="Screenshot 2026-05-14 at 8 13 36 AM" src="https://github.com/user-attachments/assets/81f8879d-4d1d-424a-822e-686b634287d3" />
