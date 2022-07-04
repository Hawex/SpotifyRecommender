using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
    {
        private readonly ILogger<AddUserCommandHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;

        public AddUserCommandHandler(ILogger<AddUserCommandHandler> logger, IRecommenderServiceRepository recommenderServiceRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
        }

        public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recommenderServiceRepository.AddUser(new Domain.Models.DAO.UserDAO(request.UserDTO.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while adding new user: {exception}", ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
    }
}
