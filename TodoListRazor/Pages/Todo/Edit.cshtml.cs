using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoListRazor.Authorization;
using TodoListRazor.Data;
using TodoListRazor.Models;

namespace TodoListRazor.Pages.Todo
{
    public class EditModel : DI_BasePageModel
    {
        [BindProperty]
        public TodoTask TodoTask { get; set; } = default!;

        public EditModel(ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<IdentityUser> userManager) 
            : base(context, authorizationService, userManager)
        {
        }

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
                User, TodoTask, TodoTaskOperations.Update);

            if (!isAuthorized.Succeeded)
                return Forbid();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var task = await Context.TodoTask
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (task == null)
                return NotFound();

            TodoTask.CreatorId = task.CreatorId;
            
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, TodoTask, TodoTaskOperations.Update);

            if (!isAuthorized.Succeeded)
                return Forbid();

            Context.Attach(TodoTask).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoTaskExists(TodoTask.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TodoTaskExists(int id)
        {
          return Context.TodoTask.Any(e => e.Id == id);
        }
    }
}
