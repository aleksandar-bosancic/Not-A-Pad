using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Not_a_pad.Windows;

namespace Not_a_pad.Util
{
    public static class SerializationUtil
    {
        public static void Serialize(ObservableCollection<NoteWindow> ListOfNotes)
        {
            List<NoteModel.NoteModel> noteModels = new List<NoteModel.NoteModel>();
            Stream outStream;
            if (File.Exists("data.xml"))
            {
                File.Delete("data.xml");
                outStream = File.Open("data.xml", FileMode.Create);
            }
            else
            {
                outStream = File.Open("data.xml", FileMode.Create);
            }
            foreach (NoteWindow note in ListOfNotes)
            {
                noteModels.Add(new NoteModel.NoteModel(note.Text, note.BrushValue));
            }
            XmlSerializer serializer = new(typeof(NoteModel.NoteModel[]));
            serializer.Serialize(outStream, noteModels.ToArray());
            outStream.Close();
        }

        public static ObservableCollection<NoteWindow> Deserialize()
        {
            ObservableCollection<NoteWindow> ListOfNotes = new ObservableCollection<NoteWindow>();
            if (!File.Exists("data.xml"))
            {
                return null;
            }
            Stream inStream = File.Open("data.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(NoteModel.NoteModel[]));
            var serializedNoteModels = (NoteModel.NoteModel[])serializer.Deserialize(inStream);
            if (serializedNoteModels != null)
            {
                foreach (NoteModel.NoteModel noteModel in serializedNoteModels)
                {
                    ListOfNotes.Add(new NoteWindow(noteModel.Text, noteModel.Color));
                }
            }
            foreach (var note in ListOfNotes)
            {
                note.Show();
            }
            return ListOfNotes;
        }
        
    }
}