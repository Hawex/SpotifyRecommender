using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Models.DTO;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.GetUserRecommendations
{
    public class GetUserRecommendationsQueryHandler : IRequestHandler<GetUserRecommendationsQuery, IEnumerable<UserRecommendationDTO>>
    {
        private readonly ILogger<GetUserRecommendationsQueryHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public GetUserRecommendationsQueryHandler(ILogger<GetUserRecommendationsQueryHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }

        public async Task<IEnumerable<UserRecommendationDTO>> Handle(GetUserRecommendationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return (await _recommenderServiceRepository.GetUserRecommendations(request.UserId))?.Select(x => new UserRecommendationDTO(x.Id, x.UserId, x.TrackId, x.Score)) ?? new List<UserRecommendationDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while getting recommendations for user {userId}: {exception}", request.UserId, ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
