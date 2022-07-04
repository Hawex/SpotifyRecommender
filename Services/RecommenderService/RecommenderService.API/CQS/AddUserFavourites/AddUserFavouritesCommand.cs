using MediatR;
using RecommenderService.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.AddUserFavourites
{
    public class AddUserFavouritesCommand : IRequest<bool>
    {
        public AddUserFavouritesCommand(List<UserFavouriteDTO> userFavourites)
        {
            UserFavourites = userFavourites ?? throw new ArgumentNullException(nameof(userFavourites));
        }

        public List<UserFavouriteDTO> UserFavourites { get; }
    }
}
