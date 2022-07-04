using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.UpdateUserRecommendations
{
    public class UpdateUserRecommendationsCommandHandler : IRequestHandler<UpdateUserRecommendationsCommand, bool>
    {
        private readonly ILogger<UpdateUserRecommendationsCommandHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public UpdateUserRecommendationsCommandHandler(ILogger<UpdateUserRecommendationsCommandHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }

        public async Task<bool> Handle(UpdateUserRecommendationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recommenderServiceRepository.UpdateUserRecommendations(request.UserRecommendations.Select(x => new Domain.Models.DAO.UserRecommendationDAO(x.Id, x.UserId, x.TrackId, x.Score)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while updating user favorites: {exception}", ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
