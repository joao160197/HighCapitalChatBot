# HighCapitalChatBot

<div align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet" alt=".NET 9.0" />
  <img src="https://img.shields.io/badge/React-18.2-61DAFB?logo=react" alt="React 18.2" />
  <img src="https://img.shields.io/badge/OpenAI-GPT--3.5-412991?logo=openai" alt="OpenAI GPT-3.5" />
  <img src="https://img.shields.io/badge/License-MIT-blue.svg" alt="License: MIT" />
</div>

## ℹ️ Nota Importante

Este repositório contém apenas o backend da aplicação. O frontend está disponível em um repositório separado:
[HighCapitalChatBot-Frontend](https://github.com/joao160197/HighCapitalChatBot-Frontend)

Para executar o projeto completo, você precisará clonar ambos os repositórios. Veja as instruções de instalação abaixo.

## 🚀 Visão Geral do Projeto

O **HighCapitalChatBot** é uma aplicação web completa que permite a criação e interação com chatbots personalizados, potencializados por Inteligência Artificial. Desenvolvido como parte de um desafio técnico, o projeto demonstra uma arquitetura robusta com backend em C# .NET e frontend em ReactJS, seguindo as melhores práticas de desenvolvimento de software.



![Demonstração da Aplicação](docs/images/demo.gif)

## ✨ Funcionalidades Principais

### 🤖 Gerenciamento de Bots
- Criação de múltiplos chatbots com personalidades únicas
- Definição de contexto específico para cada bot
- Exclusão de bots

### 💬 Chat em Tempo Real
- Interface de conversação intuitiva
- Histórico completo de mensagens
- Indicador de digitação
- Respostas geradas por IA

### 🔐 Autenticação Segura
- Cadastro e login de usuários
- Autenticação JWT
- Rotas protegidas

## 🛠️ Tecnologias Utilizadas

### Backend
- **Linguagem:** C# 11
- **Framework:** .NET 9
- **API:** ASP.NET Core Web API
- **Banco de Dados:** Entity Framework Core + SQLite
- **Autenticação:** JWT (JSON Web Tokens)
- **IA:** OpenAI API (GPT-3.5-turbo)
- **Testes:** xUnit

### Frontend
- **Framework:** React 18
- **Gerenciamento de Estado:** React Context API
- **Roteamento:** React Router v6
- **UI Components:** Material-UI (MUI)
- **Requisições HTTP:** Axios
- **Formulários:** React Hook Form

## 🚀 Começando

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) 16+ e npm
- [SQLite](https://www.sqlite.org/index.html)
- Conta na [OpenAI](https://platform.openai.com/) (para chave de API)

### Instalação

1. **Clonar o repositório**
   ```bash
   git clone https://github.com/seu-usuario/HighCapitalChatBot.git
   cd HighCapitalChatBot
   ```

2. **Configurar o Backend**
   ```bash
   cd src/HighCapitalBot.API
   cp .env.example .env
   # Edite o arquivo .env com suas configurações
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

3. **Configurar o Frontend**
   ```bash
   cd ../../frontend
   npm install
   npm start
   ```

4. **Configurar o Repositório do Frontend**
   ```bash
   # Remova a pasta frontend vazia (se existir)
   rmdir /s /q frontend
   
   # Clone o repositório do frontend
   git clone https://github.com/joao160197/HighCapitalChatBot-Frontend.git frontend
   
   # Instale as dependências
   cd frontend
   npm install
   npm start
   ```

5. **Acessar a Aplicação**
   - Frontend: http://localhost:3000
   - Backend: http://localhost:5044
   - Swagger UI: http://localhost:5044/swagger

## 📦 Estrutura do Projeto

```
HighCapitalChatBot/
├── src/
│   ├── HighCapitalBot.API/         # API principal (ASP.NET Core)
│   │   ├── Controllers/            # Controladores da API
│   │   ├── Middleware/             # Middleware personalizado
│   │   └── Program.cs              # Ponto de entrada
│   │
│   ├── HighCapitalBot.Core/        # Lógica de negócio
│   │   ├── DTOs/                   # Objetos de Transferência de Dados
│   │   ├── Entities/               # Entidades do domínio
│   │   ├── Interfaces/             # Interfaces dos serviços
│   │   └── Services/               # Serviços de negócio
│   │
│   └── HighCapitalBot.Data/        # Camada de dados
│       ├── Migrations/             # Migrações do banco de dados
│       └── AppDbContext.cs         # Contexto do Entity Framework
│
├── frontend/                       # Aplicação React
│   ├── public/                     # Arquivos estáticos
│   └── src/
│       ├── components/             # Componentes React
│       ├── context/                # Contextos React
│       ├── services/               # Serviços de API
│       └── App.js                  # Componente raiz
│
├── tests/                          # Testes automatizados
├── .github/                        # Configurações do GitHub
├── .env.example                    # Exemplo de variáveis de ambiente
└── README.md                       # Este arquivo
```

## 🔧 Configuração

### Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto baseado no `.env.example`:

```env
# Configurações da API OpenAI
OPENAI_API_KEY=sua_chave_aqui
OPENAI_MODEL=gpt-3.5-turbo

# Configurações do JWT
JWT_SECRET=sua_chave_secreta
JWT_ISSUER=HighCapitalBot.API
JWT_AUDIENCE=HighCapitalBot.Client
JWT_EXPIRE_MINUTES=1440

# Configurações do Banco de Dados
CONNECTION_STRING=Data Source=HighCapitalBot.db
```

## 🧪 Testes

### Backend
```bash
cd src/HighCapitalBot.Tests
dotnet test
```

### Frontend
```bash
cd frontend
npm test
```



