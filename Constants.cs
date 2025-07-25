﻿using BetterBudgetWeb.Data;
using BetterBudgetWeb.MonthView;
using BetterBudgetWeb.Repo;
using BetterBudgetWeb.Runner;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BetterBudgetWeb
{
    public class Constants
    {

        public static bool Test = false; // true false
        public static bool AllFree = false; // true false
        public static bool IsSecondPerson;
        public static string Device { get; set; } = "WEB"; // true false
        public static string SignUpText => AllFree ? "Sign Up" : "Start 45-Day Trial!"; // true false
        public static bool tokenInvalidated { get; set; } = new();
        public static bool TokenInvalidated
        {
            get => tokenInvalidated;
            set
            {
                if (tokenInvalidated != value)
                {
                    tokenInvalidated = value;
                    TokenInvalidatedChanged?.Invoke(typeof(Constants), tokenInvalidated);
                }
            }
        }
        public static EventHandler<bool> TokenInvalidatedChanged = (sender, value) => { };
        public static string DeviceSubscribed { get; set; } = "WEB"; // true false
        public static bool HideCookieBanner = true;
        public static EventHandler<bool> WeInChanged = (sender, value) => { };

        private static bool weIn;
        public static bool WeIn
        {
            get => weIn;
            set
            {
                if (weIn != value)
                {
                    weIn = value;
                    WeInChanged?.Invoke(typeof(Constants), weIn);
                }
            }
        }
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
        public static EventHandler<List<Monthly>> DynamicCostItemsChanged = (sender, value) => { };

        private static List<Monthly> dynamicCostItems = new List<Monthly>();
        public static List<Monthly> DynamicCostItems
        {
            get => dynamicCostItems;
            set
            {
                if (dynamicCostItems != value)
                {
                    dynamicCostItems = value;
                    DynamicCostItemsChanged?.Invoke(typeof(Constants), dynamicCostItems);
                }
            }
        }
        public static EventHandler<List<Preset>> PresetsChanged = (sender, value) => { };

        private static List<Preset> presets = new List<Preset>();
        public static List<Preset> Presets
        {
            get => presets;
            set
            {
                if (presets != value)
                {
                    presets = value;
                    PresetsChanged?.Invoke(typeof(Constants), presets);
                }
            }
        }
        public static EventHandler<List<LinePlot>> PlotsChanged = (sender, value) => { };

        private static List<LinePlot> plots = new List<LinePlot>();
        public static List<LinePlot> Plots
        {
            get => plots;
            set
            {
                if (plots != value)
                {
                    plots = value;
                    PlotsChanged?.Invoke(typeof(Constants), plots);
                }
            }
        }
        public static EventHandler<List<Security>> SecuritiesChanged = (sender, value) => { };

        private static List<Security> securities = new List<Security>();
        public static List<Security> Securities
        {
            get => securities;
            set
            {
                if (securities != value)
                {
                    securities = value;
                    SecuritiesChanged?.Invoke(typeof(Constants), securities);
                }
            }
        }
        public static List<SavingsGoal> SavingsGoals { get; set; } = new();
        public static List<Monthly> StaticMonthlyCosts { get; set; } = new();
        public static List<ProjectedDatum> ProjectedData { get; set; } = new();
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static List<Monthly> RelevantMonthlies => Monthlies.Where(mon => mon.Dynamic != "SAVINGS" && (mon.Year == DateTime.Now.Year.ToString() || mon.Year == "1")).ToList();
        public static CatchAll catchAll { get; set; } = new CatchAll();
        public static DateRange DR { get; set; } = new DateRange();

        public static Dictionary<string, string> ColorScheme = new Dictionary<string, string>();

        public static string[] NotExpenses = new string[] { "Income", "Debt", "Equity", "Transfer" };
        public static string BaseUri => Test ? "https://localhost:7234/" : "https://api.couplesbetterbudget.com/";
        public static string CUR_USER_NAME { get; set; }
        public static string CUR_USER_EMAIL { get; set; }
        public static string Person1 { get; set; } = "David";
        public static string Person2 { get; set; } = "Katie";

        public static string Token = "Nice Try";
        public static Tier TIER_LEVEL { get; set; } = Tier.TRIAL;
        public static string TIER_STATUS { get; set; } = "active";
        public static bool TryingItOut => (TIER_LEVEL == Tier.TRIAL || TIER_LEVEL == Tier.DEMO) && !AllFree;

        public static bool mobileApp = false;
        public static bool MobileApp
        {
            get { return mobileApp; }
            set
            {
                if (mobileApp == value) return;
                mobileApp = value;

                MobileAppChanged?.Invoke(typeof(Constants), value);
            }
        }
        private static bool mobile = false;
        public static bool Mobile
        {
            get { return mobile; }
            set
            {
                if (mobile == value) return;
                mobile = value;

                MobileChanged?.Invoke(typeof(Constants), value);
            }
        }
        public static double Person1NetWorth => IndexRunner.CalculateNetWorth(Person1);
        public static double Person2NetWorth => IndexRunner.CalculateNetWorth(Person2);
        public static double TotalNetWorth => Person1NetWorth + Person2NetWorth;
        public static EventHandler<string> CurPageChanged = (sender, value) => { };

        private static string curPage;
        public static string CurPage
        {
            get => curPage;
            set
            {
                if (curPage != value)
                {
                    curPage = value;
                    CurPageChanged?.Invoke(typeof(Constants), curPage);
                }
            }
        }
        public EventCallback<double> Person2NetWorthChanged { get; set; }
        public static EventHandler<bool> MobileAppChanged = (sender, value) => { };
        public static EventHandler<bool> MobileChanged = (sender, value) => { };

        public static readonly List<string> Months = new List<string>{"January", "February", "March", "April", "May",
                                                                "June", "July", "August", "September",
                                                                "October", "November", "December"};

        public static async Task RedrivePeople()
        {
            Person1 = catchAll.Token.Name;
            Person2 = catchAll.Token.Person2;
            CUR_USER_EMAIL = catchAll.Token.Email;
            CUR_USER_NAME = catchAll.Token.Name;
            IsSecondPerson = catchAll.Token.IsSecondPerson;

            // Needed for Ideal Emergency Amount
            await MonthViewConstants.Init();
        }
        public static async Task Init(bool PeopleDecide = false)
        {
            SetColorScheme();
            //BaseUri = Test ? "https://localhost:7234/" : "https://betterbudgetapi.azurewebsites.net/";
            if (PeopleDecide)
            {
                if (Token != "DEMO")
                    catchAll = await CatchAllRunner.Grab();

                await RedrivePeople();

                AssignCatches();
            }
        }
        public static void AssignCatches(CatchAll ca = null)
        {
            if (ca != null)
                catchAll = new CatchAll(ca);

            Transactions = new List<Transaction>(catchAll.Transactions);
            Presets = new List<Preset>(catchAll.Presets);
            Monthlies = new List<Monthly>(catchAll.Monthlies);
            SetMonthlies(Monthlies);
            Envelopes = new List<Envelope>(catchAll.Envelopes);
            Securities = new List<Security>(catchAll.Securities);
            Balances = new List<Balance>(catchAll.Balances);
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
                DynamicCostItems.Add(new Monthly("Food\n(EXAMPLE DATA)", 250, 250, "DYNAMIC"));
            }

            if (SavingsGoals.Count == 0)
            {
                SavingsGoals.Add(new SavingsGoal(Person1 + "\n(EXAMPLE DATA)", 1000, "All", "1"));
                SavingsGoals.Add(new SavingsGoal(Person2 + "\n(EXAMPLE DATA)", 1000, "All", "1"));
            }

            if (StaticMonthlyCosts.Count == 0)
            {
                StaticMonthlyCosts.Add(new Monthly("Rent\n(EXAMPLE DATA)", 600, 600, "STATIC"));
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
            DynamicCostItems = new List<Monthly>();
            SavingsGoals = new List<SavingsGoal>();
            StaticMonthlyCosts = new List<Monthly>();
            ProjectedData = new List<ProjectedDatum>();
            Plots = new List<LinePlot>();

            foreach (var mon in Monthlies)
            {
                if (mon.Dynamic == "DYNAMIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var DCIExists = DynamicCostItems.FirstOrDefault(d => d.Name == mon.Name);
                        var NewDCI = new Monthly(mon.Name, mon.Person1Amount, mon.Person2Amount, "DYNAMIC");

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
                        var NewSMC = new Monthly(mon.Name, mon.Person1Amount, mon.Person2Amount, "STATIC");

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

            DynamicCostItems = DynamicCostItems.OrderByDescending(dci => dci.TotalAmount).ToList();
            StaticMonthlyCosts = StaticMonthlyCosts.OrderByDescending(smc => smc.TotalAmount).ToList();
            SavingsGoals = SavingsGoals.OrderByDescending(sg => sg.Goal).ToList();
            Plots = IndexRunner.GeneratePlots();
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
        public static string Pretty(double? num)
        {
            return Pretty((double)num);
        }
        public static string[] HandlePresets(string preset)
        {
            Preset pres = Presets.FirstOrDefault(p => p.Id == preset);
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
                values[2] = pres.PaidWithPerson1;
                values[3] = pres.PaidWithPerson2;
                values[4] = pres.Person1Amount.ToString();
                values[5] = pres.Person2Amount.ToString();
                values[6] = pres.PaidOffPerson1;
                values[7] = pres.PaidOffPerson2;
                values[8] = pres.TextColor.ToString().ToUpper() == "#FFFFFF" ? "#FFFFFF" : "#000000";
                values[9] = pres.HexColor.ToString();

                if (IsSecondPerson)
                {
                    for(int index = 2; index <= 6; index += 2)
                    {
                        Swap(ref values[index], ref values[index + 1]);
                    }
                }
            }

            return values;
        }

        private static void Swap(ref string one, ref string two)
        {
            string temp = one;
            one = two;
            two = temp;
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
            ColorScheme["ComponentBackground"] = "#152030";
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
        public static Tier StringToTier(string tier)
        {
            switch (tier)
            {
                case "BASIC_TIER":
                    return Tier.BASIC_TIER;
                case "ADVANCED_TIER":
                    return Tier.ADVANCED_TIER;
                case "GOD_TIER":
                    return Tier.GOD_TIER;
                case "TRIAL":
                    return Tier.TRIAL;
                default:
                    return Tier.NONE;
            }
        }

        public static string Options(bool test, string yes, string no)
        {
            return test ? yes : no;
        }
        public static string TestMobile(string MobileOption, string DesktopOption)
        {
            return Mobile ? MobileOption : DesktopOption;
        }
        public static double TestMobile(double MobileOption, double DesktopOption)
        {
            return Mobile ? MobileOption : DesktopOption;
        }
    }
}
