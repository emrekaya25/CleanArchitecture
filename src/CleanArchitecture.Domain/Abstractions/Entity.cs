using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstractions;

//Interface yerine abstract class yapıp kullanıyoruz.
public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
    public DateTimeOffset DeleteAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}

