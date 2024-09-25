using BetterBudgetWeb.Data;
using BetterBudgetWeb.Repo;
using BetterBudgetWeb.Runner;
using BetterBudgetWeb.MonthView;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace BetterBudgetWeb
{
    public class Constants
    {

        public static bool Test = false; // true false
        // Set null example:
        //      testVar2 = testVar1 ?? testVar2
        //          ^^^^^^^^^^^^^^^^
        //  If testVar1 == null, then testVar2 = testVar2
        public static Dictionary<int, string> TheMonthsFromInt = new Dictionary<int, string> {
            { 1, "January" },
            { 2, "February" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" }
        };

        public static Dictionary<string, int> TheMonthsFromString = new Dictionary<string, int> {
            { "January", 1 },
            { "February", 2 },
            { "March", 3 },
            { "April", 4 },
            { "May", 5 },
            { "June", 6 },
            { "July", 7 },
            { "August", 8 },
            { "September", 9 },
            { "October", 10 },
            { "November", 11 },
            { "December", 12 }
        };
        public static EventHandler<List<Transaction>> TransactionsChanged = (sender, value) => { };

        private static List<Transaction> transactions = new List<Transaction>();
        public static List<Transaction> Transactions
        {
            get => transactions;
            set
            {
                if (transactions != value)
                {
                    transactions = value;
                    TransactionsChanged?.Invoke(typeof(Constants), transactions);
                }
            }
        }
        public static EventHandler<List<Balance>> BalancesChanged = (sender, value) => { };

        private static List<Balance> balances = new List<Balance>();
        public static List<Balance> Balances
        {
            get => balances;
            set
            {
                if (balances != value)
                {
                    balances = value;
                    BalancesChanged?.Invoke(typeof(Constants), balances);
                }
            }
        }
        public static EventHandler<List<Envelope>> EnvelopesChanged = (sender, value) => { };

        private static List<Envelope> envelopes = new List<Envelope>();
        public static List<Envelope> Envelopes
        {
            get => envelopes;
            set
            {
                if (envelopes != value)
                {
                    envelopes = value;
                    EnvelopesChanged?.Invoke(typeof(Constants), envelopes);
                }
            }
        }
        public static List<DynamicCostItem> DynamicCostItems { get; set; } = new();
        public static List<SavingsGoal> SavingsGoals { get; set; } = new();
        public static List<StaticMonthlyCost> StaticMonthlyCosts { get; set; } = new();
        public static List<ProjectedDatum> ProjectedData { get; set; } = new();
        public static List<Preset> Presets { get; set; } = new List<Preset>();
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static List<Security> Securities { get; set; } = new List<Security>();
        public static CatchAll catchAll { get; set; } = new CatchAll();
        public static DateRange DR { get; set; } = new DateRange();

        public static Dictionary<string, string> ColorScheme = new Dictionary<string, string>();

        public static string[] NotExpenses = new string[] { "Income", "Debt", "Equity", "Transfer" };

        public static string BaseUri = "";
        public static string CUR_USER_NAME { get; set; }
        public static string Person1 { get; set; } = "David";
        public static string Person2 { get; set; } = "Katie";

        public static string Token = "Nice Try";

        private static bool mobile = true;
        public static bool Mobile
        {
            get { return mobile; }
            set
            {
                if (mobile == value) return;
                mobile = value;

                MobileChanged.InvokeAsync(value);
            }
        }
        public static double Person1NetWorth => IndexRunner.CalculateNetWorth(Person1, Balances);
        public static double Person2NetWorth => IndexRunner.CalculateNetWorth(Person2, Balances);
        public static double TotalNetWorth => Person1NetWorth + Person2NetWorth;
        public EventCallback<double> Person2NetWorthChanged { get; set; }
        public static EventCallback<bool> MobileChanged { get; set; }

        public static readonly List<string> Months = new List<string>{"January", "February", "March", "April", "May",
                                                                "June", "July", "August", "September",
                                                                "October", "November", "December"};

        public static async Task RedrivePeople()
        {
            Person1 = catchAll.Token.Name;
            Person2 = catchAll.Token.Person2;

            // Needed for Ideal Emergency Amount
            await MonthViewConstants.Init();
        }
        public static async Task Init(bool PeopleDecide = false)
        {
            SetColorScheme();
            BaseUri = Test ? "https://localhost:7234/" : "https://api.couplesbetterbudget.com/";
            //BaseUri = Test ? "https://localhost:7234/" : "https://betterbudgetapi.azurewebsites.net/";
            if (PeopleDecide)
            {
                if(Token != "DEMO")
                    catchAll = await CatchAllRunner.Grab();

                await RedrivePeople();

                AssignCatches();

                SetMonthlies(Monthlies);
            }
        }
        public static void AssignCatches(CatchAll ca = null)
        {
            if (ca != null)
                catchAll = new CatchAll(ca);

            Transactions = new List<Transaction>(catchAll.Transactions);
            Balances = new List<Balance>(catchAll.Balances);
            Monthlies = new List<Monthly>(catchAll.Monthlies);
            Envelopes = new List<Envelope>(catchAll.Envelopes);
            Presets = new List<Preset>(catchAll.Presets);
            Securities = new List<Security>(catchAll.Securities);
            if (DR != null)
            {
                if (catchAll?.DR?.UniqueMonthYears?.Count > 0)
                    DR = catchAll.DR;
            }
            else
                DR = catchAll.DR;
            TransactionRepo.Transactions = catchAll.Transactions;
            BalanceRepo.Balances = catchAll.Balances;
            MonthlyRepo.Monthlies = catchAll.Monthlies;
            EnvelopeRepo.Envelopes = catchAll.Envelopes;
            PresetRepo.Presets = catchAll.Presets;
            SnapshotRepo.Snapshots = catchAll.Snapshots;
            SecurityRepo.Securities = catchAll.Securities;

            Redrive();
        }
        private static void LoadDefaults()
        {
            if (DynamicCostItems.Count == 0)
            {
                DynamicCostItems.Add(new DynamicCostItem("Food\n(EXAMPLE DATA)", 600, 200));
                DynamicCostItems.Add(new DynamicCostItem("Fun\n(EXAMPLE DATA)", 300, 300));
                DynamicCostItems.Add(new DynamicCostItem("Gas\n(EXAMPLE DATA)", 80, 180));
            }

            if (SavingsGoals.Count == 0)
            {
                SavingsGoals.Add(new SavingsGoal(Person1 + "\n(EXAMPLE DATA)", 2000, "All", "1"));
                SavingsGoals.Add(new SavingsGoal(Person2 + "\n(EXAMPLE DATA)", 1715, "All", "1"));
            }

            if (StaticMonthlyCosts.Count == 0)
            {
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Rent\n(EXAMPLE DATA)", 820, 800));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Car\n(EXAMPLE DATA)", 399.03, 0));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Food\n(EXAMPLE DATA)", 600, 200));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Entertainment\n(EXAMPLE DATA)", 300, 300));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Phone\n(EXAMPLE DATA)", 66, 65));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Gas\n(EXAMPLE DATA)", 20, 180));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Car Insurance\n(EXAMPLE DATA)", 80, 93));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Electric\n(EXAMPLE DATA)", 100, 0));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Internet\n(EXAMPLE DATA)", 70, 0));
                StaticMonthlyCosts.Add(new StaticMonthlyCost("Subs \n(EXAMPLE DATA)", 78, 0));
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
        public static string Pretty(double num)
        {
            return num.ToString("C", CultureInfo.CurrentCulture);
        }
        public static string[] HandlePresets(string preset)
        {
            Preset pres = Presets.FirstOrDefault(p => p.Name == preset);
            // 0 - Trans Name
            // 1 - ExpenseType
            // 2 - Paid With Person1
            // 3 - Paid With Person2
            // 4 - Person1Amount
            // 5 - Person2Amount
            // 6 - Paid Off Person1
            // 7 - Paid Off Person2
            // 8 - Text Color
            // 9 - Background Color

            string[] values = new string[10];

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
                values[8] = pres.TextColor.ToString();
                values[9] = pres.HexColor.ToString();
            }

            return values;
        }

        public static double GetBudgetedAmount(string ExpType, string Month = "All", string Year = "1")
        {
            Year = Year == "All" ? "1" : Year;

            var AllMonths = MonthlyRepo.GetMonthlies().ToList();

            var ExpInQuestion = AllMonths.FirstOrDefault(mon =>
                                mon.Name == ExpType &&
                                (mon.MonthYear() == Month + " " + Year ||
                                mon.MonthYear() == Month + "  1" ||
                                mon.MonthYear() == "All " + Year ||
                                mon.MonthYear() == "All 1"));

            return ExpInQuestion == null ? 0 : ExpInQuestion.TotalAmount;
        }

        public static void SetColorScheme()
        {
            ColorScheme["Background"] = "#111827";
            ColorScheme["ComponentBackground"] = "#384153";
            ColorScheme["Text"] = "silver";
            ColorScheme["TextOnSilver"] = "white";
            ColorScheme["TableEven"] = "#203957";
            ColorScheme["TableOdd"] = "#384153";

            ColorScheme["IncomeGood"] = "forestgreen";
            ColorScheme["EquityGood"] = "yellow";
            ColorScheme["Debt"] = "lightseagreen";

            ColorScheme["Tab-Back"] = "#203957";
            ColorScheme["Tab-Active"] = "#384152";
            ColorScheme["Tab-Hover"] = "#05DDA9";
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
                    var split1 = month1.Split(' ');
                    var split2 = month2.Split(' ');

                    month1 = split1[0];
                    month2 = split2[0];

                    int year1 = -1;
                    int year2 = -1;

                    if (split1.Length > 1)
                        year1 = int.TryParse(split1[1], out int tossyear1) ? tossyear1 : -1;

                    if (split2.Length > 1)
                        year2 = int.TryParse(split2[1], out int tossyear2) ? tossyear2 : -1;

                    if (year1 < year2)
                        return -1;

                    if (year1 > year2)
                        return 1;

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

        public static string CENTER(string str, int length, char pad = ' ')
        {
            string retStr = "";
            int leftSide;

            if (str.Length < length)
            {
                leftSide = (length - str.Length) / 2;

                retStr = str.PadLeft(leftSide + str.Length, pad);
                retStr = retStr.PadRight(length, pad);
            }
            else if (str.Length > length)
            {
                leftSide = (str.Length - length) / 2;
                retStr = str.Substring(leftSide, length);
            }
            else
                retStr = str;

            return retStr;
        }
        public static int CompareBalance(Balance first, Balance second)
        {
            // -1 means first is greater
            // 0 means equal
            // 1 means second is greater

            if (first == null)
                return second == null ? 0 : 1;

            string[] best = new string[] { "Income", "Equity" };
            string[] good = new string[] { "Stocks" };
            string[] worst = new string[] { "Loan" };

            /* Test scenarios:
            * first = Income/Equity & second = Income/Equity -----> 0
            * first = Income/Equity & second = Stocks -----> -1
            * first = Income/Equity & second = Loan -----> -1
            * first = Stocks & second = Income/Equity -----> 1
            * first = Stocks & second = Stocks -----> 0
            * first = Stocks & second = Loan -----> -1
            * first = Loan & second = Income/Equity -----> 1
            * first = Loan & second = Stocks -----> 1
            * first = Loan & second = Loan -----> 0
            */

            // ================= Equals Start ================
            if ((best.Contains(first.BalanceType) && best.Contains(second.BalanceType)) || // Both Best
                (good.Contains(first.BalanceType) && good.Contains(second.BalanceType)) || // Both Good
                (worst.Contains(first.BalanceType) && worst.Contains(second.BalanceType))) // Both Worst
            {
                /*
                 * first = Income/Equity & second = Income/Equity
                 * first = Stocks & second = Stocks
                 * first = Loan & second = Loan
                 */
                return second.Value.CompareTo(first.Value);
            }
            //================= Equals End ================= 


            if ((best.Contains(first.BalanceType) && !best.Contains(second.BalanceType)) ||
                (good.Contains(first.BalanceType) && worst.Contains(second.BalanceType)))
            {
                /*
                 * first = Income/Equity & second = Stocks
                 * first = Income/Equity & second = Loan
                 * first = Stocks & second = Loan
                 */
                return -1;
            }

            /*
             * first = Stocks & second = Income/Equity
             * first = Loan & second = Income/Equity
             * first = Loan & second = Stocks
             */
            return 1;
        }
        public static string GoodOrBad(double money)
        {
            return money <= 0 ? "red" : "forestgreen";
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
