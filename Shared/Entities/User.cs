using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RuleEditor.Shared.Entities
{
    public class User : XBase
    {
		public User()
		{

		}

		[Required]
		[StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
		public string Email { get; set; }

		[Required]
		[StringLength(15, ErrorMessage = "Password cannot be longer than 15 characters")]
		public string Password { get; set; }

		[NotMapped]
		[MaxLength(15)]
		public string ConfirmPassword { get; set; }
	}
}
