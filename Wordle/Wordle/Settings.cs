using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    public class Settings
    {
        public event EventHandler StylingChanged;

        public void lightmode()
        {
            Application.Current.Resources["PrimaryColor"] = Color.FromRgb(255, 255, 255);
            Application.Current.Resources["SecondaryColor"] = Color.FromRgb(100, 100, 100);
            Application.Current.Resources["ThirdColor"] = Color.FromRgb(0, 0, 0);
            OnStylingChanged();
        }
        protected virtual void OnStylingChanged()
        {
            StylingChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
