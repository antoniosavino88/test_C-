using System;
using GestioneAuleStudio.Services;
using GestioneAuleStudio.Models;

namespace GestioneAuleStudio
{
    class Program
    {
        static SistemaGestione _sistema = new SistemaGestione();
        static SistemaAutenticazione _autenticazione = new SistemaAutenticazione();
        static string[] _fasceOrarie = { "8-9", "9-10", "10-11", "11-12", "12-13", "13-14", "14-15", "15-16", "16-17", "17-18", "18-19", "19-20" };

        static void Main(string[] args)
        {
            // Aggiungiamo un paio di aule di default per testare subito il programma
            _sistema.AggiungiAula("Galileo", 50);
            _sistema.AggiungiAula("Newton", 20);

            bool esci = false;
            while (!esci)
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DI PRENOTAZIONE AULE STUDIO ===");
                Console.WriteLine("1. Area Studente");
                Console.WriteLine("2. Area Amministratore");
                Console.WriteLine("3. Esci");
                Console.Write("Scegli un'opzione: ");

                string scelta = Console.ReadLine();
                switch (scelta)
                {
                    case "1": AreaStudente(); break;
                    case "2": AreaAmministratore(); break;
                    case "3": esci = true; break;
                }
            }
        }

        // ==============================
        //      AREA STUDENTE
        // ==============================
        static void AreaStudente()
        {
            Console.Clear();
            Console.WriteLine("=== AREA STUDENTE ===");
            Console.WriteLine("1. Accedi");
            Console.WriteLine("2. Iscriviti");
            Console.WriteLine("3. Torna al menu principale");
            Console.Write("Scegli un'opzione: ");

            string scelta = Console.ReadLine();
            try
            {
                switch (scelta)
                {
                    case "1":
                        Console.Write("Nome utente: ");
                        string nomeLogin = Console.ReadLine();
                        Console.Write("Password: ");
                        string passwordLogin = Console.ReadLine();
                        
                        var studenteLoggato = _autenticazione.LoginStudente(nomeLogin, passwordLogin);
                        Console.WriteLine("Accesso effettuato con successo!");
                        Console.WriteLine("Premi un tasto per continuare...");
                        Console.ReadKey();
                        MenuStudente(studenteLoggato.Nome);
                        break;

                    case "2":
                        Console.Write("Nome utente: ");
                        string nomeRegistrazione = Console.ReadLine();
                        Console.Write("Password: ");
                        string passwordRegistrazione = Console.ReadLine();
                        
                        _autenticazione.RegistraStudente(nomeRegistrazione, passwordRegistrazione);
                        Console.WriteLine("Registrazione effettuata con successo! Ora puoi accedere.");
                        Console.WriteLine("Premi un tasto per continuare...");
                        Console.ReadKey();
                        break;

                    case "3":
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
                Console.WriteLine("Premi un tasto per continuare...");
                Console.ReadKey();
            }
        }

        // ==============================
        //      AREA AMMINISTRATORE
        // ==============================
        static void AreaAmministratore()
        {
            Console.Clear();
            Console.WriteLine("=== AREA AMMINISTRATORE ===");
            Console.WriteLine("1. Accedi");
            Console.WriteLine("2. Registra nuovo amministratore");
            Console.WriteLine("3. Torna al menu principale");
            Console.Write("Scegli un'opzione: ");

            string scelta = Console.ReadLine();
            try
            {
                switch (scelta)
                {
                    case "1":
                        Console.Write("Nome utente: ");
                        string nomeLogin = Console.ReadLine();
                        Console.Write("Password: ");
                        string passwordLogin = Console.ReadLine();
                        
                        var adminLoggato = _autenticazione.LoginAmministratore(nomeLogin, passwordLogin);
                        Console.WriteLine("Accesso effettuato con successo!");
                        Console.WriteLine("Premi un tasto per continuare...");
                        Console.ReadKey();
                        MenuAmministratore(adminLoggato.Nome);
                        break;

                    case "2":
                        Console.Write("Nome utente: ");
                        string nomeRegistrazione = Console.ReadLine();
                        Console.Write("Password: ");
                        string passwordRegistrazione = Console.ReadLine();
                        
                        _autenticazione.RegistraAmministratore(nomeRegistrazione, passwordRegistrazione);
                        Console.WriteLine("Amministratore registrato con successo!");
                        Console.WriteLine("Premi un tasto per continuare...");
                        Console.ReadKey();
                        break;

                    case "3":
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
                Console.WriteLine("Premi un tasto per continuare...");
                Console.ReadKey();
            }
        }

        // ==============================
        //        MENU STUDENTE
        // ==============================
        static void MenuStudente(string nomeStudente)
        {
            bool indietro = false;
            while (!indietro)
            {
                Console.WriteLine($"\n--- Menu Studente: {nomeStudente} ---");
                Console.WriteLine("1. Prenota un'aula");
                Console.WriteLine("2. Visualizza le tue prenotazioni");
                Console.WriteLine("3. Modifica una prenotazione");
                Console.WriteLine("4. Cancella una prenotazione");
                Console.WriteLine("5. Torna al menu principale");
                Console.Write("Scelta: ");

                string scelta = Console.ReadLine();
                try
                {
                    switch (scelta)
                    {
                        case "1":
                        // 1. Controllo preliminare: esistono aule nel sistema?
                        var auleDisponibili = _sistema.OttieniAule();
                        if (auleDisponibili.Count == 0)
                        {
                            Console.WriteLine("Errore: Al momento non ci sono aule registrate nel sistema. Contatta l'amministratore.");
                            break; // Interrompe questo caso e torna al menu studente
                        }

                        // 2. Chiediamo prima giorno e orario (servono per calcolare i posti liberi)
                        Console.WriteLine("\n--- Dettagli Turno ---");
                        Console.Write("Giorno (DD-MM-YYYY): "); DateTime giorno = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", null);
                        
                        Console.WriteLine("\n--- Fascie Orarie Disponibili ---");
                        for (int f = 0; f < _fasceOrarie.Length; f++)
                        {
                            Console.WriteLine($"{f + 1}. {_fasceOrarie[f]}");
                        }
                        Console.Write("Seleziona il numero della fascia oraria: ");
                        int sceltaFascia = int.Parse(Console.ReadLine()) - 1;
                        
                        if (sceltaFascia < 0 || sceltaFascia >= _fasceOrarie.Length)
                        {
                            Console.WriteLine("Errore: fascia oraria non valida.");
                            break;
                        }
                        
                        string fascia = _fasceOrarie[sceltaFascia];

                        // 3. Mostriamo l'elenco delle aule con la disponibilità in tempo reale
                        Console.WriteLine($"\n--- Aule disponibili per il {giorno:dd/MM/yyyy} ({fascia}) ---");
                        for (int i = 0; i < auleDisponibili.Count; i++)
                        {
                            var a = auleDisponibili[i];
                            int postiLiberi = _sistema.CalcolaPostiDisponibili(a, giorno, fascia);
                            Console.WriteLine($"{i + 1}. {a.Nome} | Capienza max: {a.CapienzaMassima} | Posti liberi: {postiLiberi}");
                        }

                        // 4. Ora l'utente può scegliere l'aula e i posti in modo consapevole
                        Console.WriteLine("\n---------------------------------------------------");
                        Console.Write("Scegli il numero dell'aula: "); 
                        int sceltaAula = int.Parse(Console.ReadLine()) - 1;
                        
                        if (sceltaAula < 0 || sceltaAula >= auleDisponibili.Count)
                        {
                            Console.WriteLine("Errore: numero aula non valido.");
                            break;
                        }
                        
                        string aula = auleDisponibili[sceltaAula].Nome;
                        Console.Write("Posti da prenotare: "); int posti = int.Parse(Console.ReadLine());
                        
                        // 5. Inviamo la richiesta al sistema
                        _sistema.EffettuaPrenotazione(nomeStudente, aula, giorno, fascia, posti);
                        Console.WriteLine("Prenotazione effettuata con successo!");
                        break;

                        case "2":
                            var miePrenotazioni = _sistema.OttieniPrenotazioniStudente(nomeStudente);
                            foreach (var p in miePrenotazioni)
                                Console.WriteLine($"[{p.Id}] Aula: {p.AulaPrenotata.Nome} | {p.Giorno:dd/MM/yyyy} | Ore: {p.FasciaOraria} | Posti: {p.PostiRichiesti}");
                            break;

                        case "3":
                            var prenotazioniDaModificare = _sistema.OttieniPrenotazioniStudente(nomeStudente);
                            if (prenotazioniDaModificare.Count == 0)
                            {
                                Console.WriteLine("Non hai nessuna prenotazione da modificare.");
                                break;
                            }

                            Console.WriteLine("\n--- Tue Prenotazioni ---");
                            for (int i = 0; i < prenotazioniDaModificare.Count; i++)
                            {
                                var p = prenotazioniDaModificare[i];
                                Console.WriteLine($"{i + 1}. Aula: {p.AulaPrenotata.Nome} | {p.Giorno:dd/MM/yyyy} | Ore: {p.FasciaOraria} | Posti: {p.PostiRichiesti}");
                            }

                            Console.Write("Seleziona il numero della prenotazione da modificare: ");
                            int sceltaPrenotazione = int.Parse(Console.ReadLine()) - 1;

                            if (sceltaPrenotazione < 0 || sceltaPrenotazione >= prenotazioniDaModificare.Count)
                            {
                                Console.WriteLine("Errore: numero prenotazione non valido.");
                                break;
                            }

                            Guid idMod = prenotazioniDaModificare[sceltaPrenotazione].Id;
                            
                            var aulePerModifica = _sistema.OttieniAule();
                            Console.WriteLine("\n--- Aule disponibili ---");
                            for (int i = 0; i < aulePerModifica.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {aulePerModifica[i].Nome} | Capienza max: {aulePerModifica[i].CapienzaMassima}");
                            }
                            Console.Write("Seleziona il numero della nuova aula: ");
                            int sceltaNuovaAula = int.Parse(Console.ReadLine()) - 1;

                            if (sceltaNuovaAula < 0 || sceltaNuovaAula >= aulePerModifica.Count)
                            {
                                Console.WriteLine("Errore: numero aula non valido.");
                                break;
                            }

                            string nAula = aulePerModifica[sceltaNuovaAula].Nome;
                            Console.Write("Nuovo Giorno (DD-MM-YYYY): "); DateTime nGiorno = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", null);
                            
                            Console.WriteLine("\n--- Fascie Orarie Disponibili ---");
                            for (int f = 0; f < _fasceOrarie.Length; f++)
                            {
                                Console.WriteLine($"{f + 1}. {_fasceOrarie[f]}");
                            }
                            Console.Write("Seleziona il numero della nuova fascia oraria: ");
                            int sceltaNuovaFascia = int.Parse(Console.ReadLine()) - 1;
                            
                            if (sceltaNuovaFascia < 0 || sceltaNuovaFascia >= _fasceOrarie.Length)
                            {
                                Console.WriteLine("Errore: fascia oraria non valida.");
                                break;
                            }
                            
                            string nFascia = _fasceOrarie[sceltaNuovaFascia];
                            Console.Write("Nuovi Posti: "); int nPosti = int.Parse(Console.ReadLine());
                            
                            _sistema.ModificaPrenotazione(idMod, nAula, nGiorno, nFascia, nPosti);
                            Console.WriteLine("Prenotazione modificata con successo!");
                            break;

                        case "4":
                            var prenotazioniDaCancellare = _sistema.OttieniPrenotazioniStudente(nomeStudente);
                            if (prenotazioniDaCancellare.Count == 0)
                            {
                                Console.WriteLine("Non hai nessuna prenotazione da cancellare.");
                                break;
                            }

                            Console.WriteLine("\n--- Tue Prenotazioni ---");
                            for (int i = 0; i < prenotazioniDaCancellare.Count; i++)
                            {
                                var p = prenotazioniDaCancellare[i];
                                Console.WriteLine($"{i + 1}. Aula: {p.AulaPrenotata.Nome} | {p.Giorno:dd/MM/yyyy} | Ore: {p.FasciaOraria} | Posti: {p.PostiRichiesti}");
                            }

                            Console.Write("Seleziona il numero della prenotazione da cancellare: ");
                            int sceltaCancellazione = int.Parse(Console.ReadLine()) - 1;

                            if (sceltaCancellazione < 0 || sceltaCancellazione >= prenotazioniDaCancellare.Count)
                            {
                                Console.WriteLine("Errore: numero prenotazione non valido.");
                                break;
                            }

                            Guid idCanc = prenotazioniDaCancellare[sceltaCancellazione].Id;
                            if (_sistema.CancellaPrenotazione(idCanc)) Console.WriteLine("Prenotazione cancellata con successo!");
                            else Console.WriteLine("Errore durante la cancellazione.");
                            break;

                        case "5": indietro = true; break;
                    }
                }
                catch (Exception ex) // Cattura errori (es. date malformate o posti insufficienti)
                {
                    Console.WriteLine($"ERRORE: {ex.Message}");
                }
            }
        }

        // ==============================
        //      MENU AMMINISTRATORE
        // ==============================
        static void MenuAmministratore(string nomeAdmin)
        {
            bool indietro = false;
            while (!indietro)
            {
                Console.WriteLine($"\n--- Menu Amministratore: {nomeAdmin} ---");
                Console.WriteLine("1. Visualizza tutte le prenotazioni");
                Console.WriteLine("2. Aggiungi Aula");
                Console.WriteLine("3. Rimuovi Aula");
                Console.WriteLine("4. Aumenta Capienza Aula");
                Console.WriteLine("5. Elimina una prenotazione");
                Console.WriteLine("6. Torna al menu principale");
                Console.Write("Scelta: ");

                string scelta = Console.ReadLine();
                try
                {
                    switch (scelta)
                    {
                        case "1":
                            var tutte = _sistema.OttieniTuttePrenotazioni();
                            foreach (var p in tutte)
                            {
                                int liberi = _sistema.CalcolaPostiDisponibili(p.AulaPrenotata, p.Giorno, p.FasciaOraria);
                                Console.WriteLine($"[{p.Id}] Studente: {p.NomeStudente} | Aula: {p.AulaPrenotata.Nome} | {p.Giorno:dd/MM/yyyy} | Ore: {p.FasciaOraria} | Prenotati: {p.PostiRichiesti} | Liberi: {liberi}");
                            }
                            break;
                            
                        case "2":
                            Console.Write("Nome nuova aula: "); string nAula = Console.ReadLine();
                            Console.Write("Capienza massima: "); int capienza = int.Parse(Console.ReadLine());
                            _sistema.AggiungiAula(nAula, capienza);
                            Console.WriteLine("Aula aggiunta!");
                            break;

                        case "3":
                            var aulePerRimozione = _sistema.OttieniAule();
                            if (aulePerRimozione.Count == 0)
                            {
                                Console.WriteLine("Non ci sono aule da rimuovere.");
                                break;
                            }

                            Console.WriteLine("\n--- Aule Disponibili ---");
                            for (int i = 0; i < aulePerRimozione.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {aulePerRimozione[i].Nome} | Capienza max: {aulePerRimozione[i].CapienzaMassima}");
                            }
                            Console.Write("Seleziona il numero dell'aula da rimuovere (eliminerà anche le prenotazioni): ");
                            int sceltaRimozioneAula = int.Parse(Console.ReadLine()) - 1;
                            
                            if (sceltaRimozioneAula < 0 || sceltaRimozioneAula >= aulePerRimozione.Count)
                            {
                                Console.WriteLine("Errore: numero aula non valido.");
                                break;
                            }
                            
                            _sistema.RimuoviAula(aulePerRimozione[sceltaRimozioneAula].Nome);
                            Console.WriteLine("Aula rimossa con successo.");
                            break;

                        case "4":
                            var aulePerCapienza = _sistema.OttieniAule();
                            if (aulePerCapienza.Count == 0)
                            {
                                Console.WriteLine("Non ci sono aule da modificare.");
                                break;
                            }

                            Console.WriteLine("\n--- Aule Disponibili ---");
                            for (int i = 0; i < aulePerCapienza.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {aulePerCapienza[i].Nome} | Capienza attuale: {aulePerCapienza[i].CapienzaMassima}");
                            }
                            Console.Write("Seleziona il numero dell'aula da modificare: ");
                            int sceltaCapienzaAula = int.Parse(Console.ReadLine()) - 1;
                            
                            if (sceltaCapienzaAula < 0 || sceltaCapienzaAula >= aulePerCapienza.Count)
                            {
                                Console.WriteLine("Errore: numero aula non valido.");
                                break;
                            }
                            
                            string aulaSelezionata = aulePerCapienza[sceltaCapienzaAula].Nome;
                            Console.Write("Nuova capienza (maggiore dell'attuale): "); int nuovaCapienza = int.Parse(Console.ReadLine());
                            if (_sistema.ModificaCapienzaAula(aulaSelezionata, nuovaCapienza))
                                Console.WriteLine("Capienza aumentata con successo.");
                            else
                                Console.WriteLine("Errore: la nuova capienza deve essere maggiore di quella attuale.");
                            break;

                        case "5":
                            var tuttePrenotazioni = _sistema.OttieniTuttePrenotazioni();
                            if (tuttePrenotazioni.Count == 0)
                            {
                                Console.WriteLine("Non ci sono prenotazioni da cancellare.");
                                break;
                            }

                            Console.WriteLine("\n--- Tutte le Prenotazioni ---");
                            for (int i = 0; i < tuttePrenotazioni.Count; i++)
                            {
                                var p = tuttePrenotazioni[i];
                                Console.WriteLine($"{i + 1}. Studente: {p.NomeStudente} | Aula: {p.AulaPrenotata.Nome} | {p.Giorno:dd/MM/yyyy} | Ore: {p.FasciaOraria} | Posti: {p.PostiRichiesti}");
                            }

                            Console.Write("Seleziona il numero della prenotazione da cancellare: ");
                            int sceltaCancellazioneAdmin = int.Parse(Console.ReadLine()) - 1;

                            if (sceltaCancellazioneAdmin < 0 || sceltaCancellazioneAdmin >= tuttePrenotazioni.Count)
                            {
                                Console.WriteLine("Errore: numero prenotazione non valido.");
                                break;
                            }

                            Guid idAdminCanc = tuttePrenotazioni[sceltaCancellazioneAdmin].Id;
                            if (_sistema.CancellaPrenotazione(idAdminCanc)) Console.WriteLine("Prenotazione cancellata con successo!");
                            else Console.WriteLine("Errore durante la cancellazione.");
                            break;

                        case "6": indietro = true; break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERRORE: {ex.Message}");
                }
            }
        }
    }
}