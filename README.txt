by uruchomić wpisz w powershellu docker-compose up --build

# Konwencje Modelowe
Wszystkie modele w projekcie powinny dziedziczyć po klasie bazowej `BaseEntity`, która zapewnia wspólne atrybuty:
- `Id` - unikalny identyfikator encji
- `CreatedAt` - data i czas utworzenia rekordu