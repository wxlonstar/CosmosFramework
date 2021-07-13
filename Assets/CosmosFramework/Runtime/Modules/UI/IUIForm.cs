﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Cosmos.UI
{
    public interface IUIForm
    {
        object Handle { get; }
        int Priority { get; }
        string UIFormName { get; }
        string UIGroupName { get; }
    }
}
