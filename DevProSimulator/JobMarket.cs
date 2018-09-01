using System;
using System.Collections.Generic;
using System.Linq;

namespace DevProSimulator
{
    public class Candidate : Tickable {
        public Developer Developer { get;  }
        public int TicksOnMarket { get; private set; }

        public Candidate(Developer developer, int age)
        {
            Developer = developer;
            TicksOnMarket = age;
        }

        public string Name => Developer.Name;

        public void Tick()
        {
            TicksOnMarket++;
        }
    }

    public class JobMarket : Tickable
    {
        private Log Log { get; }
        private double CandidateAvailability = 0.1;
        private double MarketHotness = 5;
        public List<Candidate> Candidates = new List<Candidate>();
        
        public JobMarket(Log log)
        {
            Log = log;
        }

        public void AddNewCandidate()
        {
            var Candidate = new Candidate(new Developer(), 0);
            Candidates.Add(Candidate);
            Log.Add($"New developer on the market {Candidate.Name}");
        }
        
        public void Tick() {
            if (new Random().NextDouble() < CandidateAvailability)
                AddNewCandidate();
            Candidates.ForEach(c => c.Tick());
            Candidates = Candidates.Where(c =>
            {
                if (c.TicksOnMarket > MarketHotness)
                {
                    Log.Add($"{c.Name} was hired elsewhere.");
                    return false;
                }
                else
                {
                    return true;
                }
            }) as List<Candidate>;
        }

        public void Hire(Candidate candidate)
        {
            Candidates.Remove(candidate);
        }
    }
}
