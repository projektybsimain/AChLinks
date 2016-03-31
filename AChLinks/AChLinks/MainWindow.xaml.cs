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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace AChLinks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
            //combo();
        }



        /* private void combo()
         {
             SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
             SqlCommand sql = mySql.CreateCommand();
             SqlDataReader reader;

             try {
                 mySql.Open();
                 sql.CommandText = "select * from VisitedUrls";
                 reader = sql.ExecuteReader();
                 while (reader.Read())
                 {
                     string sName = reader.GetString(0);
                     comboBox.Items.Add(sName);
                 }
             }
             catch(Exception ex)
             {

             }
         }*/

        

        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (start.Visibility == System.Windows.Visibility.Visible)
            {
                this.start.Visibility = System.Windows.Visibility.Collapsed;
                this.start_2.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.start_2.Visibility = System.Windows.Visibility.Collapsed;
                this.start.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Visible;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Visible;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Visible;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
