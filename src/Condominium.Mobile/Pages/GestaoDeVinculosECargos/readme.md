# Análise da Tela: Gestão de Vínculos

## Objetivo da Tela
Permitir que usuários administradores ou gerentes vinculem novos funcionários/usuários por CPF e acompanhem/modifiquem os cargos da equipe vinculada ao condomínio.

## Componentes Visuais e Fluxos Identificados
1. **Header**:
   - Botão de voltar (seta para esquerda).
   - Título: "Gestão de Vínculos".
   - Ícone de Lupa (busca rápida de usuários).

2. **Ação de Vinculação**:
   - Card/Botão em destaque: "Vincular novo usuário por CPF" com ícone de usuário. Acredita-se que este botão abra uma modal ou navegue para uma tela de busca/cadastro.

3. **Listagem de Equipe (Equipe Vinculada)**:
   - Título de seção contendo um contador: "EQUIPE VINCULADA" e "12 USUÁRIOS".
   - Lista renderizando cards para cada integrante:
     - Avatar ou iniciais (alguns com dot/status online verde).
     - Nome (Ex: "Ricardo Silveira").
     - CPF parcialmente oculto (Ex: "***.452.118-**").
     - Dropdown de Cargo (Um seletor picker configurado para mudar a função, ex: "Porteiro", "Zelador", "Limpeza").

4. **Botão de Confirmação**:
   - Botão flutuante ou fixop no rodapé: "SALVAR ALTERAÇÕES" com ícone de check, utilizado para submeter em lote as alterações de reatribuição de cargos feitas nos dropdowns.

5. **Navegação (Bottom Tab Bar)**:
   - Abas disponíveis: INÍCIO, VOLUMES, GESTÃO (destacada), SEGURANÇA.

## Regras de Negócio e Lógicas Necessárias
- A tela requer chamadas de API para o preenchimento da lista com informações não sensíveis (CPF mascarado).
- O Picker de cargos deve ser prepopulado com as roles disponíveis de acordo com o escopo do condomínio logado.
- Mudanças nos Pickers devem atualizar um estado modificado, mas a persistência central ocorre ao pressionar "Salvar Alterações".
- A funcionalidade de "Vincular usuário por CPF" provavelmente precisa invocar uma caixa de diálogo ou rotear para uma tela que consulte se esse CPF já tem conta no app.
