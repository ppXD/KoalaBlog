﻿using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class EntityComplainHandlerBase : EntityHandlerBase<EntityComplain>
    {
        private readonly DbContext _dbContext;

        public EntityComplainHandlerBase(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
