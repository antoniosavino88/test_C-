namespace GestioneAuleStudio.Models
{
    // Classe base per rappresentare un utente (Studente o Amministratore)
    public abstract class Utente
    {
        public string Nome { get; set; }
        public string Password { get; private set; }

        public Utente(string nome, string password)
        {
            Nome = nome;
            Password = password;
        }

        // Metodo per verificare la password
        public bool VerificaPassword(string password)
        {
            return Password == password;
        }
    }
}
