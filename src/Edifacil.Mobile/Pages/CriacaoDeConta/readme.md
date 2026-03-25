# Análise da Tela: Criar Nova Conta

## Objetivo da Tela
Funcionar como uma página de registro (Sign Up) para moradores ou usuários do condomínio, permitindo que criem suas credenciais de acesso através do preenchimento de e-mail e criação de senha.

## Componentes Visuais e Fluxos Identificados
1. **Header**:
   - Botão de voltar (seta para esquerda) com texto "Edifácil".
   - Logo centralizado com texto "EDIFÁCIL - O APP DO CONDOMÍNIO".

2. **Corpo Principal (Card Central)**:
   - Título: "Criar Nova Conta".
   - Descrição de ajuda: "Preencha os dados abaixo para acessar sua unidade".
   - Campos de Entrada (Formulário):
     - "E-mail" (Input text para e-mail).
     - "Confirmar E-mail" (Input text para verificação).
     - "Senha" (Input password com botão "olho" para exibir/esconder).
     - "Confirmar Senha" (Input password com botão "olho" para exibir/esconder).
   - Tooltip/Mensagem Informativa: Aviso indicando "A senha deve conter pelo menos 8 caracteres, incluindo letras e números".
   - Ações:
     - Botão Primário: "SALVAR" (azul escuro).
     - Botão Secundário: "VOLTAR" (branco com bordas transparentes).

3. **Footer**:
   - Label decorativa pequena no rodapé (ex: "CONCIERGE DIGITAL EXPERIENCE • 2024").

## Regras de Negócio e Lógicas Necessárias
- O formulário requer validação complexa:
  - Formatação válida de e-mail nas duas entradas de e-mail.
  - Igualdade de valores entre os campos `E-mail` e `Confirmar E-mail`.
  - Igualdade de valores entre os campos `Senha` e `Confirmar Senha`.
  - Verificação de força de senha: Mínimo de 8 caracteres contendo ao menos 1 letra e 1 número.
- O botão "Olho" nos inputs de senha deve alternar a propriedade `IsPassword` do Entry XAML.
- O botão "VOLTAR" deve efetuar um `Navigation.PopAsync()` ou fechar a modal/página atual.
- O botão "SALVAR" deve disparar a Action de API pra criar o usuário, e deve exibir loading state. Se o usuário já existir ou der erro, expor o erro em UI.
