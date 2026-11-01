using System;
using System.Threading;

namespace Core.Models
{
    public abstract class BaseAnimal
    {
        public Guid Id { get; } = Guid.NewGuid();

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty!");
                _name = value.Trim();
            }
        }

        private int _energy;
        public int Energy
        {
            get => _energy;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("Energy must be between 0 and 100!");
                _energy = value;
            }
        }

        protected BaseAnimal(string name, int energy)
        {
            Name = name;
            Energy = energy;
        }

        public virtual string MakeSound()
        {
            return $"{Name} makes a sound.";
        }

        public abstract string CrazyAction();

        public string SimulateActivity(int workload, CancellationToken cancellationToken)
        {
            for (int i = 1; i <= workload; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(300);
            }
            return $"{Name} successfully completed simulation ({workload} steps).";
        }

        public override string ToString() => $"{Name} (Energy: {Energy}%)";
    }
}