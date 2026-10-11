using System;
using Core.Interfaces;

namespace Core.Models
{
    public class Monkey : BaseAnimal, IClimb
    {
        public Monkey(string name, int energy) : base(name, energy) { }

        public override string MakeSound() => $"{Name} chattering: Ooh-ooh-aah-aah!";

        public override string CrazyAction()
        {
            Energy = Math.Max(0, Energy - 10);
            return $"{Name} stole tourist's glasses and climbed up a palm tree! Energy decreased to {Energy}%.";
        }

        public string Climb() => $"{Name} is climbing up the vines with lightning speed!";
    }
}