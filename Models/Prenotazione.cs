using System;

namespace GestioneAuleStudio.Models
{
    // Rappresenta una singola prenotazione effettuata da uno studente
    public class Prenotazione
    {
        public Guid Id { get; private set; } // Identificatore univoco per facilitare modifica/cancellazione
        public string NomeStudente { get; set; }
        public Aula AulaPrenotata { get; set; }
        public DateTime Giorno { get; set; }
        public string FasciaOraria { get; set; } // Es. "09:00-11:00"
        public int PostiRichiesti { get; set; }

        public Prenotazione(string nomeStudente, Aula aula, DateTime giorno, string fasciaOraria, int postiRichiesti)
        {
            Id = Guid.NewGuid(); // Genera un ID automatico
            NomeStudente = nomeStudente;
            AulaPrenotata = aula;
            Giorno = giorno.Date; // Assicuriamoci di salvare solo la data, senza l'orario
            FasciaOraria = fasciaOraria;
            PostiRichiesti = postiRichiesti;
        }
    }
}