using System.Windows;
using System.Windows.Controls;

namespace Cine
{
    /// <summary>
    /// Lógica interna para CinemaScreen.xaml
    /// </summary>
    public partial class CinemaScreen : Window
    {
        public CinemaScreen()
        {
            InitializeComponent();
        }

        public void btn_Salvar(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
