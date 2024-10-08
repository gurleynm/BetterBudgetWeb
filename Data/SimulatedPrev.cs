﻿namespace BetterBudgetWeb.Data
{
    public class MonthViewPrev
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MonthType { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string JSONPrev { get; set; }
        public MonthViewPrev() { }
    }
}
