namespace Beyond_PruebaTecnica_ConsoleApp.Models
{
    public class Progression
    {
        public DateTime Date { get; set; }

        public decimal Percent { get; set; }

        public Progression(DateTime date, decimal percent)
        {
            Date = date;
            Percent = percent;
        }
    }
}
