# 🔑 Configuração da API OpenAI

Para que o chat funcione corretamente, você precisa configurar sua chave da API da OpenAI.

## 📋 Passos para Configuração

### 1. Obter a Chave da API
1. Acesse [https://platform.openai.com/api-keys](https://platform.openai.com/api-keys)
2. Faça login na sua conta OpenAI
3. Clique em "Create new secret key"
4. Copie a chave gerada (ela começa com `sk-`)

### 2. Configurar no Backend
Abra o arquivo `src/HighCapitalBot.API/appsettings.json` e substitua:

```json
{
  "OpenAISettings": {
    "ApiKey": "sua-chave-da-api-aqui",
    "ModelName": "gpt-3.5-turbo",
    "MaxTokens": 1000,
    "Temperature": 0.7
  }
}
```

**⚠️ IMPORTANTE:** Substitua `"sua-chave-da-api-aqui"` pela sua chave real da OpenAI.

### 3. Reiniciar o Backend
Após configurar a chave, reinicie o servidor do backend:

```bash
cd src/HighCapitalBot.API
dotnet run
```

## 🧪 Testando a Funcionalidade

1. **Crie um bot** na interface web
2. **Clique no bot** para abrir o chat
3. **Envie uma mensagem** e aguarde a resposta da IA

## 🔒 Segurança

- **Nunca** compartilhe sua chave da API
- **Não** faça commit da chave no Git
- Para produção, use variáveis de ambiente em vez do appsettings.json

## 💰 Custos

- O modelo `gpt-3.5-turbo` tem custo por token
- Monitore seu uso em [https://platform.openai.com/usage](https://platform.openai.com/usage)
- Considere definir limites de uso na sua conta OpenAI

## 🛠️ Solução de Problemas

### Erro: "OpenAI API key not configured"
- Verifique se a chave foi configurada corretamente no appsettings.json
- Certifique-se de que não há espaços extras na chave

### Erro: "Unauthorized" ou "Invalid API Key"
- Verifique se a chave está correta
- Confirme se sua conta OpenAI tem créditos disponíveis

### Mensagens não aparecem
- Verifique os logs do backend para erros
- Confirme se o frontend está conectado ao backend correto
