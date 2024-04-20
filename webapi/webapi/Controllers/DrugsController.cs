using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using System.ComponentModel.DataAnnotations;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DrugsController(MyContext drugContext) : ControllerBase
    {
        private readonly MyContext drugContext = drugContext;

        [HttpGet]
        public async Task<Page<DrugDto>> GetDrugs(int pageIndex, int pageSize)
        {
            return await drugContext.Drugs.AsNoTrackingWithIdentityResolution().SelectByType<Drugs, DrugDto>()
                .ToPageListAsync(new PageInfo() { PageIndex = pageIndex, PageSize = pageSize });
        }

        [HttpGet]
        public async Task<DrugDto?> GetDrug(int drugId)
        {
            return await drugContext.Drugs.AsNoTrackingWithIdentityResolution().SelectByType<Drugs, DrugDto>().FirstOrDefaultAsync(t => t.DrugId == drugId);
        }

        [HttpPost]
        public async Task AddDrugs(AddDrugVo drugVo)
        {
            drugVo.DrugId = 0;
            drugVo.DrugCode = Guid.NewGuid().ToString();
            var checkDupDrug = drugContext.Drugs.Any(t => t.DrugCode == drugVo.DrugCode);
            if (checkDupDrug)
            {
                throw new Exception("该药品编码已经被使用");
            }
            var drugs = new Drugs();
            drugContext.Drugs.Add(drugs);
            var drug2 = drugContext.Entry(drugs);
            drug2.CurrentValues.SetValues(drugVo);
            await drugContext.SaveChangesAsync();
        }

        [HttpPut("{drugId}")]
        [HttpPatch("{drugId}")]
        public async Task<IActionResult> ModifyPrice(int drugId, ChangeDrugPriceVo drugPrice)
        {
            if (drugId != drugPrice.DrugId)
            {
                return BadRequest();
            }

            var drugInfo = drugContext.Drugs.FirstOrDefault(t => t.DrugId == drugPrice.DrugId);
            if (drugInfo == null)
            {
                throw new Exception($"未找到药品编号【{drugPrice.DrugId}】的记录信息");
            }
            drugInfo.PackagePrice = drugPrice.PackagePrice;
            drugPrice.ClinicalPrice = drugPrice.ClinicalPrice;
            //TODO 这里应该记录一下每次修改单价后的记录
            await drugContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{drugId}")]
        public async Task ModifyDrugInfo(int drugId, UpdateDrugInfoVo drugInfoVo)
        {
            var drugInfo = drugContext.Drugs.FirstOrDefault(t => t.DrugId == drugInfoVo.DrugId);
            if (drugInfo == null)
            {
                throw new Exception($"未找到药品编号【{drugInfoVo.DrugId}】的记录信息");
            }
            var checkDupDrug = drugContext.Drugs.Any(t => t.DrugCode == drugInfoVo.DrugCode
                            && t.DrugId != drugInfoVo.DrugId);
            if (checkDupDrug)
            {
                throw new Exception("该药品编码已经被使用");
            }
            var drug = drugContext.Drugs.Entry(drugInfo);
            drug.CurrentValues.SetValues(drugInfoVo);
            await drugContext.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task DeleteDrug(int drugId)
        {
            await drugContext.Drugs.Where(t => t.DrugId == drugId).ExecuteDeleteAsync();
        }

        [HttpPost]
        public async Task BatchAddDrugs(List<AddDrugVo> drugListVo)
        {
            List<Drugs> drugs = new List<Drugs>();

            foreach (var drugVo in drugListVo)
            {
                var drug = new Drugs();
                drug.DrugCode = Guid.NewGuid().ToString();
                drug.DrugId = 0;
                drug.DrugName = drugVo.DrugName;
                drug.Spec = drugVo.Spec ?? "";
                drug.PinYin = drugVo.PinYin;
                drug.Sort = drugVo.Sort;
                drug.ClinicalUnit = drugVo.ClinicalUnit;
                drug.PackageUnit = drugVo.PackageUnit;
                drug.C2PQuantity = drugVo.C2PQuantity;
                drug.ApprovalNumber = drugVo.ApprovalNumber;
                drug.NationDrugCode = drugVo.NationDrugCode;
                drug.RADManufacturer = drugVo.RADManufacturer ?? "";
                drug.DosageForm = drugVo.DosageForm;
                drug.DefaultSaleUnit = drugVo.DefaultSaleUnit;
                drug.ClinicalSaleUnit = drugVo.ClinicalSaleUnit;
                drug.PackagePrice = drugVo.PackagePrice;
                drug.ClinicalPrice = drugVo.ClinicalPrice;
                drugs.Add(drug);
            }

            await drugContext.AddRangeAsync(drugs.Take(10));
            await drugContext.SaveChangesAsync();
        }

        [HttpPost]
        public async Task UploadDrugFileBatchAddDrugs(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString();
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            AnalyzeAndAddDrugs(fileName);
        }

        private async void AnalyzeAndAddDrugs(string fileName)
        {
            List<AddDrugVo> drugVo = new List<AddDrugVo>();
            var rows = MiniExcel.Query(fileName, excelType: ExcelType.XLSX).Skip(3).ToList();

            for (int i = 0; i < rows.Count(); i++)
            {
                AddDrugVo drug = new AddDrugVo();
                drug.DrugCode = Guid.NewGuid().ToString();
                drug.DrugId = 0;
                drug.DrugName = rows[i].C;
                drug.Spec = rows[i].E;
                drug.PinYin = PingYinHelper.GetFirstSpell(rows[i].C);
                drug.Sort = Convert.ToInt32(rows[i].A);
                drug.ClinicalUnit = "无";
                drug.PackageUnit = PackageUnit(rows[i].C);
                drug.C2PQuantity = 1;
                drug.ApprovalNumber = rows[i].B;
                drug.NationDrugCode = "";
                drug.RADManufacturer = rows[i].F;
                drug.DosageForm = rows[i].D;
                drug.DefaultSaleUnit = 0;
                drug.ClinicalSaleUnit = 1;
                drug.PackagePrice = 1;
                drug.ClinicalPrice = 1;
                drugVo.Add(drug);
            }

            await BatchAddDrugs(drugVo);
        }
        private List<string> h = new List<string>()
        {
            "片", "丸"
        };
        private List<string> p = new List<string>()
        {
            "液", "剂"
        };
        private string PackageUnit(string str)
        {
            if (h.Any(t => str.Contains(t))) return "盒";
            if (h.Any(t => str.Contains(t))) return "瓶";
            return "";
        }
    }

    public class ChangeDrugPriceVo
    {
        public int DrugId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "包装单位的销售价要大于或等于0")]
        public decimal PackagePrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "临床单位销售价要大于或等于0")]
        public decimal ClinicalPrice { get; set; }
    }

    public class UpdateDrugInfoVo
    {
        [Key]
        public int DrugId { get; set; }
        [Required]
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public int Sort { get; set; }
        [Required]
        public string ClinicalUnit { get; set; }
        [Required]
        public string PackageUnit { get; set; }
        //[Range(1, double.MaxValue)]
        //public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        [Required]
        public string DrugCode { get; set; }
        public string NationDrugCode { get; set; }
        public string RADManufacturer { get; set; }
        [Required]
        public string DosageForm { get; set; }
        [Required]
        public int DefaultSaleUnit { get; set; }
        [Required]
        public int ClinicalSaleUnit { get; set; }
    }
    public class AddDrugVo : DrugDto
    {
    }

    public class DrugDto
    {
        [Key]
        public int DrugId { get; set; }
        public int DrugType { get; set; }
        [Required]
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public int Sort { get; set; }
        [Required]
        public string ClinicalUnit { get; set; }
        [Required]
        public string PackageUnit { get; set; }
        [Range(1, double.MaxValue)]
        public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        [Required]
        public string DrugCode { get; set; }
        public string NationDrugCode { get; set; }
        public string RADManufacturer { get; set; }
        [Required]
        public string DosageForm { get; set; }
        [Required]
        public int DefaultSaleUnit { get; set; }
        [Required]
        public int ClinicalSaleUnit { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "包装单位的销售价要大于或等于0")]
        public decimal PackagePrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "临床单位销售价要大于或等于0")]
        public decimal ClinicalPrice { get; set; }
    }
}
