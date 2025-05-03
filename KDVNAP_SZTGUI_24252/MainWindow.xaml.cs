using System.ComponentModel;
using System.IO;
using System.Windows;
using KDVNAP_SZTGUI_24252.Data;
using KDVNAP_SZTGUI_24252.Models;

namespace KDVNAP_SZTGUI_24252;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private FlashcardDeck currentDeck;
    private int currentIndex = 0;
    private bool showFront = true;

    public event PropertyChangedEventHandler PropertyChanged;
    public string CurrentFlashcardText =>
        currentDeck != null && currentDeck.Flashcards.Count > 0
            ? (showFront ? currentDeck.Flashcards[currentIndex].Question
                         : currentDeck.Flashcards[currentIndex].Answer)
            : "Nincs betöltve pakli";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        LoadDeckList();
    }
    private void LoadDeckList()
    {
        DeckList.ItemsSource = JsonStorage.GetDeckFiles();
    }

    private void DeckList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (DeckList.SelectedItem is string selectedName)
        {
            currentDeck = JsonStorage.LoadDeck(selectedName);
            currentIndex = 0;
            showFront = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFlashcardText)));
        }
    }

    private void ToggleCard_Click(object sender, RoutedEventArgs e)
    {
        showFront = !showFront;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFlashcardText)));
    }

    private void NewDeck_Click(object sender, RoutedEventArgs e)
    {
        var newDeckWindow = new NewDeckWindow();
        newDeckWindow.ShowDialog();
        LoadDeckList(); // újratöltjük a listát
    }

    private void PrevCard_Click(object sender, RoutedEventArgs e)
    {
        if (currentDeck != null && currentDeck.Flashcards.Count > 0)
        {
            currentIndex = (currentIndex - 1 + currentDeck.Flashcards.Count) % currentDeck.Flashcards.Count;
            showFront = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFlashcardText)));
        }
    }

    private void NextCard_Click(object sender, RoutedEventArgs e)
    {
        if (currentDeck != null && currentDeck.Flashcards.Count > 0)
        {
            currentIndex = (currentIndex + 1) % currentDeck.Flashcards.Count;
            showFront = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFlashcardText)));
        }
    }

    private void DeleteDeck_Click(object sender, RoutedEventArgs e)
    {
        if (DeckList.SelectedItem is string selectedDeck)
        {
            var result = MessageBox.Show($"Törölni akarod a(z) {selectedDeck} paklit?", "Megerősítés", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string path = System.IO.Path.Combine(JsonStorage.DeckDirectory, $"{selectedDeck}.json");
                if (File.Exists(path)) File.Delete(path);

                currentDeck = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFlashcardText)));
                LoadDeckList();
            }
        }
    }
    private void EditDeck_Click(object sender, RoutedEventArgs e)
    {
        if (DeckList.SelectedItem is string selectedDeck)
        {
            var loadedDeck = JsonStorage.LoadDeck(selectedDeck);
            if (loadedDeck != null)
            {
                var editWindow = new EditDeckWindow(loadedDeck);
                var result = editWindow.ShowDialog();

                if (result == true)
                {
                    // újratöltés, ha mentés történt
                    currentDeck = JsonStorage.LoadDeck(editWindow.EditedDeckName);
                    currentIndex = 0;
                    showFront = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFlashcardText)));
                }

                LoadDeckList(); // mindig frissítjük a listát
            }
        }
    }
}