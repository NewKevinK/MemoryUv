using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Ranking.xaml
    /// </summary>
    public partial class Ranking : Window
    {

        UserGame userGame = new UserGame();
        MemoryServer service;

        public Ranking(UserGame _user)
        {
            InitializeComponent();
            InitializeListRank();
            userGame = _user;
        }

        private void InitializeListRank()
        {
            service = new MemoryServer();
            List<StatisticUser> itemListView = new List<StatisticUser>();
            List<StatisticUser> itemStatic = service.GetBetterUser();

            for (int i = 0; i < itemStatic.Count(); i++)
            {
                itemListView.Add(new StatisticUser() { id = i + 1, nameTag = itemStatic[i].nameTag, totalWins = itemStatic[i].totalWins });
            }

            listRank.ItemsSource = itemListView;
        }

        private void RegresarClick(object sender, RoutedEventArgs e)
        {
            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
        }
    }
}
