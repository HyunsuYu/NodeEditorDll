using System.Collections.Generic;
using AxisBaseTableManager;
using UnityEngine;
using Utilities;

#region Floors
public class DirectNodeTable
{
    public struct InputPack
    {
        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

        public InputPack(DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
        }

        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }
    }



    private List<FloorTable> mfloorTable;
    private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;



    public DirectNodeTable(InputPack inputPack)
    {
        mfloorTable = new List<FloorTable>();
        mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;
    }

    public List<byte[]> BakeFloorsToPNG()
    {
        List<byte[]> bakeList = new List<byte[]>();

        for(int index = 0; index < mfloorTable.Count; index++)
        {
            bakeList.Add(mfloorTable[index].BaseToTexture2D().EncodeToPNG());
        }

        return bakeList;
    }
    public bool SetNodeInFloor(int floorIndex, int x, int y, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
    {
        if (floorIndex < 0 || floorIndex >= mfloorTable.Count)
        {
            return false;
        }

        return mfloorTable[floorIndex].SetNode(x, y, targetNodePrimeNumber, paletteType);
    }
    public bool RemoveNodeInFloor(int floorIndex, int x, int y, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
    {
        if (floorIndex < 0 || floorIndex >= mfloorTable.Count)
        {
            return false;
        }

        return mfloorTable[floorIndex].RemoveNode(x, y, targetNodePrimeNumber, paletteType);
    }
    public bool SetFloorAxisLength(int floorIndex, int x, int y)
    {
        if (floorIndex < 0 || floorIndex >= mfloorTable.Count)
        {
            return false;
        }

        mfloorTable[floorIndex].SetAxisLength(x, y);

        return true;
    }
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
    private List<NodeEncoding.EncodingNodeData> mgeologyEncodingNodeDatas;
    private List<NodeEncoding.EncodingNodeData> mbiologyEncodingNodeDatas;




    public DirectNodeTableCoreInfo(InputPack inputPack)
    {
        maxisBaseTable = inputPack.AxisBaseTable;
        mgeologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Geology);
        mbiologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Biology);
    }

    public AxisBaseTable AxisBaseTable
    {
        get => maxisBaseTable;
    }
    public List<NodeEncoding.EncodingNodeData> GeologyEncodingNodeDatas
    {
        get => mgeologyEncodingNodeDatas;
    }
    public List<NodeEncoding.EncodingNodeData> BiologyEncodingNodeDatas
    {
        get => mbiologyEncodingNodeDatas;
    }
}
#endregion

#region Floor
public class FloorTable
{
    public struct InputPack
    {
        private int mx, my;

        public InputPack(int x, int y)
        {
            mx = x;
            my = y;
        }

        public int X
        {
            get => mx;
        }
        public int Y
        {
            get => my;
        }
    }



    private int[,] mgeologyNodeTable;
    private int[,] mbiologyNodeTable;
    private int mx, my;



    public FloorTable()
    {
        mx = DefaultAxisLength;
        my = DefaultAxisLength;

        mgeologyNodeTable = new int[Y, X];
        mbiologyNodeTable = new int[Y, X];
    }
    public FloorTable(InputPack inputPack)
    {
        mx = inputPack.X;
        my = inputPack.Y;

        mgeologyNodeTable = new int[Y, X];
        mbiologyNodeTable = new int[Y, X];
    }

    public int[,] GeologyNodeTable
    {
        get => mgeologyNodeTable;
    }
    public int[,] BiologyNodeTable
    {
        get => mbiologyNodeTable;
    }
    public int X
    {
        get => mx;
    }
    public int Y
    {
        get => my;
    }
    public int DefaultAxisLength
    {
        get => 50;
    }

    internal bool SetNode(int x, int y, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
    {
        if(x < 0 || x > X - 1 || y < 0 || y > Y - 1)
        {
            return false;
        }

        switch(paletteType)
        {
            case AxisBaseTablePalette.EPaletteType.Geology:
                mgeologyNodeTable[y, x] *= targetNodePrimeNumber;
                break;

            case AxisBaseTablePalette.EPaletteType.Biology:
                mbiologyNodeTable[y, x] *= targetNodePrimeNumber;
                break;
        }

        return true;
    }
    internal bool RemoveNode(int x, int y, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
    {
        if (x < 0 || x > X - 1 || y < 0 || y > Y - 1)
        {
            return false;
        }

        switch(paletteType)
        {
            case AxisBaseTablePalette.EPaletteType.Geology:
                mgeologyNodeTable[y, x] /= targetNodePrimeNumber;
                break;

            case AxisBaseTablePalette.EPaletteType.Biology:
                mbiologyNodeTable[y, x] /= targetNodePrimeNumber;
                break;
        }

        return true;
    }
    internal void SetAxisLength(int x, int y)
    {
        int[,] tempGeologyNodeTable = new int[x, y];
        int[,] tempBiologyNodeTable = new int[x, y];

        for(int coord_y = 0; coord_y < (Y < y ? Y : y); coord_y++)
        {
            for(int coord_x = 0; coord_x < (X < x ? X : x); coord_x++)
            {
                tempGeologyNodeTable[coord_y, coord_x] = mgeologyNodeTable[coord_y, coord_x];
                tempBiologyNodeTable[coord_y, coord_x] = mbiologyNodeTable[coord_y, coord_x];
            }
        }

        mgeologyNodeTable = tempGeologyNodeTable;
        mbiologyNodeTable = tempBiologyNodeTable;

        mx = x;
        my = y;
    }
    internal Texture2D BaseToTexture2D()
    {
        Texture2D texture2D = new Texture2D(X, Y);

        return texture2D;
    }
}
#endregion