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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelectionSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] toSort;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int[] input = new int[random.Next(4, 11)];
            int number;
            for (int i = 0; i < input.Length; i++)
            {
                number = random.Next(1, 150);
                while (input.Contains(number)) number = random.Next(1, 150);
                input[i] = number;
            }
            inputTextBox.Text = string.Join(", ", input);
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                toSort = inputTextBox.Text.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            }
            catch (FormatException)
            {
                errorLabel.Content = "Invalid input. Numbers must be valid integers separated with \',\'";
                errorLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            var window = new VisualizeWindow(toSort);
            window.ShowDialog();
        }
    }
}
