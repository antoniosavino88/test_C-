namespace GestioneAuleStudio.Models
{
    // Rappresenta uno studente (estende Utente)
    public class Studente : Utente
    {
        public Studente(string nome, string password) : base(nome, password)
        {
        }
    }
}
