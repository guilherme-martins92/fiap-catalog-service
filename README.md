# 🚗 Catálogo de Veículos - Microserviço

Este projeto é um microserviço de **Catálogo de Veículos**, desenvolvido como uma **AWS Lambda** utilizando **.NET 8**. Ele permite realizar as operações de **criação**, **edição**, **remoção**, **busca** e **listagem** de veículos cadastrados.

## 🛠️ Tecnologias Utilizadas

- [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [AWS Lambda](https://aws.amazon.com/lambda/)
- [Amazon DynamoDB](https://aws.amazon.com/dynamodb/)

## 📦 Funcionalidades

- ✅ Cadastrar novo veículo
- ✏️ Editar veículo existente
- ❌ Remover veículo do catálogo
- 🔍 Buscar veículo por ID
- 📄 Listar todos os veículos cadastrados

## 🧪 Testes

Os testes estão localizados no diretório `src/fiap-catalog-service-tests`. Para executá-los, utilize o seguinte comando:

```bash
dotnet test
```

## 🚀 Deploy

O deploy da Lambda pode ser feito via AWS CLI, AWS Console ou utilizando ferramentas como o [AWS Serverless Application Model (SAM)](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/what-is-sam.html) ou o [AWS CDK](https://docs.aws.amazon.com/cdk/latest/guide/home.html).

## 📄 Exemplo de Payload

### Cadastrar veículo

```json
{
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2022,
  "color": "Prata",
  "price": 95000.00
}
```
