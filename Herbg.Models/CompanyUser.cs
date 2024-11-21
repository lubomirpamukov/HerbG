using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Models
{
    public class CompanyUser
    {
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; } = null!;

        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; } = null!;

        public virtual ApplicationUser Client { get; set; } = null!;
    }
}
