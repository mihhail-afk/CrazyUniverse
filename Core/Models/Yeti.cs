namespace Core.Models
{
    public class Yeti : BaseAnimal
    {
        public int IceArmor { get; set; } = 50;

        public Yeti(string name, int energy = 100) : base(name, energy)
        {
        }

        public override string CrazyAction()
        {
            IceArmor += 10;
            return $"{Name} stomps heavily! IceArmor: {IceArmor}";
        }
    }
}