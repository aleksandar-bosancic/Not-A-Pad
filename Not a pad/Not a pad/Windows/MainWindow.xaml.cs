using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Not_a_pad.Util;

namespace Not_a_pad.Windows
{
    public partial class MainWindow
    {
        //Must be public or it wont bind to window properly for some reason.
        public ObservableCollection<NoteWindow> ListOfNotes { get; set; }

        public MainWindow()
        {
            ObservableCollection<NoteWindow> deserialized = SerializationUtil.Deserialize();
            ListOfNotes = deserialized ?? new ObservableCollection<NoteWindow>();
            InitializeComponent();
            DataContext = this;
        }

        private void New_Note(object sender, RoutedEventArgs e)
        {
            if (ListOfNotes.Count > 20)
            {
                MessageBox.Show("Maximum number of notes reached", "Limit alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ListBox.SelectedItem = null;
            NoteWindow noteWindow = new();
            noteWindow.PropertyChanged += NoteChanged;
            ListOfNotes.Add(noteWindow);
            noteWindow.Show();
        }

        private void PinButton_OnClick(object sender, RoutedEventArgs e)
        {
            NoteWindow selectedNote = (NoteWindow)ListBox.SelectedItem;
            if (selectedNote.IsVisible)
            {
                selectedNote.Hide();
            }
            else
            {
                if (selectedNote.IsLoaded)
                {
                    selectedNote.Show();
                }
                else if (selectedNote.isClosed)
                {
                    NoteWindow replaceNote = new(selectedNote.Text, selectedNote.BrushValue);
                    ListOfNotes.Remove(selectedNote);
                    ListOfNotes.Add(replaceNote);
                    replaceNote.Show();
                }
            }
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            NoteWindow selectedNote = (NoteWindow)ListBox.SelectedItem;
            selectedNote.Hide();
            ListOfNotes.Remove(selectedNote);
        }

        private void ColorCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NoteWindow selectedNote = (NoteWindow)ListBox.SelectedItem;
            if (sender is not ComboBox combo || selectedNote == null) return;
            SolidColorBrush brush = combo.SelectedItem as SolidColorBrush;
            selectedNote.Brush = brush;
            selectedNote.SetNoteColor(brush);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            SerializationUtil.Serialize(ListOfNotes);
            foreach (NoteWindow noteWindow in ListOfNotes)
            {
                noteWindow.Close();
            }
        }

        private void ListBox_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NoteWindow note = (NoteWindow)ListBox.SelectedItem;
            note?.Focus();
        }

        private void NoteChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case null:
                    return;
                case "Text":
                    break;
            }
        }
    }
}
