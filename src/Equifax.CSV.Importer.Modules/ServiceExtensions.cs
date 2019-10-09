﻿using Equifax.CSV.Importer.Logic.Abstract;
using Equifax.CSV.Importer.Logic.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Equifax.CSV.Importer.Modules
{
    public static class ServiceExtensions
    {
        public static void ConfigureIoC(this IServiceCollection services)
        {
            services.AddTransient<ICSVService, CSVService>();
        }
    }
}