﻿using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Libawai.IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Libawai.IdentityServer.Controllers
{
    public class PictureController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PictureController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        private string GetClaimValue(string claimType)
        {
            var claim = User.Claims.FirstOrDefault(x => x.Type == claimType);
            return claim?.Value;
        }

        public IActionResult Index()
        {
            var picture = GetClaimValue(JwtClaimTypes.Picture);
            if (picture != null)
            {
                ViewData["picture"] = picture;
            }

            return View();
        }

        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("File is null");
            }

            if (file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                return BadRequest("File size cannot exceed 10M");
            }

            var acceptTypes = new[] {".jpg", ".jpeg", ".png"};
            if (acceptTypes.All(t => t != Path.GetExtension(file.FileName).ToLower()))
            {
                return BadRequest("File type not valid, only jpg and png are acceptable.");
            }

            if (string.IsNullOrWhiteSpace(_webHostEnvironment.WebRootPath))
            {
                _webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadsFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "avatars");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var userName = GetClaimValue(JwtClaimTypes.PreferredUserName);
            var fileName = $"{userName}.jpg";
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            var imageStream = file.OpenReadStream();
            using var image = Image.Load(imageStream, out _);
            image.Mutate(c => c.Resize(100, 100));
            image.SaveAsJpeg(stream);

            var user = await _userManager.FindByNameAsync(userName);
            var pictureClaims = User.Claims.Where(x => x.Type == JwtClaimTypes.Picture).ToList();
            if (pictureClaims.Any())
            {
                await _userManager.RemoveClaimsAsync(user, pictureClaims);
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Picture, fileName));
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            ViewData["picture"] = fileName;

            return View("Index");
        }
    }


}
