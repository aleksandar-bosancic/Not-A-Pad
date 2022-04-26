using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Not_a_pad.Annotations;

namespace Not_a_pad.Windows
{
    public partial class NoteWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string BrushValue { get; set; }
        
        public bool isClosed { get; set; }

        private string _label;

        public string Label
        {
            get => _label;
            set
            {
                if (value == _label) return;
                _label = value;
                OnPropertyChanged();
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                if(value == _text)return;
                _text = value;
                OnPropertyChanged();
            }

        }
        
        private SolidColorBrush _brush;
        
        public SolidColorBrush Brush
        {
            get => _brush;

            set
            {
                if (value == _brush) return;
                BrushValue = value.ToString();
                Color clr = (Color)ColorConverter.ConvertFromString(BrushValue)!;
                _brush = new SolidColorBrush(clr);
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

        public NoteWindow(string text, string brushValue)
        {
            InitializeComponent();
            DataContext = this;
            Text = text;
            BrushValue = brushValue;
            Brush = (SolidColorBrush)new BrushConverter().ConvertFrom(brushValue);
            SetNoteColor(Brush);
            NoteTextBox.AppendText(Text);
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
            NoteTextBox.Background = linearGradient;
            NoteLabel.Background = brush;
        }

        private void NoteLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void NoteTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_text));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (Text?.Length >= 10)
            {
                Label = Text.Substring(0, 9);
            }
            else
            {
                if (Text?.Length == 0)
                {
                    Label = String.Empty;
                }
                else
                {
                    Label = Text?.Substring(0, Text.Length - 1);
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void NoteWindow_OnClosing(object sender, CancelEventArgs e)
        {
            isClosed = true;
        }

        public void CloseNote()
        {
            NoteWindow_OnClosing(null, new CancelEventArgs());
        }

        private void NoteTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text?.Length > 500)
            {
                MessageBox.Show("Maximum number of characters reached", "Limit alert", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            
            Text = new TextRange(NoteTextBox.Document.ContentStart, NoteTextBox.Document.ContentEnd).Text;
        }

        private void ButtonB_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
