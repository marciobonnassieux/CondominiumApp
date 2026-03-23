# Edifácil V 0.5 (Condominium Management System) - Relatório Analítico de Contexto Geral

Este documento apresenta um mapeamento estruturado, exaustivo e analítico do ecossistema **Edifácil**. A consolidação a seguir cruza especificações de negócio, arquitetura sistêmica nativa, fluxos de engenharia C#/.NET MAUI, topologia de rede local e a diretriz matriz de Design System ("The Digital Sentinel/Estate"). Este relatório atua como a fonte central de verdade (Single Source of Truth) para engenharia e design.

---

## 1. Fundamentação, Visão de Produto e Valor
Concebido como um SaaS B2B2C nacional, o **Edifácil** (*"Edifício é difícil? Edifácil resolve."*) ambiciona orquestrar a complexidade da gestão condominial com precisão cirúrgica. 
O MVP (Versão 0.5) endereça o principal ponto de atrito em condomínios de médio/grande porte: **A logística de recepção de encomendas em portarias.** Ao focar neste escopo, a plataforma valida sua resiliência transacional e a aderência visual antes de escalar para os módulos financeiro e administrativo completos.

---

## 2. Arquitetura de Software e Stack Tecnológica (Environment)
A plataforma é forjada utilizando os *toolings* de ponta da Microsoft (Versões Preview/Current), garantindo longevidade do código e máxima performance.

### 2.1. Back-End, API e Core
*   **Framework Base:** `.NET 10 SDK` rodando `Kestrel` na porta universal `5114`.
*   **Arquitetura:** Isolamento claro entre `Core` (Modelos agnósticos: *User, Profile, Unit, Delivery, Courier_Log*), `Infrastructure` (Persistência e Serviços de Segurança), e `API Controllers` pautados em *Role-Based Access Control* (RBAC).
*   **Banco de Dados:** `SQL Server LocalDB` roteado via `Entity Framework Core (EF Core)`. Permite desenvolvimento autônomo sem necessidade complexa de contêineres Docker no host do desenvolvedor.
*   **Segurança e Autenticação:** `TokenService` gerando tokens *HMAC-256 JWT* injetados via cabeçalho `Bearer` nas requisições subsequentes.
*   **Polinização de Dados (Seed):** O `DbInitializer` injeta uma *Fake Data Structure* em bancos virgens, provendo rapidamente credenciais de teste para porteiros (Ex: CPF `12345678900` / Senha `123456`) e moradores residentes.

### 2.2. Front-End Mobile (C# .NET MAUI)
*   **Alvo de Compilação:** `Android SDK (API 36)` (net10.0-android).
*   **Estruturação de Navegação:** A arquitetura ignora o tradicional `AppShell` em prol de uma `NavigationPage` pura. Essa decisão técnica limpa ruídos estruturais do sistema operacional, permitindo um controle total da *stack* temporal de push/pop das visões focadas estritamente nas regras de negócio empresariais.
*   **Testabilidade Mista:** Compilação ao vivo (`dotnet watch`) em Emuladores Locais, suportando *Bypass* nativo via Wi-Fi para testes tangíveis via ADB no dispositivo físico do desenvolvedor, alterando a âncora `10.0.2.2` para o `IPv4` local (`192.168.x.x`).

---

## 3. Topologia de Identidade e Multi-Contextualidade
A árvore de domínio reflete um modelo hierárquico estrito (Master → Administradora → Condomínio → Bloco → Unidade). No entanto, a modelagem de identidade (`User`) é flexível para suportar o mundo real logístico:
*   **Múltiplos Vínculos Operacionais:** Um único indivíduo (atrelado ao CPF e protegido por senha mista e OTP via e-mail de 300s) pode possuir múltiplos Cargos espalhados em diferentes Condomínios.
*   **Seletor de Contexto (Context Switcher):** Após a camada de autenticação primária, se o token desvendar múltiplos papéis válidos, o usuário não loga diretamente no Dashboard. Ele intercepta o `Seletor de Contexto`, que moldará os *Claims* do seu acesso momentâneo, definindo ativamente se ele verá o app operando como *Síndico do Bloco B* ou *Morador do Bloco C*.

### 3.1. Atores da Ação Sistêmica (Mapeamento UML Consolidado)
1. **Master/Dev:** Superusuário para setup SaaS das Administradoras.
2. **Gestor Adm:** Cadastra Condomínios e traça a arquitetura dos blocos/unidades.
3. **Síndico:** Parametriza e atribui cargos a porteiros e funcionários.
4. **Porteiro:** Operador do front-line (v0.5). Utiliza módulos pesados de hardware (Câmera) e exige fluidez zero-latency.
5. **Morador:** O consumidor final, recebendo Push/WhatsApp via rotinas de consultas e disparos assíncronos de notificação.
6. **Entregador (Courier):** O ator analógico-digital de encerramento de lote (Assinatura do Recibo do Lote Integral).

---

## 4. Engenharia do Fluxo Operacional: Logística Unit-First
O coração do V 0.5 bate na `ReceiveDeliveryPage`, projetada exaustivamente para reduzir fricção no balcão da portaria através do axioma **Unit-First (Unidade Primeira)**. Em vez de registrar um objeto aleatório, o porteiro engloba pacotes sequencialmente para a mesma moradia.

