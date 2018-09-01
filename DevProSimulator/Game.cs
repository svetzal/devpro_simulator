using System;
using System.Collections.Generic;
using System.Linq;

namespace DevProSimulator
{
    public class Game : Tickable
    {
        private readonly Log _log;
        private readonly JobMarket _jobMarket;
        private readonly Team _team;
        private readonly StoryDeck _storyDeck;
        
        public double CashOnHand = FinancialSettings.InitialBudget;
        public int Customers = 0;
        public int Round = 0;
        public double UnspentCapability = 0.0;
        public double ValueDelivered = 0.0;

        public List<Story> Backlog = new List<Story>();
        public List<Story> Completed = new List<Story>();
        public List<Story> Proposed = new List<Story>();

        private double _income = 0.0;

        public Game(Log log, JobMarket jobMarket, Team team, StoryDeck storyDeck)
        {
            _log = log;
            _jobMarket = jobMarket;
            _team = team;
            _storyDeck = storyDeck;
        }

        public void HireFirstCandidate()
        {
            if (_jobMarket.Candidates.Count > 0) Hire(_jobMarket.Candidates.First());
        }
        
        public void Hire(Candidate candidate)
        {
            _jobMarket.Hire(candidate);
            _team.Hire(candidate.Developer);
            _log.Add($"Hired {candidate.Name}");
        }

        public void AcceptFirstStory()
        {
            if (Proposed.Count > 0) Accept(Proposed.First());
        }

        private void Accept(Story story)
        {
            Proposed.Remove(story);
            Backlog.Add(story);
            _log.Add($"Accepted {story.Description}");
        }

        public void RejectFirstStory()
        {
            if (Proposed.Count > 0) Reject(Proposed.First());
        }
        
        public void Reject(Story story)
        {
            Proposed.Remove(story);
            _log.Add($"Rejected {story.Description}");
        }
        
        public void Tick()
        {
            Round++;
            AddNewSuggestions();
            _jobMarket.Tick();
            ApplyWork();
            _team.Tick();
            StoriesTick();
            AddDefects();
            CashOnHand -= _team.CycleCost;
            CalculateValueDelivered();
            AddCustomers();
            RemoveCustomersDueToOpenDefects();
            CalculateIncome();
        }

        private void StoriesTick()
        {
            Proposed.ForEach(s => s.Tick());
            Backlog.ForEach(s => s.Tick());
            Completed.ForEach(s => s.Tick());
        }

        private void AddDefects()
        {
            Completed.ForEach(s =>
            {
                if (new Random().NextDouble() < s.ChanceOfDefect)
                {
                    var defect = _storyDeck.CreateDefect(s);
                    Proposed.Add(defect);
                }
            });
        }

        private void AddCustomers()
        {
            if (ValueDelivered > BusinessSettings.ValueRequiredForMVP)
            {
                Customers += Convert.ToInt32(Math.Floor(ValueDelivered * BusinessSettings.NewCustomersPerDeliveredValuePerCycle));
            }
        }

        private void RemoveCustomersDueToOpenDefects()
        {
            var openDefects = Proposed.Select(s => s.IsDefect).Count();
            Customers -= Convert.ToInt32(Math.Floor(openDefects * BusinessSettings.CustomerLossPerOpenBugPerCycle));
            if (Customers < 0) Customers = 0;
        }

        private void CalculateIncome()
        {
            _income = Customers * ValueDelivered * FinancialSettings.IncomeForValueDeliveredFactor;
            CashOnHand += _income;
        }

        private void CalculateValueDelivered()
        {
            ValueDelivered = Completed.Sum(s => s.Value);
        }

        private void ApplyWork()
        {
            if (_team.Count > 0)
            {
                var remainingActual = _team.TotalActual;
                foreach (var story in Backlog)
                {
                    if (remainingActual > 0 && story.RemainingWork > 0)
                    {
                        remainingActual = story.progress(remainingActual);
                    }
                }
                RemoveFinishedWorkFromBacklog();
                UnspentCapability += _team.TotalPotential - _team.TotalActual + remainingActual;
            }
        }

        private void RemoveFinishedWorkFromBacklog()
        {
            foreach (var story in Backlog.Where(s => Math.Abs(s.RemainingWork) < 0.0001))
            {
                Completed.Add(story);
                Backlog.Remove(story);
            }
        }

        private void AddNewSuggestions()
        {
            var factor = StorySettings.MaxNewStoriesPerCycle;
            if (factor >= 1.0)
            {
                var quantity = Math.Ceiling(new Random().NextDouble() * factor);
                for (var i = 0; i < quantity; i++)
                    Proposed.Add(_storyDeck.NextFeature());
            }
            else
            {
                if (new Random().NextDouble() < factor)
                {
                    Proposed.Add(_storyDeck.NextFeature());
                }
            }
        }
    }
}