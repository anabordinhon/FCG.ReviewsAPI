# FCG Reviews API

API para cadastro e consulta de avaliacoes de jogos, com persistencia em MongoDB e autenticacao via JWT.

## Objetivo

Esta API permite:

- registrar uma avaliacao para um jogo;
- consultar as avaliacoes de um jogo especifico;
- calcular a media das notas e o total de avaliacoes por jogo.

Cada usuario autenticado pode enviar apenas uma avaliacao por jogo.

## Tecnologias

- .NET
- ASP.NET Core Web API
- MongoDB
- Swagger
- JWT Bearer Authentication

## Estrutura do projeto

```text
FCG.Reviews.API              -> camada de entrada HTTP
FCG.Reviews.Application      -> casos de uso e regras de aplicacao
FCG.Reviews.Domain           -> entidades e contratos de dominio
FCG.Reviews.Infrastructure   -> acesso ao MongoDB
FCG.Reviews.Tests            -> testes automatizados
k8s                          -> manifestos de deploy e MongoDB
```

## Como funciona com o MongoDB

A aplicacao le as configuracoes de banco a partir de `FCG.Reviews.API/appsettings.json`:

```json
"MongoDB": {
  "ConnectionString": "",
  "DatabaseName": "fcg_reviews"
}
```

Ao iniciar, a API:

- cria uma conexao com o MongoDB usando `MongoDB:ConnectionString`;
- acessa o banco definido em `MongoDB:DatabaseName`;
- utiliza a collection `game_reviews`;
- cria um indice por `GameId` e `UserId` para otimizar consultas.

## Configuracao local

Preencha os valores de conexao com o MongoDB e a chave JWT em `FCG.Reviews.API/appsettings.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "fcg_reviews"
  },
  "Jwt": {
    "SecretKey": "sua-chave-secreta-aqui"
  }
}
```

## Executando a aplicacao

Na raiz da solucao, execute:

```bash
dotnet restore
dotnet run --project FCG.Reviews.API
```

Em desenvolvimento, a aplicacao usa estas URLs:

- `https://localhost:54793`
- `http://localhost:54794`

## Swagger

Com a API em execucao, acesse:

- [Swagger HTTPS](https://localhost:54793/swagger)
- [Swagger HTTP](http://localhost:54794/swagger)

## Autenticacao

Os endpoints de avaliacoes exigem autenticacao com token Bearer.

O identificador do usuario e lido do claim `NameIdentifier` do JWT. Esse valor deve ser um numero inteiro, pois ele e usado como `UserId` da avaliacao.

Exemplo de header:

```http
Authorization: Bearer {seu-token}
```

## Endpoints

### `POST /api/reviews`

Cria uma nova avaliacao para um jogo.

Regras:

- o usuario precisa estar autenticado;
- a nota deve estar entre `1` e `5`;
- o mesmo usuario nao pode avaliar o mesmo jogo mais de uma vez.

Exemplo de requisicao:

```json
{
  "gameId": "11111111-1111-1111-1111-111111111111",
  "rating": 5,
  "comment": "Excelente jogo."
}
```

Exemplo de resposta de sucesso:

```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "id": "6651abc1234567890fedcba1",
    "gameId": "11111111-1111-1111-1111-111111111111",
    "userId": 10,
    "rating": 5,
    "comment": "Excelente jogo.",
    "createdAt": "2026-05-16T12:00:00Z"
  }
}
```

Possiveis retornos:

- `201 Created` quando a avaliacao e criada;
- `400 Bad Request` quando o usuario ja avaliou o jogo;
- `401 Unauthorized` quando o token esta ausente ou invalido;
- `500 Internal Server Error` se a nota enviada estiver fora do intervalo permitido, pois essa regra hoje gera excecao na camada de dominio.

### `GET /api/reviews/game/{gameId}`

Retorna o resumo de avaliacoes de um jogo.

Exemplo:

```http
GET /api/reviews/game/11111111-1111-1111-1111-111111111111
```

Exemplo de resposta:

```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "gameId": "11111111-1111-1111-1111-111111111111",
    "averageRating": 4.5,
    "totalReviews": 2,
    "reviews": [
      {
        "id": "6651abc1234567890fedcba1",
        "gameId": "11111111-1111-1111-1111-111111111111",
        "userId": 10,
        "rating": 5,
        "comment": "Excelente jogo.",
        "createdAt": "2026-05-16T12:00:00Z"
      },
      {
        "id": "6651abc1234567890fedcba2",
        "gameId": "11111111-1111-1111-1111-111111111111",
        "userId": 12,
        "rating": 4,
        "comment": "Muito bom.",
        "createdAt": "2026-05-15T18:30:00Z"
      }
    ]
  }
}
```

Possiveis retornos:

- `200 OK` com os dados do jogo;
- `400 Bad Request` quando nao houver avaliacoes para o jogo informado;
- `401 Unauthorized` quando o token esta ausente ou invalido.

## Collection no MongoDB

As avaliacoes sao armazenadas na collection `game_reviews`.

Exemplo de documento:

```json
{
  "_id": { "$oid": "6651abc1234567890fedcba1" },
  "gameId": "11111111-1111-1111-1111-111111111111",
  "userId": 10,
  "rating": 5,
  "comment": "Excelente jogo.",
  "createdAt": { "$date": "2026-05-16T12:00:00Z" }
}
```

## Health check

A API expoe um endpoint de saude em:

```http
GET /health
```

## Kubernetes

O projeto possui manifestos em `k8s/` para publicacao da API e do MongoDB:

- `k8s/reviewsapi-deployment.yaml`
- `k8s/reviewsapi-service.yaml`
- `k8s/reviewsapi-secret-template.yaml`
- `k8s/mongodb-statefulset.yaml`
- `k8s/reviewsapi-namespace.yaml`

## Observacoes importantes

- a chave JWT precisa ser a mesma usada pelo servico emissor do token;
- o claim `NameIdentifier` deve conter o ID numerico do usuario;
- a API ordena as avaliacoes mais recentes primeiro na consulta por jogo;
- a media das notas e retornada com duas casas decimais.
