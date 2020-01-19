using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Linq;
using TestBackend.CrossCutting;
using TestBackend.Domain.Commands;
using TestBackend.Domain.Models;
using TestBackend.Infrastructure;
using TestBackend.Service;

namespace TestBackend.Test
{
    public class Tests
    {
        public IConfiguration Configuration { get; set; }

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            ConnectionStrings.TestbackendConnectionString = Configuration.GetConnectionString("TestBackendConnection");
        }

        [Test]
        public void TestValidationLogin()
        {
            var userService = new UserService(new UserRepository());

            var command = new LoginCommand()
            {
                Email = "email@email.com",
                Password = "senha123"
            };

            var result = userService.ValidaLogin(command);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void TestValidationCreate()
        {
            var userService = new UserService(new UserRepository());

            var command = new CreateUserCommand()
            {
                Name = "teste",
                Email = "email@email.com",
                Password = "senha123"
            };

            var result = userService.ValidaNovoUsuario(command);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void TestValidationUpdate()
        {
            var userService = new UserService(new UserRepository());

            var command = new UpdateUserCommand()
            {  
                UserId = 4,
                Name = "teste",
                Email = "email@email.com",
                Password = "senha123"
            };

            var result = userService.ValidaUpdate(command);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void TestValidationDelete()
        {
            var userService = new UserService(new UserRepository());

            var command = new DeleteUserCommand()
            {
                UserId = 4
            };

            var result = userService.ValidaDelete(command);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void TestRepoCreate()
        {
            var userRepository = new UserRepository();

            var guid = Guid.NewGuid().ToString();

            var usuario = new Usuario()
            {
                Nome = "Teste 1234",
                Email = $"email{guid.Substring(0, 6)}@email.com",
                Senha = guid
            };

            var result = userRepository.Insert(usuario);

            Assert.IsTrue(result > 0);
        }

        [Test]
        public void TestRepoLogin()
        {
            var userRepository = new UserRepository();

            var guid = Guid.NewGuid().ToString();

            var usuario = new Usuario()
            {
                Nome = "Teste 1234",
                Email = $"email{guid.Substring(0, 6)}@email.com",
                Senha = guid
            };

            userRepository.Insert(usuario);

            var result = userRepository.SelectByLoginAndPassword(usuario.Email, usuario.Senha);

            Assert.IsTrue(result.UsuarioId > 0);
        }

        [Test]
        public void TestRepoUpdate()
        {
            var userRepository = new UserRepository();

            var guid = Guid.NewGuid().ToString();

            var usuario = new Usuario()
            {
                Nome = $"Teste{guid.Substring(0, 6)}",
                Email = $"email{guid.Substring(0, 6)}@email.com",
                Senha = guid
            };

            usuario.UsuarioId = userRepository.Insert(usuario);
            usuario.Email = $"email_email{guid.Substring(0, 6)}@email.com";

            userRepository.Update(usuario);

            var anotherUser = userRepository.SelectByEmail(usuario.Email);

            Assert.IsTrue(anotherUser.Email == usuario.Email);
        }

        [Test]
        public void TestRepoDelete()
        {
            var userRepository = new UserRepository();

            var guid = Guid.NewGuid().ToString();

            var usuario = new Usuario()
            {
                Nome = $"Teste{guid.Substring(0, 6)}",
                Email = $"email{guid.Substring(0, 6)}@email.com",
                Senha = guid
            };

            int userId = userRepository.Insert(usuario);

            Assert.That(() => userRepository.Delete(userId), Throws.Nothing);
        }
    }
}