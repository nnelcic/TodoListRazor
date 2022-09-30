using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using TodoListRazor.Models;

namespace TodoListRazor.Authorization
{
    public class TodoCreatorAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, TodoTask>
    {
        UserManager<IdentityUser> _userManager;
        public TodoCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        } 

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            TodoTask resource)
        {
            // failed
            // return Task.CompletedTask;
            // success
            // context.Succeed(requirement);

            if(context.User == null || resource == null)
                return Task.CompletedTask;

            if (requirement.Name != Constants.CreateOperationName &&
               requirement.Name != Constants.ReadOperationName &&
               requirement.Name != Constants.UpdateOperationName &&
               requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            // if you are the creator (id matches) then it succeed
            if (resource.CreatorId == _userManager.GetUserId(context.User))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
