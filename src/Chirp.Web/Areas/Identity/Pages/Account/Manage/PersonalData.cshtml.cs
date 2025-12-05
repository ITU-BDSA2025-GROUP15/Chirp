// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chirp.Web.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly IAuthorService _authorService;
        private readonly ICheepService _cheepService;
        private readonly UserManager<Author> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public required List<CheepDTO> Cheeps { get; set; }
        public required string[] Follows { get; set; }
        public required Dictionary<string, string> PersonalData { get; set; } = new Dictionary<string, string>();

        public PersonalDataModel(
            IAuthorService authorService,
            ICheepService cheepService,
            UserManager<Author> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _authorService = authorService;
            _cheepService = cheepService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var personalDataProps = typeof(Author).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            
            foreach (var p in personalDataProps)
            {
                PersonalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            Cheeps = _cheepService.GetAllCheepsFromAuthor(user.Name);

            Follows = await _authorService.GetFollowingByName(user.Name);

            return Page();
        }
    }
}
