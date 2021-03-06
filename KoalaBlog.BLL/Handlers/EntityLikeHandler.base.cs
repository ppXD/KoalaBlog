﻿using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class EntityLikeHandlerBase : EntityHandlerBase<EntityLike>
    {
        private readonly DbContext _dbContext;

        public EntityLikeHandlerBase(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
