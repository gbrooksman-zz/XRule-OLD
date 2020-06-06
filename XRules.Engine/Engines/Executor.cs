using NRules;
using NRules.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XRules.Engine.Engines
{
    public class Executor
    {

        public Executor()
        {

        }

        public void  RunProject()
        {
            //Load rules
            var repository = new RuleRepository();
            //repository.Load(x => x.From(typeof(PreferredCustomerDiscountRule).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();
          

            //Start match/resolve/act cycle
            session.Fire();
        }
    }
}
