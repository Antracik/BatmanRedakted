using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BatmanRedakted
{
    class Program
    {

        private static readonly object padlock = new object();

        static void Main()
        {
            var (total, _) = GetCodeParrlel();

            Console.WriteLine($"FINAL NUMBER OF ATTEMPTS BEFORE CODE FOUND {total}");
            
        }

        public static IEnumerable<string> GeneratePermutations()
        {
            var list = new List<string>();

            char start = '0', end = '9';

            for (char first = start; first <= (int)end; first++)
                for (char second = start; second <= (int)end; second++)
                    for (char third = start; third <= (int)end; third++)
                        list.Add($"{first}{second}{third}");

            return list;
        }

        public static (int attempts, string code) GetCodeParrlel()
        {
            HttpClient client = new HttpClient();
            var perms = GeneratePermutations();
            int total = 0;
            string codeFinal = "";
            string comboCode = "";

            for (int i = 1; i <= 4; i++)
            {
                Parallel.ForEach(perms, 
                    (code, state) =>
                     {
                         lock (padlock)
                         {
                             if (total % 1000 == 0)
                                 Console.WriteLine($"Attempts so far {total}");

                             Interlocked.Increment(ref total);
                         }

                         var response = client.GetAsync($"https://r3dakt3d.com/check?number={i}&code={code}&comboCode={comboCode}").Result;
                         string content = response.Content.ReadAsStringAsync().Result;
                         var json = JsonSerializer.Deserialize<ResponseJson>(content);

                         if (json.success)
                         {
                             Interlocked.Exchange(ref codeFinal, code);
                             state.Break();
                         }

                     }
                 );

                if (codeFinal != "")
                    Console.WriteLine($"Day {i} code => {codeFinal}");
                else
                {
                    Console.WriteLine($"Could not find code for day {i}, aborting...");
                    break;
                }
                comboCode += codeFinal;
                codeFinal = "";
            }

            return (total, codeFinal);

        }

    }
}
