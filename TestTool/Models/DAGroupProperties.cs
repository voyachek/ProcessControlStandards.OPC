using System.ComponentModel;
using System.Globalization;

namespace ProcessControlStandards.OPC.TestTool.Models
{
    public class DAGroupProperties : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [Category("Basic")]
        public string Name { get; set; }

        [Category("Basic")]
        public int ClientId { get; set; }

        [Category("Basic")]
        public int ServerId { get; set; }

        [Category("Updating")]
        public bool Active { get; set; }

        [Category("Updating")]
        public int UpdateRate { get; set; }

        [Category("Updating")]
        public float PercentDeadband { get; set; }

        [Category("Locale")]
        public int TimeBias { get; set; }

        [Category("Locale")]
        public CultureInfo Locale { get; set; }

        public void PropertiesChanged()
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
