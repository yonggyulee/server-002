using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Common.Constants;
using Mirero.DAQ.Domain.Update.Entity;

namespace Mirero.DAQ.Infrastructure.Database.Update;

public sealed class UpdateDbContextInmemory : UpdateDbContext
{
    private readonly IConfiguration _configuration;

    public UpdateDbContextInmemory(DbContextOptions<UpdateDbContextInmemory> options, IConfiguration configuration) :
        base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        
        Volumes.RemoveRange(Volumes);
        MppSetupVersions.RemoveRange(MppSetupVersions);
        RcSetupVersions.RemoveRange(RcSetupVersions);
        SaveChanges();

        var volumes = new List<Volume>
        {
            new()
            {
                Id = "Mpp",
                Capacity = 0,
                Title = "Mpp",
                Type = VolumeType.LocalFileSystem,
                Uri = _configuration.GetValue<string>("Directory:MppSetupFileDirectory")
            },
            new()
            {
                Id = "Rc",
                Capacity = 0,
                Title = "Rc",
                Type = VolumeType.LocalFileSystem,
                Uri = _configuration.GetValue<string>("Directory:RcSetupFileDirectory")
            }
        };
        
        Volumes.AddRange(volumes);
        SaveChanges();

        var mppSetupVersions =
            Directory.GetFiles(Volumes.SingleOrDefault(v => v.Id == "Mpp").Uri, "*.setup", SearchOption.AllDirectories)
                .Select(f =>
                {
                    var _ = Path.GetFileNameWithoutExtension(f).Split('.');
                    var year = Convert.ToInt32(_[0].TrimStart('0'));
                    var month = Convert.ToInt32(_[1].TrimStart('0'));
                    var day = Convert.ToInt32(_[2].TrimStart('0'));
                    var no = Convert.ToInt32(_[3].TrimStart('0'));
                    var product = _[4];
                    var site = _[5];
                    var type = _[6];
        
                    return new MppSetupVersion
                    {
                        Id = $"{year}.{month}.{day}.{no}.{product}.{site}.{type}",
                        Version = $"{year}.{month}.{day}.{no}",
                        Year = year,
                        Month = month,
                        Day = day,
                        No = no,
                        Product = product,
                        Site = site,
                        Type = type
                    };
                });
        
        var rcSetupVersions =
            Directory.GetFiles(Volumes.SingleOrDefault(v => v.Id == "Rc").Uri, "*.setup", SearchOption.AllDirectories)
                .Select(f =>
                {
                    var _ = Path.GetFileNameWithoutExtension(f).Split('.');
                    var year = Convert.ToInt32(_[0].TrimStart('0'));
                    var month = Convert.ToInt32(_[1].TrimStart('0'));
                    var day = Convert.ToInt32(_[2].TrimStart('0'));
                    var no = Convert.ToInt32(_[3].TrimStart('0'));
                    var product = _[4];
                    var site = _[5];
                    var type = _[6];
        
                    return new RcSetupVersion
                    {
                        Id = $"{year}.{month}.{day}.{no}.{product}.{site}.{type}",
                        Version = $"{year}.{month}.{day}.{no}",
                        Year = year,
                        Month = month,
                        Day = day,
                        No = no,
                        Product = product,
                        Site = site,
                        Type = type
                    };
                });
        
        MppSetupVersions.AddRange(mppSetupVersions);
        RcSetupVersions.AddRange(rcSetupVersions);
        
        SaveChanges();
    }
}