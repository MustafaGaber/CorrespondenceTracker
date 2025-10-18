using CorrespondenceTracker.Application.Users.Commands.CreateUser;
using CorrespondenceTracker.Application.Users.Commands.DeleteUser;
using CorrespondenceTracker.Application.Users.Commands.UpdateUser;
using CorrespondenceTracker.Application.Users.Queries.GetUser;
using CorrespondenceTracker.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IGetUsersQuery _getUsersQuery;
        private readonly IGetUserQuery _getUserQuery;
        private readonly ICreateUserCommand _createUserCommand;
        private readonly IUpdateUserCommand _updateUserCommand;
        private readonly IDeleteUserCommand _deleteUserCommand;

        public UsersController(
            IGetUsersQuery getUsersQuery,
            IGetUserQuery getUserQuery,
            ICreateUserCommand createUserCommand,
            IUpdateUserCommand updateUserCommand,
            IDeleteUserCommand deleteUserCommand)
        {
            _getUsersQuery = getUsersQuery;
            _getUserQuery = getUserQuery;
            _createUserCommand = createUserCommand;
            _updateUserCommand = updateUserCommand;
            _deleteUserCommand = deleteUserCommand;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _getUsersQuery.Execute();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _getUserQuery.Execute(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _createUserCommand.Execute(request);
            return Ok(result);
        }

        [HttpPut("{id}")] // Update
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] CreateUserRequest request)
        {
            await _updateUserCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _deleteUserCommand.Execute(id);
            return Ok();
        }
    }
}