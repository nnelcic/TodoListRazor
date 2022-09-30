using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListRazor.Authorization;
using TodoListRazor.Data;
using TodoListRazor.Models;

namespace TodoListRazor.Pages.Todo
{
    public class DeleteModel : DI_BasePageModel
    {
        [BindProperty]
        public TodoTask TodoTask { get; set; }

        public DeleteModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        { }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.TodoTask == null)
                return NotFound();

            var todotask = await Context.TodoTask.FirstOrDefaultAsync(m => m.Id == id);

            if (todotask == null)
                return NotFound();
            else
                TodoTask = todotask;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, TodoTask, TodoTaskOperations.Delete);
            if (!isAuthorized.Succeeded)
                return Forbid();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || Context.TodoTask == null)
            {
                return NotFound();
            }

            var todotask = await Context.TodoTask.FindAsync(id);
            
            if (todotask != null)
                TodoTask = todotask;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, TodoTask, TodoTaskOperations.Delete);
            if (!isAuthorized.Succeeded)
                return Forbid();

            Context.TodoTask.Remove(TodoTask);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
