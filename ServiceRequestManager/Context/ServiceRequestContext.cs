using Microsoft.EntityFrameworkCore;
using ServiceRequestManager.Models;

namespace ServiceRequestManager.Context
{
	public partial class ServiceRequestContext: DbContext
	{
        public ServiceRequestContext(DbContextOptions<ServiceRequestContext> options) : base(options) { }
        public DbSet<ServiceRequestEntity> ServiceRequests { get; set; }
    }
}
