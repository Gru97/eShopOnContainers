using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.Reporting
{
    public class QueryModelConfiguration
    {
        public QueryModelConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

    }
}
