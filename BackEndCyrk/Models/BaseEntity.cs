using System;

namespace BackEndCyrk.Models
{
    /// <summary>
    /// Klasa bazowa dla wszystkich modeli w systemie.
    /// Dostarcza wspólne właściwości używane we wszystkich tabelach.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Unikalny identyfikator encji.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data i czas utworzenia rekordu.
        /// Domyślnie ustawiona na bieżący czas UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}