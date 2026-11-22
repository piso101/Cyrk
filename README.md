# Cyrk

Prosty projekt demonstracyjny: backend ASP.NET Core 8 z PostgreSQL oraz aplikacja Android.

## Uruchomienie

1. Skopiuj `.env.example` do `.env`.
2. Ustaw własne wartości `POSTGRES_USER`, `POSTGRES_PASSWORD` i `JWT_SECRET_KEY`.
3. Uruchom:

```bash
docker compose up --build
```

Swagger API: `http://localhost:8080/swagger`

## Bezpieczeństwo konfiguracji

- Nie commituj pliku `.env`.
- `JWT_SECRET_KEY` musi mieć co najmniej 32 znaki.
- Używaj unikalnych danych logowania dla każdej lokalnej i produkcyjnej bazy.
- Dane w `db/02_dummy_data.sql` są fikcyjnymi danymi demonstracyjnymi.
