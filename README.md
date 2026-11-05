# FCG Payments API

Microsserviço responsável por processar pagamentos de compras de jogos da plataforma FIAP Cloud Games.

## Funcionalidades

- Consumir evento `OrderPlacedEvent` do RabbitMQ
- Simular processamento de pagamento (90% de aprovação)
- Publicar evento `PaymentProcessedEvent` com resultado
- Consultar histórico de pagamentos do usuário

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (InMemory Database)
- JWT Bearer Authentication
- RabbitMQ.Client
- Swagger/OpenAPI

## Variáveis de Ambiente

- `Jwt__Secret`: Chave secreta para validação do JWT
- `RabbitMQ__Host`: Hostname do servidor RabbitMQ (padrão: localhost)
- `RabbitMQ__Port`: Porta do RabbitMQ (padrão: 5672)
- `RabbitMQ__Username`: Usuário do RabbitMQ (padrão: guest)
- `RabbitMQ__Password`: Senha do RabbitMQ (padrão: guest)

## Como Executar

### Com .NET CLI

```bash
dotnet restore
dotnet build
dotnet run
```

### Com Docker

```bash
docker build -t fcg-payments-api .
docker run -p 8080:8080 fcg-payments-api
```

## Endpoints

- `GET /api/payments` - Listar pagamentos do usuário autenticado
- `GET /api/payments/{orderId}` - Consultar status de um pagamento específico

## Eventos Consumidos

### OrderPlacedEvent
**Routing Key:** `order.placed`

```json
{
  "userId": "guid",
  "gameId": "guid",
  "price": 299.90,
  "orderId": "guid",
  "timestamp": "datetime"
}
```

## Eventos Publicados

### PaymentProcessedEvent
**Routing Key:** `payment.processed`

```json
{
  "orderId": "guid",
  "userId": "guid",
  "gameId": "guid",
  "price": 299.90,
  "status": "Approved",
  "reason": null,
  "timestamp": "datetime"
}
```

## Simulação de Pagamento

O serviço simula o processamento de pagamento com as seguintes características:
- Delay aleatório de 1-3 segundos
- 90% de aprovação
- 10% de rejeição (motivo: "Saldo insuficiente")
