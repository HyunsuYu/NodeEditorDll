using System;
using System.Collections.Generic;
using AxisBaseTableManager;
using UnityEngine;
using Utilities;

#region FullPack
public class DirectFullPack
{
    public struct InputPack
    {
        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
        private List<byte[]> mfloorPNGs;

        public InputPack(DirectNodeTableCoreInfo directNodeTableCoreInfo, List<byte[]> floorPNGs)
        {
            mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            mfloorPNGs = floorPNGs;
        }

        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }
        public List<byte[]> FloorPNGs
        {
            get => mfloorPNGs;
        }
    }



    private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
    private List<byte[]> mfloorPNGs;



    public DirectFullPack(InputPack inputPack)
    {
        mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;
        mfloorPNGs = inputPack.FloorPNGs;
    }

    public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
    {
        get => mdirectNodeTableCoreInfo;
    }
    public List<byte[]> FloorPNGs
    {
        get => mfloorPNGs;
    }

    public List<Texture2D> DisorderByteDatas()
    {
        List<Texture2D> texture2Ds = new List<Texture2D>();

        for(int index = 0; index < mdirectNodeTableCoreInfo.FloorAxisLengths.Count; index++)
        {
            texture2Ds.Add(new Texture2D(mdirectNodeTableCoreInfo.FloorAxisLengths[index].x, mdirectNodeTableCoreInfo.FloorAxisLengths[index].y));
            texture2Ds[index].LoadImage(mfloorPNGs[index]);
        }

        return texture2Ds;
    }
}
#endregion

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

    public List<Texture2D> BakeFloorsToTextyre2D()
    {
        List<Texture2D> texture2Ds = new List<Texture2D>();

        for(int index = 0; index < mfloorTable.Count; index++)
        {
            texture2Ds.Add(mfloorTable[index].BakeToTexture2D(mdirectNodeTableCoreInfo));
        }

        return texture2Ds;
    }
    public List<byte[]> BakeFloorsToPNG()
    {
        List<byte[]> bakeList = new List<byte[]>();
        List<Texture2D> texture2Ds = BakeFloorsToTextyre2D();

        for(int index = 0; index < mfloorTable.Count; index++)
        {
            bakeList.Add(texture2Ds[index].EncodeToPNG());
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
    public void AddFloor()
    {
        mfloorTable.Add(new FloorTable());
        mdirectNodeTableCoreInfo.FloorAxisLengths.Add(new Vector2Int(FloorTable.DefaultAxisLength, FloorTable.DefaultAxisLength));
    }
    public void AddFloor(int x, int y)
    {
        mfloorTable.Add(new FloorTable(new FloorTable.InputPack(x, y)));
        mdirectNodeTableCoreInfo.FloorAxisLengths.Add(new Vector2Int(x, y));
    }
    public bool AddFloor(int index)
    {
        if(index < 0 || index > mfloorTable.Count - 1)
        {
            return false;
        }

        mfloorTable.Insert(index, new FloorTable());
        mdirectNodeTableCoreInfo.FloorAxisLengths.Insert(index, new Vector2Int(FloorTable.DefaultAxisLength, FloorTable.DefaultAxisLength));

        return true;
    }
    public bool AddFloor(int index, int x, int y)
    {
        if (index < 0 || index > mfloorTable.Count - 1)
        {
            return false;
        }

        mfloorTable.Insert(index, new FloorTable(new FloorTable.InputPack(x, y)));
        mdirectNodeTableCoreInfo.FloorAxisLengths.Insert(index, new Vector2Int(x, y));

        return true;
    }
    public bool RemoveFloor(int index)
    {
        if (index < 0 || index > mfloorTable.Count - 1)
        {
            return false;
        }

        mfloorTable.Remove(mfloorTable[index]);
        mdirectNodeTableCoreInfo.FloorAxisLengths.Remove(mdirectNodeTableCoreInfo.FloorAxisLengths[index]);

        return false;
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
    private List<Vector2Int> mfloorAxisLengths;




    public DirectNodeTableCoreInfo(InputPack inputPack)
    {
        maxisBaseTable = inputPack.AxisBaseTable;
        mgeologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Geology);
        mbiologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Biology);
        mfloorAxisLengths = new List<Vector2Int>();
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
    public List<Vector2Int> FloorAxisLengths
    {
        get => mfloorAxisLengths;
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
    private float[,] mheightTable;
    private int mx, my;



    public FloorTable()
    {
        mx = DefaultAxisLength;
        my = DefaultAxisLength;

        mgeologyNodeTable = new int[Y, X];
        mbiologyNodeTable = new int[Y, X];
        mheightTable = new float[Y, X];
    }
    public FloorTable(InputPack inputPack)
    {
        mx = inputPack.X;
        my = inputPack.Y;

        mgeologyNodeTable = new int[Y, X];
        mbiologyNodeTable = new int[Y, X];
        mheightTable = new float[Y, X];
    }

    public int[,] GeologyNodeTable
    {
        get => mgeologyNodeTable;
    }
    public int[,] BiologyNodeTable
    {
        get => mbiologyNodeTable;
    }
    public float[,] HeightTable
    {
        get => mheightTable;
    }
    public int X
    {
        get => mx;
    }
    public int Y
    {
        get => my;
    }
    public static int DefaultAxisLength
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
    internal bool SetHeightValue(int x, int y, float targetValue)
    {
        if (x < 0 || x > X - 1 || y < 0 || y > Y - 1)
        {
            return false;
        }

        if(targetValue < 0.0f)
        {
            targetValue = 0.0f;
        }
        else if(targetValue > 1.0f)
        {
            targetValue = 1.0f;
        }
        mheightTable[y, x] = targetValue;

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
    internal void GenerateHeightTable(List<float> overlapScales, float powNum)
    {
        for(int coord_y = 0; coord_y < Y; coord_y++)
        {
            for(int coord_x = 0; coord_x < X; coord_x++)
            {
                //  noise generating
                float noiseValue = 0.0f;

                for(int index = 0; index < overlapScales.Count; index++)
                {
                    noiseValue += 1 / overlapScales[index] * Mathf.PerlinNoise(coord_x / X * overlapScales[index], coord_y / Y * overlapScales[index]);
                }
                noiseValue = (float)Math.Pow(noiseValue, powNum);

                if (noiseValue < 0.0f)
                {
                    noiseValue = 0.0f;
                }
                else if (noiseValue > 1.0f)
                {
                    noiseValue = 1.0f;
                }

                mheightTable[coord_y, coord_x] = noiseValue;
            }
        }
    }
    internal Texture2D BakeToTexture2D(DirectNodeTableCoreInfo directNodeTableCoreInfo)
    {
        Texture2D texture2D = new Texture2D(X, Y);

        float[,] geologyFloatTable = new float[Y, X];
        float[,] biologyFloatTable = new float[Y, X];

        geologyFloatTable = NodeEncoding.CompressionNodes(X, Y, GeologyNodeTable, directNodeTableCoreInfo.GeologyEncodingNodeDatas);
        biologyFloatTable = NodeEncoding.CompressionNodes(X, Y, BiologyNodeTable, directNodeTableCoreInfo.BiologyEncodingNodeDatas);

        for(int coord_y = 0; coord_y < Y; coord_y++)
        {
            for(int coord_x = 0; coord_x < X; coord_x++)
            {
                texture2D.SetPixel(coord_x, coord_y, new Color(geologyFloatTable[coord_y, coord_x], biologyFloatTable[coord_y, coord_x], mheightTable[coord_y, coord_x]));
            }
        }

        texture2D.Apply();

        return texture2D;
    }
}
#endregion