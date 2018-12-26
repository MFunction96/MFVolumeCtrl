using System;
using System.Windows;

namespace MFVolumeTool.Views
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow
    {
        public Action<string> AcAddItem { get; set; }

        public InputWindow(string content, string inputBox)
        {
            InitializeComponent();
            LbContent.Content = content;
            TbInput.Text = inputBox;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            AcAddItem(TbInput.Text);
            Close();
        }

        private void BtnCancal_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbInput.Focus();
        }
    }
}
