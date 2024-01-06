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
        public event EventHandler StylingChanged2;
        public void lightmode()
        {
            Application.Current.Resources["PrimaryColor"] = Color.FromRgb(255, 255, 255);
            Application.Current.Resources["SecondaryColor"] = Color.FromRgb(0, 0, 0);
            OnStylingChanged();
        }
        public void darkmode()
        {
            Application.Current.Resources["PrimaryColor"] = Color.FromRgb(0, 0, 0);
            Application.Current.Resources["SecondaryColor"] = Color.FromRgb(255, 255, 255);
            OnStylingChanged2();
        }
        protected virtual void OnStylingChanged()
        {
            StylingChanged?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnStylingChanged2()
        {
            StylingChanged2?.Invoke(this, EventArgs.Empty);
        }
    }
}
