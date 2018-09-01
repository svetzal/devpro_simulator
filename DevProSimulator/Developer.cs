using System;

namespace DevProSimulator
{
    public class Developer : Tickable
    {
        public string Name { get; }
        public double Level { get; }
        public double Distractible { get; private set; }

        public Developer(string name, double level)
        {
            Name = name;
            Level = level;
            AdjustDistractibility();
        }

        public Developer()
        {
            Name = Faker.Name.FullName();
            Level = new Random().NextDouble() * EmployeeSettings.MaxLevel;
            AdjustDistractibility();
        }

        public double Capability => Level * EmployeeSettings.CapabilityScale;
        public double AdjustedCapability => Capability - Distractible;

        private void AdjustDistractibility()
        {
            Distractible = new Random().NextDouble() * Level / EmployeeSettings.MaxLevel;
        }

        public double Salary => EmployeeSettings.SalaryBase
                                + Level * EmployeeSettings.SalaryRise;

        public double AnnualSalary => Salary * 365;

        public void Tick()
        {
            AdjustDistractibility();
        }
    }
}