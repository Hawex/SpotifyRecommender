using MediatR;
using RecommenderService.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.GetUsers
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDTO>>
    {
    }
}
