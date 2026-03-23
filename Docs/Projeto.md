# Edifácil V 0.5 - Documento de Visão Geral e Escopo

---

## 1. Contexto do Projeto
O **Edifácil** é uma plataforma SaaS projetada para simplificar a operação diária de condomínios. A versão inicial (0.5) foca na resolução do gargalo de recebimento de encomendas em portarias de prédios residenciais.

## 2. Hierarquia Operacional
Para garantir escalabilidade nacional, a estrutura de dados respeita a seguinte árvore:
- **Master** -> Administradoras -> Condomínios -> Blocos -> Unidades.

## 3. Regras de Negócio Críticas
- **Segurança:** Senha mínima de 8 caracteres (1 especial, 1 número, 1 maiúscula/minúscula).
- **Validação:** Código OTP por e-mail com validade de 300 segundos.
- **Multivínculo:** Um único CPF pode possuir múltiplos cargos em diferentes condomínios.
- **Logística:** Fluxo "Unidade-First" para agilizar o registro de múltiplos pacotes.

## 4. Mapa de Telas (V 0.5)
### Módulo de Acesso
- Login (CPF/E-mail)
- Cadastro Inicial
- Esqueci Senha (OTP 300s)
- Seletor de Contexto (Condomínio/Cargo)
- Perfil do Usuário (Nome, CPF, Telefone, Foto, Cargo)

### Módulo Gerencial
- Dashboard Admin (Administradora)
- Cadastro de Condomínios
- Configuração de Estrutura (Blocos e Unidades)
- Atribuição de Usuários a Unidades/Cargos

### Módulo Operacional (Portaria)
- Dashboard de Operações
- Recepção de Encomendas (v5 - Agrupado por Unidade)
- Registro de Evidência (Scanner/Foto)
- Resumo e Assinatura de Lote
- Baixa de Encomendas (Saída)

## 5. Casos de Uso (UML Mermaid)

```mermaid
graph LR
    Master[Ator: Master Dev]
    Adm[Ator: Gestor Adm]
    Sindico[Ator: Sindico]
    Porteiro[Ator: Porteiro]
    Morador[Ator: Morador]
    Entregador[Ator: Entregador]

    subgraph Acesso_e_Perfil
        UC01(UC01: Login Email ou CPF)
        UC02(UC02: Recuperar Senha OTP)
        UC03(UC03: Configurar Perfil)
        UC04(UC04: Selecionar Contexto)
    end

    subgraph Gerencial_SaaS
        UC05(UC05: Cadastrar Administradora)
        UC06(UC06: Cadastrar Condominio)
        UC07(UC07: Configurar Blocos e Unidades)
        UC08(UC08: Atribuir Cargos)
    end

    subgraph Operacional_Encomendas
        UC09(UC09: Registrar Entrada)
        UC10(UC10: Assinar Protocolo Lote)
        UC11(UC11: Registrar Saida ou Baixa)
    end

    Master --- UC05
    Adm --- UC06
    Adm --- UC07
    Sindico --- UC07
    Sindico --- UC08
    Porteiro --- UC09
    Porteiro --- UC11
    Entregador --- UC10
```

---

*Documento gerado para a versão piloto (54 apartamentos).*