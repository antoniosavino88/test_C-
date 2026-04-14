using System;
using System.Collections.Generic;
using System.Linq;
using GestioneAuleStudio.Models;

namespace GestioneAuleStudio.Services
{
    public class SistemaAutenticazione
    {
        // Liste che fungono da "database" temporaneo in memoria
        private List<Studente> _studenti = new List<Studente>();
        private List<Amministratore> _amministratori = new List<Amministratore>();

        public SistemaAutenticazione()
        {
            // Aggiungiamo un amministratore di default per testare
            _amministratori.Add(new Amministratore("admin", "admin123"));
        }

        // ========== METODI PER GLI STUDENTI ==========

        public bool RegistraStudente(string nome, string password)
        {
            // Controlla se lo studente esiste già
            if (_studenti.Any(s => s.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Uno studente con questo nome è già registrato.");
            }

            // Controlla se il nome non è vuoto
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Nome e password non possono essere vuoti.");
            }

            _studenti.Add(new Studente(nome, password));
            return true;
        }

        public Studente LoginStudente(string nome, string password)
        {
            var studente = _studenti.FirstOrDefault(s => s.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (studente == null)
            {
                throw new Exception("Studente non trovato.");
            }

            if (!studente.VerificaPassword(password))
            {
                throw new Exception("Password errata.");
            }

            return studente;
        }

        // ========== METODI PER GLI AMMINISTRATORI ==========

        public bool RegistraAmministratore(string nome, string password)
        {
            // Controlla se l'amministratore esiste già
            if (_amministratori.Any(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Un amministratore con questo nome è già registrato.");
            }

            // Controlla se il nome non è vuoto
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Nome e password non possono essere vuoti.");
            }

            _amministratori.Add(new Amministratore(nome, password));
            return true;
        }

        public Amministratore LoginAmministratore(string nome, string password)
        {
            var amministratore = _amministratori.FirstOrDefault(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (amministratore == null)
            {
                throw new Exception("Amministratore non trovato.");
            }

            if (!amministratore.VerificaPassword(password))
            {
                throw new Exception("Password errata.");
            }

            return amministratore;
        }
    }
}
