# EdifÃ¡cil V 0.5 (Edifacil Management System) - RelatÃ³rio AnalÃ­tico de Contexto Geral

Este documento apresenta um mapeamento estruturado, exaustivo e analÃ­tico do ecossistema **EdifÃ¡cil**. A consolidaÃ§Ã£o a seguir cruza especificaÃ§Ãµes de negÃ³cio, arquitetura sistÃªmica nativa, fluxos de engenharia C#/.NET MAUI, topologia de rede local e a diretriz matriz de Design System ("The Digital Sentinel/Estate"). Este relatÃ³rio atua como a fonte central de verdade (Single Source of Truth) para engenharia e design.

---

## 1. FundamentaÃ§Ã£o, VisÃ£o de Produto e Valor
Concebido como um SaaS B2B2C nacional, o **EdifÃ¡cil** (*"EdifÃ­cio Ã© difÃ­cil? EdifÃ¡cil resolve."*) ambiciona orquestrar a complexidade da gestÃ£o condominial com precisÃ£o cirÃºrgica. 
O MVP (VersÃ£o 0.5) endereÃ§a o principal ponto de atrito em condomÃ­nios de mÃ©dio/grande porte: **A logÃ­stica de recepÃ§Ã£o de encomendas em portarias.** Ao focar neste escopo, a plataforma valida sua resiliÃªncia transacional e a aderÃªncia visual antes de escalar para os mÃ³dulos financeiro e administrativo completos.

---

## 2. Arquitetura de Software e Stack TecnolÃ³gica (Environment)
A plataforma Ã© forjada utilizando os *toolings* de ponta da Microsoft (VersÃµes Preview/Current), garantindo longevidade do cÃ³digo e mÃ¡xima performance.

### 2.1. Back-End, API e Core
*   **Framework Base:** `.NET 10 SDK` rodando `Kestrel` na porta universal `5114`.
*   **Arquitetura:** Isolamento claro entre `Core` (Modelos agnÃ³sticos: *User, Profile, Unit, Delivery, Courier_Log*), `Infrastructure` (PersistÃªncia e ServiÃ§os de SeguranÃ§a), e `API Controllers` pautados em *Role-Based Access Control* (RBAC).
*   **Banco de Dados:** `SQL Server LocalDB` roteado via `Entity Framework Core (EF Core)`. Permite desenvolvimento autÃ´nomo sem necessidade complexa de contÃªineres Docker no host do desenvolvedor.
*   **SeguranÃ§a e AutenticaÃ§Ã£o:** `TokenService` gerando tokens *HMAC-256 JWT* injetados via cabeÃ§alho `Bearer` nas requisiÃ§Ãµes subsequentes.
*   **PolinizaÃ§Ã£o de Dados (Seed):** O `DbInitializer` injeta uma *Fake Data Structure* em bancos virgens, provendo rapidamente credenciais de teste para porteiros (Ex: CPF `12345678900` / Senha `123456`) e moradores residentes.

### 2.2. Front-End Mobile (C# .NET MAUI)
*   **Alvo de CompilaÃ§Ã£o:** `Android SDK (API 36)` (net10.0-android).
*   **EstruturaÃ§Ã£o de NavegaÃ§Ã£o:** A arquitetura ignora o tradicional `AppShell` em prol de uma `NavigationPage` pura. Essa decisÃ£o tÃ©cnica limpa ruÃ­dos estruturais do sistema operacional, permitindo um controle total da *stack* temporal de push/pop das visÃµes focadas estritamente nas regras de negÃ³cio empresariais.
*   **Testabilidade Mista:** CompilaÃ§Ã£o ao vivo (`dotnet watch`) em Emuladores Locais, suportando *Bypass* nativo via Wi-Fi para testes tangÃ­veis via ADB no dispositivo fÃ­sico do desenvolvedor, alterando a Ã¢ncora `10.0.2.2` para o `IPv4` local (`192.168.x.x`).

---

