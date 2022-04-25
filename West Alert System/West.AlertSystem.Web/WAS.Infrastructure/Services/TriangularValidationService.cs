using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Constants;
using WAS.Application.Interface.Services;


namespace WAS.Infrastructure.Services
{
    public class TriangularValidationService : ITriangularValidationService
    {
        static Random randomIndex = new Random();

        public List<string> GetStaticRandomNames()
        {
            List<string> list = new List<string>();
            int firstIndex = 0, lastIndex = 0;
            string name="";
            for (int i = 0; i < 7; i++)
            {
                firstIndex = randomIndex.Next(RandomNames.getRandomNames("firstName").Count);
                lastIndex = randomIndex.Next(RandomNames.getRandomNames("").Count);
                name = RandomNames.getRandomNames("firstName")[firstIndex] + "." +RandomNames.getRandomNames("")[lastIndex]+ "@westpharma.com";
                list.Add(name);
            }
            return list;
        }

        public List<string> GetSelectedRandomNames(List<string> inputList,int count,bool includeStaticNames=true)
        {
            var selected = new List<string>();
            var randomStaticList= new List<string>();
            if (includeStaticNames)
            {
                randomStaticList = GetStaticRandomNames();
                if (inputList != null && inputList.Count > 0)
                    randomStaticList.AddRange(inputList);
            }
            else
                randomStaticList = inputList;

            double needed = count;
            double available = randomStaticList.Count;
            var rand = new Random();
            while (selected.Count < count)
            {
                if (rand.NextDouble() < needed / available)
                {
                    selected.Add(randomStaticList[(int)available - 1]);
                    needed--;
                }
                available--;
            }

            return selected;
        }

        public List<string> InsertToRandomPosition(List<string> inputList, string element)
        {
            if(inputList!=null && inputList.Count > 0)
            {
                int listIndex = randomIndex.Next(inputList.Count);
                inputList.Insert(listIndex, element);
            }
            return inputList;
        }


    }
}
