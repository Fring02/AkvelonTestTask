using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base;

//Base Entity has the common fields shareable between inheriting entities - in that project, it's id
public abstract class BaseEntity<TId>
{
    //Mark Id as a primary key in database explicitly, even though it's known that 
    //the property with name 'Id' is considered as a primary key already in EF.
    [Key]
    public TId Id { get; set; }
}