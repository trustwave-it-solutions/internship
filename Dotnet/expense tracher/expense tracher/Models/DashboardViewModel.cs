namespace expense_tracher.Models
{
    public class DashboardViewModel
    {
        public string UserName { get; set; }
        public List<ExpenseViewModel> Expenses { get; set; } = new List<ExpenseViewModel>();
        public List<IncomeViewModel> Incomes { get; set; } = new List<IncomeViewModel>();

        public double TotalExpense
        {
            get
            {
                return (double)Expenses.Sum(e => e.Amount);
            }
        }
        public double TotalIncome
        {
            get
            {
                return (double)Incomes.Sum(i => i.Amount);
            }
        }
    }
}
