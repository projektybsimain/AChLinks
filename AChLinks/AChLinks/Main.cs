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
    public partial class Main : MainWindow
    {
        public Main()
        {
            //SqlConnection mySql = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Crawler; Integrated Security=SSPI");
            //combo();
        }

        public void combo()
        {
            comboBox.Items.Add("a");
            comboBox.Items.Add('c');
        }
    }
}
