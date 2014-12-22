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
using FileAdmiral.Engine.ViewModels;

namespace FileAdmiral
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //PowerShellViewModel cpvm = new PowerShellViewModel("C:\\");
                ICommandShellViewModel cpvm = new PowerShellViewModel("C:\\");
                DataContext = cpvm;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void CommandInput_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ((ICommandShellViewModel)DataContext).SendCommand(CommandInput.Text);
                CommandInput.Text = "";
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = ((TextBox)sender);
            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
            //CommandInput.Padding = new Thickness(PromptDisplay.ActualWidth + 4, CommandInput.Padding.Top, CommandInput.Padding.Right, CommandInput.Padding.Bottom);
        }
    }
}
