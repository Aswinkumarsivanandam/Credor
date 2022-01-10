using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common;
using Credor.Web.API;
using AutoMapper;
using Credor.Web.API.Common.Audit;
using Credor.Web.API.Common.Filters;
using Microsoft.AspNetCore.Http.Features;

namespace Credor
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
            // services.AddDbContext<ApplicationDbContext>(options =>
            // options.UseSqlServer(
            //   Configuration.GetConnectionString("Database")));
            services.AddControllers(configure =>
            {
                AuditConfiguration.ConfigureAudit(services);
                AuditConfiguration.AddAudit("data");
            });
            services.AddHttpClient();
            services.AddCors();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
              //.AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddRazorPages();
            // Register the Swagger generator, defining 1 or more Swagger documents  
            services.AddSwaggerGen();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            services.AddTransient<IInvestmentService, InvestmentService>();
            services.AddTransient<IInvestmentRepository, InvestmentRepository>();
            services.AddTransient<IMyInvestmentService, MyInvestmentService>();
            services.AddTransient<IMyInvestmentRepository, MyInvestmentRepository>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IProfileRespository, ProfileRepository>();
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
            services.AddTransient<IBankAccountService, BankAccountService>();
            services.AddTransient<IUpdatesRepository, UpdatesRepository>();
            services.AddTransient<IUpdatesService, UpdatesService>();
            services.AddTransient<IDistributionsService, DistributionsService>();
            services.AddTransient<IDistributionsRepository, DistributionsRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ILeadRepository, LeadRepository>();
            services.AddTransient<ILeadService, LeadService>();
            services.AddTransient<IInvestorRepository, InvestorRepository>();
            services.AddTransient<IInvestorService, InvestorService>();
            services.AddTransient<IPortfolioService, PortfolioService>();
            services.AddTransient<IPortfolioRepository, PortfolioRepository>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<AuditLogFilter>();
            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });            
           
            // Enable middleware to serve generated Swagger as a JSON endpoint.              
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),  
            // specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Credor API");
                c.RoutePrefix = string.Empty;
            });
             }
    }
}
