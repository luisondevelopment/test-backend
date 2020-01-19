Olá!

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
)
