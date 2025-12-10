using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using GLeague.Data;
using GLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GLeague.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            // Account
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            // Player profile
            [Required, MaxLength(100)]
            [Display(Name = "First name")]
            public string FirstName { get; set; } = string.Empty;

            [Required, MaxLength(100)]
            [Display(Name = "Last name")]
            public string LastName { get; set; } = string.Empty;

            [Phone]
            [MaxLength(20)]
            [Display(Name = "Phone number")]
            public string? PhoneNumber { get; set; }

            [MaxLength(100)]
            [Display(Name = "City of residence")]
            public string? CityOfResidence { get; set; }

            [Display(Name = "Experience level")]
            public ExperienceLevel ExperienceLevel { get; set; } = ExperienceLevel.None;

            [Display(Name = "Preferred position")]
            public PlayerPosition PreferredPosition { get; set; } = PlayerPosition.Unknown;

            [Display(Name = "Jersey size")]
            public JerseySize JerseySize { get; set; } = JerseySize.Unknown;

            [MaxLength(20)]
            [Display(Name = "Jersey name")]
            public string? JerseyName { get; set; }

            [MaxLength(4)]
            [Display(Name = "Jersey number")]
            public string? JerseyNumber { get; set; }

            [Display(Name = "Height (inches)")]
            public int? HeightInInches { get; set; }

            [Display(Name = "Weight (lbs)")]
            public int? WeightInPounds { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Date of birth")]
            public DateTime? DateOfBirth { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            // NOTE: EmailConfirmed remains false; this is our "pending approval" state.
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("New player account created for {Email}", Input.Email);

                // Create player profile tied to this user
                var profile = new PlayerProfile
                {
                    UserId = user.Id,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,
                    CityOfResidence = Input.CityOfResidence,
                    ExperienceLevel = Input.ExperienceLevel,
                    PreferredPosition = Input.PreferredPosition,
                    JerseySize = Input.JerseySize,
                    JerseyName = Input.JerseyName,
                    JerseyNumber = Input.JerseyNumber,
                    HeightInInches = Input.HeightInInches,
                    WeightInPounds = Input.WeightInPounds,
                    DateOfBirth = Input.DateOfBirth
                };

                _db.PlayerProfiles.Add(profile);
                await _db.SaveChangesAsync();

                // Put them in the Player role (you already seed this role in Program.cs)
                await _userManager.AddToRoleAsync(user, "Player");

                // Do NOT sign them in. They must be approved first.
                TempData["RegistrationMessage"] =
                    "Thanks! Your player registration has been submitted. " +
                    "You’ll be able to sign in once a league admin approves your account.";

                return RedirectToPage("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
