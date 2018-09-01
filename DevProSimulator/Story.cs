using System;
using System.Collections.Generic;

namespace DevProSimulator
{
    public class Story : Tickable
    {
        public static string DefectType = "defect";
        public static string FeatureType = "feature";

        private int _timeInProduction = 0;
        private int _age = 0;
        private List<Story> _defects = new List<Story>();
        private Developer _assignee = null;
        
        public int Size { get; }
        public double RemainingWork { get; private set; }
        public int Value { get; }
        public string Type { get; }
        public string Description { get; }

        public Story(int value, int size, string description, string type)
        {
            Value = value;
            Size = size;
            Type = type;
            Description = description;

            RemainingWork = size;
        }

        public bool IsDefect => Type == DefectType;
        public bool IsFeature => Type == FeatureType;

        public double ChanceOfDefect => 1 / _timeInProduction * Size * StorySettings.BaseDefectRate;
        public string ValueLabel => new String('$', Value);
        public string SizeLabel => new String('◼', Size);

        public void Assign(Developer developer)
        {
            _assignee = developer;
        }

        public void Tick()
        {
            if (_assignee == null)
                _age++;
            else
                _timeInProduction++;
        }

        public double progress(double amount)
        {
            if (RemainingWork > amount)
            {
                RemainingWork -= amount;
                return 0;
            }
            else
            {
                var remainder = amount - RemainingWork;
                RemainingWork = 0;
                return remainder;
            }
        }

        public void AddDefect(Story defect)
        {
            _defects.Add(defect);
        }
    }
}