## 3. Topologia de Identidade e Multi-Contextualidade
A Ã¡rvore de domÃ­nio reflete um modelo hierÃ¡rquico estrito (Master â†’ Administradora â†’ CondomÃ­nio â†’ Bloco â†’ Unidade). No entanto, a modelagem de identidade (`User`) Ã© flexÃ­vel para suportar o mundo real logÃ­stico:
*   **MÃºltiplos VÃ­nculos Operacionais:** Um Ãºnico indivÃ­duo (atrelado ao CPF e protegido por senha mista e OTP via e-mail de 300s) pode possuir mÃºltiplos Cargos espalhados em diferentes CondomÃ­nios.
*   **Seletor de Contexto (Context Switcher):** ApÃ³s a camada de autenticaÃ§Ã£o primÃ¡ria, se o token desvendar mÃºltiplos papÃ©is vÃ¡lidos, o usuÃ¡rio nÃ£o loga diretamente no Dashboard. Ele intercepta o `Seletor de Contexto`, que moldarÃ¡ os *Claims* do seu acesso momentÃ¢neo, definindo ativamente se ele verÃ¡ o app operando como *SÃ­ndico do Bloco B* ou *Morador do Bloco C*.

### 3.1. Atores da AÃ§Ã£o SistÃªmica (Mapeamento UML Consolidado)
1. **Master/Dev:** SuperusuÃ¡rio para setup SaaS das Administradoras.
2. **Gestor Adm:** Cadastra CondomÃ­nios e traÃ§a a arquitetura dos blocos/unidades.
3. **SÃ­ndico:** Parametriza e atribui cargos a porteiros e funcionÃ¡rios.
4. **Porteiro:** Operador do front-line (v0.5). Utiliza mÃ³dulos pesados de hardware (CÃ¢mera) e exige fluidez zero-latency.
5. **Morador:** O consumidor final, recebendo Push/WhatsApp via rotinas de consultas e disparos assÃ­ncronos de notificaÃ§Ã£o.
6. **Entregador (Courier):** O ator analÃ³gico-digital de encerramento de lote (Assinatura do Recibo do Lote Integral).

---

## 4. Engenharia do Fluxo Operacional: LogÃ­stica Unit-First
O coraÃ§Ã£o do V 0.5 bate na `ReceiveDeliveryPage`, projetada exaustivamente para reduzir fricÃ§Ã£o no balcÃ£o da portaria atravÃ©s do axioma **Unit-First (Unidade Primeira)**. Em vez de registrar um objeto aleatÃ³rio, o porteiro engloba pacotes sequencialmente para a mesma moradia.

### 4.1. InovaÃ§Ãµes TÃ¡ticas da Interface do Porteiro
*   **Componente Pop-up ComboBox (SobreposiÃ§Ã£o Visual):** Menus Dropdown sistÃªmicos nativos (como `<Picker>`) foram abandonados. Utiliza-se uma Grade semitransparente em tela-cheia com integraÃ§Ã£o de `SearchBar`. A Collection View acoplada Ã© filtrada ativamente na memÃ³ria (In-Memory Filter), mantendo o contexto grÃ¡fico da tela imaculado em segundo plano.
*   **Telemetria Visual de CÃ¢mera (IntegraÃ§Ã£o ZXing):** A intenÃ§Ã£o nativa captura controle total de bloqueios no manifesto Android (`Camera Permission Checks`). O scanner a lazer decodifica Matrix/EAN injetando a string no formulÃ¡rio sem intervenÃ§Ã£o de digitaÃ§Ã£o de usuÃ¡rio. Em caso de correspondÃªncias simples (sem etiqueta), um fluxo *Fallback* de registro de fotografia manual Ã© efetuado, provendo as mesmas consistÃªncias auditÃ¡veis.
*   **ReconciliaÃ§Ã£o e Baixa Consolidada Passiva:** Ao fim da agregaÃ§Ã£o de todas as etiquetas para todas as unidades na mesma van (Lote), a plataforma engole a necessidade da contagem manual e cospe um Resumo Digital. A caneta passiva entra nesse estÃ¡gio: o `Entregador` visualiza a lista gerada no dispotivo e emite uma assinatura Ãºnica certificando o despejo.
*   **PropagaÃ§Ã£o Silenciosa (Deferred Publish):** Mensagens (Push) nÃ£o estouram no meio do cadastro. Elas sÃ£o estancadas e publicadas simultaneamente Ã s unidades afetadas somente no final transacional (pÃ³s-assinatura), prevenindo ansiedades improdutivas para retiradas apressadas.

