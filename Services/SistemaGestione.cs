using System;
using System.Collections.Generic;
using System.Linq;
using GestioneAuleStudio.Models;

namespace GestioneAuleStudio.Services
{
    public class SistemaGestione
    {
        // Liste che fungono da "database" temporaneo in memoria
        private List<Aula> _aule = new List<Aula>();
        private List<Prenotazione> _prenotazioni = new List<Prenotazione>();

        // --- FUNZIONALITÀ AMMINISTRATORE ---

        public void AggiungiAula(string nome, int capienza)
        {
            if (_aule.Any(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Esiste già un'aula con questo nome.");
            
            _aule.Add(new Aula(nome, capienza));
        }

        public void RimuoviAula(string nome)
        {
            var aula = _aule.FirstOrDefault(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (aula != null)
            {
                _aule.Remove(aula);
                // Rimuove anche tutte le prenotazioni collegate a quell'aula
                _prenotazioni.RemoveAll(p => p.AulaPrenotata.Nome == nome);
            }
        }

        public bool ModificaCapienzaAula(string nome, int nuovaCapienza)
        {
            var aula = _aule.FirstOrDefault(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (aula != null)
            {
                return aula.AumentaCapienza(nuovaCapienza);
            }
            return false;
        }

        public List<Prenotazione> OttieniTuttePrenotazioni() => _prenotazioni;

        // --- FUNZIONALITÀ STUDENTE & LOGICA CONDIVISA ---

        public List<Aula> OttieniAule() => _aule;

        // Calcola quanti posti sono rimasti in una specifica aula, giorno e fascia oraria
        public int CalcolaPostiDisponibili(Aula aula, DateTime giorno, string fasciaOraria)
        {
            int postiOccupati = _prenotazioni
                .Where(p => p.AulaPrenotata.Nome == aula.Nome && p.Giorno == giorno.Date && p.FasciaOraria == fasciaOraria)
                .Sum(p => p.PostiRichiesti);

            return aula.CapienzaMassima - postiOccupati;
        }

        public Prenotazione EffettuaPrenotazione(string studente, string nomeAula, DateTime giorno, string fascia, int posti)
        {
            var aula = _aule.FirstOrDefault(a => a.Nome.Equals(nomeAula, StringComparison.OrdinalIgnoreCase));
            if (aula == null) throw new Exception("Aula non trovata.");

            int postiDisponibili = CalcolaPostiDisponibili(aula, giorno, fascia);
            if (posti > postiDisponibili)
            {
                throw new Exception($"Prenotazione rifiutata: l'aula ha solo {postiDisponibili} posti disponibili per questo turno.");
            }

            var nuovaPrenotazione = new Prenotazione(studente, aula, giorno, fascia, posti);
            _prenotazioni.Add(nuovaPrenotazione);
            return nuovaPrenotazione;
        }

        public List<Prenotazione> OttieniPrenotazioniStudente(string nomeStudente)
        {
            return _prenotazioni.Where(p => p.NomeStudente.Equals(nomeStudente, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool CancellaPrenotazione(Guid id)
        {
            var daCancellare = _prenotazioni.FirstOrDefault(p => p.Id == id);
            if (daCancellare != null)
            {
                _prenotazioni.Remove(daCancellare);
                return true;
            }
            return false;
        }

        public void ModificaPrenotazione(Guid id, string nuovoNomeAula, DateTime nuovoGiorno, string nuovaFascia, int nuoviPosti)
        {
            var prenotazioneEsistente = _prenotazioni.FirstOrDefault(p => p.Id == id);
            if (prenotazioneEsistente == null) throw new Exception("Prenotazione non trovata.");

            // Per modificare in sicurezza, rimuoviamo temporaneamente la vecchia prenotazione 
            // e proviamo ad inserire quella nuova. Se fallisce, ripristiniamo quella vecchia.
            _prenotazioni.Remove(prenotazioneEsistente);

            try
            {
                EffettuaPrenotazione(prenotazioneEsistente.NomeStudente, nuovoNomeAula, nuovoGiorno, nuovaFascia, nuoviPosti);
            }
            catch
            {
                // Ripristino (Rollback) in caso di fallimento (es. posti non disponibili)
                _prenotazioni.Add(prenotazioneEsistente);
                throw; // Rilancia l'eccezione al chiamante
            }
        }
    }
}