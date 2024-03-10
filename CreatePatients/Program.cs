using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CreatePatients
{
    internal class Program
    {
        #region Static
        private static readonly HttpClient client = new HttpClient();
        #endregion

        #region Enum
        public enum Gender
        {
            Male = 0,
            Female,
            Other,
            Unknown
        }
        #endregion

        #region Classes
        private class Patient
        {
            #region Properties
            public Guid Id { get; set; }
            public HumanName Name { get; set; }
            public Gender? Gender { get; set; }
            public DateTime BirthDate { get; set; }
            public bool? Active { get; set; }
            #endregion

        }

        private class HumanName
        {
            #region Properties
            public Guid Id { get; private set; }
            public string Use { get; set; }
            public string Family { get; set; }
            public List<string> Given { get; set; }
            #endregion
        }
        #endregion

        #region Assistents
        private static void Main(string[] args)
        {
            Program.Run().GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            Console.WriteLine("Press [Enter] to run...");

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Enter)
                return;

            Console.WriteLine();

            Console.WriteLine("**************START**************");

            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Random random = new Random();

            try
            {
                for (int index = 1; index <= 100; index++)
                {
                    Patient patient = new Patient
                    {
                        Name = new HumanName
                        {
                            Use = "official",
                            Family = $"Name - {index}",
                            Given = new List<string> { $"Test {index}", $"Test {index + 1}" }
                        },
                        BirthDate = DateTime.UtcNow.AddYears(-index),
                        Gender = (Gender)random.Next(0, 3),
                        Active = index % 2 == 0,
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Patients", patient);

                    Console.WriteLine($"{response.StatusCode} : {patient.Name.Family}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("**************END**************");

            Console.ReadLine();
        }
        #endregion
    }
}
