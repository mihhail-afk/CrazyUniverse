using System;
using Core.Interfaces;

namespace Core.Models
{
    public class Zebra : BaseAnimal, IClimb
    {
        public Zebra(string name, int energy)
            : base(name, energy)
        {
        }

        public override string MakeSound()
        {
            return $"{Name} neighs loudly!";
        }

        public string Climb()
        {
            return $"{Name} climbed onto a rock.";
        }

        public override string CrazyAction()
        {
            Energy = Math.Min(100, Energy + 15);

            return $"{Name} painted its stripes rainbow-colored and started dancing! Energy increased to {Energy}%.";
        }
    }
}

