using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    DispatcherTimer timer = new DispatcherTimer();
    int tenthsOfSecondsElapsed;
    int matchesFound;

    public MainWindow()
    {
        InitializeComponent();

        timer.Interval = TimeSpan.FromSeconds(.1);
        timer.Tick += Timer_Tick;

        //SetUpGame();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        tenthsOfSecondsElapsed--;

        timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");

        if (matchesFound == 8)
        {
            timer.Stop();
            timeTextBlock.Text = timeTextBlock.Text + " - Win! Play again?";
        } 
        else if (tenthsOfSecondsElapsed <= 0)
        {
            timer.Stop();
            timeTextBlock.Text = "0.0s - Timed Out! Play again?";
        }
    }

    private static List<string> GetAnimalEmojiList()
    {
        List<string> animalEmojiForTheGame = new List<string>();

        List<string> animalEmoji = new List<string>()
        {
            "🐙", "🐡", "🐘", "🐳", "🐪", "🦕", "🦘", "🦔", "🦇", "🦉", "🦒", "🐅", "🦏", "🦓", "🦧", "🐩"
        };

        Random random = new Random();

        for (int i = 0; i < 8; i++)
        {
            int index = random.Next(animalEmoji.Count);

            animalEmojiForTheGame.Add(animalEmoji[index]);
            animalEmojiForTheGame.Add(animalEmoji[index]);

            animalEmoji.RemoveAt(index);
        }

        return animalEmojiForTheGame;
    }

    private void SetUpGame()
    {
        List<string> animalEmoji = GetAnimalEmojiList();

        Random random = new Random();

        foreach(TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
        {
            if (textBlock.Name != "timeTextBlock")
            {
                textBlock.Visibility = Visibility.Visible;
                int index = random.Next(animalEmoji.Count);
                textBlock.Text = animalEmoji[index];
                animalEmoji.RemoveAt(index); 
            }
        }

        timer.Start();
        tenthsOfSecondsElapsed = 200;
        matchesFound = 0;
    }
    
    TextBlock lastTextBlockClicked;
    bool findingMatch = false;

    private void TextBlock_MouseDown(object? sender, MouseButtonEventArgs e)
    {
        if (tenthsOfSecondsElapsed <= 0) return;

        TextBlock? textBlock = sender as TextBlock;

        if (findingMatch == false)
        {
            textBlock.Visibility = Visibility.Hidden;
            lastTextBlockClicked = textBlock;
            findingMatch = true;
        } 
        else if (textBlock.Text == lastTextBlockClicked.Text)
        {
            matchesFound++;
            textBlock.Visibility = Visibility.Hidden;
            findingMatch = false;
        } 
        else
        {
            lastTextBlockClicked.Visibility = Visibility.Visible;
            findingMatch = false;
        }
    }

    private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (matchesFound == 8 || tenthsOfSecondsElapsed <= 0)
        {
            SetUpGame();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        mainInterface.Visibility = Visibility.Collapsed;
        mainGrid.Visibility = Visibility.Visible;
        menuGame.Visibility = Visibility.Visible;
        SetUpGame();
    }

    private void BackToMenu_Click(object sender, RoutedEventArgs e)
    {
        mainGrid.Visibility = Visibility.Collapsed;
        menuGame.Visibility = Visibility.Collapsed;
        mainInterface.Visibility = Visibility.Visible;
    }

    private void RestartGame_Click(object sender, RoutedEventArgs e)
    {
        SetUpGame();
    }
}