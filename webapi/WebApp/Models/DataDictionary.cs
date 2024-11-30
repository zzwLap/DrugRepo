using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;
public record DataDictionary
{
    [Key]
    public int Id { get; set; }
    [Comment("值")]
    public int Value { get; set; }
    [Comment("名称")]
    [StringLength(100)]
    public string DisplayName { get; set; }
    [Comment("描述")]
    [StringLength(100)]
    public string? Description { get; set; }
    [Comment("类别名称")]
    [StringLength(100)]
    public string CategoryName { get; set; }
    [Comment("类别编码--有没有必须呢")]
    public int CategoryCode { get; set; }
}

public record HierarchyDictionary
{
    [Key]
    public int Id { get; set; }
    public int ParentId { get; set; }
    [Comment("值")]
    public int Value { get; set; }
    [Comment("名称")]
    [StringLength(100)]
    public required string DisplayName { get; set; }
    [Comment("备注")]
    [StringLength(100)]
    public string? Description { get; set; }
    [Comment("类别名称")]
    [StringLength(100)]
    public required string CategoryName { get; set; }
}
