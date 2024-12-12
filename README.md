# API-Login
 
API que realiza cadastro e login de usuários em uma database. \
Realiza armazenamento das senhas em hash usando a bibliteca BCrypt.


## Tecnologias
- C# .NET
- Entity Framework
- SQL Server
- BCrypt


## Como Rodar:
1. Necessário ter instalado o __.Net 9.0__

2. Em seu terminal, acesse a pasta raiz __API-Login__

3. Use o comando `dotnet restore` para instalar todas as dependências que estão no __API-Login.csproj__

4. Na raiz do projeto, edite o __"ConnectionStrings"__ do arquivo __appsettings.Development.json__ para o seu banco SQL Server ou configure o banco de sua preferência.

5. Após configuração do banco, aplique as migrations usando o comando `dotnet-ef database update`

6. Execute o comando `dotnet run`
   
OBS.: O projeto esta usando Swagger para testes. Então você pode acessar o seu http://localhost:{porta}/swagger para ter acesso a interface do Swagger.
