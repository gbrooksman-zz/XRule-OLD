using System;
using System.Collections.Generic;
using System.Text;

namespace RuleEditor.Shared.Entities
{
    public class XProject : XBase
    {
        public XProject()
        {

        }


        public string Name { get; set; }

        public List<Guid> RuleIdList { get; set; }

        public bool IsActive { get; set; }

    }
}
