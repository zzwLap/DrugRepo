using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using webapi.Vo;

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataDictionaryController(MyContext drugContext) : ControllerBase
    {
        private const string CommonDict = "普通字典";
        private readonly MyContext drugContext = drugContext;

        [HttpGet]
        public Task<List<DictionaryDto>> GetAllDict()
        {
            var dict = drugContext.DataDictionary.Select(t => new DictionaryDto() { DisplayName = t.DisplayName, CategoryName = t.CategoryName, Id = t.Id, Value = t.Value });
            return dict.ToListAsync();
        }

        [HttpGet]
        public Task<List<DictionaryDto>> GetDictionaries([FromQuery] List<string> categoryNames)
        {
            var dict = drugContext.DataDictionary.Where(t => categoryNames.Contains(t.CategoryName))
                  .Select(t => new DictionaryDto() { DisplayName = t.DisplayName, CategoryName = t.CategoryName, Id = t.Id, Value = t.Value });
            return dict.ToListAsync();
        }

        [HttpGet]
        public Task<List<DictionaryDto>> GetDictionary(string categoryName)
        {
            var dict = drugContext.DataDictionary.Where(t => categoryName == t.CategoryName)
                  .Select(t => new DictionaryDto() { DisplayName = t.DisplayName, CategoryName = t.CategoryName, Id = t.Id, Value = t.Value });
            return dict.ToListAsync();
        }

        [HttpPost]
        public async Task AddDictionary(DataDictionaryVo dict)
        {
            var catelogInfo = drugContext.HierachyDictionary.FirstOrDefault(t => t.CategoryName == CommonDict && t.DisplayName == dict.CategoryName);
            if (catelogInfo == null)
            {
                throw new Exception($"未找到词典{dict.CategoryName}的目录项");
            }

            var checkDupDict = drugContext.DataDictionary.Any(t => t.CategoryName == dict.CategoryName && dict.DisplayName == t.DisplayName);
            if (checkDupDict)
            {
                throw new Exception("不允许创建重复的字典");
            }

            var maxValueId = drugContext.DataDictionary.Where(t => t.CategoryName == dict.CategoryName)
                                    .Select(t => t.Value).DefaultIfEmpty().Max();

            var dict3 = new DataDictionary()
            {
                CategoryCode = catelogInfo.Value,
                Id = 0,
                Description = dict.Description,
                DisplayName = dict.DisplayName,
                CategoryName = dict.CategoryName,
                Value = maxValueId + 1
            };

            if (maxValueId == 0)
            {
                var checkFirstKey = drugContext.DataDictionary.Any(t => t.CategoryName == dict.CategoryName);
                if (!checkFirstKey)
                {
                    dict3.Value = 0;
                }
            }

            drugContext.DataDictionary.Add(dict3);
            await drugContext.SaveChangesAsync();
        }

        [HttpPut("{dictId}")]
        public async Task ModifyDictionary(int dictId, ModifyDictionaryVo newDict)
        {
            if (String.IsNullOrWhiteSpace(newDict.DisplayName))
            {
                throw new Exception($"字典名称【{dictId}】必填");
            }

            var dict = drugContext.DataDictionary.FirstOrDefault(t => t.Id == dictId);
            if (dict == null)
            {
                throw new Exception($"未找到字典编号【{dictId}】的记录");
            }
            var checkDupDict = drugContext.DataDictionary.Any(t => t.CategoryName == dict.CategoryName && dict.DisplayName == t.DisplayName && t.Id != dictId);
            if (checkDupDict)
            {
                throw new Exception("不允许创建重复的字典");
            }

            dict.DisplayName = newDict.DisplayName;
            dict.Description = newDict.Description;
            await drugContext.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task DeleteDictionary(int dictId)
        {
            await drugContext.DataDictionary.Where(t => t.Id == dictId).ExecuteDeleteAsync();
        }

        [HttpPost]
        public async Task AddHierachyDictionary(HierachyDictionaryVo dict)
        {
            await AddHierachyDictionary(dict.CategoryName, new AddHierachyDictionaryVo() { Description = dict.Description, DisplayName = dict.DisplayName, ParentId = dict.ParentId });
        }

        private async Task AddHierachyDictionary(string categoryName, AddHierachyDictionaryVo dict)
        {
            var dict4 = drugContext.HierachyDictionary.Any(t => t.CategoryName == categoryName && dict.DisplayName == t.DisplayName);
            if (dict4)
            {
                throw new Exception("不允许创建重复的字典");
            }

            var maxValudId = drugContext.HierachyDictionary.Where(t => t.CategoryName == categoryName)
                                    .Select(t => t.Value).DefaultIfEmpty().Max();

            var dict3 = new HierarchyDictionary() { Id = 0, Description = dict.Description, DisplayName = dict.DisplayName, CategoryName = categoryName, Value = maxValudId + 1, ParentId = dict.ParentId };

            if (dict.ParentId == 0)
            {
                var checkTopLevel = await drugContext.HierachyDictionary.AnyAsync(t => t.CategoryName == categoryName && t.ParentId == 0);
                if (checkTopLevel)
                {
                    throw new Exception("只允许有一个顶级目录");
                }
            }

            if (dict.ParentId != 0)
            {
                var parentLevelInfo = await drugContext.HierachyDictionary.FirstOrDefaultAsync(t => t.CategoryName == categoryName && t.Value == dict.ParentId);
                if (parentLevelInfo == null)
                {
                    throw new Exception($"未找到当前层级的父级编号{dict.ParentId}");
                }
            }

            drugContext.HierachyDictionary.Add(dict3);
            await drugContext.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<List<HierachyDictionaryDto>> GetAllHierachyDictionary()
        {
            var result = await drugContext.HierachyDictionary.Select(t => new HierachyDictionaryDto()
            {
                Value = t.Value,
                Description = t.Description,
                Id = t.Id,
                DisplayName = t.DisplayName,
                CategoryName = t.CategoryName,
                ParentId = t.ParentId,
            }).ToListAsync();
            return result;
        }
        [HttpGet]
        public async Task<List<HierachyDictionaryDto>> GetHierachyDictionary(string categoryName)
        {
            return await drugContext.HierachyDictionary.Where(t => t.CategoryName == categoryName).Select(t => new HierachyDictionaryDto()
            {
                Value = t.Value,
                Description = t.Description,
                Id = t.Id,
                DisplayName = t.DisplayName,
                CategoryName = t.CategoryName,
                ParentId = t.ParentId,
            }).ToListAsync();
        }

        [HttpGet]
        public async Task<List<HierachyDictionaryDto>> GetCommonHierachyDictionary(string categoryName)
        {
            return await GetHierachyDictionary(CommonDict);
        }

        [HttpPost]
        public async Task AddCommonHierachyDictionary(AddHierachyDictionaryVo dict)
        {
            await AddHierachyDictionary(CommonDict, dict);
        }

        [HttpPut("{hierachyId}")]
        public async Task ModifyCommonHierachyDictionary(int hierachyId, ModifyDictionaryVo newDict)
        {
            var dict4 = drugContext.HierachyDictionary.Any(t => t.CategoryName == CommonDict && newDict.DisplayName == t.DisplayName && t.Id != hierachyId);
            if (dict4)
            {
                throw new Exception("不允许创建重复的层级名称");
            }

            var dict = await drugContext.HierachyDictionary.FirstOrDefaultAsync(t => t.CategoryName == CommonDict && t.Id == hierachyId);
            if (dict == null)
            {
                throw new Exception($"未找到层级编号【{hierachyId}】的记录");
            }

            if (String.IsNullOrWhiteSpace(newDict.DisplayName))
            {
                throw new Exception($"层级名称【{hierachyId}】必填");
            }
            dict.Description = newDict.Description;
            dict.DisplayName = newDict.DisplayName;
            await drugContext.SaveChangesAsync();
        }
    }

    public record HierachyDictionaryVo
    {
        public int ParentId { get; set; }
        public string DisplayName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public record AddHierachyDictionaryVo
    {
        public int ParentId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }

    public record HierachyDictionaryDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string DisplayName { get; set; }
        public int Value { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public record DictionaryDto
    {
        public string DisplayName { get; set; }
        public string CategoryName { get; set; }
        public int Id { get; set; }
        public int Value { get; set; }
    }

    public record ModifyDictionaryVo
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
