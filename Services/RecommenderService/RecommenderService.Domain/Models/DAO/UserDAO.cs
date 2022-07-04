using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DAO
{
    public class UserDAO : RepositoryEntityBase
    {
        public UserDAO(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public UserDAO(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }
        public string Name { get; set; }

    }
}
