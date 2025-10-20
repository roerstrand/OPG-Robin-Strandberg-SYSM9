using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class MainWindowViewModel
    {
        public User LoggedIn { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public bool Login(string username, string password)
        {
            foreach (User u in Users)
            {
                if (u.UserName == username && u.Password == password)
                {
                    LoggedIn = u;
                    return true;
                }
            }

            MessageBox.Show("Warning! Wrong password or username.");
            return false;
        }

        public void Logout()
        {
            LoggedIn = null;
        }
    }
    
    public void openRegister() {
    
    }

    public void forgotPassword()
    {
        
    }
}