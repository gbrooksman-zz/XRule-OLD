using System;
using System.Collections.Generic;
using System.Text;

namespace RuleEditor.Shared.Entities
{
    public class XRule
    {

        public XRule()
        {

        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public int Repeatability { get; set; }

        public string Body { get; set; }

        public bool IsActive { get; set; }

    }
}
