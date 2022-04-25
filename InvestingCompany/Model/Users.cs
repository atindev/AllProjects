namespace InvestingCompany.Model
{
    public class Users : BasicDetails
    {
        public int MaxLimit { get; set; }

        public virtual ContractDetails gthds { get; set; }
    }
}
