using System.ComponentModel.DataAnnotations;

namespace JohnyLastDep.UI.Models
{
    public class CreateRoom
    {

        [Required(ErrorMessage = "Room name is required.")]
        public string RoomName { get; set; }
    }
}
