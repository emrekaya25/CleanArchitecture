﻿using System;
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
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }

    #region Audit Log
    public DateTimeOffset CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public DateTimeOffset DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    #endregion
}

