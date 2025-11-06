using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

// public class SubmitCheepModel : PageModel
// {
//     protected readonly ICheepService _service;

//     [BindProperty]
//     public string Message { get; set; }

//     public static Author CurrentAuthor;

//     public SubmitCheepModel(ICheepService service)
//     {
//         _service = service;
//     }
//     public ActionResult OnPost()
//     {
//         _service.PostCheep(CurrentAuthor, Message);
//         return Page();
//     }
// }