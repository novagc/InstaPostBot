using MySql.Data.EntityFramework;
using System.Data.Common;
using System.Data.Entity;

namespace InstaPostBot.Other
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class MySqlContext : DbContext
	{
		public MySqlContext(DbConnection existingConnection, bool contextOwnsConnection)
			: base(existingConnection, contextOwnsConnection)
		{

		}

		public DbSet<Shedule> Shedules { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Shedule>()
				.MapToStoredProcedures();
		}
	}
}
