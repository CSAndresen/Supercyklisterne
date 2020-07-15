using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuperCyklisterneAPI.Entities;
using SuperCyklisterneAPI.Helpers;
using SuperCyklisterneAPI.Models;
using SuperCyklisterneAPI.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/medlemmer")]
    public class MedlemmerController : ControllerBase
    {
        private readonly IHjemmesideRepository _HjemmesideRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _environment;

        public MedlemmerController(IHjemmesideRepository HjemmesideRepository, IMapper mapper, IOptions<AppSettings> appSettings, IWebHostEnvironment environment)
        {
            _HjemmesideRepository = HjemmesideRepository ??
                throw new ArgumentNullException(nameof(HjemmesideRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _appSettings = appSettings.Value;
            _environment = environment ??
                throw new ArgumentNullException(nameof(environment));
        }

        [AllowAnonymous]
        [HttpGet()]
        public IActionResult HentMedlemmer()
        {
            var MedlemmerFraRepo = _HjemmesideRepository.HentMedlemmer();
            return new JsonResult(MedlemmerFraRepo);
        }

        // Hent alle medlemmers billeder (Til /medlemmer)
        [AllowAnonymous]
        [HttpGet("images/alle")]
        public IActionResult AlleBilleder()
        {
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploads = Path.Combine(_environment.WebRootPath, "images/");
            if (!Directory.Exists(uploads)) return NoContent();

            var imagespath = Path.Combine(_environment.WebRootPath, "images");
            var filters = new String[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp", ".svg" };
            var baseUrl = "http://localhost:51647/images";

            var imgUrls = Directory.EnumerateFiles(imagespath, "*.*", SearchOption.AllDirectories)
            .Where(fileName => filters.Any(filter => fileName.EndsWith(filter)))
            .Select(fileName => Path.GetRelativePath(imagespath, fileName)) // get relative path
            .Select(fileName => Path.Combine(baseUrl, fileName))  
            .Select(fileName => fileName.Replace("\\", "/"));
            return new JsonResult(imgUrls);
        }

        [AllowAnonymous]
        [HttpGet("images/{MedlemsId}")]
        public IActionResult HentMedlemsBilleder(int MedlemsId)
        {
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var imagespath = Path.Combine(_environment.WebRootPath, "images/" + MedlemsId);
            var filters = new String[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp", ".svg" };
            var baseUrl = "http://localhost:51647/images/" + MedlemsId + "/";

            if (!Directory.Exists(imagespath)) return NoContent();

            var imgUrls = Directory.EnumerateFiles(imagespath, "*.*", SearchOption.AllDirectories)
            .Where(fileName => filters.Any(filter => fileName.EndsWith(filter)))
            .Select(fileName => Path.GetRelativePath(imagespath, fileName)) // get relative path
            .Select(fileName => Path.Combine(baseUrl, fileName))
            .Select(fileName => fileName.Replace("\\", "/"));

            return new JsonResult(imgUrls);
        }

        [AllowAnonymous]
        [HttpGet("{MedlemsId}", Name = "HentMedlem")]
        public IActionResult HentMedlem(int MedlemsId)
        {
            var MedlemFraRepo = _HjemmesideRepository.HentMedlem(MedlemsId);
            if (MedlemFraRepo == null) { return NotFound(); }
            return new JsonResult(MedlemFraRepo);
        }

        [AllowAnonymous]
        [HttpPost()]
        public ActionResult<MedlemDto> TilføjMedlem(MedlemTilTilføjelseDto medlem)
        {
            var MedlemEntity = _mapper.Map<Medlemmer>(medlem);
            _HjemmesideRepository.TilføjMedlem(MedlemEntity);
            _HjemmesideRepository.Save();

            var MedlemTilReturnering = _mapper.Map<MedlemDto>(MedlemEntity);
            return CreatedAtRoute("HentMedlem", new { MedlemsId = MedlemTilReturnering}, MedlemTilReturnering);

        }

        [HttpPatch("{MedlemsId}")]
        public ActionResult OpdaterMedlem(int MedlemsId,
            JsonPatchDocument<MedlemTilOpdateringDto> patchDocument)
        {
            var MedlemFraRepo = _HjemmesideRepository.HentMedlem(MedlemsId);

            if (MedlemFraRepo == null)
            {
                return NotFound();
            }

            var medlemTilOpdatering = _mapper.Map<MedlemTilOpdateringDto>(MedlemFraRepo);
            patchDocument.ApplyTo(medlemTilOpdatering, ModelState);

            if (!TryValidateModel(medlemTilOpdatering))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(medlemTilOpdatering, MedlemFraRepo);
            _HjemmesideRepository.OpdaterMedlem(MedlemFraRepo);
            _HjemmesideRepository.Save();

            return Ok(MedlemFraRepo);
        }

        [AllowAnonymous]
        [HttpDelete("{MedlemsId}")]
        public ActionResult SletMedlem(int MedlemsId)
        {
            var MedlemFraRepo = _HjemmesideRepository.HentMedlem(MedlemsId);

            if (MedlemFraRepo == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploads = Path.Combine(_environment.WebRootPath, "images/" + MedlemsId);
            if (Directory.Exists(uploads)) Directory.Delete(uploads, true);

            _HjemmesideRepository.SletMedlem(MedlemFraRepo);
            _HjemmesideRepository.Save();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult MedlemLogin([FromBody] MedlemLoginDto medlem)
        {
            var Medlem = _HjemmesideRepository.MedlemLogin(medlem.Email, medlem.Kodeord);

            if (Medlem == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Medlem.MedlemsId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                medlemsId = Medlem.MedlemsId,
                email = Medlem.Email,
                navn = Medlem.Navn,
                Token = tokenString
            });
        }

        // Logud [AllowAnonymous]

        [AllowAnonymous]
        [HttpPost("upload/{MedlemsId}")]
        public async Task Upload(IFormFile file, int MedlemsId)
        {
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploads = Path.Combine(_environment.WebRootPath, "images/" + MedlemsId);

            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            if (file.Length > 0)
            {
                using var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
        }

        [HttpDelete("images/{MedlemsId}/{FileString}")]
        public IActionResult SletBillede(int MedlemsId, string FileString)
        {
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var imagespath = Path.Combine(_environment.WebRootPath, "images/" + MedlemsId);

            if (!Directory.Exists(imagespath)) return NoContent();

            var filepath = Path.Combine(imagespath, FileString);
            FileInfo file = new FileInfo(filepath);


            file.Delete();
            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
