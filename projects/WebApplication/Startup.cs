using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var client = new HttpClient();
            app.Run(async context => {
                var response = await client.GetAsync("http://microsoft.com").TimeoutAfter(TimeSpan.FromSeconds(30));
                await context.Response.WriteAsync(response.StatusCode.ToString());
            });
        }
    }

    public static class TaskExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeSpan)
        {
            var completedTask = await Task.WhenAny(task, Task.Delay(timeSpan));

            if (completedTask != task)
            {
                throw new TimeoutException("Timeout elapsed");
            }

            return task.Result;
        }
    }
}
