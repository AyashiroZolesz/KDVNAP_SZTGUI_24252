using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KDVNAP_SZTGUI_24252.Models;

namespace KDVNAP_SZTGUI_24252.Data
{
    public static class JsonStorage
    {
        public static string DeckDirectory => "Decks";

        public static List<string> GetDeckFiles()
        {
            if (!Directory.Exists(DeckDirectory))
                Directory.CreateDirectory(DeckDirectory);

            var files = Directory.GetFiles(DeckDirectory, "*.json");
            var names = new List<string>();
            foreach (var file in files)
                names.Add(Path.GetFileNameWithoutExtension(file));

            return names;
        }

        public static FlashcardDeck LoadDeck(string deckName)
        {
            string path = Path.Combine(DeckDirectory, $"{deckName}.json");
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<FlashcardDeck>(json);
        }

        public static void SaveDeck(FlashcardDeck deck)
        {
            if (!Directory.Exists(DeckDirectory))
                Directory.CreateDirectory(DeckDirectory);

            string path = Path.Combine(DeckDirectory, $"{deck.Name}.json");

            string json = JsonSerializer.Serialize(deck, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
