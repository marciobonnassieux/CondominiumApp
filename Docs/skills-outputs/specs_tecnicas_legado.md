# ⚙️ Especificações Técnicas (Legado e Fundamentação)

# Resumo Arquitetural
Projeto baseado em **.NET 10** seguindo princípios de arquitetura modular (possível transição para Clean Architecture/Hexagonal).

# Stack Tecnológica
*   **Mobile:** .NET MAUI 10.0 (Windows, Android, iOS, macOS).
*   **Backend:** ASP.NET Core API 10.0.
*   **Persistência:** EF Core 10.0 com SQL Server (via DbContext).
*   **Segurança:** Implementação de JWT (JSON Web Tokens) Bearer Authentication.

# Interfaces e Serviços Críticos
*   **ITokenService:** (Injected via `TokenService`). Gerencia a geração e decodificação de Bearer Tokens para autorização entre Mobile e API.
*   **ApiService (Mobile):** Centraliza as chamadas `HttpClient` com configuração estática de `BaseAddress` e injeção do token JWT salvo em `Preferences`.

# Entidades Principais (Domínio)
*   **Unit:** Representa uma unidade habitacional (Bloco/Apartamento).
*   **User:** Cadastro de usuários e funcionários com diferenciação por `Role`.
*   **Delivery:** Estrutura de registro de encomendas com vínculo por `UnitId`.
*   **CourierLog:** Histórico de entregadores que frequentam o condomínio.

# Padrões de Injeção de Dependência
*   Uso de `builder.Services.AddScoped<I... , ...>` na API.
*   MAUI utiliza `builder.Services` no `MauiProgram.cs` para registro de Views e ViewModels (MVVM).

# Endpoints e Contratos (Legado)
*   `POST /api/auth/login`: Recebe `Identifier` (Email/CPF) e `Password`. Retorna `Token`.
*   `GET /api/units`: Retorna lista de unidades habitacionais. Requer autorização Bearer.
*   `GET /api/units/{id}/deliveries`: Histórico de entregas de uma unidade específica.
