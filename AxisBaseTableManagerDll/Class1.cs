using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AxisBaseTableManager
{
    public class AxisBaseTableManager<EHighOrderedNodeType, EMiddleOrderedNOdeType, ELowOrderedNodeType>
    {
        public struct Node
        {
            public int mnodeHaskCode;
        }


        
        private Node[,,] mnodeTable;
        private AxisBaseTablePalette maxisBaseTablePalette;
        private OrderedCube morderedCube;



        public AxisBaseTableManager(in AxisBaseTablePalette axisBaseTablePalette, in OrderedCube orderedCube)
        {
            maxisBaseTablePalette = axisBaseTablePalette;
            morderedCube = orderedCube;
        }

        public Node[,,] NodeTable
        {
            get => mnodeTable;
        }
        public AxisBaseTablePalette AxisBaseTablePalette
        {
            get => maxisBaseTablePalette;
        }
        public OrderedCube OrderedCube
        {
            get => morderedCube;
        }
    }
    public class OrderedCube
    {
        public struct Node
        {
            public string mnodeOrderedType;
            public OrderedNodeType.EOrderedTypeClassify morderedTypeClassify;
        };



        private OrderedNodeType morderedNodeType;
        private Node[,,] morderedCubeTable;
        private bool mbisEditing;
        private int mxLength, myLength, mzLength;



        public OrderedCube(int x, int y, int z)
        {
            mxLength = x;
            myLength = y;
            mzLength = z;

            morderedCubeTable = new Node[mxLength, myLength, mzLength];
        }

        public Node[,,] OrderedCubeTable
        {
            get => morderedCubeTable;
        }
        public bool IsEditing
        {
            get => mbisEditing;
        }
        public int X
        {
            get => mxLength;
        }
        public int Y
        {
            get => myLength;
        }
        public int Z
        {
            get => mzLength;
        }

        public void SetTableEditingMode(in OrderedNodeType orderedNodeType)
        {
            mbisEditing = true;
            morderedNodeType = orderedNodeType;
        }
        public void SetTableRefrenceMode()
        {
            mbisEditing = false;
            morderedNodeType = null;
        }
        public bool SetNode(int x, int y, int z, string orderedType, OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
        {
            if (mbisEditing && x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                switch(orderedTypeClassify)
                {
                    case OrderedNodeType.EOrderedTypeClassify.Low:
                        if(!morderedNodeType.LowOrderedNodeType.ContainsKey(orderedType))
                        {
                            return false;
                        }
                        break;

                    case OrderedNodeType.EOrderedTypeClassify.Middle:
                        if(!morderedNodeType.MiddleOrderedNodeType.ContainsKey(orderedType))
                        {
                            return false;
                        }
                        break;

                    case OrderedNodeType.EOrderedTypeClassify.High:
                        if(!morderedNodeType.HighOrderedNodeType.ContainsKey(orderedType))
                        {
                            return false;
                        }
                        break;
                }
                morderedCubeTable[x, y, z].mnodeOrderedType = orderedType;
                morderedCubeTable[x, y, z].morderedTypeClassify = orderedTypeClassify;
                return true;
            }
            return false;
        }
        public bool RemoveNode(int x, int y, int z)
        {
            if (mbisEditing && x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                morderedCubeTable[x, y, z].mnodeOrderedType = default(string);
                morderedCubeTable[x, y, z].morderedTypeClassify = default(OrderedNodeType.EOrderedTypeClassify);
                return true;
            }
            return false;
        }
        public void  SetXYZLength(int newX, int newY, int newZ)
        {
            Node[,,] tempNodeTalbe = new Node[newX, newY, newZ];

            for(int coord_x = 0; coord_x < ((mxLength < newX) ? mxLength : newX); coord_x++)
            {
                for(int coord_y = 0; coord_y < ((myLength < newY) ? myLength : newY); coord_y++)
                {
                    for(int coord_z = 0; coord_z < ((mzLength < newZ) ? mzLength : newZ); coord_z++)
                    {
                        tempNodeTalbe[coord_x, coord_y, coord_z].mnodeOrderedType = morderedCubeTable[coord_x, coord_y, coord_z].mnodeOrderedType;
                        tempNodeTalbe[coord_x, coord_y, coord_z].morderedTypeClassify = morderedCubeTable[coord_x, coord_y, coord_z].morderedTypeClassify;
                    }
                }
            }

            morderedCubeTable = tempNodeTalbe;
            mxLength = newX;
            myLength = newY;
            mzLength = newZ;
        }
    }
    public class AxisBaseTablePalette
    {
        public class Node
        {
            public string mnodeName;
            public byte[] mnodePNG;

            public OrderedNodeType.EOrderedTypeClassify morderedTypeClassify;
            public string mnodeorderedTypeKind;

            public Node(string nodeName, in byte[] nodePNG)
            {
                mnodeName = nodeName;
                mnodePNG = nodePNG;

                morderedTypeClassify = new OrderedNodeType.EOrderedTypeClassify();
                mnodeorderedTypeKind = default(string);
            }
        }



        private Dictionary<int, Node> mnodeTable;
        private OrderedNodeType morderedNodeTypeTable;



        public AxisBaseTablePalette(in OrderedNodeType orderedNodeType)
        {
            mnodeTable = new Dictionary<int, Node>();
            morderedNodeTypeTable = orderedNodeType;
        }

        public Dictionary<int, Node> NodeTable
        {
            get => mnodeTable;
        }
        public OrderedNodeType OrderedNodeTypeTable
        {
            get => morderedNodeTypeTable;
        }

        public void AddNode(string nodeName, in byte[] nodePNG)
        {
            mnodeTable.Add(nodeName.GetHashCode(), new Node(nodeName, nodePNG));
        }
        public void SetOrderedType(string targetNodeName, string orderedTypeName, OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
        {
            switch(orderedTypeClassify)
            {
                case OrderedNodeType.EOrderedTypeClassify.Low:
                    if(morderedNodeTypeTable.LowOrderedNodeType.ContainsKey(orderedTypeName))
                    {
                        mnodeTable[targetNodeName.GetHashCode()].morderedTypeClassify = OrderedNodeType.EOrderedTypeClassify.Low;
                    }
                    else
                    {
                        return;
                    }
                    break;

                case OrderedNodeType.EOrderedTypeClassify.Middle:
                    if(morderedNodeTypeTable.MiddleOrderedNodeType.ContainsKey(orderedTypeName))
                    {
                        mnodeTable[targetNodeName.GetHashCode()].morderedTypeClassify = OrderedNodeType.EOrderedTypeClassify.Middle;
                    }
                    else
                    {
                        return;
                    }
                    break;

                case OrderedNodeType.EOrderedTypeClassify.High:
                    if(morderedNodeTypeTable.HighOrderedNodeType.ContainsKey(orderedTypeName))
                    {
                        mnodeTable[targetNodeName.GetHashCode()].morderedTypeClassify = OrderedNodeType.EOrderedTypeClassify.High;
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            mnodeTable[targetNodeName.GetHashCode()].mnodeorderedTypeKind = orderedTypeName;
        }
        public void DeleteNode(string nodeName)
        {
            mnodeTable.Remove(nodeName.GetHashCode());
        }
        public byte[] GetPaletteJsonByteData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
    public class OrderedNodeType
    {
        public enum EOrderedTypeClassify
        {
            Low = 1,
            Middle = 2,
            High = 3
        };

        public struct Node
        {
            public string mnodeTypeName;
            public List<Tuple<string, EOrderedTypeClassify>> minfluencedNodeTypes;

            public Node(string nodeTypeName)
            {
                mnodeTypeName = nodeTypeName;
                minfluencedNodeTypes = new List<Tuple<string, EOrderedTypeClassify>>();
            }
        }



        private Dictionary<string, Node> mlowOrderedNodeTypes;
        private Dictionary<string, Node> mmiddleOrderedNodeTypes;
        private Dictionary<string, Node> mhighOrderedNodeTypes;



        public OrderedNodeType()
        {
            mlowOrderedNodeTypes = new Dictionary<string, Node>();
            mmiddleOrderedNodeTypes = new Dictionary<string, Node>();
            mhighOrderedNodeTypes = new Dictionary<string, Node>();
        }

        public Dictionary<string, Node> LowOrderedNodeType
        {
            get => mlowOrderedNodeTypes;
        }
        public Dictionary<string, Node> MiddleOrderedNodeType
        {
            get => mmiddleOrderedNodeTypes;
        }
        public Dictionary<string, Node> HighOrderedNodeType
        {
            get => mhighOrderedNodeTypes;
        }

        public void Add(string nodeTypeName, EOrderedTypeClassify orderedTypeClassify)
        {
            switch(orderedTypeClassify)
            {
                case EOrderedTypeClassify.Low:
                    mlowOrderedNodeTypes.Add(nodeTypeName, new Node(nodeTypeName));
                    break;

                case EOrderedTypeClassify.Middle:
                    mmiddleOrderedNodeTypes.Add(nodeTypeName, new Node(nodeTypeName));
                    break;

                case EOrderedTypeClassify.High:
                    mhighOrderedNodeTypes.Add(nodeTypeName, new Node(nodeTypeName));
                    break;
            }
        }
        public void Add(string nodeTypeName, EOrderedTypeClassify orderedTypeClassify, string baseNodeTypeName, EOrderedTypeClassify baseOrderedTypeClassify)
        {
            if(orderedTypeClassify == baseOrderedTypeClassify || baseOrderedTypeClassify == EOrderedTypeClassify.High)
            {
                return;
            }
            else
            {
                switch (orderedTypeClassify)
                {
                    case EOrderedTypeClassify.Low:
                        mlowOrderedNodeTypes.Add(nodeTypeName, new Node(nodeTypeName));
                        break;

                    case EOrderedTypeClassify.Middle:
                        mmiddleOrderedNodeTypes.Add(nodeTypeName, new Node(nodeTypeName));
                        break;

                    case EOrderedTypeClassify.High:
                        mhighOrderedNodeTypes.Add(nodeTypeName, new Node(nodeTypeName));
                        break;
                }
                switch (baseOrderedTypeClassify)
                {
                    case EOrderedTypeClassify.Low:
                        mlowOrderedNodeTypes[baseNodeTypeName].minfluencedNodeTypes.Add(new Tuple<string, EOrderedTypeClassify>(nodeTypeName, orderedTypeClassify));
                        break;

                    case EOrderedTypeClassify.Middle:
                        mmiddleOrderedNodeTypes[baseNodeTypeName].minfluencedNodeTypes.Add(new Tuple<string, EOrderedTypeClassify>(nodeTypeName, orderedTypeClassify));
                        break;
                }
            }
        }
        public void Remove(string nodeTypeName, EOrderedTypeClassify orderedTypeClassify)
        {
            switch(orderedTypeClassify)
            {
                case EOrderedTypeClassify.Low:
                    mlowOrderedNodeTypes[nodeTypeName].minfluencedNodeTypes.Clear();
                    mlowOrderedNodeTypes.Remove(nodeTypeName);
                    break;

                case EOrderedTypeClassify.Middle:
                    mmiddleOrderedNodeTypes[nodeTypeName].minfluencedNodeTypes.Clear();
                    mmiddleOrderedNodeTypes.Remove(nodeTypeName);
                    break;

                case EOrderedTypeClassify.High:
                    mhighOrderedNodeTypes.Remove(nodeTypeName);
                    break;
            }
        }
    }
}