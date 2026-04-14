namespace GestioneAuleStudio.Models
{
    // Rappresenta un amministratore (estende Utente)
    public class Amministratore : Utente
    {
        public Amministratore(string nome, string password) : base(nome, password)
        {
        }
    }
}
