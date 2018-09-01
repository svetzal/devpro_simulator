namespace DevProSimulator
{
    public class BusinessSettings
    {
        public static readonly int ValueRequiredForMVP = 100;
        public static readonly double NewCustomersPerDeliveredValuePerCycle = 0.01;
        public static readonly double CustomerLossPerOpenBugPerCycle = 0.01;
    }

    public class FinancialSettings
    {
        public static readonly double InitialBudget = 30000;
        public static readonly double IncomeForValueDeliveredFactor = 0.01;
    }
    
    public class TeamSettings
    {
        public static readonly double DistractibilityFactor = 0.15;
    }

    public class EmployeeSettings
    {
        public static readonly int MaxLevel = 10;
        public static readonly double CapabilityScale = 2.5;
        public static readonly double SalaryBase = 30000 / 365;
        public static readonly double SalaryRise = 100000 / 365;
    }
    
    public class StorySettings
    {
        public static readonly int[] Sizes = {1, 2, 3, 5, 7};

        public static readonly double MaxNewStoriesPerCycle = 0.85;
        public static readonly double BaseDefectRate = 0.03;
    }
}