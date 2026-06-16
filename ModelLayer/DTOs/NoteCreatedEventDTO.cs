namespace ModelLayer.DTOs
{
    public class NoteCreatedEventDTO
    {
        public string EventName { get; set; } = string.Empty;

        public int NoteId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime OccurredOn { get; set; }
    }
}