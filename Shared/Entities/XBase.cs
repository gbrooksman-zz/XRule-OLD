using System;
using System.Collections.Generic;
using System.Text;

namespace RuleEditor.Shared.Entities
{
    public class XBase
    {
        public XBase()
        {

        }

        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
