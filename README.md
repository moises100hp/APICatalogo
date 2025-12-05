# 🛒 APICatalogo

![Net Version](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/License-MIT-green)
![Status](https://img.shields.io/badge/Status-Completed-success)

## 📋 Sobre o Projeto

A **APICatalogo** é uma API RESTful robusta desenvolvida com **.NET 8**, focada no gerenciamento de produtos e categorias.

Este projeto foi construído com o objetivo de implementar as melhores práticas de desenvolvimento de software, padrões de arquitetura e recursos avançados de segurança e performance. Não é apenas um CRUD, mas uma aplicação estruturada para cenários reais de produção.

## 🚀 Tecnologias Utilizadas

* **[.NET 8](https://dotnet.microsoft.com/en-us/)** - Framework principal.
* **[Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)** - ORM para acesso a dados.
* **[AutoMapper](https://automapper.org/)** - Mapeamento entre objetos (Entidades e DTOs).
* **[Identity & JWT](https://jwt.io/)** - Autenticação e Autorização segura.
* **[Serilog](https://serilog.net/)** - Logs estruturados.
* **[Swagger (OpenAPI)](https://swagger.io/)** - Documentação interativa.
* **[X.PagedList](https://github.com/dncuug/X.PagedList)** - Otimização de paginação.
* **xUnit** - Testes de Unidade.

## ✨ Funcionalidades e Arquitetura

O projeto aplica diversos conceitos avançados:

* **Padrão Repository e Unit of Work:** Desacoplamento da lógica de negócios e persistência de dados.
* **DTOs (Data Transfer Objects):** Prevenção de *overposting* e exposição controlada de dados.
* **Paginação, Filtro e Ordenação:** Consultas otimizadas para grandes volumes de dados.
* **Autenticação JWT:** Geração e validação de Tokens (Access Token e Refresh Token).
* **Autorização:** Controle de acesso baseado em *Claims*.
* **Rate Limiting:** Proteção contra excesso de requisições.
* **CORS:** Configuração de políticas de acesso cruzado.
* **Versionamento de API:** Suporte a múltiplas versões de endpoints.
* **HttpPatch:** Implementação para atualizações parciais de recursos.

## 🔧 Como Executar o Projeto

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
* SQL Server (ou outro banco compatível configurado no `appsettings.json`).
* Visual Studio 2022 ou VS Code.

### Passo a passo

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/moises100hp/APICatalogo.git](https://github.com/moises100hp/APICatalogo.git)
    ```

2.  **Navegue até a pasta do projeto:**
    ```bash
    cd APICatalogo
    ```

3.  **Configure a String de Conexão:**
    Edite o arquivo `appsettings.json` e ajuste a `DefaultConnection` para o seu banco de dados local.

4.  **Aplique as Migrations (Criar Banco de Dados):**
    ```bash
    dotnet ef database update
    ```

5.  **Execute a aplicação:**
    ```bash
    dotnet run
    ```

6.  **Acesse a documentação:**
    Abra o navegador em `https://localhost:7066/swagger/index.html` (ou a porta configurada no seu ambiente) para testar os endpoints.

## 🧪 Testes

O projeto inclui testes de unidade para garantir a confiabilidade dos serviços. Para rodá-los:

```bash
dotnet test