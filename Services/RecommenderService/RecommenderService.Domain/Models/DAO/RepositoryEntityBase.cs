using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DAO
{
    public abstract class RepositoryEntityBase
    {
        public string Id { get; init; }
    }
}
