using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RuleEditor.Shared.Entities
{
    public class XRule : XBase
    {

        public XRule()
        {

        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(-1000, 1000)]        
        public int Priority { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
       
        public string Tags { get; set; }

        [Required]
        public int Repeatability { get; set; }

        [Required]
        [StringLength(5000)]
        public string RawBody { get; set; }

        [StringLength(5000)]
        public string FormattedBody { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}
