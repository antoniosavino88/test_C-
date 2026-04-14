# Sistema di Prenotazione Aule Studio - Antonio Savino

Un'applicazione C# console per gestire la prenotazione e l'amministrazione di aule studio universitarie.

## Funzionalità Principali

### Autenticazione
- **Registrazione e Login** per Studenti
- **Registrazione e Login** per Amministratori
- Credenziali basate su Nome e Password
- Amministratore di default: `admin` / `admin123`

### Menu Studente
Gli studenti autenticati possono:

1. **Prenota un'aula**
   - Selezionare la data (formato DD-MM-YYYY)
   - Scegliere la fascia oraria da una lista (8-9, 9-10, ..., 19-20)
   - Visualizzare le aule disponibili con i posti liberi
   - Selezionare l'aula per numero
   - Specificare il numero di posti da prenotare

2. **Visualizza le tue prenotazioni**
   - Elenco di tutte le prenotazioni effettuate dallo studente
   - Mostra: Aula, data, ora, numero di posti

3. **Modifica una prenotazione**
   - Selezionare la prenotazione da modificare da una lista
   - Cambiare aula (da lista numerata)
   - Cambiare data
   - Cambiare fascia oraria (da lista numerata)
   - Cambiare numero di posti

4. **Cancella una prenotazione**
   - Selezionare la prenotazione da cancellare da una lista
   - Conferma della cancellazione

### Menu Amministratore
Gli amministratori autenticati possono:

1. **Visualizza tutte le prenotazioni**
   - Elenco di tutte le prenotazioni nel sistema
   - Mostra: Studente, Aula, data, ora, posti prenotati, posti liberi

2. **Aggiungi Aula**
   - Inserire il nome dell'aula
   - Specificare la capienza massima

3. **Rimuovi Aula**
   - Selezionare l'aula da rimuovere da una lista numerata
   - Le prenotazioni associate verrà automaticamente eliminate

4. **Aumenta Capienza Aula**
   - Selezionare l'aula da una lista numerata
   - Specificare la nuova capienza (deve essere maggiore di quella attuale)

5. **Elimina una prenotazione**
   - Selezionare la prenotazione da eliminare da una lista numerata
   - Conferma dell'eliminazione

## Fasce Orarie Disponibili
- 8-9
- 9-10
- 10-11
- 11-12
- 12-13
- 13-14
- 14-15
- 15-16
- 16-17
- 17-18
- 18-19
- 19-20

## Formato Date
Tutte le date devono essere inserite nel formato **DD-MM-YYYY** (es. 14-04-2026)

## Struttura del Progetto

```
GestioneAuleStudio/
├── Models/
│   ├── Aula.cs              # Rappresenta un'aula studio
│   ├── Prenotazione.cs      # Rappresenta una prenotazione
│   ├── Utente.cs            # Classe base per Studente e Amministratore
│   ├── Studente.cs          # Rappresenta uno studente
│   └── Amministratore.cs    # Rappresenta un amministratore
├── Services/
│   ├── SistemaGestione.cs        # Logica di gestione prenotazioni e aule
│   └── SistemaAutenticazione.cs  # Logica di login e registrazione
├── Program.cs                    # Menu principale e interfaccia utente
└── README.md                     # Questo file
```

## Come Avviare

1. Compilare il progetto:
   ```
   dotnet build
   ```

2. Eseguire l'applicazione:
   ```
   dotnet run
   ```

3. Selezionare l'area (Studente/Amministratore) dal menu principale

4. Se è la prima volta, iscriversi o registrare un nuovo utente

## Note Importanti

- I dati vengono mantenuiti in memoria durante l'esecuzione (non persistenti)
- La password è memorizzata in chiaro (solo a scopo didattico)
- Nomi e aule sono case-insensitive (maiuscole/minuscole non importano)
- Il numero di posti disponibili viene calcolato in tempo reale
- Non è possibile prenotare più posti di quelli disponibili
