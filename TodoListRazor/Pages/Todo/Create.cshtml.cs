using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListRazor.Authorization;
using TodoListRazor.Data;
using TodoListRazor.Models;

namespace TodoListRazor.Pages.Todo
{
    public class CreateModel : DI_BasePageModel
    {
        [BindProperty]
        public TodoTask TodoTask { get; set; }

        public CreateModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        { }

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            TodoTask.CreatorId = UserManager.GetUserId(User);
            TodoTask.IsCompleted = false;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, TodoTask, TodoTaskOperations.Create);

            if (!isAuthorized.Succeeded)
                return Forbid();

            Context.TodoTask.Add(TodoTask);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
