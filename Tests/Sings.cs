namespace Tests
{
    public class Sings
    {
        private static Sings obj;

        private Sings()
        {

        }

        public static Sings GetObj()
        {
            obj ??= new Sings();
            return obj;
        }
    }
}
