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

## Requisitos:
1. [Docker](https://docs.docker.com/engine/install/)
2. [.NET 9](https://dotnet.microsoft.com/pt-br/download)

## Como Rodar:

1. Em seu terminal, acesse a pasta raiz __API-Login__

2. Para subir os serviços `docker-compose up` 

3. Aplique as migrations `dotnet-ef database update`
   
__OBS.:__ O projeto esta usando Swagger para testes. Então você pode acessar o seu http://localhost:5200/swagger para ter acesso a interface do Swagger.
