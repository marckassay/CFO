using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJob
{
    public class WOD : TableEntity
    {
        public string Title { get; set; }

        public string Body { get; set; }
    }
}
