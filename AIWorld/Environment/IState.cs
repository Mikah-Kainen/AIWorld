﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IState<TStateData>
    {
        TStateData GetData();
    }
}
