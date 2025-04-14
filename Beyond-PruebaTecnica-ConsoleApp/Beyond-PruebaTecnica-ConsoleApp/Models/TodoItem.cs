using Beyond_PruebaTecnica_ConsoleApp.Exceptions;
using System.Text;

namespace Beyond_PruebaTecnica_ConsoleApp.Models
{
    public class TodoItem
    {
        public int Id { get; }
        public string Title { get; }
        public string Description { get; private set; }
        public string Category { get; }
        public List<Progression> Progressions { get; }
        public bool IsCompleted => TotalProgress >= 100;

        public decimal TotalProgress => Progressions.Sum(p => p.Percent);

        public TodoItem(int id, string title, string description, string category)
        {
            Id = id;
            Title = title;
            Description = description;
            Category = category;
            Progressions = new List<Progression>();
        }

        public void UpdateDescription(string newDescription)
        {
            if (TotalProgress > 50)
                throw new UpdateNotAllowedException();

            Description = newDescription;
        }

        public void AddProgression(DateTime date, decimal percent)
        {
            if (percent <= 0 || percent >= 100)
                throw new InvalidProgressException();

            if (Progressions.Count != 0 && date <= Progressions.Max(p => p.Date))
                throw new InvalidProgressDateException();

            if (TotalProgress + percent > 100)
                throw new ProgressOverflowException();

            Progressions.Add(new Progression(date, percent));
        }

        public string Print()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Id}) {Title} - {Description} ({Category}) Completed:{IsCompleted}");

            decimal acumulado = 0;
            foreach (var p in Progressions.OrderBy(p => p.Date))
            {
                acumulado += p.Percent;
                int barLength = (int)(acumulado / 2);
                string progressBar = new string('O', barLength).PadRight(50);
                sb.AppendLine($"{p.Date.ToShortDateString()} - {acumulado,4}% |{progressBar}|");
            }

            return sb.ToString();
        }
    }
}
