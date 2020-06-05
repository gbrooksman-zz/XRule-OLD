using LiteDB;
using RuleEditor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuleEditor.Server;

namespace RuleEditor.Server.Engines
{
    public class ProjectEngine
    {
        public ProjectEngine()
        {

        }

        public XProject Save(XProject project)
        {
            using (var db = new LiteDatabase(AppDB.Name))
            {
                var projectCol = db.GetCollection<XProject>("projects");
                projectCol.Upsert(project);
            }
            return project;
        }

        public bool Delete(Guid id)
        {
            bool deleted = false;

            using (var db = new LiteDatabase(AppDB.Name))
            {
                var projectCol = db.GetCollection<XProject>("projects");
                deleted = projectCol.Delete(id);
            }
            return deleted;
        }

        public XProject Get(Guid id)
        {
            XProject project = new XProject();

            using (var db = new LiteDatabase(AppDB.Name))
            {
                var projectCol = db.GetCollection<XProject>("projects");
                project = projectCol.FindById(id);
            }
            return project;
        }

        public List<XProject> GetList()
        {
            List<XProject> projectList = new List<XProject>();

            using (var db = new LiteDatabase(AppDB.Name))
            {
                var projectCol = db.GetCollection<XProject>("projects");
                if (projectCol.Count() > 0)
                {
                    projectList = projectCol.FindAll().ToList();
                }
            }
            return projectList;
        }
    }
}
