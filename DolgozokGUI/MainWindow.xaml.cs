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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DolgozokGUI.Views;
using DolgozokNyilvantartasPluszGUI.Classes;

namespace DolgozokGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ApiUrl = "https://retoolapi.dev/Kc6xuH/data";
        private HttpClient _httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            FetchDolgozok();
        }

        private async void FetchDolgozok()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(ApiUrl);
                var dolgozok = JsonConvert.DeserializeObject<List<Dolgozo>>(response);
                dgDolgozok.ItemsSource = dolgozok;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az adatok lekérésekor: {ex.Message}");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addDolgozoWindow = new NewDolgozo();
            addDolgozoWindow.ShowDialog();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgDolgozok.SelectedItem is Dolgozo selectedDolgozo)
            {
                var editWindow = new EditDolgozo(selectedDolgozo);
                editWindow.ShowDialog();
                FetchDolgozok(); 
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy dolgozót a szerkesztéshez!", "Szerkesztés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedDolgozok = dgDolgozok.SelectedItems.Cast<Dolgozo>().ToList();

            if (selectedDolgozok == null || selectedDolgozok.Count == 0)
            {
                MessageBox.Show("Kérlek, válassz ki legalább egy dolgozót a törléshez!", "Törlés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var messageBoxResult = MessageBox.Show($"Valóban szeretnéd törölni a kiválasztott dolgozókat ({selectedDolgozok.Count} db)?", "Dolgozók törlése", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var dolgozo in selectedDolgozok)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"{ApiUrl}/{dolgozo.Id}");
                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Hiba történt {dolgozo.Name} dolgozó törlésekor.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hiba: {ex.Message}", "Kivétel", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("A kiválasztott dolgozók sikeresen törölve.", "Sikeres törlés", MessageBoxButton.OK, MessageBoxImage.Information);
                FetchDolgozok();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            FetchDolgozok();
        }
    }
}
