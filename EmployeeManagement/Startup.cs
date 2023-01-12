using EmployeeManagement.Configuration;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement;

public static class Startup
{
    static Startup()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", false);

        IConfiguration config = builder.Build();

        ConnectionStrings = config.GetSection("ConnectionStrings").Get<ConnectionStrings>();
    }
    
    public static ConnectionStrings? ConnectionStrings { get;  }
}