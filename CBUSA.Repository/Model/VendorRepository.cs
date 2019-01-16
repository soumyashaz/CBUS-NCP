﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Dynamic;

namespace CBUSA.Repository.Model
{
    class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(CBUSADbContext Context)
            : base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
    }
}
