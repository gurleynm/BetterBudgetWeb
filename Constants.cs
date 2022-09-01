using BetterBudgetWeb.Data;
using BetterBudgetWeb.Repo;
using BetterBudgetWeb.Runner;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BetterBudgetWeb
{
    public class Constants
    {
        public static List<DynamicCostItem> DynamicCostItems { get; set; }
        public static List<SavingsGoal> SavingsGoals { get; set; }
        public static List<StaticMonthlyCost> StaticMonthlyCosts { get; set; }
        public static List<ProjectedDatum> ProjectedData { get; set; }
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static List<Balance> Balances { get; set; } = new List<Balance>();
        public static List<Preset> Presets { get; set; } = new List<Preset>();
        private static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static List<Envelope> Envelopes { get; set; } = new List<Envelope>();

        public static bool DarkMode = Nighttime();
        public static Dictionary<string, string> ColorScheme = new Dictionary<string, string>();

        public static string BaseUri = "https://davidbetterbudgetapi.azurewebsites.net/";
        public static string Person1 { get; set; } = "David";
        public static string Person2 { get; set; } = "Kaitie";

        public static bool Us = true;
        private static bool Test = true;
        public static string Key { get; set; } = "";

        public static string PassKey { get; set; } = "no";

        public static readonly List<string> Months = new List<string>{"January", "February", "March", "April", "May",
                                                                "June", "July", "August", "September",
                                                                "October", "November", "December"};

        public static async Task Init()
        {
            DetermineDarkLight();
            if (Us)
            {
                BaseUri = Test ? "https://localhost:7234/" : "https://betterbudgetapi.azurewebsites.net/";
                Person1 = "Nathan";
                Person2 = "Lindsey";
                Key = "Lindseylicious";
            }
            else
            {
                BaseUri = Test ? "https://localhost:7121/" : "https://davidbetterbudgetapi.azurewebsites.net/";
                Person1 = "David";
                Person2 = "Katie";
                Key = "Doofenblast!";
            }

            PassKey = Key;
            CatchAll catchAll = await CatchAllRunner.Grab();
            
            AssignCatches(catchAll);

            SetMonthlies(Monthlies);
        }
        public static void AssignCatches(CatchAll caught)
        {
            Transactions = caught.Transactions;
            Balances = caught.Balances;
            Monthlies = caught.Monthlies;
            Envelopes = caught.Envelopes;
            Presets = caught.Presets;

            TransactionRepo.Transactions = caught.Transactions;
            BalanceRepo.Balances = caught.Balances;
            MonthlyRepo.Monthlies = caught.Monthlies;
            EnvelopeRepo.Envelopes = caught.Envelopes;
            PresetRepo.Presets = caught.Presets;
            SnapshotRepo.Snapshots = caught.Snapshots;

            Redrive();
        }
        private static void LoadDefaults()
        {
            if (Constants.DynamicCostItems.Count == 0)
            {
                Constants.DynamicCostItems.Add(new DynamicCostItem("Food\n(EXAMPLE DATA)", 600, 200));
                Constants.DynamicCostItems.Add(new DynamicCostItem("Fun\n(EXAMPLE DATA)", 300, 300));
                Constants.DynamicCostItems.Add(new DynamicCostItem("Gas\n(EXAMPLE DATA)", 80, 180));
            }

            if (Constants.SavingsGoals.Count == 0)
            {
                Constants.SavingsGoals.Add(new SavingsGoal(Person1 + "\n(EXAMPLE DATA)", 2000, "All", "1"));
                Constants.SavingsGoals.Add(new SavingsGoal(Person2 + "\n(EXAMPLE DATA)", 1715, "All", "1"));
            }

            if (Constants.StaticMonthlyCosts.Count == 0)
            {
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Rent\n(EXAMPLE DATA)", 820, 800));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Car\n(EXAMPLE DATA)", 399.03, 0));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Food\n(EXAMPLE DATA)", 600, 200));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Entertainment\n(EXAMPLE DATA)", 300, 300));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Phone\n(EXAMPLE DATA)", 66, 65));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Gas\n(EXAMPLE DATA)", 20, 180));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Car Insurance\n(EXAMPLE DATA)", 80, 93));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Electric\n(EXAMPLE DATA)", 100, 0));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Internet\n(EXAMPLE DATA)", 70, 0));
                Constants.StaticMonthlyCosts.Add(new StaticMonthlyCost("Subs \n(EXAMPLE DATA)", 78, 0));
            }
        }
        public static List<Monthly> GetMonthlies() { return Monthlies; }
        public static async void SetMonthlies(List<Monthly> monthlies)
        {
            Monthlies = new List<Monthly>(monthlies);
            Redrive();
        }
        public static void Redrive()
        {
            DynamicCostItems = new List<DynamicCostItem>();
            SavingsGoals = new List<SavingsGoal>();
            StaticMonthlyCosts = new List<StaticMonthlyCost>();
            ProjectedData = new List<ProjectedDatum>();

            foreach (var mon in Monthlies)
            {
                if (mon.Dynamic == "DYNAMIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var DCIExists = DynamicCostItems.FirstOrDefault(d => d.Name == mon.Name);
                        var NewDCI = new DynamicCostItem(mon.Name, mon.Person1Amount, mon.Person2Amount);

                        if (DCIExists == null)
                            DynamicCostItems.Add(NewDCI);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                DynamicCostItems.Remove(DCIExists);
                                DynamicCostItems.Add(NewDCI);
                            }
                        }
                    }
                }
                else if (mon.Dynamic == "STATIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var SMCExists = StaticMonthlyCosts.FirstOrDefault(s => s.Name == mon.Name);
                        var NewSMC = new StaticMonthlyCost(mon.Name, mon.Person1Amount, mon.Person2Amount);

                        if (SMCExists == null)
                            StaticMonthlyCosts.Add(NewSMC);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                StaticMonthlyCosts.Remove(SMCExists);
                                StaticMonthlyCosts.Add(NewSMC);
                            }
                        }
                    }
                }
                else if (mon.Dynamic == "SAVINGS")
                {
                    SavingsGoals.Add(new SavingsGoal(Person1, mon.Person1Amount, mon.Month, mon.Year));
                    SavingsGoals.Add(new SavingsGoal(Person2, mon.Person2Amount, mon.Month, mon.Year));
                }
                else if (mon.Dynamic == "PROJECTED DATA")
                {
                    bool tp = int.TryParse(mon.Year, out int year);
                    if (!tp) year = DateTime.Now.Year;

                    ProjectedData.Add(new ProjectedDatum(mon.Month, year, mon.Person1Amount, mon.Person2Amount));
                }
            }

            LoadDefaults();

            DynamicCostItems = DynamicCostItems.OrderByDescending(dci => dci.Amount).ToList();
            StaticMonthlyCosts = StaticMonthlyCosts.OrderByDescending(smc => smc.TotalAmount).ToList();
            SavingsGoals = SavingsGoals.OrderByDescending(sg => sg.Goal).ToList();
        }
        public static string MonthYear()
        {
            return DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString();
        }
        public static bool CheckMonthYear(Monthly mon)
        {
            string currentMonthYear = MonthYear();
            string mCM = mon.MonthYear();
            string[] splitter = mCM.Split(" ");

            string month = currentMonthYear.Split(" ")[0];
            string year = DateTime.Now.Year.ToString();
            bool everyYear = false;

            if (splitter.Length > 1)
            {
                month = splitter[0];
                year = splitter[1];
                everyYear = year == "1";
            }

            return mCM == currentMonthYear
                || (month == currentMonthYear.Split(" ")[0] && everyYear)
                || (mCM.Contains("All") && everyYear)
                || (mCM.Contains("All") && year == DateTime.Now.Year.ToString());
        }
        public static string SHA256(string value)
        {
            if (value == null) return null;
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static string Pretty(double num)
        {
            return num.ToString("C", CultureInfo.CurrentCulture);
        }
        public static string[] HandlePresets(string preset)
        {
            Preset pres = Constants.Presets.FirstOrDefault(p => p.Name == preset);
            // 0 - Trans Name
            // 1 - ExpenseType
            // 2 - Paid With Person1
            // 3 - Paid With Person2
            // 4 - Person1Amount
            // 5 - Person2Amount
            // 6 - Paid Off Person1
            // 7 - Paid Off Person2

            string[] values = new string[8];

            if (pres != null)
            {
                values[0] = pres.Name;
                values[1] = pres.ExpenseType;
                values[2] = pres.PaidWithPerson1 == null ? null : BalanceRepo.GetId(pres.PaidWithPerson1);
                values[3] = pres.PaidWithPerson2 == null ? null : BalanceRepo.GetId(pres.PaidWithPerson2);
                values[4] = pres.Person1Amount.ToString();
                values[5] = pres.Person2Amount.ToString();
                values[6] = pres.PaidOffPerson1 == null ? null : BalanceRepo.GetId(pres.PaidOffPerson1);
                values[7] = pres.PaidOffPerson2 == null ? null : BalanceRepo.GetId(pres.PaidOffPerson2);
            }

            return values;
        }
        public static void DetermineDarkLight()
        {
            if (DarkMode)
            {
                ColorScheme["Background"] = "#111827";
                ColorScheme["Text"] = "silver";
                ColorScheme["TableEven"] = "#203957";
                ColorScheme["TableOdd"] = "#384152";

                ColorScheme["IncomeGood"] = "forestgreen";
                ColorScheme["Debt"] = "lightseagreen";

                ColorScheme["Tab-Back"] = "#203957";
                ColorScheme["Tab-Active"] = "#384152";
                ColorScheme["Tab-Hover"] = "#05DDA9";
            }
            else
            {
                ColorScheme["Background"] = "white";
                ColorScheme["Text"] = "black";
                ColorScheme["TableEven"] = "#dddddd";
                ColorScheme["TableOdd"] = "#ffffff";

                ColorScheme["IncomeGood"] = "green";
                ColorScheme["Debt"] = "blue";

                ColorScheme["Tab-Back"] = "#f1f1f1";
                ColorScheme["Tab-Active"] = "#ccc";
                ColorScheme["Tab-Hover"] = "#ddd";
            }
        }

        public static bool Nighttime()
        {
            TimeSpan NinePM = new TimeSpan(21, 0, 0); //9 PM
            TimeSpan NineAM = new TimeSpan(9, 0, 0);  //9 AM
            TimeSpan now = DateTime.Now.TimeOfDay;

            return now > NinePM || now < NineAM;
        }

        public static int SortMonths(string month1, string month2)
        {
            if (month1 == null)
            {
                if (month2 == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                if (month2 == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    if (!Months.Contains(month1))
                    {
                        if (!Months.Contains(month2))
                            return 0;
                        else
                            return -1;
                    }

                    int indexMonth1 = Months.IndexOf(month1);
                    int indexMonth2 = Months.IndexOf(month2);

                    if (indexMonth1 < indexMonth2)
                        return -1;
                    else if (indexMonth2 < indexMonth1)
                        return 1;
                    else
                        return 0;
                }
            }
        }
    }
}
