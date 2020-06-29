using System.Windows.Controls;


namespace Statistics.Views.Interface
{
    /// <summary>
    /// Logique d'interaction pour StatisticsButton.xaml
    /// </summary>
    public partial class StatisticsButtonHeader : Button
    {
        public StatisticsButtonHeader(string Content)
        {
            InitializeComponent();

            btHeaderName.Text = Content;
        }
    }
}
