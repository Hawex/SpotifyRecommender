using MediatR;
using RecommenderService.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.AddUser
{
    public class AddUserCommand : IRequest<bool>
    {
        public AddUserCommand(UserDTO userDTO)
        {
            UserDTO = userDTO ?? throw new ArgumentNullException(nameof(userDTO));
        }

        public UserDTO UserDTO { get; }
    }
}
