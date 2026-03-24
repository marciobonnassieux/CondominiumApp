# Relatório do Sistema: Edifácil (O Aplicativo do Condomínio)

Este é um mapa traduzido de como o nosso aplicativo e o nosso sistema funcionam nos bastidores, escrito de forma simples e direta para que qualquer pessoa consiga entender como a tecnologia está dividida.

---

## 1. O Aplicativo do Cliente (O que você vê e usa)
O nosso aplicativo de celular (que chamamos de **Mobile**) possui várias telas e engrenagens para funcionar perfeitamente na mão do usuário. 

### As Telas do App
Estas são as "páginas" e caminhos que o morador ou o porteiro podem navegar atualmente:
- **Login:** A porta de entrada do aplicativo, onde você coloca seu e-mail e senha.
- **Criação de Conta:** A etapa onde um novo usuário se cadastra pela primeira vez.
- **Verificação de Segurança (OTP):** Aquela tela onde você digita um código secreto enviado por SMS ou E-mail para confirmar que você é você mesmo.
- **Seletor de Perfil (Contexto):** Muito útil para quem tem mais de um papel no condomínio. Por exemplo, se você for "Morador" e "Síndico", aqui você escolhe qual "chapéu" vai usar ao entrar.
- **Painel da Portaria (Dashboard):** A central de controle que fica na mão do porteiro, reunindo atalhos rápidos e resumo de informações.
- **Registro de Encomendas (Volume):** A tela usada pela portaria para dar entrada em pacotes, caixas e cartas que chegam.
- **Seleção de Apartamento (Unidade):** Para apontar de qual bloco ou apartamento é determinado pacote ou pessoa.
- **Gestão de Cargos e Vínculos:** Onde o sistema define quem é o porteiro, o zelador, o morador titular ou os dependentes.
- **Configuração de Estrutura:** As configurações gerais das áreas do condomínio, como os blocos e pátios.
- **Perfil do Usuário:** Onde você vê sua foto, seus dados e suas informações pessoais gerais.
- **Resumo e Assinatura:** A tela de confirmação oficial, possivelmente usada para pegar a assinatura digital ou confirmar o recebimento de algo importante.

### As Engrenagens do App
Além do que você vê, o aplicativo tem motores invisíveis trabalhando lá dentro:
- **As "Cascas" e as "Cores" (App e AppShell):** Eles definem a navegação de menu e o visual padrão, as cores gerais e a moldura externa do nosso app.
- **O Cérebro da Tela (ViewModels):** Por trás de cada tela bonita, existe um cérebro dedicado que entende que, quando você aperta "salvar", ele deve empacotar os dados e enviá-los de modo correto.
- **Os Mensageiros (Services):** Eles pegam os pedidos do usuário, saem do aplicativo e viajam pela internet até entregá-los na "central", trazendo depois as respostas de volta.

---

## 2. A "Central" ou Servidor (O que trabalha pela internet)
Tudo o que o aplicativo faz é validado, processado e salvo pela nossa Central na nuvem, que está dividida nas seguintes áreas:

- **As Regras de Ouro (Core):** É o livro de regras da vida real. Ali o sistema sabe exatamente o que significa as coisas do mundo físico: o que é um Apartamento, quais as regras de um Morador, o que é um Pacote, etc.
- **O Balcão de Atendimento (Api):** É como se fosse a recepção na sede da empresa. O aplicativo do celular manda um pedido de lá longe, essa área recebe pela internet, confere se o pedido faz sentido e aciona quem precisa.
- **O Cofre e Arquivo Oficial (Infrastructure e Banco de Dados):** É a área de extrema segurança onde todos os dados são guardados de forma permanente. Ele arquiva desde senhas até o histórico de quem pegou cada pacote. Qualquer mudança ou criação de novas alas (tabelas de banco de dados) só é feita ali, sob controle rigoroso.

---
**Status Atual:** Tudo mapeado. Nenhuma modificação sugerida, apenas a leitura limpa e descritiva do que compõe a nossa solução nos dias de hoje.
