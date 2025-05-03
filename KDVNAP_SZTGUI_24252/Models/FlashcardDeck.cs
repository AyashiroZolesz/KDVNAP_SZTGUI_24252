using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDVNAP_SZTGUI_24252.Models
{
    public class FlashcardDeck
    {
        public string Name { get; set; }
        public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    }
}
