using System;
using System.Windows;

namespace MFVolumeTool
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow
    {
        public Action<string> AcAddItem { get; set; }

        public InputWindow(string text)
        {
            InitializeComponent();
            TbSerivce.Text = text;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            AcAddItem(TbSerivce.Text);
            Close();
        }

        private void BtnCancal_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbSerivce.Focus();
        }
    }
}
