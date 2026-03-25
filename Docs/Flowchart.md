# Fluxograma de Telas (Edifacil App)

Este diagrama Mermaid mapeia em alto espectro a jornada do Perfil **"Porteiro/Janitor"** efetuando Check-in de Pacotes via rotas HTTP no dispositivo.

```mermaid
graph TD
    A[Sistema Operacional Android / Wi-Fi Aparelho FÃ­sico] -->|Inicializa| B(App.xaml.cs)
    B -->|NavigationPage| C[LoginPage.xaml]
    
    subgraph "1. Handshake de SeguranÃ§a"
        C -->|Preenchimento Livre de Teclado| D("CPF: 12345678900 / Senha: ***")
        D -->|BotÃ£o POST| E{API: /api/auth/login}
        E -->|ðŸ”´ Status 401 Falha| C
    end
    
    subgraph "2. Ãrea de Trabalho Logada"
        E -->|ðŸŸ¢ Status 200 Token Salvo| F[DashboardPage.xaml]
        F -->|Grid Acessar| G[UnitsPage.xaml]
        F -->|Grid Acessar| H[ReceiveDeliveryPage.xaml]
        
        G -.->|Injeta Bearer JWT AutomÃ¡tico| I[(GET Server API local)]
    end
    
    subgraph "3. Controle e Registro FÃ­sico Interno da GUI"
        H -->|BotÃ£o ðŸ” Buscar Unidade| Pop[Modal Flutuante de Dropdown]
        Pop -->|Filtra Collection View| PopSelect[Apto Encontrado na Lista?]
        PopSelect -->|Tocado| H
        
        H -->|BotÃ£o ðŸ“· Acionar Scan| J{Possui PermissÃ£o CÃ¢mera Android?}
        J -->|Alerta Bloqueio| H
        J -->|Inicializa ZXing Lente| K[ScanPage.xaml]
        K -->|Auto-DeteccÃ£o do Laser| L[Dispara AÃ§Ã£o e Retorna o NÃºmero IlegÃ­vel]
        L -->|Campo Barcode Injetado| M{Aciona BotÃ£o Registrar}
        
        M -->|Limpa Texto e Grava Tabela Visual| H
    end
```

