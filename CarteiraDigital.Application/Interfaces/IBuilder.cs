﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteiraDigital.Application.Interfaces
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
