using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestBackend.Domain.Commands;
using TestBackend.Domain.Models;
using TestBackend.Service;

namespace TestBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly Settings _appSettings;

        public UserController(UserService userService, IOptions<Settings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginCommand command)
        {
            var response = _userService.Login(command);
            
            if (!response.Ok())
                return BadRequest(response);

            var token = GerarJwt(response.Data as Usuario);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Create(CreateUserCommand command)
        {
            var response = _userService.Insert(command);

            if (!response.Ok())
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> Delete(DeleteUserCommand command)
        {
            var response = _userService.Delete(command);

            if (!response.Ok())
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [Route("{userId}")]
        public async Task<ActionResult> Update([FromBody]UpdateUserCommand command, int userId)
        {
            command.UserId = userId;
            var response = _userService.Update(command);

            if (!response.Ok())
                return BadRequest(response);

            return Ok(response);
        }

        [NonAction]
        private string GerarJwt(Usuario usuario)
        {
            var identityClaims = new ClaimsIdentity();
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome)
            };
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Readme()
        {
            #region breve descrição
            return Ok(@"
Olá, Junto Seguros :)

O framework utilizado foi .NET Core 2.2 junto com o banco de dados SQL Server, e dado o tempo do desafio, não me atentei a boas práticas arquiteturais,
pois tive como objetivo fazer algo simples a ponto de ter uma aplicação funcional e possibilitando o teste da camada de serviço e repositório.

Para a autenticação utilizei JSON Web Tokens, devido ao método ser consolidado no mercado e também pelo .NET possuir bibliotecas
que facilitam muito a sua implementação.

Dado o caso de uso, profissionalmente eu utilizaria o próprio Microsoft Identity, porém a aplicação
estaria pronta apenas utilizando o modelo de projeto do Visual Studio.
Creio que o intuito do teste era ver código escrito manualmente pelo programador, então fiz dessa forma que não é usual no mercado.

Agradeço a oportunidade e espero que gostem :D

Ps.: Foi triste ter que colocar os commands no Domain.

A aplicação possui os seguintes endpoints:

(POST) api/user/register
Cria um usuário no sistema. Através deste usuário, é possível realizar a autenticação para acessar os outros métodos de CRUD.
É necessário enviar no corpo da requisição os campos Nome, Email e Password, todos textos.

(POST) api/user/login 
Responsável por retornar o token de autenticação assim possibilitando acesso aos métodos abaixo
É necessário enviar no corpo da requisição os campos Email e Password, ambos textos.

(DELETE) api/user
Deleta um usuário na base de dados através do seu identificador UserId.
É necessário enviar no corpo da requisição o campo UserId do tipo inteiro.

(PUT) api/user/{userId}
Atualiza um usuário na base através do seu identificador UserId.
É necessário enviar no corpo da requisição os campos Name, Email, Pasword, todos textos. Também é preciso colocar o identificador
userId na rota.

Script da tabela abaixo:

CREATE TABLE Usuario (
	UsuarioId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(100) NOT NULL,
	Email VARCHAR(100),
	Senha VARCHAR(100)
)");
            #endregion
        }
    }
}
