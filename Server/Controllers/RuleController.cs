using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuleEditor.Server.Engines;
using RuleEditor.Shared.Entities;

namespace RuleEditor.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly RuleEngine ruleEngine;

        [HttpPost]
        [Route("Save")]
        public XRule Save([FromBody] XRule rule)
        {
            Contract.Requires(rule != null);
            return ruleEngine.Save(rule);
        }

        [HttpDelete]
        [Route("Delete")]
        public bool Delete([FromRoute] Guid id)
        {
            Contract.Requires(id != null);
            Contract.Requires(id != Guid.Empty);

            return ruleEngine.Delete(id);
        }

        [HttpGet("{id}")]
        [Route("GetOne")]
        public XRule GetOne([FromRoute] Guid id)
        {
            Contract.Requires(id != null);
            Contract.Requires(id != Guid.Empty);

            return ruleEngine.Get(id);
        }

        [HttpGet("{projectid}")]
        [Route("GetForProject")]
        public List<XRule> GetForProject([FromRoute] Guid projectid)
        {
            Contract.Requires(projectid != null);
            Contract.Requires(projectid != Guid.Empty);

            return ruleEngine.GetForProject(projectid);
        }
    }
}
