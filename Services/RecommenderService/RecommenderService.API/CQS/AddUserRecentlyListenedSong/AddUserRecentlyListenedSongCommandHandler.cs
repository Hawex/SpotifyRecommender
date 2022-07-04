using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.AddUserRecentlyListenedSong
{
    public class AddUserRecentlyListenedSongCommandHandler : IRequestHandler<AddUserRecentlyListenedSongCommand, bool>
    {
        private readonly ILogger<AddUserRecentlyListenedSongCommandHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public AddUserRecentlyListenedSongCommandHandler(ILogger<AddUserRecentlyListenedSongCommandHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }
        public async Task<bool> Handle(AddUserRecentlyListenedSongCommand request, CancellationToken cancellationToken)
        {

            try
            {
                return await _recommenderServiceRepository.AddUserRecentlyListenedSong(new Domain.Models.DAO.UserRecentlyListenedSongDAO(request.TrackId, request.UserId, DateTime.Now));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while adding recently listened song: {exception}", ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
