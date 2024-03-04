using DolgozokNyilvantartasPluszGUI.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DolgozokNyilvantartasPluszGUI
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var url = "https://retoolapi.dev/Kc6xuH/data";
            var dolgozok = await GetDolgozokAsync(url);

            if (dolgozok != null && dolgozok.Any())
            {
                // 1. Feladat: Kiírja az elemek számát
                Console.WriteLine($"Dolgozók száma: {dolgozok.Count}");

                // 2. Feladat: Kiírja a legmagasabb fizetéssel rendelkező dolgozó nevét
                var legmagasabbFizetesu = dolgozok.OrderByDescending(d => d.Salary).First();
                Console.WriteLine($"A legmagasabb fizetéssel rendelkező dolgozó: {legmagasabbFizetesu.Name}");
            }
            else
            {
                Console.WriteLine("Nincs dolgozó az adatbázisban.");
            }

            // 3. Feladat: Kiírja az egyes munkakörökben hányan dolgoznak

            var poziciok = new List<string> { "CMO", "DevOps Engineer", "Internal Tools Lead" };

            if (poziciok != null && poziciok.Any())
            {
                foreach (var pozicio in poziciok)
                {
                    var dolgozokSzama = await GetDolgozokSzamaPozicioSzerintAsync(pozicio);
                    Console.WriteLine($"{pozicio}: {dolgozokSzama} dolgozó");
                }
            }
            else
            {
                Console.WriteLine("Nincs pozíció adatbázisban.");
            }

            Console.ReadKey();
        }

        public static async Task<List<Dolgozo>> GetDolgozokAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(url);
                    var dolgozok = JsonConvert.DeserializeObject<List<Dolgozo>>(response);
                    return dolgozok;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba történt az adatok lekérése közben: {ex.Message}");
                    return null;
                }
            }
        }

        public static async Task<int> GetDolgozokSzamaPozicioSzerintAsync(string pozicio)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"https://retoolapi.dev/Kc6xuH/data?position={pozicio}";
                try
                {
                    var response = await httpClient.GetStringAsync(url);
                    var dolgozok = JsonConvert.DeserializeObject<List<Dolgozo>>(response);
                    return dolgozok.Count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba történt az adatok lekérése közben: {ex.Message}");
                    return 0;
                }
            }
        }


    }
}
