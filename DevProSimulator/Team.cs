using System.Collections.Generic;
using System.Linq;

namespace DevProSimulator
{
    public class Team : Tickable
    {
        private readonly List<Developer> _members = new List<Developer>();

        public int Count => _members.Count;
        
        public void Hire(Developer developer)
        {
            _members.Add(developer);
        }

        public double TotalPotential => _members.Sum(d => d.Capability);

        public double Distractibility => _members.Sum(d => d.Distractible) * TeamSettings.DistractibilityFactor * Count;

        public double TotalActual => _members.Sum(d => d.AdjustedCapability) - Distractibility;

        public double CycleCost => _members.Sum(d => d.Salary);

        public void Tick()
        {
            _members.ForEach(d => d.Tick());
        }
    }
}