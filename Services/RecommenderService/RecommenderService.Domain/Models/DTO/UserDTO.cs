using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DTO
{
    public class UserDTO
    {
        public UserDTO(string id, string name, bool isUserReadyForRecommendation)
        {
            Id = id;
            Name = name;
            IsUserReadyForRecommendation = isUserReadyForRecommendation;
        }

        public UserDTO(string name)
        {
            Name = name;
        }

        public string Id { get; init; }

        public string Name { get; set; }
        public bool IsUserReadyForRecommendation { get; set; }

    }
}
