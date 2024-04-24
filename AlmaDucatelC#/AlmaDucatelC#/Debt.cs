public class Debt
{
    public int DebtNumber { get; set; }
    public string FullName { get; set; }
    public decimal Amount { get; set; }
    public int InterestRate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsDeleted { get; set; }

    public Debt(int debtNumber, string fullName, decimal amount, int interestRate, DateTime registrationDate)
    {
        DebtNumber = debtNumber;
        FullName = fullName;
        Amount = amount;
        InterestRate = interestRate;
        RegistrationDate = registrationDate;
    }
}
