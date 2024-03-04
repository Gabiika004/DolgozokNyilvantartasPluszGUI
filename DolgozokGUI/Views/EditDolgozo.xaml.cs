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
    /// Interaction logic for EditDolgozo.xaml
    /// </summary>
    public partial class EditDolgozo : Window
    {
        private Dolgozo _currentDolgozo;
        private HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://retoolapi.dev/Kc6xuH/data";

        public EditDolgozo(Dolgozo dolgozo)
        {
            InitializeComponent();
            _currentDolgozo = dolgozo;
            LoadDataIntoFields();
        }

        private void LoadDataIntoFields()
        {
            txtName.Text = _currentDolgozo.Name;
            txtSalary.Text = _currentDolgozo.Salary.ToString();
            txtPosition.Text = _currentDolgozo.Position;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSalary.Text) ||
                string.IsNullOrWhiteSpace(txtPosition.Text))
            {
                MessageBox.Show("Minden mező kitöltése kötelező!", "Hiányos adat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool salaryIsValid = int.TryParse(txtSalary.Text, out int salary) && salary > 0;
            if (!salaryIsValid)
            {
                MessageBox.Show("A fizetésnek pozitív egész számnak kell lennie!", "Érvénytelen adat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentDolgozo.Name = txtName.Text;
            _currentDolgozo.Salary = salary;
            _currentDolgozo.Position = txtPosition.Text;

            var json = JsonConvert.SerializeObject(_currentDolgozo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync($"{ApiUrl}/{_currentDolgozo.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("A dolgozó adatai sikeresen frissítve.", "Sikeres Módosítás", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hiba történt a dolgozó adatainak frissítése közben.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba: {ex.Message}", "Kivétel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
