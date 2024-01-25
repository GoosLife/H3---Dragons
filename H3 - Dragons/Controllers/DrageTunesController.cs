using H3___Dragons.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace H3___Dragons.Controllers
{
    [ApiController]
    [Route("dragetunes")]
    public class DrageTunesController : Controller
    {
        [HttpGet]
        [Route("")]
        public string GetDragetunes()
        {
            // First, authenticate the user
            if (Request.Cookies["authorization"] != null)
            {
                try
                {
                    JwtManager.ValidateToken(Request.Cookies["authorization"]);
                }
                // Inform the user their login has expired
                catch (Exception ex)
                {
                    return "Your login has expired. Please log in again.";
                }
            }
            else
            {
                return "Please log in to access DrageTunes.";
            }

            // Suppose we retrieve this from a database
            string DrageTunes = 
                "1.  \"Dragon's Midnight Flight\"\r\n" +
                "2.  \"Whispers of the Drage\"\r\n" +
                "3.  \"Dragonfire Serenade\"\r\n" +
                "4.  \"Drage Dancing under the Stars\"\r\n" +
                "5.  \"The Dragon and the Dreamer\"\r\n" +
                "6.  \"Echoes of a Drage's Roar\"\r\n" +
                "7.  \"Dragon's Lullaby\"\r\n" +
                "8.  \"Sapphire Drage Skies\"\r\n" +
                "9.  \"The Drage's Secret Melody\"\r\n" +
                "10. \"Realm of the Fire Dragon\"";
            
            
            return DrageTunes;
        }

        // Musician dragons can upload songs
        [HttpPost]
        [Route("upload")]
        public string UploadSong(string songTitle)
        {
            // First, authenticate the user
            if (Request.Cookies["authorization"] != null)
            {
                try
                {
                    JwtManager.ValidateToken(Request.Cookies["authorization"]!);

                    if (!JwtManager.HasClaim(Request.Cookies["authorization"]!, "role", "musician"))
                    {
                        return "You must be a musician dragon to upload songs.";
                    }
                }

                // Inform the user their login has expired
                catch (Exception ex)
                {
                    return "Your login has expired. Please log in again.";
                }
            }

            else
            {
                return "Please log in to access DrageTunes.";
            }

            // Upload the song
            return "Song " + songTitle + " uploaded successfully.";
        }
    }
}
