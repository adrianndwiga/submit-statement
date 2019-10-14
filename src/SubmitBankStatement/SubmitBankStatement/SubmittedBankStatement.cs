namespace SubmitBankStatement
{
    public enum Bank
    {
        Barclays, 
        Santander,
        CapitalOne,
        BarclayCard
    }

    public enum AccountType
    {
        PersonalCurrentAccount,
        BusinessCurrentAccount,
        PersonalCreditCard
    }

    public class SubmittedBankStatement
    {
        public string Name { get; set; }
        public Bank Bank { get; set; }
        public AccountType AccountType { get; set; }
        public string Content { get; set; }
    }

    public class SubmittedBankStatementQueueItem
    {
        public SubmittedBankStatementQueueItem(SubmittedBankStatement submittedBankStatement)
        {
            Name = submittedBankStatement.Name;
            Bank = submittedBankStatement.Bank;
            AccountType = submittedBankStatement.AccountType;
        }

        public string Name { get; private set; }
        public Bank Bank { get; private set; }
        public AccountType AccountType { get; private set; }
    }
}