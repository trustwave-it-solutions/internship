namespace expense_tracher.Models
{
    public class IncomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }

        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int PaymentModeId { get; set; }
        public string? PaymentMode { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public List<CategoryViewModel> categoryList { get; set; } = new List<CategoryViewModel>();
    }
}
