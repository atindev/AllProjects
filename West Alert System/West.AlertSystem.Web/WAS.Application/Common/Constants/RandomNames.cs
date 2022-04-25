
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Constants
{
    public static class RandomNames
    {
        private static readonly List<string> FirstName = new List<string>() {
                 "Doreatha","Kiara","Larhonda","Lila","Karlene",
                 "Milford","Rutha","Adelaida","Maribeth","Kortney",
                 "Lee","Kathleen","Stuart","Jitendra","Darren",
                 "Jordan","Wayne","Julie","Asvathama","Aditi"
                };
        private static readonly List<string> LastName = new List<string>() {
                 "Poduri","Ilyas","Salim","Sritharan","Trilochana",
                 "Suksma","Dudding","Bold","Tellis","Gayles",
                 "Everts","Vergara","Hulbert","Sayler","Mitali",
                 "Ghosal","Quan","Phinney","Hulbert","Xiang"
                };

        public static List<string> getRandomNames(string type)
        {
            if (type == "firstName")
                return FirstName;
            else
                return LastName;
        }
    }
}
