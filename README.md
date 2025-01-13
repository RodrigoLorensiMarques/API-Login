# API-Login
 
API que realiza cadastro e gerenciamento de acessos de usuários. \
Realiza armazenamento das senhas em hash e verifica a senha recebida no login com a hash armazenada, alem de retornar token JWT após verificação de credenciais para uso em endpoints protegidos e com roles de autorização.


## Tecnologias
- C# .NET
- Entity Framework
- SQL Server
- BCrypt
- JwtBearer
- Docker


## API Endpoints
### User
- __GET /User:__ Acessa um usuário com nome e senha.
- __POST /User/Custumer:__ Cria um novo usuário com role cliente.
- __POST /User/administrator:__ Cria um novo usuário com role de admin.
- __GET /User/administrator:__ Autoriza via Token apenas acesso de usuários admin.
- __GET /User/protected:__ Autoriza qualquer usuário via Token.

### Recovery
- __PUT /Recovery/ChangePassword:__ Permite usuários autorizados alterar senha.

## Tabelas
![DbDiagram](https://github.com/RodrigoLorensiMarques/API-Login/blob/main/DbDiagrama.png)

## Como Rodar
### Requisitos:
1. [.NET 9](https://dotnet.microsoft.com/pt-br/download)
2. [Docker](https://docs.docker.com/engine/install/) 

### Passo a Passo:

1. Em seu terminal, acesse a pasta raiz do repositório

2. Para subir os serviços use `docker-compose up` 

3. Dentro da pasta WebApi, restaure os pacotes usando `dotnet restore`

4. Aplique as migrations usando `dotnet-ef database update`
   
__OBS.:__ O projeto esta usando Swagger para testes. Então você pode acessar o http://localhost:5200/swagger para ter acesso a interface do Swagger.
