using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Algebra.App.Pages
{
    public class PrimeFactorsModel
    {
        [Required]
        public string NumberSingleOrList { get; set; }
        public int Method { get; set; }
    }
}
