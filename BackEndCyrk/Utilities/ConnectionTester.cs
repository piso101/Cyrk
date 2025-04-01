using Npgsql;
using System;
using System.Data;
using System.Text;

namespace BackEndCyrk.Utilities
{
    public static class ConnectionTester
    {
        public static string TestNpgsqlConnection(string connectionString)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("=== DIAGNOSTYKA POŁĄCZENIA POSTGRESQL ===");
            result.AppendLine($"Czas testu: {DateTime.Now}");
            result.AppendLine();

            try
            {
                result.AppendLine("Próba parsowania connection stringa...");
                var connStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
                result.AppendLine("✓ Connection string poprawnie sparsowany");
                result.AppendLine();

                result.AppendLine("Parsowane parametry połączenia:");
                result.AppendLine($"- Host: {connStringBuilder.Host}");
                result.AppendLine($"- Port: {connStringBuilder.Port}");
                result.AppendLine($"- Database: {connStringBuilder.Database}");
                result.AppendLine($"- Username: {connStringBuilder.Username}");
                result.AppendLine($"- Password: {'*' * (connStringBuilder.Password?.Length ?? 0)}");
                result.AppendLine($"- Timeout: {connStringBuilder.Timeout} sekund");
                result.AppendLine($"- SearchPath: {connStringBuilder.SearchPath}");
                result.AppendLine();

                result.AppendLine("Testowanie TCP połączenia do hosta i portu...");
                using (var tcpClient = new System.Net.Sockets.TcpClient())
                {
                    try
                    {
                        var connectResult = tcpClient.BeginConnect(connStringBuilder.Host, (int)connStringBuilder.Port, null, null);
                        var success = connectResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                        if (!success)
                        {
                            throw new Exception("Timeout podczas próby połączenia TCP");
                        }
                        tcpClient.EndConnect(connectResult);
                        result.AppendLine("✓ Połączenie TCP działa");
                    }
                    catch (Exception ex)
                    {
                        result.AppendLine($"✗ Błąd TCP: {ex.Message}");
                        result.AppendLine("  To wskazuje na problem sieciowy, firewall lub że serwer bazy nie działa");
                        result.AppendLine("  Sprawdź czy:");
                        result.AppendLine("  - Poprawnie ustawiono adres hosta (dla Docker użyj 'host.docker.internal' lub nazwy serwisu)");
                        result.AppendLine("  - Port 5432 jest dostępny i otwarty");
                        result.AppendLine("  - Kontener z bazą danych jest uruchomiony");
                    }
                }
                result.AppendLine();

                result.AppendLine("Próba połączenia do bazy PostgreSQL...");
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    result.AppendLine("✓ Połączenie do PostgreSQL ustanowione");

                    result.AppendLine("Sprawdzanie wersji PostgreSQL...");
                    using (var cmd = new NpgsqlCommand("SELECT version()", conn))
                    {
                        var version = cmd.ExecuteScalar()?.ToString();
                        result.AppendLine($"✓ Wersja PostgreSQL: {version}");
                    }

                    result.AppendLine("Sprawdzanie uprawnień użytkownika...");
                    using (var cmd = new NpgsqlCommand("SELECT current_user, current_database(), has_database_privilege(current_user, current_database(), 'CONNECT')", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result.AppendLine($"✓ Zalogowany użytkownik: {reader[0]}");
                                result.AppendLine($"✓ Aktualna baza danych: {reader[1]}");
                                result.AppendLine($"✓ Uprawnienie CONNECT: {reader[2]}");
                            }
                        }
                    }

                    result.AppendLine("Sprawdzanie dostępnych schematów...");
                    using (var cmd = new NpgsqlCommand("SELECT schema_name FROM information_schema.schemata", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            StringBuilder schemas = new StringBuilder();
                            while (reader.Read())
                            {
                                schemas.Append(reader[0].ToString()).Append(", ");
                            }
                            if (schemas.Length > 0) schemas.Length -= 2; // usuń ostatnie ", "
                            result.AppendLine($"✓ Dostępne schematy: {schemas}");
                        }
                    }

                    result.AppendLine("Testowanie czy tabele istnieją...");
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT table_name 
                        FROM information_schema.tables 
                        WHERE table_schema = 'public'", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            StringBuilder tables = new StringBuilder();
                            bool tablesExist = false;
                            while (reader.Read())
                            {
                                tablesExist = true;
                                tables.Append(reader[0].ToString()).Append(", ");
                            }
                            if (tables.Length > 0) tables.Length -= 2;

                            if (tablesExist)
                            {
                                result.AppendLine($"✓ Znalezione tabele: {tables}");
                            }
                            else
                            {
                                result.AppendLine("✗ Nie znaleziono tabel w schemacie 'public'");
                            }
                        }
                    }
                }

                result.AppendLine();
                result.AppendLine("✅ PODSUMOWANIE: Wszystkie testy zakończone pomyślnie");
            }
            catch (Exception ex)
            {
                result.AppendLine($"✗ BŁĄD: {ex.Message}");
                if (ex.InnerException != null)
                {
                    result.AppendLine($"✗ INNER EXCEPTION: {ex.InnerException.Message}");
                }
                result.AppendLine();
                result.AppendLine("❌ PODSUMOWANIE: Wystąpił błąd podczas testowania połączenia");
            }

            return result.ToString();
        }
    }
}