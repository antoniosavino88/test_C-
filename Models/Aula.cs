namespace GestioneAuleStudio.Models
{
    // Rappresenta un'aula studio fisica
    public class Aula
    {
        public string Nome { get; set; }
        public int CapienzaMassima { get; private set; } // Private set per impedire modifiche dirette esterne

        public Aula(string nome, int capienzaMassima)
        {
            Nome = nome;
            CapienzaMassima = capienzaMassima;
        }

        // Metodo per aumentare la capienza (richiesto dai requisiti)
        public bool AumentaCapienza(int nuovaCapienza)
        {
            if (nuovaCapienza > CapienzaMassima)
            {
                CapienzaMassima = nuovaCapienza;
                return true;
            }
            return false; // Rifiuta se la nuova capienza è minore o uguale
        }
    }
}