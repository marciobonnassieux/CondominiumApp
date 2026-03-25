# Análise da Tela: Configurar Edifício (Gestão Administrativa)

## Objetivo da Tela
Permitir que usuários administradores visualizem os blocos estruturais existentes do condomínio e adicionem novos blocos, configurando automaticamente as unidades dentro de um intervalo de numeração.

## Componentes Visuais e Fluxos Identificados
1. **Header e Título**:
   - Cabecalho com botão de menu hamburguer, logo "EDIFÁCIL" e ícone de notificações.
   - Subtítulo: "GESTÃO ADMINISTRATIVA".
   - Título principal: "Configurar Edifício".

2. **Seção "Blocos Existentes"**:
   - Cabeçalho da seção indicando o número de blocos (ex: "4 BLOCOS").
   - Lista de Cards representando cada bloco. Cada card contém:
     - Ícone de prédio.
     - Nome do Bloco (ex: "Bloco A").
     - Intervalo de numeração das unidades (ex: "UNIDADES 101 A 808").
     - Ações: Ícone de edição e ícone de exclusão (lixeira).

3. **Seção "Adicionar Bloco"**:
   - Formulário embutido em um card.
   - Textos de ajuda explicando que os detalhes inseridos irão criar a estrutura e as unidades.
   - Campos de Entrada:
     - "NOME DO BLOCO" (Input de texto, ex: "Bloco D" ou "Ala Norte").
     - "DE (UNIDADE)" (Input numérico, ex: 101).
     - "ATÉ (UNIDADE)" (Input numérico, ex: 808).
   - Botão Primário: "CONFIRMAR BLOCO" com ícone de "+".
   - Alerta Informativo: Uma caixa de mensagem indicando que as unidades serão geradas automaticamente dentro do intervalo definido, seguindo o padrão de numeração do condomínio.

4. **Navegação Inferior (Bottom Tab Bar)**:
   - Ícones e labels: INÍCIO (ativo), VOLUMES, PESSOAS, SEGURANÇA.

## Regras de Negócio e Lógicas Necessárias
- O formulário "Adicionar Bloco" requer validação para garantir que os intervalos "DE" e "ATÉ" são numéricos e lógicos (o valor "ATÉ" deve ser compatível ou maior que o valor "DE").
- As ações de edição e exclusão de bloco precisam de fluxos de confirmação (Dialogs) para evitar deleções acidentais, além de verificar se existem entidades dependentes (como moradores ou boletos) atrelados às unidades desse bloco.
- Ao clicar em "Confirmar Bloco", uma chamada de API/serviço deve ser feita para criar a estrutura em lote.
