***PosDbContext***
```
dotnet ef dbcontext scaffold "Server=localhost;Database=pos;User=root;Password=sasa@123;" Pomelo.EntityFrameworkCore.MySql -o PosDbContext -c PosDbContext -f
```

***POSServiceDbContext***
```
dotnet ef dbcontext scaffold "Server=localhost;Database=posservice;User=root;Password=sasa@123;" Pomelo.EntityFrameworkCore.MySql -o PosDbContext -c PosDbContext -f
```