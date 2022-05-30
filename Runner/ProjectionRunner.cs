using BetterBudgetWeb.Data;
using BetterBudgetWeb.Repo;

namespace BetterBudgetWeb.Runner
{
    public class ProjectionRunner
    {
        private static string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static List<ProjectedDatum> ActualValues => Constants.ProjectedData;
        private static List<Monthly> Monthlies = new List<Monthly>();
        public static void GetGoals(ref SavingsGoal Default1, ref SavingsGoal Default2, ref string MonthYear, ref List<ProjectedDatum> Goals)
        {

            int Month = 10;
            int Year = 2021;

            ProjectedDatum pro;
            ProjectedDatum newPro;
            int index = 1;
            ProjectedDatum HasActual = null;
            bool first = true;

            while (!(Month == 4 && Year == 2023))
            {
                Month = (Month + 1) % Months.Length;
                if (Month == 0)
                    Year++;

                pro = Goals[index - 1];

                newPro = new ProjectedDatum(pro, Months[Month], Year);

                string ThisMonthYear = (Months[Month] + " " + Year.ToString());

                SavingsGoal ThisMonthPerson1 = Constants.SavingsGoals.FirstOrDefault(sg => sg.Person == Constants.Person1 && sg.MonthYear() == ThisMonthYear);
                SavingsGoal ThisMonthPerson2 = Constants.SavingsGoals.FirstOrDefault(sg => sg.Person == Constants.Person2 && sg.MonthYear() == ThisMonthYear);

                HasActual = ActualValues.FirstOrDefault(av => av.MonthYear() == (Months[Month] + " " + Year.ToString()));
                if (HasActual != null)
                {
                    newPro.Person1Projected += ThisMonthPerson1 == null ? Default1 == null ? 0 : Default1.Goal : ThisMonthPerson1.Goal;
                    if (Constants.Person2 == "Lindsey")
                    {
                        if (Month < 5 && Year == 2022)
                            newPro.Person2Projected += ThisMonthPerson1 == null ? Default2 == null ? 0 : 1715 : ThisMonthPerson2.Goal;
                        else
                            newPro.Person2Projected += ThisMonthPerson1 == null ? Default2 == null ? 0 : Default2.Goal : ThisMonthPerson2.Goal;
                    }
                    newPro.Person1Actual = HasActual.Person1Actual;
                    newPro.Person2Actual = HasActual.Person2Actual;
                }
                else
                {
                    newPro.Person1Projected = pro.Person1Projected + 2000;
                    newPro.Person2Projected = pro.Person2Projected + 1715;

                    newPro.Person1Actual = pro.Person1Actual;
                    newPro.Person2Actual = pro.Person2Actual;
                    newPro.Person1Actual += ThisMonthPerson1 == null ? Default1 == null ? 0 : Default1.Goal : ThisMonthPerson1.Goal;
                    newPro.Person2Actual += ThisMonthPerson1 == null ? Default2 == null ? 0 : Default2.Goal : ThisMonthPerson2.Goal;
                    SetAnticipatedExpense(ref newPro);
                    newPro.IsProjected = true;


                    if (first)
                    {
                        MonthYear = Months[Month] + " " + Year;
                        first = false;
                    }
                }

                Goals.Add(newPro);
                index++;
            }
        }
        private static void SetAnticipatedExpense(ref ProjectedDatum pd)
        {
            if (Monthlies == null || Monthlies.Count == 0)
                Monthlies = MonthlyRepo.GetMonthlies();

            List<DynamicCostItem> tempDCI = new List<DynamicCostItem>();
            List<StaticMonthlyCost> tempStatic = new List<StaticMonthlyCost>();

            foreach (var mon in Monthlies)
            {
                if (CheckMonthYear(mon, pd.MonthYear()))
                {
                    if (mon.Dynamic == "DYNAMIC")
                    {
                        var DCIExists = tempDCI.FirstOrDefault(d => d.Name == mon.Name);
                        var NewDCI = new DynamicCostItem(mon.Name, mon.Person1Amount, mon.Person2Amount);

                        if (DCIExists == null)
                            tempDCI.Add(NewDCI);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                tempDCI.Remove(DCIExists);
                                tempDCI.Add(NewDCI);
                            }
                        }

                    }
                    else if (mon.Dynamic == "STATIC")
                    {
                        var SMCExists = tempStatic.FirstOrDefault(s => s.Name == mon.Name);
                        var NewSMC = new StaticMonthlyCost(mon.Name, mon.Person1Amount, mon.Person2Amount);

                        if (SMCExists == null)
                            tempStatic.Add(NewSMC);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                tempStatic.Remove(SMCExists);
                                tempStatic.Add(NewSMC);
                            }
                        }
                    }
                }
            }

            foreach (var dci in tempDCI)
                pd.AnticipatedExpense += dci.Amount;

            foreach (var stat in tempStatic)
                pd.AnticipatedExpense += stat.TotalAmount;
        }
        public static bool CheckMonthYear(Monthly mon, string currentMonthYear)
        {
            return mon.MonthYear() == currentMonthYear || mon.MonthYear().Contains("All");
        }
    }
}
