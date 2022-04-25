using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Language : Entity
    {
        /// <summary>
        /// Primary key of language
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// language code
        /// </summary>
        public string Code { get; set; }

    }
}
