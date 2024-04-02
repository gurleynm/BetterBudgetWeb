using BetterBudgetWeb.Data;
using BetterBudgetWeb.Repo;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static BetterBudgetWeb.Shared.Reports;
using System.Diagnostics;

namespace BetterBudgetWeb.Runner
{
    public class ReportRunner
    {
        public static List<DynamicCostItem> DynamicCostItems { get; set; }
        public static List<SavingsGoal> SavingsGoals { get; set; }
        public static List<StaticMonthlyCost> StaticMonthlyCosts { get; set; }
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static List<Balance> Balances { get; set; } = new List<Balance>();
        public static List<ProjectedDatum> ProjectedData { get; set; }

        public static int Month { get; set; } = DateTime.Now.Month;
        public static int Year { get; set; } = DateTime.Now.Year;

        public static async Task<List<Snapshot>> SetSnapshot(DateTime snapDate)
        {
            string thisMonth = snapDate.ToString("MMMM");
            string thisYear = snapDate.Year.ToString();

            var Trans = new List<Transaction>(Constants.Transactions.Where(t => t.MonthYear() == thisMonth + " " + thisYear));

            int monthInt = snapDate.Month;
            int yearInt = snapDate.Year;
            SetMonthlies(monthInt, yearInt);
            int TodayTransTotal = Trans.Count();
            int TodayTransPerson1Total = Trans.Where(t => t.Person1Amount != 0).Count();
            int TodayTransPerson2Total = Trans.Where(t => t.Person2Amount != 0).Count();

            foreach (var d in DynamicCostItems)
            {
                double[] dciSums = SumForDynamic(Trans, d.Name);
                d.SpentReport = dciSums[0] + dciSums[1];
            }

            var TodayDCIs = new List<DynamicCostItem>(DynamicCostItems);
            var TodaySMCs = new List<StaticMonthlyCost>(StaticMonthlyCosts);

            double TodayPerson1Projected = TodayDCIs.Sum(d => d.Person1Amount) + TodaySMCs.Sum(d => d.Person1Amount);
            double TodayPerson2Projected = TodayDCIs.Sum(d => d.Person2Amount) + TodaySMCs.Sum(d => d.Person2Amount);

            double TodayPerson1Actual = Trans.Where(t => !Constants.NotExpenses.Contains(t.ExpenseType)).Sum(d => d.Person1Amount);
            double TodayPerson2Actual = Trans.Where(t => !Constants.NotExpenses.Contains(t.ExpenseType)).Sum(d => d.Person2Amount);

            double TodayPerson1Income = Trans.Where(t => t.ExpenseType == "Income" || t.ExpenseType == "Equity").Sum(d => d.Person1Amount);
            double TodayPerson2Income = Trans.Where(t => t.ExpenseType == "Income" || t.ExpenseType == "Equity").Sum(d => d.Person2Amount);

            Dynamo TodayDyno = new Dynamo
            {
                Month = thisMonth,
                Year = thisYear,
                DynamicCosts = new JArray()
            };

            foreach (var dci in DynamicCostItems)
            {
                dynamic jsonObject = new JObject();
                jsonObject.Name = dci.Name;
                jsonObject.Projected = dci.Person1Amount + dci.Person2Amount;
                jsonObject.Actual = dci.SpentReport;

                TodayDyno.DynamicCosts.Add(jsonObject);
            }

            var test = (string)JsonConvert.SerializeObject(TodayDyno);
            DateTime thisDateTime = new DateTime(yearInt, monthInt, 1);
            List<Snapshot> snaps = await SnapshotRepo.AddOrUpdateAsync(new Snapshot
            {
                Month = thisDateTime.ToString("MMMM"),
                Year = thisDateTime.Year,
                Person1Income = TodayPerson1Income,
                Person2Income = TodayPerson2Income,
                Person1Transactions = TodayTransPerson1Total,
                Person2Transactions = TodayTransPerson2Total,
                TotalTransactions = TodayTransTotal,
                Person1AmountProjected = TodayPerson1Projected,
                Person2AmountProjected = TodayPerson2Projected,
                Person1AmountActual = TodayPerson1Actual,
                Person2AmountActual = TodayPerson2Actual,
                DynamicJSON = test
            });

            return snaps; 
        }

