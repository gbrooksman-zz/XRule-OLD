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
    public class ProjectController : ControllerBase
    {
        private readonly ProjectEngine projectEngine;

        public ProjectController()
        {
            projectEngine = new ProjectEngine();
        }

        [HttpPost]
        [Route("Save")]
        public XProject Save([FromBody] XProject project)
        {
            Contract.Requires(project != null);
            return projectEngine.Save(project);
        }

        [HttpDelete]
        [Route("Delete")]
        public bool Delete([FromRoute] Guid id)
        {
            Contract.Requires(id != null);
            Contract.Requires(id != Guid.Empty);

            return projectEngine.Delete(id);
        }

        [HttpGet("{id}")]
        [Route("GetOne")]
        public XProject GetOne([FromRoute] Guid id)
        {
            Contract.Requires(id != null);
            Contract.Requires(id != Guid.Empty);

            return projectEngine.Get(id);
        }

        [HttpGet]
        [Route("GetAll")]
        public List<XProject> GetAll()
        {
            List<XProject> projectList = new List<XProject>();
            projectList =  projectEngine.GetList();
            return projectList;
        }
    }
}
