using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using RuleEditor.Shared.Entities;

namespace RuleEditor.Server
{
    public class AppDB
    {
        private static readonly string _connectionString;

        public static string Name { get; }

        static AppDB()
        {
            string dir = Directory.GetCurrentDirectory();
            _connectionString = @"/data/rule_editor.db";
            Name = _connectionString;
        }

        public static void AffirmStructure()
        {
            using var db = new LiteDatabase(_connectionString);
            var projectCol = db.GetCollection<XProject>("projects");
            projectCol.EnsureIndex("Name");
            var ruleCol = db.GetCollection<XRule>("rules");
            ruleCol.EnsureIndex("Name");
            var userCol = db.GetCollection<User>("users");
            userCol.EnsureIndex("UserName");
        }
    }
}
