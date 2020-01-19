using Dapper;
using TestBackend.Domain.Models;

namespace TestBackend.Infrastructure
{
    public class UserRepository
    {
        public int Insert(Usuario usuario)
        {
            const string sql = @"
                INSERT Usuario (Nome, Email, Senha)
                OUTPUT INSERTED.UsuarioId
                VALUES (@Nome, @Email, @Senha)";

            using (var _conn = ConnectionFactory.GetTestBackendOpenConnection())
            {
                return _conn.QueryFirstOrDefault<int>(sql, usuario);
            }
        }

        public Usuario SelectByLoginAndPassword(string email, string password)
        {
            const string query = @"
                SELECT * FROM Usuario
                WHERE email = @email AND senha = @password";

            using (var _conn = ConnectionFactory.GetTestBackendOpenConnection())
            {
                return _conn.QueryFirstOrDefault<Usuario>(query, new { email, password });
            }
        }

        public Usuario SelectByEmail(string email)
        {
            const string query = @"
                SELECT * FROM Usuario
                WHERE email = @email";

            using (var _conn = ConnectionFactory.GetTestBackendOpenConnection())
            {
                return _conn.QueryFirstOrDefault<Usuario>(query, new { email });
            }
        }

        public Usuario SelectByUserId(int usuarioId)
        {
            const string query = @"
                SELECT * FROM Usuario
                WHERE UsuarioId = @usuarioId";

            using (var _conn = ConnectionFactory.GetTestBackendOpenConnection())
            {
                return _conn.QueryFirstOrDefault<Usuario>(query, new { usuarioId });
            }
        }

        public void Update(Usuario usuario)
        {
            const string sql = @"
                UPDATE Usuario
                SET Nome = @nome, Email = @email, Senha = @senha
                WHERE UsuarioId = @usuarioId";

            using (var _conn = ConnectionFactory.GetTestBackendOpenConnection())
            {
                _conn.Execute(sql, usuario);
            }
        }

        public void Delete(int usuarioId)
        {
            const string sql = @"
                DELETE FROM Usuario WHERE UsuarioId = @usuarioId";

            using (var _conn = ConnectionFactory.GetTestBackendOpenConnection())
            {
                _conn.Execute(sql, new { usuarioId });
            }
        }
    }
}
