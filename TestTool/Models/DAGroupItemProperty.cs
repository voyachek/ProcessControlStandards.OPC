using System.Runtime.InteropServices;

namespace ProcessControlStandards.OPC.TestTool.Models
{
    public class DAGroupItemProperty
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public VarEnum Type { get; set; }

        public VarEnum SubType { get; set; }

        public object Value { get; set; }

        public int Error { get; set; }
    }
}
