using System.Collections.Generic;
using Newtonsoft.Json;
using AxisBaseTableManager;
using UnityEngine.Animations;

#region Floors
public class DirectNodeTable
{

}

public class DirectNodeTableCoreInfo
{
    public struct InputPack
    {
        AxisBaseTable maxisBaseTable;

        public InputPack(AxisBaseTable axisBaseTable)
        {
            maxisBaseTable = axisBaseTable;
        }

        public AxisBaseTable AxisBaseTable
        {
            get => maxisBaseTable;
        }
    }



    private AxisBaseTable maxisBaseTable;




    public DirectNodeTableCoreInfo(InputPack inputPack)
    {
        maxisBaseTable = inputPack.AxisBaseTable;
    }
}
#endregion

#region Floor
public class FloorTable
{

}
#endregion

#region Node
public class HighNodeData
{

}
public class MiddleNodeData
{

}
public class LowNodeData
{

}
#endregion