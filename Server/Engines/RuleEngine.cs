using LiteDB;
using RuleEditor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuleEditor.Server;

namespace RuleEditor.Server.Engines
{
    public class RuleEngine
    {
        public RuleEngine()
        {

        }

        public XRule Save(XRule rule)
        {
            using (var db = new LiteDatabase(AppDB.Name))
            {
                var ruleCol = db.GetCollection<XRule>("rules");
                ruleCol.Upsert(rule);
            }
            return rule;
        }

        public bool Delete(Guid id)
        {
            bool deleted = false;

            using (var db = new LiteDatabase(AppDB.Name))
            {
                var ruleCol = db.GetCollection<XRule>("rules");
                deleted = ruleCol.Delete(id);
            }
            return deleted;
        }

        public List<XRule> GetForProject(Guid projectId)
        {
            List<XRule> ruleList = new List<XRule>();

            using (var db = new LiteDatabase(AppDB.Name))
            {
                var ruleCol = db.GetCollection<XRule>("rules");
                var projectCol = db.GetCollection<XProject>("projects");

                XProject project = projectCol.FindById(projectId);

                project.RuleIdList.ForEach(r =>
                {
                    ruleList.Add(ruleCol.FindById(r));
                });

            }
            return ruleList;
        }

        public XRule Get(Guid ruleId)
        {
            XRule rule = new XRule();

            using (var db = new LiteDatabase(AppDB.Name))
            {
                var ruleCol = db.GetCollection<XRule>("rules");
                rule = ruleCol.FindById(ruleId); 
            }
            return rule;
        }
    }
}
