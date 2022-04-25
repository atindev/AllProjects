using System;

namespace Test123
{
    public class P1 : P3
    {
        public P1(int v)
        {
            V = v;
        }

        /// <summary>
        /// Gets the v.
        /// </summary>
        /// <value>
        /// The v.
        /// </value>
        public int V { get; }

        /// <summary>
        /// Asds the virtual.
        /// </summary>
        /// <returns></returns>
        public virtual void asd_virtual()
        {
            Console.WriteLine("P1_Virtual");
        }

        /// <summary>
        /// Asds the sealed.
        /// </summary>
        /// <returns></returns>
        ////public sealed void asd_sealed()
        ////{
        ////    Console.WriteLine("Sealed");
        ////}

        /// <summary>
        /// Asds the default.
        /// </summary>
        public void asd_default()
        {
            Console.WriteLine("P1_Default" + V);
        }

        /// <summary>
        /// Asds the abstract.
        /// </summary>
        public override void asd_abstract()
        {
            Console.WriteLine("P1_Abstract" + V);
        }
    }
}
