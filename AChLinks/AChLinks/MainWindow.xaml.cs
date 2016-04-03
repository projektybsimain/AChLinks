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
using System.Data;
using System.Drawing;
using System.Windows.Controls.DataVisualization.Charting;




namespace AChLinks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string []table = { "%COM", "%UK", "%ORG", "%NET", "%INFO", "%GOV", "%"};
        public List<string> list;
        public int[] x;
        public MainWindow()
        {
            InitializeComponent();
            combo();
            panel_start();
            statistic();
        }

        public void statistic()
        {
            SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
           
            x = new int[7];
                  
            for (int j = 0; j < x.Length; j++)
            {
                SqlCommand sql = mySql.CreateCommand();
                mySql.Open();
                sql.Parameters.Add("@Name", SqlDbType.NChar).Value = table[j];
                sql.CommandText = "Execute countLinks @Name";
                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    string c = String.Format("{0}", reader[0]);
                    x[j] = Int32.Parse(c);
                }
                mySql.Close();
            }
            com.Content = "COM = " + x[0];
            uk.Content = "UK = " + x[1];
            org.Content = "ORG = " + x[2];
            net.Content = "NET = " + x[3];
            info.Content = "INFO = " + x[4];
            gov.Content = "GOV = " + x[5];
        }

        public void panel_start()
        {
            this.start.Visibility = System.Windows.Visibility.Visible;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void combo()
         {
             SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
             SqlCommand sql = mySql.CreateCommand();
             SqlDataReader reader;
             list = new List<string>();
            try {
                 mySql.Open();
                 sql.CommandText = "select Name from Domains";
                 reader = sql.ExecuteReader();
                 while (reader.Read())
                 {
                     string sName = reader.GetString(0);
                     list.Add(sName);
                 }
                //textBox.AppendText(list[2]);
                 search.ItemsSource = list;
            }
               
             catch (Exception ex)
             {

             }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Visible;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Visible;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Visible;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Visible;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Visible;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void mcChart_Loaded(object sender, RoutedEventArgs e)
        {
            ((PieSeries)mcChart.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>("COM", x[0]),
            new KeyValuePair<string,int>("UK", x[1]),
            new KeyValuePair<string,int>("ORG", x[2]),
            new KeyValuePair<string,int>("NET", x[3]),
            new KeyValuePair<string,int>("INFO", x[4]),
            new KeyValuePair<string,int>("GOV", x[5]),
            new KeyValuePair<string,int>("OTHER", x[6]-x[5]-x[4]-x[3]-x[2]-x[1]-x[0]) };
        }

        private void donofollow_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
            int[] tab = new int[2];
            int i = 0;
            for (int j = 0; j < tab.Length; j++)
            {
                SqlCommand sql = mySql.CreateCommand();
                mySql.Open();
                sql.Parameters.Add("@Name", SqlDbType.NChar).Value = i;
                sql.CommandText = "Execute countDoNoFollow @Name";
                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    string c = String.Format("{0}", reader[0]);
                    tab[j] = Int32.Parse(c);
                }
                mySql.Close();
                i++;
            }

            ((BarSeries)donofollow.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>("NoFollow", tab[0]),
            new KeyValuePair<string,int>("DoFollow", tab[1]) };
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            this.donofollow.Visibility = System.Windows.Visibility.Collapsed;
            this.labeldonofollow.Visibility = System.Windows.Visibility.Collapsed;
            this.mcChart_Copy.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
