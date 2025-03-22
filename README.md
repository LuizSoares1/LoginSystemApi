<h1>LoginSystemApi - Backend de Autenticação e Gerenciamento de Usuários</h1>

Este repositório contém o backend de uma API RESTful desenvolvida em .NET 8 para autenticação e gerenciamento de usuários. A API oferece funcionalidades como registro, login, listagem de usuários (restrita a administradores), exclusão de usuários e alteração de senha, utilizando autenticação baseada em JWT (JSON Web Tokens). O banco de dados é implementado em memória com Entity Framework Core para fins de demonstração.

<h2>Funcionalidades:</h2>

<ul>
<li>Registro de novos usuários (POST /api/Auth/register).</li>
<li>Autenticação com geração de token JWT (POST /api/Auth/login).</li>
<li>Listagem de usuários cadastrados (GET /api/Auth/users, apenas para Admins).</li>
<li>Exclusão de usuários por ID (DELETE /api/Auth/users/{id}, apenas para Admins).</li>
<li>Alteração de senha do usuário autenticado (PUT /api/Auth/change-password).</li>
</ul>

<h2>Tecnologias:</h2>
<ul>
<li>.NET 8</li>
<li>Entity Framework Core (In-Memory Database)</li>
<li>JWT para autenticação</li>
<li>Swagger/OpenAPI para documentação (disponível em swagger.json)</li>
</ul>
<h2>Configuração:</h2>
<ul>
<li>CORS habilitado para integração com frontend em http://localhost:5173.</li>
<li>Documentação da API gerada automaticamente via Swagger, acessível em http://localhost:5246/.</li>
</ul>
<h2>Como Executar:</h2>
<ul>
<li>Clone o repositório: git clone https://github.com/seu-usuario/LoginSystemApi.git</li>
<li>Navegue até o diretório: cd LoginSystemApi</li>
<li>Execute: dotnet run --environment Development</li>
<li>Acesse a API em http://localhost:5246/ para explorar os endpoints no Swagger UI.</li>
</ul>
O arquivo swagger.json está incluído na raiz do repositório para documentação detalhada da API.
