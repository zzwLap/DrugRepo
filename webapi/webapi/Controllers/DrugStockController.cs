using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugStockController(MyContext drugContext) : ControllerBase
    {
        [HttpGet]
        public Task<List<DisplayDrug>> GetDrugs(QueryDrugDto query)
        {
            var drug = drugContext.Drugs.AsQueryable();

            if (String.IsNullOrWhiteSpace(query.PinYin))
            {
                drug = drug.Where(t => t.PinYin.StartsWith(query.PinYin));
            }
            if (String.IsNullOrWhiteSpace(query.DrugName))
            {
                drug = drug.Where(t => t.DrugName.StartsWith(query.DrugName));
            }

            return drug.OrderBy(t => t.Sort).SelectByType<Drugs, DisplayDrug>().ToListAsync();
        }
    }

    public class QueryDrugDto
    {
        public string PinYin { get; set; }
        public string DrugName { get; set; }
    }

    public class DisplayDrug
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public string ClinicalUnit { get; set; }
        public string PackageUnit { get; set; }
        public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        public string DrugCode { get; set; }
        public string DosageForm { get; set; }
        public decimal PackagePrice { get; set; }
    }

    public class AddStockIn
    {
        public int WarehouseNo { get; set; }
        public string WarehouseName { get; set; }
        public string Remarks { get; set; }
    }

    public class AddStockInDetail
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public int UnitType { get; set; }
        public string PackageName { get; set; }
        public decimal PurchasePrice { get; set; }
    }

    public class IdGeneratorService
    {
        public static string GetNextId()
        {
            return NewId.Next().ToString();
        }
    }
}
