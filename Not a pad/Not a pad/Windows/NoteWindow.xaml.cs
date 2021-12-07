using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Not_a_pad.Annotations;

namespace Not_a_pad.Windows
{
    [Serializable]
    public partial class NoteWindow : INotifyPropertyChanged
    {
        public string Text { get; set; }
        public string Label { get; set; }

        private SolidColorBrush _brush;

        public SolidColorBrush Brush
        {
            get => _brush;

            set
            {
                if (value == _brush) return;
                _brush = value;
                OnPropertyChanged();
            }
        }

        public NoteWindow()
        {
            InitializeComponent();
            DataContext = this;
            Text = "";
            Label = "";
            Brush = (SolidColorBrush)new BrushConverter().ConvertFrom("#feff9c");
            SetNoteColor(Brush);
        }

        public NoteWindow(string label)
        {
            InitializeComponent();
            DataContext = this;
            Label = label;
            Brush = (SolidColorBrush) new BrushConverter().ConvertFrom("#feff9c");
            SetNoteColor(Brush);
        }

        public NoteWindow(string label, string text, SolidColorBrush brush)
        {
            InitializeComponent();
            DataContext = this;
            Label = label;
            Text = text;
            Brush = brush;
            SetNoteColor(Brush);
        }

        public void SetNoteColor(SolidColorBrush brush)
        {
            Brush = brush;
            OnPropertyChanged();
            GradientStop colorStop = new()
            {
                Color = brush.Color,
                Offset = 0.80
            };
            GradientStop lightStop = new()
            {
                Color = Colors.White,
                Offset = 0.1
            };
            LinearGradientBrush linearGradient = new();
            linearGradient.GradientStops.Add(colorStop);
            linearGradient.GradientStops.Add(lightStop);
            this.NoteTextBox.Background = linearGradient;
            this.NoteLabel.Background = brush;
        }

        private void NoteLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void NoteTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void NoteLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clickedLabel = sender as Label;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NoteWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void NoteTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = new TextRange(NoteTextBox.Document.ContentStart, NoteTextBox.Document.ContentEnd).Text;
        }

        private void ButtonB_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
