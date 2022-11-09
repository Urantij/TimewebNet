using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeWebNet;

namespace TimewebNet.Categories;

public abstract class CategoryApi
{
    protected readonly TimeWebApi api;

    protected CategoryApi(TimeWebApi api)
    {
        this.api = api;
    }
}
