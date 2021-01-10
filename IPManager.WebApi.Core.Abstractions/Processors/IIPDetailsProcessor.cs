﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Models;

namespace IPManager.WebApi.Core.Abstractions.Processors
{
    public interface IIPDetailsProcessor
    {
        Task<IPDetails> GetDetails(string ip);

    }
}