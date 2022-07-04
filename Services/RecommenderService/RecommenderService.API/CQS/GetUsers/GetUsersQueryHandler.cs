using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Models.DTO;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDTO>>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = new List<UserDTO>();
                var users = await _recommenderServiceRepository.GetUsers();
                foreach (var user in users)
                {
                    var isUserReadyForRecommendation = (await _recommenderServiceRepository.GetUserFavourites(user.Id))?.Count() > 0;
                    res.Add(new UserDTO(user.Id, user.Name, isUserReadyForRecommendation));
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while getting users: {exception}", ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