---

## 5. Arquitetura ExofÃ­sica e Design System: "The Digital Sentinel"
Paralelo ao lema operacional, o braÃ§o visual ("The Digital Estate"/"The Digital Sentinel") repudia o brutalismo dos ERPs padrÃ£o do mercado. A aplicaÃ§Ã£o de portaria se disfarÃ§a como um painel diretivo premium.

### 5.1. Regimes de Cores, Tonalidade e Textura
*   **O "Night and Light" Tonal (Midnight Azure):** Fugas brutas do "preto estrito" e brancos agressivos.
    * `primary:` **Deep Midnight** (`#002045` / `#1A365D`)
    * `surface` base background: **Cool Azure** (`#f8f9ff`). Traz limpeza atmosfÃ©rica.
    * Textos corporativos em **Charcoal** (`on-surface: #0d1c2f`).
*   **Fios e Disjuntores (No-Line Protocol):** SubdivisÃµes e *borders* rÃ­gidas `1px` compÃµe "sujeira Ã³ptica". Seccionamento deve emergir inteiramente de deltas tonais (mudar o fill para `surface_container_low`). Somente *Ghost Borders* (traÃ§os em 15% opacidade) ganham anistia para *Hover/Focus states*.
*   **Ãndices Dourados (Discrete Gold / Warm Gold):** O token `tertiary` (`#C5A059`) atua cirurgicamente como guia cognitivo tÃ¡til, utilizado apenas em nÃºmeros grandiosos de placar ou notificaÃ§Ãµes vitais do sistema.
*   **Textura "Mesh" e "Glassmorphism":** Em superfÃ­cies abertas, um padrÃ£o radial SVG translÃºcido paira (2-3%), fornecendo uma sensaÃ§Ã£o tÃ¡til premium. Barras de navegaÃ§Ã£o (`Bottom Navigation`) aderem a `backdrop-blur-xl` com fundo branco em 80% e leve *Ambient Shadow* (`-16px Y offset` em `0.06 alpha`), repousando levemente como vidro leitoso.

### 5.2. Tipografia Escalonada e Formologia
A adoÃ§Ã£o **Dual-Sans-Serif** dita o regimento do compasso de leitura.
*   **Manrope (The Anchor):** AplicaÃ§Ãµes estruturais geomÃ©tricas, pesadas em *Extra-Bold* e compactadas internamente (`-0.025em`). *Headlines (36px)* em portas de acesso emanam autoridade e controle.
*   **Inter (The Workhorse):** Designada para tabelas massivas de protocolo logÃ­stico onde clareza tÃ©cnica num espaÃ§o diminuto (`14px body` / `10px labels`) Ã© inegociÃ¡vel. Letreiros menores ganham uppercase com folgas de entrelinhas espaÃ§adas (`0.1em`).
*   **Silhueta AssimÃ©trica:** Os repositÃ³rios de interface grÃ¡fica renegam a caixa utilitÃ¡ria clÃ¡ssica. BotÃµes de chamamento e "cards estatÃ­sticos" quebram grides modulares assumindo silhuetas mais abertas utilizando raios controlados nativamente (`Border-Radius: 16px`), desassociando o programa de um utilitÃ¡rio arcaico de estoque industrial. BotÃµes principais interagem com uma fluida reduÃ§Ã£o fÃ­sica em pressionamentos (`200ms scale-down a 95%`).

---

## 6. SÃ­ntese Final
O cenÃ¡rio do Edifacil Management System Ã© balizado por uma dicotomia de sucesso:
Possui no seu reator lÃ³gicas rigorosas (EF Core, JWT HMAC, ValidaÃ§Ã£o de permissÃµes Android), enquanto externamente veste uma mÃ¡scara altamente orquestrada, de atrito zero para usuÃ¡rios nÃ£o-tÃ©cnicos como porteiros correndo contra o tempo fÃ­sico da rua, acobertada em um traje Ã³tico comparado somente com frotas veiculares ou hotÃ©is estrelados. A soma de *Unit-First Flow*, *Glassmorphism/Midnight Colors*, e *C# Mobile Architecture* encapsula a estratÃ©gia do MVP V 0.5 para o sucesso da implantaÃ§Ã£o real nos condensados urbanos do Brasil.

