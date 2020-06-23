using System.Windows.Controls;


namespace Statistics.Views.Interface
{
    /// <summary>
    /// Logique d'interaction pour StatisticsButton.xaml
    /// </summary>
    public partial class StatisticsButton : Button
    {
        public StatisticsButton(string Content)
        {
            InitializeComponent();

            sBtName.Text = Content;
        }
    }
}
