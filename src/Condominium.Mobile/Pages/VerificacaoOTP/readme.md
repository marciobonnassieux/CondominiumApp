# Análise da Tela: Verificação de Segurança (Entrada OTP)

## Objetivo da Tela
Garantir o fluxo de autenticação em duas etapas (2FA) ou confirmação de e-mail ao exigir que o usuário insira um código (One Time Password) de 6 dígitos que foi encaminhado por endereço externo (email corporativo / SMS).

## Componentes Visuais e Fluxos Identificados
1. **Container Principal (Card Modal/Page):**
   - Retângulo branco centralizado. Fundo da View com tom levemente azul claro solid.
   - Ícone no Topo-Esquerdo: Seta de voltar para cancelar/retornar ao fluxo originador.

2. **Destaque Visual:**
   - Círculo de cor base soft blue contendo um ícone de Cadeado Destrancado azul escuro.
   - Título: "Verificação de Segurança".
   - Subtítulo com instrução: "Enviamos um código de 6 dígitos para o seu email corporativo.". A mensagem pode ser dinâmica dependendo do destino do OTP.

3. **Controle de Entrada OTP (Code Input):**
   - Uma fileira de 6 pequenas caixas quadradas/arrendondadas (boxes).
   - Comportamento de foco: Ao digitar um número na primeira caixa, o foco (cursor) pula automaticamente para a seguinte. O item focado exibe um Outline (borda) azulada (Ex: "4", "8", Cursor "|", ".", ".", ".").

4. **Componentes Relacionados a Tempo**:
   - Badge ("Pill") oval de cor bege/amarelo claro exibindo um ícone de relógio e contador regressivo de tempo "04:59".
   - Logo abaixo, um texto clicável/label informativo estático: "Reenviar código".

5. **Ação Principal**:
   - Botão inferior largo: "VERIFICAR" + Seta linear/direita. Fundo azul escuro, texto branco.

## Regras de Negócio e Lógicas Necessárias
- O Entry do OTP precisa ser um Custom Control ou comportamento segmentado que só aceite NÚMEROS (Numeric Keyboard) e de comprimento "1", encadeando o Focus para o próximo Entry ao preencher.
- Um Timer nativo (`System.Timers.Timer` ou loop assíncrono referenciado pelo ViewModel) gerencia a contagem de 5 minutos (05:00 a 00:00).
- Enquanto o timer for `> 0`, o link "Reenviar Código" deve permanecer desabilitado e com opacidade menor. Ao expirar (00:00), ativa o botão Reenviar.
- "Verificar" acerta o enpoint da API validando o código. O botão precisa estar desabilitado caso não tenham os 6 dígitos preenchidos.
- Sucesso valida o contexto (Logado/Confirmado) e navega.Erro exige vibração tátil e limpar a máscara, aplicando classe de erro nas bordas vermelhas.
