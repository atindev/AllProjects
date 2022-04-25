using System;

namespace CreateDataMES
{
    class PrecountCalc
    {
        public void Check()
        {
            for (long i = 1; i < 1000; i++)
            {
                if (CalculationLogic(i) == "0")
                {
                    Console.WriteLine("FOUND:" + i);
                    break;
                }
            }
        }

        private string CalculationLogic(long input)
        {
            int index;
            int mult = 3;
            long sum = 0;

            index = input.ToString().Length;

            while (index > 0)
            {
                sum += mult * Convert.ToInt64(input.ToString()[index - 1].ToString());
                ////sum += mult * Convert.ToInt64(input.ToString().Substring(index - 1, 1));
                mult = 4 - mult;
                index--;
            }

            if (sum % 10 == 0)
            {
                return "0";
            }
            else
            {
                return (10 - (sum % 10)).ToString();
            }
        }
    }
}
