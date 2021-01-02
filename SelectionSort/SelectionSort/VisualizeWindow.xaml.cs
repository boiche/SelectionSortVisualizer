using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SelectionSort
{
    /// <summary>
    /// Interaction logic for VisualizeWindow.xaml
    /// </summary>
    public partial class VisualizeWindow : Window
    {
        private bool isPaused = true;
        private bool isFinished = false;
        private int currentStepIdx = default;
        private int currentSelectedLabel = default;
        private int currentToCompareLabel = default;
        private readonly List<Action<Label, Label>> steps;
        private readonly List<Label> labels;

        public VisualizeWindow(int[] array)
        {                        
            InitializeComponent();
            steps = new List<Action<Label, Label>>();
            labels = new List<Label>();
            CreateLabels(array);
            Visualizer.InitializeGrid(visualiseGrid);            
            PopulateSteps();
            NormalizeLabels(array);
        }

        private void NormalizeLabels(int[] array)
        {
            for (int i = 0; i < array.Length; i++) 
                labels[i].Content = array[i];
        }

        private void CreateLabels(int[] array)
        {
            int left = 5;
            int top = 50;
            for (int i = 0; i < array.Length; i++)
            {
                Label label = new Label
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = array[i],
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    Width = 60,
                    Height = 30,
                    Margin = new Thickness(left, top, 0, 0)
                };
                
                visualiseGrid.Children.Add(label);

                left += 70;
                labels.Add(label);
            }
        }

        private void PopulateSteps()
        {
            int toSwapIdx = 0;
            int currentMin;
            for (int i = 0; i < labels.Count; i++)
            {
                toSwapIdx = i;
                currentMin = int.Parse(labels[i].Content.ToString());
                steps.Add(Visualizer.SelectComparisonLabel(labels[i]));
                steps.Add(Visualizer.MarkMinimum(null, labels[i]));
                for (int k = i + 1; k < labels.Count; k++)
                {
                    steps.Add(Visualizer.VisualizeComparison(labels[i], labels[k]));
                    if (currentMin > int.Parse(labels[k].Content.ToString()))
                    {
                        steps.Add(Visualizer.MarkMinimum(null, labels[k]));
                        currentMin = int.Parse(labels[k].Content.ToString());
                        toSwapIdx = k;
                    }                    
                }
                steps.Add(Visualizer.VisualizeSwap());
                SwapLabels(i, toSwapIdx);
            }
            steps.Add(Visualizer.Finish());
        }

        private void SwapLabels(int i, int toSwapIdx)
        {
            string c = labels[i].Content.ToString();
            labels[i].Content = labels[toSwapIdx].Content.ToString();
            labels[toSwapIdx].Content = c;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentStepIdx >= steps.Count)
            {
                isFinished = true;
                return;
            }
            
            steps[currentStepIdx].Invoke(labels[currentSelectedLabel], labels[currentToCompareLabel]);
            if (steps[currentStepIdx].Method.Name.Contains("VisualizeComparison") && !steps[currentStepIdx + 1].Method.Name.Contains("MarkMinimum")) currentToCompareLabel++;
            if (steps[currentStepIdx].Method.Name.Contains("MarkMinimum")) currentToCompareLabel++;
            if (currentToCompareLabel == labels.Count)
            {
                currentSelectedLabel++;
                if (currentSelectedLabel == labels.Count) currentSelectedLabel--;
                currentToCompareLabel = currentSelectedLabel;
            }
            currentStepIdx++;
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            nextButton.IsEnabled = false;
            if (playButton.Content.ToString() == "Play")
            {
                playButton.Content = "Pause";
                isPaused = false;
            }
            else
            {
                nextButton.IsEnabled = true;
                playButton.Content = "Play";
                isPaused = true;
            }
            
            while (!isFinished && !isPaused)
            {
                NextButton_Click(sender, e);
                await Task.Delay((int)speedSlider.Value);
            }            
        }
    }
}
