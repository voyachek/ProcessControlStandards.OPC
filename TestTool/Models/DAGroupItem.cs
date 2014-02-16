using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ProcessControlStandards.OPC.TestTool.Models
{
    public class DAGroupItem : INotifyPropertyChanged
    {
        public bool Selected { get; set; }

        public string Name { get; set; }

        public int ClientId { get; set; }

        public int ServerId { get; set; }

        public VarEnum CanonicalDataType { get; set; }

        public VarEnum CanonicalDataSubType { get; set; }

        public int AccessRights { get; set; }

        public object Value { get; set; }

        public int Quality { get; set; }

        public int Error { get; set; }

        public DateTime Timestamp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refreshed()
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