        public static void SetMonthlies(int month, int year)
        {
            Monthlies = Constants.GetMonthlies();

            Month = month;
            Year = year;

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
                    SavingsGoals.Add(new SavingsGoal(Constants.Person1, mon.Person1Amount, mon.Month, mon.Year));
                    SavingsGoals.Add(new SavingsGoal(Constants.Person2, mon.Person2Amount, mon.Month, mon.Year));
                }
                else if (mon.Dynamic == "PROJECTED DATA")
                {
                    bool tp = int.TryParse(mon.Year, out int yearp);
                    if (!tp) yearp = DateTime.Now.Year;

                    ProjectedDatum pd = new ProjectedDatum(mon.Month, yearp, mon.Person1Amount, mon.Person2Amount);

                    ProjectedData.Add(pd);
                }
            }

            DynamicCostItems = DynamicCostItems.OrderByDescending(dci => dci.Amount).ToList();
            StaticMonthlyCosts = StaticMonthlyCosts.OrderByDescending(smc => smc.TotalAmount).ToList();
            SavingsGoals = SavingsGoals.OrderByDescending(sg => sg.Goal).ToList();
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
        public static string MonthYear() { return new DateTime(Year, Month, 1).ToString("MMMM") + " " + Year.ToString(); }
        public static double[] SumForDynamic(List<Transaction> Trans, string Expense)
        {
            double[] sums = new double[] { 0, 0, 0 };

            foreach (Transaction t in Trans)
            {
                if (t.ExpenseType == Expense && t.MonthYear() == MonthYear())
                {
                    sums[0] += t.Person1Amount;
                    sums[1] += t.Person2Amount;
                    sums[2] += t.TotalAmount;
                }
            }

            return sums;
        }
        public static double SumForDynamicAll(List<Transaction> Trans, int person = 0)
        {
            double sum = 0;

            foreach (Transaction t in Trans)
            {
                if (DynamicCostItems.Select(d => d.Name).Contains(t.ExpenseType) && t.MonthYear() == MonthYear())
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
            }

            return sum;
        }
        
        public static double SumForStaticAll(List<Transaction> Trans, int person = 0)
        {
            double sum = 0;

            foreach (Transaction t in Trans)
            {
                if (StaticMonthlyCosts.Select(d => d.Name).Contains(t.Name) && t.MonthYear() == MonthYear())
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
            }

            return sum;
        }

        public static double GetMonthExpense(List<Transaction> Trans, int person, string selMonth)
        {
            if (selMonth == "All")
                return GetAllExpense(Trans, person);

            return SumForDynamicAll(Trans, person) + SumForStaticAll(Trans, person);
        }
        
        public static double GetAllExpense(List<Transaction> Trans, int person = 0)
        {
            double sum = 0;

            foreach (Transaction t in Trans)
            {
                if (StaticMonthlyCosts.Select(d => d.Name).Contains(t.Name))
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
                else if (DynamicCostItems.Select(d => d.Name).Contains(t.ExpenseType))
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
            }

            return sum;
        }


        public static double GetMonthNet(List<Transaction> Trans, int person, string selMonth)
        {
            double dynam = GetMonthExpense(Trans, person, selMonth);
            double income;

            if (person == 1)
                income = Trans.Where(tr => tr.ExpenseType == "Income").Sum(t => t.Person1Amount);
            else if (person == 2)
                income = Trans.Where(tr => tr.ExpenseType == "Income").Sum(t => t.Person2Amount);
            else
                income = Trans.Where(tr => tr.ExpenseType == "Income").Sum(t => t.TotalAmount);

            return income - dynam;
        }
    }
}
