using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Libawai.Core.Entities;
using Libawai.Core.Interfaces;
using Libawai.Infrastructure.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libawai.API.Controllers
{
    [Route("api/postimages")]
    public class PostImageController:ControllerBase
    {
        private readonly IPostImageRepository _postImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostImageController(
            IPostImageRepository postImageRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment hostEnvironment)
        {
            _postImageRepository = postImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Upload(IFormFile file)
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

            if (string.IsNullOrWhiteSpace(_hostEnvironment.WebRootPath))
            {
                _hostEnvironment.WebRootPath
                    = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadsFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var postImage = new PostImage
            {
                FileName = fileName
            };

            _postImageRepository.Add(postImage);

            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PostImage, PostImageResource>(postImage);
            return Ok(result);
        }
    }
}
