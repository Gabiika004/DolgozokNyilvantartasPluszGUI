using DolgozokNyilvantartasPluszGUI.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DolgozokGUI.Views
{
    /// <summary>
    /// Interaction logic for NewDolgozo.xaml
    /// </summary>
    public partial class NewDolgozo : Window
    {
        private HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://retoolapi.dev/Kc6xuH/data";

        public NewDolgozo()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            // Ellenőrizzük, hogy a szükséges mezők ki vannak-e töltve
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSalary.Text) ||
                string.IsNullOrWhiteSpace(txtPosition.Text))
            {
                MessageBox.Show("Minden mező kitöltése kötelező!", "Hiányos adat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ellenőrizzük, hogy a fizetés pozitív szám-e
            if (!int.TryParse(txtSalary.Text, out int salary) || salary <= 0)
            {
                MessageBox.Show("A fizetésnek pozitív számnak kell lennie!", "Érvénytelen adat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ha minden ellenőrzés sikeres, létrehozzuk a Dolgozo objektumot
            var dolgozo = new Dolgozo
            {
                Name = txtName.Text,
                Salary = salary,
                Position = txtPosition.Text
            };

            // API hívás a dolgozó hozzáadására
            var json = JsonConvert.SerializeObject(dolgozo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(ApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Dolgozó sikeresen hozzáadva.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Ablak bezárása
                }
                else
                {
                    MessageBox.Show("Hiba történt a dolgozó hozzáadása közben.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba: {ex.Message}", "Kivétel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Ablak bezárása
        }
    }
}
