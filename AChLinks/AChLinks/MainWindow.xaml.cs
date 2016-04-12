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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


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
        public string result;
        public int incoming_number;
        public int out_number;
        public string platforma_out;
        SqlConnection mySql;
        public MainWindow()
        {
            InitializeComponent();
            autobox_start();
            panel_start();
            statistic();
            mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
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

            ((PieSeries)mcChart.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>("COM", x[0]),
            new KeyValuePair<string,int>("UK", x[1]),
            new KeyValuePair<string,int>("ORG", x[2]),
            new KeyValuePair<string,int>("NET", x[3]),
            new KeyValuePair<string,int>("INFO", x[4]),
            new KeyValuePair<string,int>("GOV", x[5]),
            new KeyValuePair<string,int>("OTHER", x[6]-x[5]-x[4]-x[3]-x[2]-x[1]-x[0]) };

           
            try
            {
                int liczba = 0;
                mySql.Open();
                SqlCommand sql2 = mySql.CreateCommand();
                sql2.CommandText = "select count(distinct Anchor) from Backlinks";
                SqlDataReader reader2 = sql2.ExecuteReader();
               while (reader2.Read())
                {
                    string anchor_l = reader2[0].ToString();
                    liczba = int.Parse(anchor_l);
                }
                mySql.Close();
                anchor.Content = "anchorów:" + liczba;
            }

            catch (Exception ex)
            {

            }
        }

        public void panel_start()
        {
            this.start.Visibility = System.Windows.Visibility.Visible;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
            this.Not_found_.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void autobox_start()
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

        private bool autobox_check()
        {
            result = search.Text.ToString();
            if (result == "")
            {
                this.Not_found_.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            if (!list.Exists(x => x == result))
            {
                this.Not_found_.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            return true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (autobox_check() == false)
            {
                return;
            }

            incoming();
            outgoing();
        }

        private void incoming()
        {
            incoming_number = 0;
            name_link.Content = "Domena: " + result;
            sub_dom_name_label.Content = "Subdomena: " + sub_Domains();
            SqlCommand sql = mySql.CreateCommand();
            mySql.Open();
            sql.Parameters.Add("@Name", SqlDbType.NChar).Value = result;
            sql.CommandText = "Execute add_incoming @Name";
            SqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                string temp = String.Format("{0}", reader[0]);
                listBox_incoming.Items.Add(temp);
                incoming_number++;
            }
            mySql.Close();
            incoming_number_label.Content = "Liczba linków przychodzących o danej domenie: " + incoming_number;
            i_p_l();


        }

        private void outgoing()
        {
            out_number = 0;
            name_link_outgoing.Content = "Domena: " + result;
            sub_dom_name_label_outgoing.Content = "Subdomena: " + sub_Domains();
            SqlCommand sql = mySql.CreateCommand();
            mySql.Open();
            sql.Parameters.Add("@Name2", SqlDbType.NChar).Value = result;
            sql.CommandText = "Execute add_outgoing @Name2";
            SqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                string temp = String.Format("{0}", reader[0]);
                listBox_outgoing.Items.Add(temp);
                out_number++;
            }
            mySql.Close();
            outgoing_number_label.Content= "Liczba linków wychodzących o danej domenie: " + out_number;
            o_p_l();

        }

        private void i_p_l()
        {
            string p_inc = "";
            SqlCommand sql = mySql.CreateCommand();
            mySql.Open();
            sql.Parameters.Add("@Name2", SqlDbType.NChar).Value = result;
            sql.CommandText = "Execute inc_platform @Name2";
            SqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                string temp = String.Format("{0}", reader[0]);
                p_inc = temp;
            }
            mySql.Close();
            if (p_inc == "")
            {
                p_inc = "Brak informacji";
            }
            platform_incoming_label.Content = "Platforma: " + p_inc;
        }

        private void o_p_l()
        {
            string p_out = "";
            SqlCommand sql = mySql.CreateCommand();
            mySql.Open();
            sql.Parameters.Add("@Name2", SqlDbType.NChar).Value = result;
            sql.CommandText = "Execute out_platform @Name2";
            SqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                string temp = String.Format("{0}", reader[0]);
                p_out = temp;
            }
            mySql.Close();
            if (p_out=="")
            {
                p_out = "Brak informacji";
            }
            platforma_out_Label.Content = "Platforma: " + p_out;
        }


        private string sub_Domains()
        {
            string temp="";
            SqlCommand sql = mySql.CreateCommand();
            mySql.Open();
            sql.Parameters.Add("@Name", SqlDbType.NChar).Value = result;
            sql.CommandText = "Execute check_domain @Name";
            SqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
               temp = String.Format("{0}", reader[0]);
            }
            mySql.Close();
            if (temp == "")
            {
                return "Brak Subdomeny";
            }
            return temp;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Visible;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
            this.Not_found_.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (autobox_check() == false)
            {
                return;
            }
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Visible;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
            this.Not_found_.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (autobox_check() == false)
            {
                return;
            }
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Visible;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
            this.Not_found_.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Visible;
            this.start_5.Visibility = System.Windows.Visibility.Collapsed;
            this.Not_found_.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            this.start.Visibility = System.Windows.Visibility.Collapsed;
            this.start_2.Visibility = System.Windows.Visibility.Collapsed;
            this.start_3.Visibility = System.Windows.Visibility.Collapsed;
            this.start_4.Visibility = System.Windows.Visibility.Collapsed;
            this.start_5.Visibility = System.Windows.Visibility.Visible;
            this.Not_found_.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void mcChart_Loaded(object sender, RoutedEventArgs e)
        {
           /* ((ScatterSeries)mcChart.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>("COM", x[0]),
            new KeyValuePair<string,int>("UK", x[1]),
            new KeyValuePair<string,int>("ORG", x[2]),
            new KeyValuePair<string,int>("NET", x[3]),
            new KeyValuePair<string,int>("INFO", x[4]),
            new KeyValuePair<string,int>("GOV", x[5]),
            new KeyValuePair<string,int>("OTHER", x[6]-x[5]-x[4]-x[3]-x[2]-x[1]-x[0]) };*/
        }

        private void LinkType_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
            int[] tab = new int[7];
            string[] temp = { "Other", "Profile", "Comment", "Guestbook", "Article", "Wiki", "SocialNetwork" };
            for (int j = 0; j < tab.Length; j++)
            {
                SqlCommand sql = mySql.CreateCommand();
                mySql.Open();
                sql.Parameters.Add("@Name", SqlDbType.NChar).Value = temp[j];
                sql.CommandText = "Execute countLinkType @Name";
                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    string c = String.Format("{0}", reader[0]);
                    tab[j] = Int32.Parse(c);
                }
                mySql.Close();
            }

            ((BarSeries)LinksType_chart.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>(temp[0], tab[0]),
            new KeyValuePair<string,int>(temp[1], tab[1]),
            new KeyValuePair<string,int>(temp[2], tab[2]),
            new KeyValuePair<string,int>(temp[3], tab[3]),
            new KeyValuePair<string,int>(temp[4], tab[4]),
            new KeyValuePair<string,int>(temp[5], tab[5]),
            new KeyValuePair<string,int>(temp[6], tab[6]) };
        }

        private void Inc_out_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
            string[] tab = { "SourceDomainID", "TargetDomainID" };
            int[] i = new int[2];
            for (int j = 0; j < tab.Length; j++)
            {
                SqlCommand sql = mySql.CreateCommand();
                mySql.Open();
                if (j == 0)
                {
                    sql.CommandText = "select count(distinct SourceDomainID) from Backlinks";
                }
                else
                {
                    sql.CommandText = "select count(distinct TargetDomainID) from Backlinks";
                }

                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    String c = String.Format("{0}", reader[0]);
                    i[j] = Int32.Parse(c);
                }
                mySql.Close();
            }

            ((BubbleSeries)Inc_out.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>("Incoming", i[0]),
            new KeyValuePair<string,int>("Outgoing", i[1]) };
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

            dofoll_label.Content = "DoFollow : " + tab[0];
            nofoll_label.Content = "Nofollow : " + tab[1];

            ((BarSeries)donofollow.Series[0]).ItemsSource =
            new KeyValuePair<string, int>[]{
            new KeyValuePair<string,int>("NoFollow", tab[0]),
            new KeyValuePair<string,int>("DoFollow", tab[1]) };
        }


        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (Inc_out.Visibility == Visibility)
            {
                this.Inc_out.Visibility = System.Windows.Visibility.Hidden;
                this.Inc_out_label.Visibility = System.Windows.Visibility.Hidden;
                this.mcChart.Visibility = System.Windows.Visibility.Hidden;
                this.mcChartlabel.Visibility = System.Windows.Visibility.Hidden;

                this.LinksType_chart.Visibility = System.Windows.Visibility.Visible;
                this.labeldonofollow.Visibility = System.Windows.Visibility.Visible;
                this.donofollow.Visibility =System.Windows.Visibility.Visible;
                this.LinksType.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.Inc_out.Visibility = System.Windows.Visibility.Visible;
                this.Inc_out_label.Visibility = System.Windows.Visibility.Visible;
                this.mcChart.Visibility = System.Windows.Visibility.Visible;
                this.mcChartlabel.Visibility = System.Windows.Visibility.Visible;

                this.LinksType_chart.Visibility = System.Windows.Visibility.Hidden;
                this.labeldonofollow.Visibility = System.Windows.Visibility.Hidden;
                this.donofollow.Visibility = System.Windows.Visibility.Hidden;
                this.LinksType.Visibility = System.Windows.Visibility.Hidden;
            }



            
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Test.pdf", FileMode.Create));
            iTextSharp.text.Image obrazek = iTextSharp.text.Image.GetInstance("logo-pz.png");
            obrazek.SetAbsolutePosition(doc.PageSize.Width - 570 , doc.PageSize.Height - 100 );
            obrazek.ScalePercent(30f);
            obrazek.Border = iTextSharp.text.Rectangle.BOX;
            obrazek.BorderWidth = 5;

            doc.Open();
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("Wygenerowany raport", FontFactory.GetFont("Courier", 32, BaseColor.BLACK));
            iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph("eqeqwewqeqw raport", FontFactory.GetFont("Courier", 12, BaseColor.BLACK));

            doc.Add(obrazek);
            doc.Add(paragraph);
            doc.Add(paragraph2);

            doc.Close();
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Test.pdf", FileMode.Create));
            doc.Open();
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("Mój wlasny test");

            doc.Add(paragraph);

            doc.Close();
        }

        private void listBox_incoming_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listBox_outgoing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
