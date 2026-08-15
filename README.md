# Products.WebAPI

API para gerenciamento de estoque e vendas — cadastro de produtos, controle de entrada/saída de estoque, vendas com checkout e leitura de código de barras, além de dashboard de resumo.

Projeto desenvolvido para atender a necessidade real de um pequeno negócio: sistema completo de gestão de produtos, estoque e vendas. O código aqui disponibilizado é uma versão genérica, adaptada para não expor dados do cliente original.

## Stack

- **.NET 8** / ASP.NET Core Web API
- **Entity Framework Core** + **Pomelo.EntityFrameworkCore.MySql** (MySQL)
- **MediatR** — implementação de CQRS
- **FluentValidation** — validação de comandos/queries
- **JWT Bearer Authentication** — autenticação e autorização baseada em roles
- **MinIO** — storage de imagens de produto
- **BCrypt.Net** — hash de senhas
- **Swashbuckle (Swagger)** — documentação da API
- **Docker** — containerização e deploy

## Arquitetura

O projeto segue **Vertical Slice Architecture**: em vez de organizar o código por camada técnica (Controllers, Services, Repositories em pastas separadas), cada funcionalidade é organizada por *feature*, em `Features/`, contendo tudo que aquela operação específica precisa (Command/Query, Handler, Validator).

```
Features/
  Auth/
  Brand/
    CreateBrand/
    UpdateBrand/
    GetBrand/
    DeleteBrand/
  Category/
  Products/
  Sales/
  StockMovement/
  User/
  Dashboard/
```

Combinado com **CQRS** (separação entre Commands e Queries) via **MediatR**, os Controllers ficam bem enxutos — apenas recebem a requisição HTTP e despacham para o handler correspondente através do `ISender`.

Validação de entrada é feita com **FluentValidation**, plugada automaticamente no pipeline do MediatR através de um `ValidationBehavior` (`Common/Behaviors/`).

### Estrutura de pastas
- `Controllers/` — endpoints HTTP, um controller por recurso
- `Features/` — lógica de negócio, organizada por vertical slice
- `Common/` — entidades, enums, interfaces e utilitários compartilhados
- `Infrastructure/` — implementações concretas (persistência, storage, etc.)
- `IoC/` — configuração de injeção de dependência, separada por responsabilidade (Auth, Persistence, Storage, MediatR)
- `Migrations/` — migrations do Entity Framework Core

## Funcionalidades

- **Autenticação**: login com JWT, roles (`Admin`, `Common`) controlando acesso a endpoints via policies de autorização
- **Produtos**: CRUD completo, upload de imagem (armazenada no MinIO), consulta por código de barras
- **Categorias e Marcas**: CRUD de apoio para organização de produtos
- **Estoque**: registro de entrada de estoque e ajustes, histórico de movimentações
- **Vendas**: checkout de venda, consulta de vendas realizadas
- **Dashboard**: endpoint de resumo com indicadores gerais
- **Usuários**: gerenciamento de usuários do sistema

## Como rodar localmente

### Pré-requisitos
- .NET 8 SDK
- Docker e Docker Compose

### 1. Suba as dependências (MySQL + MinIO)
```bash
docker compose up -d
```
Isso sobe um MySQL local (porta 3306) e um MinIO local (portas 9000/9001), conforme definido em `docker-compose.yml`.

### 2. Configure a connection string
As configurações padrão de desenvolvimento já apontam para o MySQL local do `docker-compose.yml` (`appsettings.json`). Ajuste conforme necessário.

### 3. Aplique as migrations
```bash
dotnet ef database update
```

### 4. Rode a API
```bash
dotnet run
```
Por padrão, em ambiente de desenvolvimento, a documentação Swagger fica disponível em `/swagger`.

## Deploy

O projeto inclui um `Dockerfile` multi-stage (build com SDK, execução com runtime ASP.NET), pronto para deploy em qualquer plataforma baseada em containers.

Variáveis de ambiente esperadas em produção (sobrescrevem `appsettings.json` seguindo a convenção do ASP.NET Core, com `__` representando aninhamento):
- `ConnectionStrings__Default` — connection string do banco MySQL
- `Jwt__Key` — chave de assinatura dos tokens JWT (gerar uma nova para produção, não reaproveitar a de desenvolvimento)
- `AllowedOrigins` — origens liberadas para CORS, separadas por vírgula (ex: URL do frontend em produção)

## Frontend

Este backend foi desenvolvido em conjunto com um frontend dedicado. O frontend possui um modo para funcionar de forma independente (sem a API), e também pode ser conectado a esta API configurando a URL dela no `.env` do frontend.

