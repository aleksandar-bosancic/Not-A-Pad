using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Not_a_pad.Annotations;

namespace Not_a_pad.Windows
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public ObservableCollection<NoteWindow> ListOfNotes { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ListOfNotes = new ObservableCollection<NoteWindow>();
            DataContext = this;
        }

        private void New_Note(object sender, RoutedEventArgs e)
        {
            this.ListBox.SelectedItem = null;
            int count = ListOfNotes.Count;
            NoteWindow noteWindow = new NoteWindow("Note " + count);
            ListOfNotes.Add(noteWindow);
            foreach (var not in ListOfNotes)
            {
                Trace.WriteLine(not.Label + " " + not.Brush.ToString());
            }
            noteWindow.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PinButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedNote = (NoteWindow)ListBox.SelectedItem;
            selectedNote?.Show();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedNote = (NoteWindow)ListBox.SelectedItem;
            selectedNote.Hide();
            ListOfNotes.Remove(selectedNote);
        }

        private void ColorCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedNote = (NoteWindow)ListBox.SelectedItem;
            if (sender is ComboBox combo && selectedNote != null)
            {
                var brush = combo.SelectedItem as SolidColorBrush;
                selectedNote.Brush = brush;
                selectedNote.SetNoteColor(brush);
            }
        }
    }
}
