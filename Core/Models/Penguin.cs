using System;
using Core.Interfaces;

namespace Core.Models
{
    public class Penguin : BaseAnimal, ISwimmable
    {
        public Penguin(string name, int energy) : base(name, energy) { }

        public override string MakeSound() => $"{Name} squawks: Noink-noink!";

        public override string CrazyAction()
        {
            Energy = Math.Max(0, Energy - 5);
            return $"{Name} tried to fly off an iceberg, failed gracefully, and slid on its belly! Energy decreased to {Energy}%.";
        }

        public string Swim() => $"{Name} is diving deep into the icy ocean like a torpedo!";
    }
}