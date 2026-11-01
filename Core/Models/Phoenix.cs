namespace Core.Models
{
    public class Phoenix : BaseAnimal
    {
        public int ReincarnationCount { get; set; } = 1;

        public Phoenix(string name, int energy = 100) : base(name, energy)
        {
        }

        public override string CrazyAction()
        {
            ReincarnationCount++;
            return $"{Name} shines brightly! Reincarnations: {ReincarnationCount}";
        }
    }
}