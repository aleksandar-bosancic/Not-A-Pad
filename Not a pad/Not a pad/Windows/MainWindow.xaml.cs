using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using Not_a_pad.Annotations;

namespace Not_a_pad.Windows
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public ObservableCollection<NoteWindow> ListOfNotes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            //Deserialize();
            InitializeComponent();
            ListOfNotes = new ObservableCollection<NoteWindow>();
            DataContext = this;
        }

        private void Serialize()
        {
            NoteWindow[] serializationList = ListOfNotes.ToArray();
            Stream outStream = File.Open("data.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(NoteWindow[]));
            serializer.Serialize(outStream, serializationList);
            outStream.Close();
        }

        private void Deserialize()
        {
            if (File.Exists("data.xml"))
            {
                Stream inStream = File.Open("data.xml", FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(NoteWindow[]));
                var serializationList = (NoteWindow[])serializer.Deserialize(inStream);
                if (serializationList != null)
                    ListOfNotes = new ObservableCollection<NoteWindow>(serializationList);
                foreach (var note in ListOfNotes)
                {
                    note.Show();
                }
            }
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            //Serialize();
        }
    }
}
