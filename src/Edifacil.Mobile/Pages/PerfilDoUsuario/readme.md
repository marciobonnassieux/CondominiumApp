# Análise da Tela: Configurar Perfil (Perfil do Usuário)

## Objetivo da Tela
Oferecer ao usuário logado um painel para atualizar e revisar suas informações pessoais essenciais (nome, foto, telefone, CPF).

## Componentes Visuais e Fluxos Identificados
1. **Header Principal**:
   - Seta de voltar.
   - Título "Perfil".
   - Logo no canto direito "EDIFÁCIL".

2. **Avatar/Foto do Perfil**:
   - Imagem de perfil redonda com botão sobreposto contendo um ícone de câmera para acionar a galeria/câmera e atualizar a foto.

3. **Formulário de Dados Pessoais**:
   - "NOME COMPLETO": Entrada de texto com ícone de usuário. (Ex: Roberto da Silva).
   - "CPF": Entrada formatada (Ex: 000.000.000-00) com ícone de documento.
   - "TELEFONE": Entrada formatada, possivelmente com máscara de (DDD) + Num. com ícone de telefone.
   - "E-MAIL CADASTRADO": Campo desabilitado/read-only exibindo o email usado no login, com ícone de envelope e ícone de cadeado.

4. **Aviso de Privacidade**:
   - Card informativo visual com brasão de segurança indicando a criptografia dos dados ("Segurança de Dados").

5. **Ação Principal**:
   - Botão final ancorado na base ou no final do scroll ("SALVAR ALTERAÇÕES" com ícone de check).

## Regras de Negócio e Lógicas Necessárias
- Integrar validação de foto local usando MediaPicker, subindo essa imagem pro armazenamento em cloud antes ou durante o "Salvar Alterações".
- Aplicar Behaviors/Máscaras de formatação nos campos CPF e Telefone.
- Desabilitar a edição do e-mail, exigindo potencialmente um fluxo em separado (ou proibindo) alteração de e-mail por motivos de identificação root.
- Apenas enviar dados via PUT/PATCH se de fato ocorreu alteração (verificação via IsDirty boolean/flag in viewmodel).
