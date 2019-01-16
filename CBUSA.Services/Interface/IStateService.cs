﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
   public interface IStateService
    {
       IEnumerable<State> GetState();
     
       IEnumerable<State> GetStateByName(string State);
    }
}
