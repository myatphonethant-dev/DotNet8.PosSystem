***CmsDbContext***
```
dotnet ef dbcontext scaffold "Server=localhost;Database=cms;User=root;Password=sasa@123;" Pomelo.EntityFrameworkCore.MySql -o Data -c CmsDbContext -f
```
***PointDbContext***
```
dotnet ef dbcontext scaffold "Server=localhost;Database=point;User=root;Password=sasa@123;" Pomelo.EntityFrameworkCore.MySql -o Data -c PointDbContext -f
```
