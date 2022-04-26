using System;

namespace Not_a_pad.NoteModel
{
    [Serializable]
    public class NoteModel
    {
        public string Text { get; set; }
        public string Color { get; set; }

        public NoteModel()
        {
            Text = "";
            Color = "#feff9c";
        }

        public NoteModel(string text, string color)
        {
            Text = text;
            Color = color;
        }
    }
}