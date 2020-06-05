using RuleEditor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuleEditor.Client
{
    public interface IAppStateContainer
    {
        XProject Project { get; set; }
        XRule Rule { get; set; }
    }


    public class AppStateContainer : IAppStateContainer
    {
        public XProject Project { get; set; }

        public XRule Rule { get; set; }
    }
}
