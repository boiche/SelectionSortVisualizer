using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SelectionSort
{
    public static class Visualizer
    {
        private static Grid grid;
        private delegate void VisComparison(Label first, Label second);
        private static Label commentLabel;
        private static Label startedLabel;
        private static Label currentComparisonLabel;
        private static Label currentMinimumLabel;

        private static readonly string FINAL_COMMENT = "Array sorted";
        private static readonly string MARK_MINIMUM_COMMENT = "Encountered smaller element than {0}, ({1} < {0}) so we mark the current minimum = {1}";
        private static readonly string COMPARISON_COMMENT = "Compare is {0} smaller than {1}";
        private static readonly string SELECT_COMPARISON_COMMENT = "We get the first unsorted element {0} and begin to itterate the remaining unsorted array";
        private static readonly string SWAP_COMMENT = "{0} is the smallest element from the usorted part of the array, so we swap it with the first unsorted element ({1})";

        public static void InitializeGrid(Grid currentGrid)
        {
            grid = currentGrid;
            commentLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 1000,
                Name = "stepComment"
            };
            grid.Children.Add(commentLabel);
        }

        public static Action<Label, Label> VisualizeComparison(Label first, Label second) => new Action<Label, Label>((first, second) =>
        {
            grid.Children.Remove(currentComparisonLabel);
            currentComparisonLabel.Margin = new Thickness(second.Margin.Left, currentComparisonLabel.Margin.Top, 0, 0);
            grid.Children.Add(currentComparisonLabel);

            commentLabel.Content = string.Format(COMPARISON_COMMENT, second.Content, currentMinimumLabel.Content);
        });

        public static Action<Label, Label> SelectComparisonLabel(Label toSelect) => new Action<Label, Label>((toSelect, empty) =>
        {
            Label selectedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = toSelect.Height,
                Width = toSelect.Width,
                BorderBrush = toSelect.BorderBrush,
                BorderThickness = toSelect.BorderThickness,
                VerticalAlignment = VerticalAlignment.Top,
                Content = toSelect.Content,
                Background = new SolidColorBrush(Colors.Orange),
                Margin = new Thickness(toSelect.Margin.Left, toSelect.Margin.Top + toSelect.Height + 10, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            toSelect.Background = new SolidColorBrush(Colors.LightSkyBlue);
            grid.Children.Add(selectedLabel);
            currentComparisonLabel = selectedLabel;
            startedLabel = toSelect;

            commentLabel.Content = string.Format(SELECT_COMPARISON_COMMENT, startedLabel.Content.ToString());
        });

        internal static Action<Label, Label> MarkMinimum(Label a, Label b) => new Action<Label, Label>((empty, label) =>
        {
            if (currentMinimumLabel != null)
            {
                currentMinimumLabel.Background = new SolidColorBrush(Colors.White);
                commentLabel.Content = string.Format(MARK_MINIMUM_COMMENT, currentMinimumLabel.Content.ToString(), label.Content.ToString());
            }

            label.Background = new SolidColorBrush(Colors.LightGreen);
            currentMinimumLabel = label;
            currentComparisonLabel.Content = label.Content.ToString();
        });

        internal static Action<Label, Label> VisualizeSwap() => new Action<Label, Label>((empty, empty1) =>
        {
            commentLabel.Content = string.Format(SWAP_COMMENT, currentMinimumLabel.Content, startedLabel.Content);

            grid.Children.Remove(currentComparisonLabel);
            if (currentMinimumLabel == null) return;
            string c = startedLabel.Content.ToString();
            startedLabel.Content = currentMinimumLabel.Content.ToString();
            currentMinimumLabel.Content = c;
            currentMinimumLabel.Background = new SolidColorBrush(Colors.White);
            startedLabel.Background = new SolidColorBrush(Colors.White);

            currentMinimumLabel = null;
            currentComparisonLabel = null;            
        });

        public static Action<Label, Label> Finish() => new Action<Label, Label>((empty, empty1) =>
        {
            foreach (var item in grid.Children)
            {
                if (item is Label a && a.Name == "") a.Background = new SolidColorBrush(Colors.LightGreen);
            }

            commentLabel.Content = FINAL_COMMENT;
        });
    }
}
