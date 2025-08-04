# HighCapitalChatBot

## 🚀 Visão Geral do Projeto

O **HighCapitalChatBot** é uma aplicação web completa que permite a criação e interação com chatbots personalizados, potencializados por Inteligência Artificial. Desenvolvido como parte de um desafio técnico, o projeto demonstra uma arquitetura robusta com backend em C# .NET e frontend em ReactJS, seguindo as melhores práticas de desenvolvimento de software.

A plataforma permite que usuários criem múltiplos bots, cada um com seu próprio "contexto" (personalidade), e conversem com eles em tempo real. O histórico das conversas é salvo e exibido, proporcionando uma experiência de chat contínua e rica.

---

## ✨ Funcionalidades Implementadas

-   **🤖 Criação de Chatbots Personalizados:**
    -   Crie bots informando um nome e um contexto inicial (ex: "Você é um assistente de marketing especialista em mídias sociais").
    -   Visualize todos os bots criados em uma tela de gerenciamento.

-   **💬 Interação em Tempo Real:**
    -   Interface de chat moderna para conversar com os bots selecionados.
    -   Respostas geradas pela API da OpenAI, utilizando o contexto do bot e o histórico da conversa.

-   **🗃️ Persistência de Dados:**
    -   Todas as mensagens (usuário e bot) são salvas no banco de dados.
    -   O histórico completo da conversa é recuperado ao reabrir um chat.

-   **🔐 Autenticação de Usuários:**
    -   Sistema de login e registro com autenticação baseada em JWT (JSON Web Tokens) para proteger as rotas.

-   **💪 Tratamento de Erros Robusto:**
    -   O sistema lida de forma elegante com falhas na API da IA, exibindo uma mensagem amigável ao usuário sem interromper a aplicação.

---

## 🛠️ Tecnologias Utilizadas

### **Backend**
-   **Linguagem:** C#
-   **Framework:** .NET 9
-   **API:** ASP.NET Core Web API
-   **Banco de Dados:** Entity Framework Core com SQLite
-   **API de IA:** OpenAI
-   **Arquitetura:** Injeção de Dependência, Padrão Repositório, Services

### **Frontend**
-   **Framework:** ReactJS
-   **Gerenciador de Pacotes:** npm
-   **Estilização:** CSS Padrão / Component-Based Styling
-   **Comunicação com API:** Axios

---

## 📂 Estrutura do Projeto

```
/
├── src/
│   ├── HighCapitalBot.Api/         # Projeto do Backend (Controllers, Program.cs, etc.)
│   ├── HighCapitalBot.Core/        # Lógica de negócio, Entidades, DTOs, Services, Interfaces
│   ├── HighCapitalBot.Data/        # Contexto do EF Core, Repositórios, Migrações
│   └── HighCapitalBot.sln          # Solução do Visual Studio
├── frontend/                       # Projeto do Frontend (React)
│   ├── public/
│   └── src/
└── README.md
```

---

## ⚙️ Instalação e Execução

Siga os passos abaixo para configurar e executar o projeto em seu ambiente local.

### **Pré-requisitos**
-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) ou superior
-   [Node.js e npm](https://nodejs.org/en/)
-   Um editor de código como [Visual Studio Code](https://code.visualstudio.com/) ou Visual Studio

### **1. Configuração do Backend**

1.  **Clone o repositório:**
    ```bash
    git clone <URL_DO_REPOSITORIO>
    cd HighCapitalBot
    ```

2.  **Configure a API da OpenAI:**
    -   Navegue até o projeto da API: `cd src/HighCapitalBot.Api`
    -   Abra o arquivo `src/HighCapitalBot.API/appsettings.Development.json` (crie-o se não existir, copiando de `appsettings.json`).
    -   Insira sua chave da API da OpenAI no campo `ApiKey` dentro de `OpenAiSettings`:
        ```json
        "OpenAiSettings": {
          "ApiKey": "SUA_CHAVE_API_AQUI",
          "ModelName": "gpt-3.5-turbo"
        }
        ```

3.  **Execute o Backend:**
    -   Ainda no diretório `src/HighCapitalBot.Api`, execute o comando:
        ```bash
        dotnet run
        ```
    -   O servidor backend estará rodando em `http://localhost:5044`.
    -   O banco de dados (`HighCapitalBot.db`) será criado automaticamente na primeira execução.

### **2. Configuração do Frontend**

1.  **Navegue até a pasta do frontend:**
    ```bash
    # A partir da raiz do projeto
    cd ../frontend 
    ```

2.  **Instale as dependências:**
    ```bash
    npm install
    ```

3.  **Execute o Frontend:**
    ```bash
    npm start
    ```
    -   A aplicação React estará disponível em `http://localhost:3000`.

Agora você pode se registrar, criar seu primeiro bot e começar a conversar!
