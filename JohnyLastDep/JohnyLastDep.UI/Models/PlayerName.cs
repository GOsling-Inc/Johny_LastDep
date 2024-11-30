using System.ComponentModel.DataAnnotations;

namespace JohnyLastDep.UI.Models
{
	public class PlayerName
	{
		[Required(ErrorMessage = "Player name is required.")]
		public string Name { get; set; }
	}
}
