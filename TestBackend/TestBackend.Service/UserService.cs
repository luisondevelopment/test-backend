using System.Collections.Generic;
using TestBackend.Domain;
using TestBackend.Domain.Commands;
using TestBackend.Domain.Models;
using TestBackend.Infrastructure;

namespace TestBackend.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ApplicationResponse Insert(CreateUserCommand command)
        {
            var errors = ValidaNovoUsuario(command);
            int userId = 0;

            if (_userRepository.SelectByEmail(command.Email) != null)
                errors.Add("Este login já foi cadastrado no sistema");

            if (errors.Count == 0)
                userId = _userRepository.Insert(new Usuario() { Email = command.Email, Nome = command.Email, Senha = command.Password });

            return new ApplicationResponse(new { userId }, errors);
        }

        public List<string> ValidaNovoUsuario(CreateUserCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command.Name) || command.Name.Length > 100)
                errors.Add("Nome inválido");

            if (string.IsNullOrEmpty(command.Email) || command.Email.Length > 100)
                errors.Add("Email inválido");

            if (string.IsNullOrEmpty(command.Password) || command.Password.Length > 100)
                errors.Add("Senha inválida");

            return errors;
        }

        public ApplicationResponse Login(LoginCommand command)
        {
            var errors = ValidaLogin(command);
            Usuario user = null;

            if (errors.Count == 0)
                user = _userRepository.SelectByLoginAndPassword(command.Email, command.Password);

            if (user == null)
                errors.Add("Usuário ou senha inválidos");

            return new ApplicationResponse(user, errors);
        }

        public List<string> ValidaLogin(LoginCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command.Email))
                errors.Add("Login vazio");

            if (string.IsNullOrEmpty(command.Password))
                errors.Add("Senha vazia");

            return errors;
        }

        public ApplicationResponse Delete(DeleteUserCommand command)
        {
            var errors = ValidaDelete(command);

            if (_userRepository.SelectByUserId(command.UserId) == null)
                errors.Add("Este usuário não existe");

            if (errors.Count == 0)
                _userRepository.Delete(command.UserId);

            return new ApplicationResponse(null, errors);
        }

        public List<string> ValidaDelete(DeleteUserCommand command)
        {
            var errors = new List<string>();

            if (command.UserId == 0)
                errors.Add("UserId inválido");

            return errors;
        }

        public ApplicationResponse Update(UpdateUserCommand command)
        {
            var errors = ValidaUpdate(command);

            if (_userRepository.SelectByEmail(command.Email) != null)
                errors.Add("Este login já foi cadastrado no sistema");

            var usuario = new Usuario()
            {
                UsuarioId = command.UserId,
                Email = command.Email,
                Nome = command.Email,
                Senha = command.Password
            };

            if (errors.Count == 0)
                _userRepository.Update(usuario);

            return new ApplicationResponse(usuario, errors);
        }

        public List<string> ValidaUpdate(UpdateUserCommand command)
        {
            var errors = new List<string>();

            if (command.UserId == 0)
                errors.Add("UserId inválido");

            if (string.IsNullOrEmpty(command.Name) || command.Name.Length > 100)
                errors.Add("Nome inválido");

            if (string.IsNullOrEmpty(command.Email) || command.Email.Length > 100)
                errors.Add("Email inválido");

            if (string.IsNullOrEmpty(command.Password) || command.Password.Length > 100)
                errors.Add("Senha inválida");

            return errors;
        }
    }
}
