namespace Core.Models
{
    public class Dragon : BaseAnimal
    {
        public int FirePower { get; set; } = 100;

        public Dragon(string name, int energy = 100) : base(name, energy)
        {
        }

        public override string CrazyAction()
        {
            FirePower += 20;
            return $"{Name} breathes fierce fire! FirePower: {FirePower}";
        }
    }
}