using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.WebApp.Models
{
    public class UserModel
    {
        [Required]
        public string Login { get; set; }
        public string Id { get; set; }
        public bool IsUserReadyForRecommendation { get; set; }
    }
}
