using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditDeckWindow.xaml
    /// </summary>
    public partial class EditDeckWindow : Window
    {
        public string EditedDeckName => deck.Name;

        private FlashcardDeck deck;
        private int selectedIndex = -1;
        public EditDeckWindow(FlashcardDeck deckToEdit)
        {
            InitializeComponent();
            deck = deckToEdit;
            DeckNameText.Content = deck.Name;
            FlashcardList.ItemsSource = deck.Flashcards;
        }

        private void FlashcardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FlashcardList.SelectedIndex >= 0)
            {
                selectedIndex = FlashcardList.SelectedIndex;
                var selected = deck.Flashcards[selectedIndex];
                QuestionBox.Text = selected.Question;
                AnswerBox.Text = selected.Answer;
            }
        }

        private void AddOrUpdateCard_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionBox.Text) || string.IsNullOrWhiteSpace(AnswerBox.Text))
                return;

            if (selectedIndex >= 0)
            {
                // Frissítés
                deck.Flashcards[selectedIndex].Question = QuestionBox.Text;
                deck.Flashcards[selectedIndex].Answer = AnswerBox.Text;
            }
            else
            {
                // Új kártya
                deck.Flashcards.Add(new Flashcard
                {
                    Question = QuestionBox.Text,
                    Answer = AnswerBox.Text
                });
            }

            RefreshList();
        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex >= 0)
            {
                deck.Flashcards.RemoveAt(selectedIndex);
                selectedIndex = -1;
                RefreshList();
                QuestionBox.Clear();
                AnswerBox.Clear();
            }
        }

        private void SaveDeck_Click(object sender, RoutedEventArgs e)
        {
            JsonStorage.SaveDeck(deck);
            MessageBox.Show("Pakli mentve!");
            DialogResult = true;
            Close();
        }

        private void RefreshList()
        {
            FlashcardList.ItemsSource = null;
            FlashcardList.ItemsSource = deck.Flashcards;
        }
    }
}
