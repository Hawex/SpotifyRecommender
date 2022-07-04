using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.UpdateUserFavouritesScore
{
    public class UpdateUserFavouritesScoreCommandHandler : IRequestHandler<UpdateUserFavouritesScoreCommand, bool>
    {
        private readonly ILogger<UpdateUserFavouritesScoreCommandHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public UpdateUserFavouritesScoreCommandHandler(ILogger<UpdateUserFavouritesScoreCommandHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }


        public async Task<bool> Handle(UpdateUserFavouritesScoreCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recommenderServiceRepository.UpdateUserFavouritesScore(request.UserFavouriteId, request.Score);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while updating user favorites: {exception}", ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
