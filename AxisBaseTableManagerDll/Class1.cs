using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AxisBaseTableManager
{
    public class AxisBaseTableManager
    {
        internal struct Node
        {
            



            internal Node()
            {

            }
        }




        private Node[,,] mnodeTable;
        private AxisBaseTablePalette maxisBaseTablePalette;



        public AxisBaseTableManager(in AxisBaseTablePalette axisBaseTablePalette)
        {
            maxisBaseTablePalette = axisBaseTablePalette;
            
        }
    }

    public class AxisBaseTablePalette
    {
        internal struct Node
        {

        }



        private int[] mordinaryNum;
        private List<Node> mnodeTable;



        public AxisBaseTablePalette()
        {
            Random random = new Random();

            mordinaryNum = new int[6];
            for(int i = 0; i < 6; i++)
            {
                mordinaryNum[i] = random.Next(0, 256);
            }



            random = null;
        }

        public int[] OrdinaryNumber
        {
            get => mordinaryNum;
        }
    }
}