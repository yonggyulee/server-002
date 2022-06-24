using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Entities;
using Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;
using Mirero.DAQ.Infrastructure.Identity;
using Group = Mirero.DAQ.Domain.Account.Entities.Group;
using User = Mirero.DAQ.Domain.Account.Entities.User;

namespace Mirero.DAQ.Infrastructure.Database.Account;

public class AccountDbContext : DbContext
{
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Domain.Account.Entities.System> Systems { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<LoginHistory> LoginHistories { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<GroupFeature> GroupFeatures { get; set; } = null!;
    public DbSet<UserPrivilege> UserPrivileges { get; set; } = null!;
    public DbSet<GroupSystem> GroupSystems { get; set; } = null!;
    public DbSet<RolePrivilege> RolePrivileges { get; set; } = null!;

    public AccountDbContext(DbContextOptions options) : base(options)
    {
    }

    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("account");
        builder.ApplyConfiguration(new GroupEntityTypeConfiguration());
        builder.ApplyConfiguration(new SystemEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new LoginHistoryEntityTypeConfiguration());
        builder.ApplyConfiguration(new RefreshTokenEntityTypeConfiguration());
        builder.ApplyConfiguration(new GroupFeatureEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserPrivilegeEntityTypeConfiguration());
        builder.ApplyConfiguration(new GroupSystemEntityTypeConfiguration());
        builder.ApplyConfiguration(new RolePrivilegeEntityTypeConfiguration());

        builder.Entity<Group>().HasData(new Group { Id = "default", Title = "기본 그룹" });

        builder.Entity<User>().HasData(new User
        {
            Id = "administrator",
            Password = Encrypt.HashToSHA256("mirero2816!"),
            Name = "관리자",
            RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            LastPasswordChangedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            GroupId = "default",
            RoleId = RoleId.SuperAdministrator,
            Enabled = true
        });

        builder.Entity<User>().HasData(new User
        {
            Id = "pipeline",
            Password = Encrypt.HashToSHA256("mirero2816!"),
            Name = "파이프라인",
            RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            LastPasswordChangedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            GroupId = "default",
            RoleId = RoleId.SuperAdministrator,
            Enabled = true
        });

        var rolePrivileges = new List<RolePrivilege>()
        {
            new() { RoleId = RoleId.GroupAdministrator, PrivilegeId = PrivilegeId.DeleteUser }
        };
        
        rolePrivileges.AddRange(typeof(PrivilegeId).GetProperties().Select(p => new RolePrivilege
            { RoleId = RoleId.SuperAdministrator, PrivilegeId = p.Name }));

        builder.Entity<RolePrivilege>().HasData(rolePrivileges);
    }
}