### 4.1. Inovações Táticas da Interface do Porteiro
*   **Componente Pop-up ComboBox (Sobreposição Visual):** Menus Dropdown sistêmicos nativos (como `<Picker>`) foram abandonados. Utiliza-se uma Grade semitransparente em tela-cheia com integração de `SearchBar`. A Collection View acoplada é filtrada ativamente na memória (In-Memory Filter), mantendo o contexto gráfico da tela imaculado em segundo plano.
*   **Telemetria Visual de Câmera (Integração ZXing):** A intenção nativa captura controle total de bloqueios no manifesto Android (`Camera Permission Checks`). O scanner a lazer decodifica Matrix/EAN injetando a string no formulário sem intervenção de digitação de usuário. Em caso de correspondências simples (sem etiqueta), um fluxo *Fallback* de registro de fotografia manual é efetuado, provendo as mesmas consistências auditáveis.
*   **Reconciliação e Baixa Consolidada Passiva:** Ao fim da agregação de todas as etiquetas para todas as unidades na mesma van (Lote), a plataforma engole a necessidade da contagem manual e cospe um Resumo Digital. A caneta passiva entra nesse estágio: o `Entregador` visualiza a lista gerada no dispotivo e emite uma assinatura única certificando o despejo.
*   **Propagação Silenciosa (Deferred Publish):** Mensagens (Push) não estouram no meio do cadastro. Elas são estancadas e publicadas simultaneamente às unidades afetadas somente no final transacional (pós-assinatura), prevenindo ansiedades improdutivas para retiradas apressadas.

---

## 5. Arquitetura Exofísica e Design System: "The Digital Sentinel"
Paralelo ao lema operacional, o braço visual ("The Digital Estate"/"The Digital Sentinel") repudia o brutalismo dos ERPs padrão do mercado. A aplicação de portaria se disfarça como um painel diretivo premium.

### 5.1. Regimes de Cores, Tonalidade e Textura
*   **O "Night and Light" Tonal (Midnight Azure):** Fugas brutas do "preto estrito" e brancos agressivos.
    * `primary:` **Deep Midnight** (`#002045` / `#1A365D`)
    * `surface` base background: **Cool Azure** (`#f8f9ff`). Traz limpeza atmosférica.
    * Textos corporativos em **Charcoal** (`on-surface: #0d1c2f`).
*   **Fios e Disjuntores (No-Line Protocol):** Subdivisões e *borders* rígidas `1px` compõe "sujeira óptica". Seccionamento deve emergir inteiramente de deltas tonais (mudar o fill para `surface_container_low`). Somente *Ghost Borders* (traços em 15% opacidade) ganham anistia para *Hover/Focus states*.
*   **Índices Dourados (Discrete Gold / Warm Gold):** O token `tertiary` (`#C5A059`) atua cirurgicamente como guia cognitivo tátil, utilizado apenas em números grandiosos de placar ou notificações vitais do sistema.
*   **Textura "Mesh" e "Glassmorphism":** Em superfícies abertas, um padrão radial SVG translúcido paira (2-3%), fornecendo uma sensação tátil premium. Barras de navegação (`Bottom Navigation`) aderem a `backdrop-blur-xl` com fundo branco em 80% e leve *Ambient Shadow* (`-16px Y offset` em `0.06 alpha`), repousando levemente como vidro leitoso.

### 5.2. Tipografia Escalonada e Formologia
A adoção **Dual-Sans-Serif** dita o regimento do compasso de leitura.
*   **Manrope (The Anchor):** Aplicações estruturais geométricas, pesadas em *Extra-Bold* e compactadas internamente (`-0.025em`). *Headlines (36px)* em portas de acesso emanam autoridade e controle.
*   **Inter (The Workhorse):** Designada para tabelas massivas de protocolo logístico onde clareza técnica num espaço diminuto (`14px body` / `10px labels`) é inegociável. Letreiros menores ganham uppercase com folgas de entrelinhas espaçadas (`0.1em`).
*   **Silhueta Assimétrica:** Os repositórios de interface gráfica renegam a caixa utilitária clássica. Botões de chamamento e "cards estatísticos" quebram grides modulares assumindo silhuetas mais abertas utilizando raios controlados nativamente (`Border-Radius: 16px`), desassociando o programa de um utilitário arcaico de estoque industrial. Botões principais interagem com uma fluida redução física em pressionamentos (`200ms scale-down a 95%`).

---

## 6. Síntese Final
O cenário do Condominium Management System é balizado por uma dicotomia de sucesso:
Possui no seu reator lógicas rigorosas (EF Core, JWT HMAC, Validação de permissões Android), enquanto externamente veste uma máscara altamente orquestrada, de atrito zero para usuários não-técnicos como porteiros correndo contra o tempo físico da rua, acobertada em um traje ótico comparado somente com frotas veiculares ou hotéis estrelados. A soma de *Unit-First Flow*, *Glassmorphism/Midnight Colors*, e *C# Mobile Architecture* encapsula a estratégia do MVP V 0.5 para o sucesso da implantação real nos condensados urbanos do Brasil.
