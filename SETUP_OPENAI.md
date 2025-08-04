# 🔑 Configuração da API OpenAI

Para ativar as respostas inteligentes do chat, siga estes passos simples:

## 🚀 Passo 1: Obter uma Chave da API
1. Acesse [https://platform.openai.com/api-keys](https://platform.openai.com/api-keys)
2. Faça login na sua conta OpenAI
3. Clique em "Create new secret key"
4. Copie a chave gerada (ela começa com `sk-`)

## ⚙️ Passo 2: Configurar no Backend
1. Abra o arquivo `src/HighCapitalBot.API/appsettings.json`
2. Localize a seção `OpenAISettings`
3. Substitua `"SUA_CHAVE_API"` pela sua chave:

```json
"OpenAISettings": {
  "ApiKey": "sua-chave-aqui",
  "ModelName": "gpt-3.5-turbo",
  "MaxTokens": 1000,
  "Temperature": 0.7
}
```

## 🔄 Passo 3: Ativar o Serviço de IA
1. Abra o arquivo `src/HighCapitalBot.Core/Services/ChatService.cs`
2. Localize a linha que contém o comentário `// var botResponseContent = await _aiService.GetResponseWithHistoryAsync(messages);`
3. Descomente a linha removendo as barras `//` do início
4. Comente ou remova a linha abaixo que contém a mensagem fixa

## ▶️ Passo 4: Reiniciar o Servidor
Execute no terminal:
```bash
cd src/HighCapitalBot.API
dotnet run
```

## 🧪 Como Testar
1. Crie um novo bot ou use um existente
2. Envie uma mensagem no chat
3. Você deve receber uma resposta gerada pela IA

## 🔒 Dicas de Segurança
- Nunca compartilhe sua chave da API
- Nunca faça commit da chave no Git
- Para produção, use variáveis de ambiente

## 💡 Dúvidas Comuns

### Onde encontro minha chave da API?
Acesse: [https://platform.openai.com/api-keys](https://platform.openai.com/api-keys)

### Como sei se está funcionando?
Se você receber respostas geradas pela IA (e não a mensagem fixa), está tudo certo!

### Estou recebendo erros de autenticação
- Verifique se copiou a chave corretamente
- Confirme se sua conta tem créditos disponíveis
- Verifique os logs do servidor para mensagens de erro detalhadas

### Quanto custa usar a API da OpenAI?
Consulte os preços em: [https://openai.com/pricing](https://openai.com/pricing)
