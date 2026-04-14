using System;
using GestioneAuleStudio.Services;
using GestioneAuleStudio.Models;

namespace GestioneAuleStudio
{
    class Program
    {
        static SistemaGestione _sistema = new SistemaGestione();

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
                Console.WriteLine("1. Accesso Studente");
                Console.WriteLine("2. Accesso Amministratore");
                Console.WriteLine("3. Esci");
                Console.Write("Scegli un'opzione: ");

                string scelta = Console.ReadLine();
                switch (scelta)
                {
                    case "1": MenuStudente(); break;
                    case "2": MenuAmministratore(); break;
                    case "3": esci = true; break;
                }
            }
        }

        // ==============================
        //        MENU STUDENTE
        // ==============================
        static void MenuStudente()
        {
            Console.Write("\nInserisci il tuo nome: ");
            string nomeStudente = Console.ReadLine();

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
                        Console.Write("Giorno (YYYY-MM-DD): "); DateTime giorno = DateTime.Parse(Console.ReadLine());
                        Console.Write("Fascia Oraria (es. 09-11): "); string fascia = Console.ReadLine();

                        // 3. Mostriamo l'elenco delle aule con la disponibilità in tempo reale
                        Console.WriteLine($"\n--- Aule disponibili per il {giorno:dd/MM/yyyy} ({fascia}) ---");
                        foreach (var a in auleDisponibili)
                        {
                            int postiLiberi = _sistema.CalcolaPostiDisponibili(a, giorno, fascia);
                            Console.WriteLine($"- {a.Nome} | Capienza max: {a.CapienzaMassima} | Posti liberi: {postiLiberi}");
                        }

                        // 4. Ora l'utente può scegliere l'aula e i posti in modo consapevole
                        Console.WriteLine("\n---------------------------------------------------");
                        Console.Write("Digita il Nome dell'Aula scelta: "); string aula = Console.ReadLine();
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
                            Console.Write("Inserisci l'ID della prenotazione da modificare: "); 
                            Guid idMod = Guid.Parse(Console.ReadLine());
                            Console.Write("Nuovo Nome Aula: "); string nAula = Console.ReadLine();
                            Console.Write("Nuovo Giorno (YYYY-MM-DD): "); DateTime nGiorno = DateTime.Parse(Console.ReadLine());
                            Console.Write("Nuova Fascia Oraria: "); string nFascia = Console.ReadLine();
                            Console.Write("Nuovi Posti: "); int nPosti = int.Parse(Console.ReadLine());
                            
                            _sistema.ModificaPrenotazione(idMod, nAula, nGiorno, nFascia, nPosti);
                            Console.WriteLine("Prenotazione modificata!");
                            break;

                        case "4":
                            Console.Write("Inserisci l'ID della prenotazione da cancellare: "); 
                            Guid idCanc = Guid.Parse(Console.ReadLine());
                            if (_sistema.CancellaPrenotazione(idCanc)) Console.WriteLine("Cancellata!");
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
        static void MenuAmministratore()
        {
            bool indietro = false;
            while (!indietro)
            {
                Console.WriteLine("\n--- Menu Amministratore ---");
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
                            Console.Write("Nome aula da rimuovere (eliminerà anche le prenotazioni): "); 
                            _sistema.RimuoviAula(Console.ReadLine());
                            Console.WriteLine("Operazione completata.");
                            break;

                        case "4":
                            Console.Write("Nome aula: "); string aMod = Console.ReadLine();
                            Console.Write("Nuova capienza (maggiore dell'attuale): "); int nCapienza = int.Parse(Console.ReadLine());
                            if (_sistema.ModificaCapienzaAula(aMod, nCapienza))
                                Console.WriteLine("Capienza aumentata con successo.");
                            else
                                Console.WriteLine("Errore: la nuova capienza deve essere maggiore di quella attuale.");
                            break;

                        case "5":
                            Console.Write("Inserisci l'ID della prenotazione da cancellare: "); 
                            Guid idAdminCanc = Guid.Parse(Console.ReadLine());
                            _sistema.CancellaPrenotazione(idAdminCanc);
                            Console.WriteLine("Cancellata.");
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