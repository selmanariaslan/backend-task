using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Core.Entities.CommonModel
{
    public class CommonUpsertModel
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
    }
}
