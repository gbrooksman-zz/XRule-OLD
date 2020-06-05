using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using RuleEditor.Shared.Entities;
using Serilog;

namespace RuleEditor.Server.Engines
{
    public class UserEngine
    {
        public UserEngine()
        {

        }

        public User Save(User user)
        {
			Contract.Requires(user != null);
			Log.Debug("User Add start");
			try
			{
				

			
				Log.Debug($"Added user --  id: {user.Id} email: {user.Email}");
					
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Inserting user into database");
				throw;
			}
			Log.Debug("User Add finish");
			return user;
		}


    }
}
