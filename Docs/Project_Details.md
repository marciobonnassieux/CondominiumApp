# Estrutura do Projeto e Regras de Negócio

Este consolidado documenta as implementações arquitetônicas e de integração produzidas do ecossistema principal.

## Padrões Arquitetônicos de Backend
A camada robusta orientada à responsabilidade está estruturada da seguinte forma:
1. **Core:** Mapeamento Orientado à Objeto sem dependência técnica limitante. Responsável pelos Modelos centrais: `User`, `Profile`, `Unit`, `Delivery`.
2. **Infrastructure:** Acopla interfaces vitais utilizando injeção dependente. Cria o banco `(localdb)` via EF Core através do `CondominiumDbContext` lidando em formato transacional com herança. Cria ativamente geração JWT em HMAC-256 no `TokenService`.
3. **API (Controllers):** Consumo restritivo por Rotas REST baseado no Claim do token via **RBAC (Role Based Access Control)**.

## Solução Visual e Componentes (Mobile App - MAUI)
A arquitetura se comunica de forma não obstrutiva baseando os roteamentos no Token injetado:
1. **Página de Autenticação (`LoginPage`):** Força o usuário a interagir via CPF/Senha, injetando dados encriptados `POST` na API e convertendo acesso verídico sob Preferências Locais (`Preferences.Set()`).
2. **Geração de Login e Fluxo (`LoginPage` => `Dashboard`)**: Interface blindada de CPF (com máscara automática). Após acesso validado, o porteiro aterrissa na `DashboardPage`.
3. **Lista de Unidades (`UnitsPage`)**: Injeta automaticamente o token para listar via API.
4. **Recepção e Câmera (`ReceiveDeliveryPage`)**: Permite que o porteiro digitalize (*Scan*) com a câmera nativa Códigos de Barra ou QR Codes do pacote externo vinculando aos dados gerados utilizando `ZXing`. Pede autenticação JWT ativa no `HttpClient` por trás.

---

## Estrutura Oculta: Seed de População do LocalDB
O sistema foi projetado para fluir na velocidade máxima desde a primeira compilação. Um robô interno (classe `DbInitializer`) examina se o Banco SQL está vazio e grava usuários base do Condomínio sob os diferentes papéis (*Roles*).

### Dados Registrados de Teste Interno
Para acessar o APP ou testar os Endpoints Fechados no Swagger Interativo (Scalar), você precisará do **CPF e da Senha padrão: `123456`**.

#### 1. Credencial Realista de Check-in (ROLE: Janitor)
Acesse como Funcionário Padrão para habilitar a capacidade de gerar encomendas de teste para qualquer residente que buscar.
- **CPF:** `12345678900`
- **Senha:** `123456`
- **Nome Fantasia:** João Porteiro

#### 2. Residentes Fakes
Consomem exclusivamente as rotas informacionais de sua listagem com base em sua Unidade (ROLE: Resident).
- **CPF:** `98765432100` *(Maria - Bloco A/201 e 101)*
- **CPF:** `11122233344` *(Carlos - Bloco A/102)*
