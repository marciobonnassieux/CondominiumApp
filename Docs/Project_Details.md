# Estrutura do Projeto e Regras de NegÃ³cio

## Pilares de Backend
1. **Core:** Mapeamento agnÃ³stico restrito aos Modelos centrais: `User`, `Profile`, `Unit`, e o motor logÃ­stico de `Delivery` e `Courier_Log`.
2. **Infrastructure:** ContÃ©m as lÃ³gicas duras do `EdifacilDbContext` efetuando rastreio no EF Core para SQL Server. ProvÃª a motorizaÃ§Ã£o de criptografia no `TokenService` gerando *HMAC-256 JWT*.
3. **API Controllers:** Isolamento explÃ­cito de rotas baseadas na validaÃ§Ã£o do Claim de Token via PolÃ­tica de Perfis (RBAC).

---

## SoluÃ§Ã£o Visual C# (Mobile App .NET MAUI)
A arquitetura de visÃ£o foi projetada para focar em uma experiÃªncia **Cinza Chumbo** com detalhes chamativos em **Amarelo (`#F1C40F`)**, abdicando do AppShell nativo para uma linearidade via `NavigationPage` pura que limpa ruÃ­dos para lÃ³gicas empresariais:

1. **Camada de AutenticaÃ§Ã£o (`LoginPage`)**: Bloqueia fisicamente a tentativa com mÃ¡scara pura numÃ©rica no CPF. Ao chancelar sucesso via API, a variÃ¡vel persistente `Preferences.Set("AuthToken", ...)` salva e envia a navegaÃ§Ã£o logada para a Ã¡rea de Dashboard.
2. **Painel de Controle (`DashboardPage`)**: Central grÃ¡fica conectando lÃ³gicas independentes (Ver aptos ou Escanear).

### GestÃ£o do Fluxo de Encomendas
A mecÃ¢nica mestra do porteiro ocorre na `ReceiveDeliveryPage`, blindada com dois mÃ³dulos centrais customizados do zero:
- **Pop-up ComboBox (SobreposiÃ§Ã£o):** Ignorando menus estÃ¡ticos nativos do sistema operacional, uma Grade dinÃ¢mica semitransparente em tela-cheia flutua ao ser invocada abarcando uma `SearchBar`. Ela filtra ativamente as unidades mapeadas em memÃ³ria permitindo o preenchimento sem perder contexto visual.
- **IntegraÃ§Ã£o de Scanner CÃ¢mera (ZXing):** A pÃ¡gina consome permissÃµes exclusivas do manifesto nativo Android e dispara a intenÃ§Ã£o visual de lente. Ao absorver barras EAN ou Matrix QR, a tela fecha e despeja instantaneamente a Label decodificada para salvamento.

---

## Dados Fakes Automatizados no Root

A API injeta dados sementeiros via `DbInitializer` automaticamente sempre que constata um banco virgem:
*Para cruzar a barreira do Login TestÃ¡vel, utilize a credencial nativa da Role:*

- **Identidade do Worker (Porteiro):** CPF: `12345678900` | Senha: `123456`
- **Residentes passivos de teste:** CPF `98765432100` e `11122233344` (A senha global base Ã© sempre `123456`).

