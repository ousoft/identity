﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Oyang.Identity.Domain
{
    public interface IRepository
    {

    }
    public interface IRepository<TEntity> :IRepository where TEntity : Entity
    {

    }
}
