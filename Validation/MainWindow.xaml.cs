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
using System.ComponentModel.DataAnnotations;

namespace Validation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click_BtnAuthPassword(object sender, RoutedEventArgs e)
        {
            Users users = new Users()
            {
                Login = tbLogin.Text,
                Email = tbMail.Text,
                Password = tbPassword.Text,
                RepPassword = tbRepPassword.Text,
            };

            var validationRes = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var validationContext = new ValidationContext(users, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(users, validationContext, validationRes, validateAllProperties: true);

            if (!isValid)
            {
                foreach (var validationResult in validationRes)
                {
                    MessageBox.Show(validationResult.ErrorMessage);
                }
            }
            else
            {
                MessageBox.Show("Валидация прошла успешно!");
            }
        }
    }
}
