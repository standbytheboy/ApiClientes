# API de Cadastro de Clientes

Esta é uma API RESTful desenvolvida em ASP.NET Core 8 para gerenciar o cadastro de clientes e seus respectivos endereços. O projeto utiliza Entity Framework Core para a persistência de dados em um banco SQL Server e inclui validação de CPF em tempo real através de um serviço externo oferecido pelo Governo.

## ✨ Funcionalidades

  - **CRUD de Clientes**: Operações completas para criar, ler, atualizar e deletar clientes.
  - **CRUD de Endereços**: Operações completas para gerenciar múltiplos endereços associados a um cliente.
  - **Validação de CPF**: Validação de CPF em tempo real no momento do cadastro, utilizando o serviço público do `scpa-backend.saude.gov.br`.
  - **Relacionamento de Dados**: Um cliente pode ter múltiplos endereços, e a exclusão de um cliente remove seus endereços em cascata (`OnDelete(DeleteBehavior.Cascade)`).
  - **Documentação de API**: Geração automática de documentação interativa com Swagger (OpenAPI).

## 🚀 Tecnologias Utilizadas

  - **Backend**: C\#, ASP.NET Core 8
  - **ORM**: Entity Framework Core 8
  - **Banco de Dados**: Microsoft SQL Server
  - **Documentação**: Swashbuckle (Swagger)
  - **Injeção de Dependência**: Padrão nativo do ASP.NET Core para `DbContext`, `HttpClient` e serviços.

## 📋 Pré-requisitos

  - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  - [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (versão Express, Developer ou superior)
  - Um editor de código de sua preferência (Visual Studio 2022, VS Code, Rider).

## ⚙️ Como Executar o Projeto

1.  **Clone o repositório:**

    ```bash
    git clone <URL_DO_SEU_REPOSITORIO>
    cd ApiClientes
    ```

2.  **Configure a Conexão com o Banco de Dados:**

      - Abra o arquivo `appsettings.json`.
      - Altere a string de conexão `DefaultConnection` para apontar para a sua instância do SQL Server. O banco de dados `DbClientes` será criado automaticamente.

    <!-- end list -->

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=SEU_SERVIDOR;Database=DbClientes;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

3.  **Instale as Ferramentas do EF Core (se não tiver):**

    ```bash
    dotnet tool install --global dotnet-ef
    ```

4.  **Aplique as Migrations:**
    Este comando irá criar o banco de dados e as tabelas `Clientes` e `Enderecos` com base na configuração do projeto.

    ```bash
    dotnet ef database update
    ```

5.  **Execute a Aplicação:**

    ```bash
    dotnet run
    ```

6.  **Acesse a Documentação:**

      - Após a execução, a API estará disponível.
      - Abra seu navegador e acesse a URL da documentação do Swagger para testar os endpoints: **`http://localhost:5222/swagger`**.

## Endpoints da API

A API expõe os seguintes endpoints:

### Cliente (`/api/Cliente`)

  - `GET /`: Retorna uma lista simplificada de todos os clientes.
  - `GET /{id}`: Retorna os detalhes completos de um cliente específico, incluindo seus endereços.
  - `POST /`: Cria um novo cliente. O CPF é validado antes da criação.
  - `PUT /{id}`: Atualiza os dados de um cliente existente.
  - `DELETE /{id}`: Exclui um cliente e seus endereços associados.

### Endereço (`/api/Endereco`)

  - `GET /`: Retorna todos os endereços cadastrados.
  - `GET /{id}`: Retorna um endereço específico.
  - `POST /`: Adiciona um novo endereço a um cliente (o `ClienteId` deve ser informado no corpo da requisição).
  - `PUT /{id}`: Atualiza um endereço existente.
  - `DELETE /{id}`: Exclui um endereço.

## 🏗️ Estrutura do Projeto

  - `/Controllers`: Contém os controladores da API (`ClienteController`, `EnderecoController`) que gerenciam as rotas e requisições HTTP.
  - `/Models`: Contém as entidades de domínio (`Cliente`, `Endereco`) que representam as tabelas do banco de dados.
  - `/Data`: Contém a classe `AppDbContext`, responsável pela comunicação com o banco de dados via Entity Framework Core.
  - `/Services`: Contém a lógica de negócio desacoplada, como o `ClienteService` para validação de CPF.
  - `/Migrations`: Contém os arquivos de migração gerados pelo EF Core para controle de versão do schema do banco de dados.
