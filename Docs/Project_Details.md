# Estrutura do Projeto e Regras de Negócio

## Pilares de Backend
1. **Core:** Mapeamento agnóstico restrito aos Modelos centrais: `User`, `Profile`, `Unit`, e o motor logístico de `Delivery` e `Courier_Log`.
2. **Infrastructure:** Contém as lógicas duras do `CondominiumDbContext` efetuando rastreio no EF Core para SQL Server. Provê a motorização de criptografia no `TokenService` gerando *HMAC-256 JWT*.
3. **API Controllers:** Isolamento explícito de rotas baseadas na validação do Claim de Token via Política de Perfis (RBAC).

---

## Solução Visual C# (Mobile App .NET MAUI)
A arquitetura de visão foi projetada para focar em uma experiência **Cinza Chumbo** com detalhes chamativos em **Amarelo (`#F1C40F`)**, abdicando do AppShell nativo para uma linearidade via `NavigationPage` pura que limpa ruídos para lógicas empresariais:

1. **Camada de Autenticação (`LoginPage`)**: Bloqueia fisicamente a tentativa com máscara pura numérica no CPF. Ao chancelar sucesso via API, a variável persistente `Preferences.Set("AuthToken", ...)` salva e envia a navegação logada para a área de Dashboard.
2. **Painel de Controle (`DashboardPage`)**: Central gráfica conectando lógicas independentes (Ver aptos ou Escanear).

### Gestão do Fluxo de Encomendas
A mecânica mestra do porteiro ocorre na `ReceiveDeliveryPage`, blindada com dois módulos centrais customizados do zero:
- **Pop-up ComboBox (Sobreposição):** Ignorando menus estáticos nativos do sistema operacional, uma Grade dinâmica semitransparente em tela-cheia flutua ao ser invocada abarcando uma `SearchBar`. Ela filtra ativamente as unidades mapeadas em memória permitindo o preenchimento sem perder contexto visual.
- **Integração de Scanner Câmera (ZXing):** A página consome permissões exclusivas do manifesto nativo Android e dispara a intenção visual de lente. Ao absorver barras EAN ou Matrix QR, a tela fecha e despeja instantaneamente a Label decodificada para salvamento.

---

## Dados Fakes Automatizados no Root

A API injeta dados sementeiros via `DbInitializer` automaticamente sempre que constata um banco virgem:
*Para cruzar a barreira do Login Testável, utilize a credencial nativa da Role:*

- **Identidade do Worker (Porteiro):** CPF: `12345678900` | Senha: `123456`
- **Residentes passivos de teste:** CPF `98765432100` e `11122233344` (A senha global base é sempre `123456`).
