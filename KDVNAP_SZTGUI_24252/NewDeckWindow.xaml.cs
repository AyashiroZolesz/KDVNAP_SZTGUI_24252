using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KDVNAP_SZTGUI_24252.Data;
using KDVNAP_SZTGUI_24252.Models;

namespace KDVNAP_SZTGUI_24252
{
    /// <summary>
    /// Interaction logic for NewDeckWindow.xaml
    /// </summary>
    public partial class NewDeckWindow : Window
    {

        private List<Flashcard> flashcards = new();
        public NewDeckWindow()
        {
            InitializeComponent();
        }

        private void AddFlashcard_Click(object sender, RoutedEventArgs e)
        {
            if (FlashcardList.SelectedItem is Flashcard selected)
            {
                selected.Question = QuestionBox.Text;
                selected.Answer = AnswerBox.Text;
            }
            else
            {
                flashcards.Add(new Flashcard
                {
                    Question = QuestionBox.Text,
                    Answer = AnswerBox.Text
                });
            }

            FlashcardList.ItemsSource = null;
            FlashcardList.ItemsSource = flashcards;
            QuestionBox.Clear();
            AnswerBox.Clear();
        }

        private void SaveDeck_Click(object sender, RoutedEventArgs e)
        {
            string deckName = DeckNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(deckName) || flashcards.Count == 0)
            {
                MessageBox.Show("Adj meg nevet és legalább egy kártyát!");
                return;
            }

            string path = System.IO.Path.Combine(JsonStorage.DeckDirectory, $"{deckName}.json");
            if (File.Exists(path))
            {
                MessageBox.Show("Már létezik ilyen nevű pakli! Válassz másik nevet.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var deck = new FlashcardDeck
            {
                Name = deckName,
                Flashcards = flashcards
            };

            JsonStorage.SaveDeck(deck);
            MessageBox.Show("Pakli mentve!");
            Close();
        }


        private void FlashcardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FlashcardList.SelectedItem is Flashcard selected)
            {
                QuestionBox.Text = selected.Question;
                AnswerBox.Text = selected.Answer;
            }
        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            if (FlashcardList.SelectedItem is Flashcard selected)
            {
                flashcards.Remove(selected);
                FlashcardList.Items.Refresh();
                QuestionBox.Clear();
                AnswerBox.Clear();
            }
        }
    }
}
