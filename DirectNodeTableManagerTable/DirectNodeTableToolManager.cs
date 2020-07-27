using System.Collections.Generic;
using Newtonsoft.Json;

namespace DirectNodeTableManagerTool
{
    public class DIrectNodeTableManager
    {
        public struct InputPack
        {
            public AxisBaseTableManager.AxisBaseTable maxisBaseTable;

            public InputPack(in AxisBaseTableManager.AxisBaseTable axisBaseTable)
            {
                maxisBaseTable = axisBaseTable;
            }
        };



        private AxisBaseTableManager.AxisBaseTable maxisBaseTable;



        public DIrectNodeTableManager(in InputPack inputPack)
        {
            maxisBaseTable = inputPack.maxisBaseTable;
        }

        public AxisBaseTableManager.AxisBaseTable AxisBaseTable
        {
            get => maxisBaseTable;
        }

        public byte[] GetTableJsonByteData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}