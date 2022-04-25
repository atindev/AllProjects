namespace Test123
{
    public class P2 : P1
    {
        public P2() : base(1)
        {

        }

        public const int x = 5;

        public override void asd_abstract()
        {
            System.Console.WriteLine("P2_abstract" + x);
        }

        public override void asd_virtual()
        {
            System.Console.WriteLine("P2_virtual");
        }

        new public void asd_default()
        {
            System.Console.WriteLine("P2_Default");
        }
    }
}
