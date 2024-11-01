using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Validation;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityServerAspNetIdentity.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        [BindProperty]
        public RegisterModel Input { get; set; } = default!;
        [BindProperty]
        public bool RegisterSuccess {get;set;} = false;
        [BindProperty]
        public bool PasswordErrorExists {get;set;}
        [BindProperty]
        public List<string> PasswordErrors {get;set;}
        private readonly UserManager<ApplicationUser> userManager;

        public Index(UserManager<ApplicationUser> _userManager)
        {
            userManager=_userManager;
        }

        public async Task<IActionResult> OnGet(string? returnUrl)
        {
            await buildModel(returnUrl);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if(Input.button!="Register")
            {
                return Redirect("~/");
            }
            if(ModelState.IsValid)
            {
                var user=new ApplicationUser
                {
                    UserName=Input.UserName,
                    Email=Input.email,
                    EmailConfirmed=true
                };
                var result= await userManager.CreateAsync(user, Input.Password);
                if(result.Succeeded)
                {
                    result=userManager.AddToRoleAsync(user,"User").Result;
                    if(result.Succeeded)
                    {
                        var createdUser=await userManager.FindByEmailAsync(user.Email);
                        var id=createdUser.Id;
                        result = userManager.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name,Input.FullName),
                            new Claim(JwtClaimTypes.Role,"User"),
                            new Claim(JwtClaimTypes.Id,id)
                        }).Result;
                        if(result.Succeeded){
                            RegisterSuccess=true;
                        }
                    }
                }
                else
                {
                PasswordErrorExists=true;
                PasswordErrors=result.Errors.Select(x=>x.Description.ToString()).ToList();
                }
            }
            return Page();
        }
        private async Task buildModel(string? returnUrl)
        {
            Input=new RegisterModel{
                returnUrl=returnUrl
            };
        }
    }
}