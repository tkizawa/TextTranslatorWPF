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

namespace TextTranslatorWPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtText.Text))
            {
                var result=MessageBox.Show("テキストを入力してください。");
                return;
            }

            string text = string.Empty;

            await Translate.Run(txtText.Text);
            txtText2.Text = Translate.TextOut;

            await TextSpeaker.Run(txtText.Text,"ja-jp");
            await TextSpeaker.Run(txtText2.Text, "en");
        }
    }
}
