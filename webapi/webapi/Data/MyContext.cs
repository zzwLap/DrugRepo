using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<webapi.Models.User> User { get; set; } = default!;
        public DbSet<webapi.Models.DataDictionary> DataDictionary { get; set; } = default!;
        public DbSet<webapi.Models.HierarchyDictionary> HierachyDictionary { get; set; } = default!;
        public DbSet<webapi.Models.DrugStockIn> DrugStockIn { get; set; } = default!;
        public DbSet<webapi.Models.DrugStockInDetail> DrugStockInDetail { get; set; } = default!;
        public DbSet<webapi.Models.DrugStockOut> DrugStockOut { get; set; } = default!;
        public DbSet<webapi.Models.DrugStockOutDetail> DrugStockOutDetail { get; set; } = default!;
        public DbSet<webapi.Models.DrugTransfer> DrugTransfer { get; set; } = default!;
        public DbSet<webapi.Models.DrugTransferDetail> DrugTransferDetail { get; set; } = default!;
        public DbSet<webapi.Models.DrugSendBatchOrder> DrugSendBatchOrder { get; set; } = default!;
    }
}
