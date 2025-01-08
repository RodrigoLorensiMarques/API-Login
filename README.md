# API-Login
 
API que realiza cadastro e login de usuários em uma database. \
Realiza armazenamento das senhas em hash e verifica a senha recebida no login com a hash armazenada, alem de retornar token JWT após verificação de credenciais para uso em endpoints protegidos e com roles de autorização.


## Tecnologias
- C# .NET
- Entity Framework
- SQL Server
- BCrypt
- JwtBearer
- Docker

## Como Rodar
### Requisitos:
1. [.NET 9](https://dotnet.microsoft.com/pt-br/download)
2. [Docker](https://docs.docker.com/engine/install/) 

### Passo a Passo:

1. Em seu terminal, acesse a pasta raiz do repositório

2. Para subir os serviços use `docker-compose up` 

3. Dentro do diretório do projeto, restaure os pacotes usando `dotnet restore`

4. Aplique as migrations usando `dotnet-ef database update`
   
__OBS.:__ O projeto esta usando Swagger para testes. Então você pode acessar o http://localhost:5200/swagger para ter acesso a interface do Swagger.
