namespace ModelLayer.Entities
{
    public class Label
    {
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<NoteLabel> NoteLabels { get; set; }
            = new List<NoteLabel>();
    }
}