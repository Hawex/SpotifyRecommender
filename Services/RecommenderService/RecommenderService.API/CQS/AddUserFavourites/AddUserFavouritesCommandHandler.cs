using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.AddUserFavourites
{
    public class AddUserFavouriteArtistCommandHandler : IRequestHandler<AddUserFavouritesCommand, bool>
    {
        private readonly ILogger<AddUserFavouriteArtistCommandHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public AddUserFavouriteArtistCommandHandler(ILogger<AddUserFavouriteArtistCommandHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }
        public async Task<bool> Handle(AddUserFavouritesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recommenderServiceRepository.AddUserFavourites(request.UserFavourites.Select(x => new Domain.Models.DAO.UserFavouriteDAO(x.UserId, x.EntityType, x.EntityIdentifier, x.Score)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while adding user favourites: {exception}", ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
