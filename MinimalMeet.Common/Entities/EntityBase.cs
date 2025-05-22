using System.ComponentModel.DataAnnotations;

namespace MinimalMeet.Common.Entities;

public class EntityBase<TEntityId>
{
    [Key]
    public TEntityId Id { get; set; }
}
