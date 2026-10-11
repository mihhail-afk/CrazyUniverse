using System;
using Core.Interfaces;

namespace Core.Models
{
    public class SuperLion : BaseAnimal, ISwimmable, IClimb
    {
        public SuperLion(string name, int energy) : base(name, energy) { }

        public override string MakeSound() => $"{Name} roars proudly: ROOOAR!";

        public override string CrazyAction()
        {
            Energy = Math.Max(0, Energy - 20);
            return $"{Name} jumped through a ring of fire while juggling coconuts! Energy decreased to {Energy}%.";
        }

        public string Swim() => $"{Name} swims across the river like an absolute king of beasts!";
        public string Climb() => $"{Name} scales a steep mountain cliff effortlessly!";
    }
}