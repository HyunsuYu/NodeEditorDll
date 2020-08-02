using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AxisBaseTableManager
{
    public class AxisBaseTable
    {
        private int[,,] mnodeTable;
        private AxisBaseTablePalette maxisBaseTablePalette;
        private int mxLength, myLength, mzLength;



        public AxisBaseTable(in AxisBaseTablePalette axisBaseTablePalette)
        {
            maxisBaseTablePalette = axisBaseTablePalette;
            mxLength = maxisBaseTablePalette.OrderedCube.X;
            myLength = maxisBaseTablePalette.OrderedCube.Y;
            mzLength = maxisBaseTablePalette.OrderedCube.Z;
            mnodeTable = new int[mxLength, myLength, mzLength];
            for(int coord_x = 0; coord_x < mxLength; coord_x++)
            {
                for(int coord_y = 0; coord_y < myLength; coord_y++)
                {
                    for(int coord_z = 0; coord_z < mzLength; coord_z++)
                    {
                        mnodeTable[coord_x, coord_y, coord_z] = -1;
                    }
                }
            }
        }
        public AxisBaseTable(in AxisBaseTablePalette axisBaseTablePalette, int x, int y, int z)
        {
            maxisBaseTablePalette = axisBaseTablePalette;
            mxLength = x;
            myLength = y;
            mzLength = z;
            mnodeTable = new int[mxLength, myLength, mzLength];
            for (int coord_x = 0; coord_x < mxLength; coord_x++)
            {
                for (int coord_y = 0; coord_y < myLength; coord_y++)
                {
                    for (int coord_z = 0; coord_z < mzLength; coord_z++)
                    {
                        mnodeTable[coord_x, coord_y, coord_z] = -1;
                    }
                }
            }
        }
        public AxisBaseTable(in AxisBaseTable axisBaseTableManager)
        {
            mnodeTable = axisBaseTableManager.NodeTable;
            maxisBaseTablePalette = axisBaseTableManager.AxisBaseTablePalette;
            mxLength = axisBaseTableManager.X;
            myLength = axisBaseTableManager.Y;
            mzLength = axisBaseTableManager.Z;
        }

        public int[,,] NodeTable
        {
            get => mnodeTable;
        }
        public AxisBaseTablePalette AxisBaseTablePalette
        {
            get => maxisBaseTablePalette;
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

        public bool SetNode(int x, int y, int z, int nodeKindCode)
        {
            if(x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                if (maxisBaseTablePalette.NodeTable[nodeKindCode].OrderedTypeClassify == maxisBaseTablePalette.OrderedCube.OrderedCubeTable[(x > maxisBaseTablePalette.OrderedCube.X - 1) ? maxisBaseTablePalette.OrderedCube.X - 1 : x, (y > maxisBaseTablePalette.OrderedCube.Y - 1) ? maxisBaseTablePalette.OrderedCube.Y - 1 : y, (z > maxisBaseTablePalette.OrderedCube.Z - 1) ? maxisBaseTablePalette.OrderedCube.Z - 1 : z].OrderedTypeClassify)
                {
                    mnodeTable[x, y, z] = nodeKindCode;
                    return true;
                }
            }
            return false;
        }
        public bool RemoveNode(int x, int y, int z)
        {
            if(x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                mnodeTable[x, y, z] = -1;
                return true;
            }
            return false;
        }
        public void SetXYZLength(int newX, int newY, int newZ)
        {
            int[,,] tempNodeTable = new int[newX, newY, newZ];

            for (int coord_x = 0; coord_x < ((mxLength < newX) ? mxLength : newX); coord_x++)
            {
                for (int coord_y = 0; coord_y < ((myLength < newY) ? myLength : newY); coord_y++)
                {
                    for (int coord_z = 0; coord_z < ((mzLength < newZ) ? mzLength : newZ); coord_z++)
                    {
                        tempNodeTable[coord_x, coord_y, coord_z] = mnodeTable[coord_x, coord_y, coord_z];
                    }
                }
            }

            mnodeTable = tempNodeTable;
            mxLength = newX;
            myLength = newY;
            mzLength = newZ;
        }
        public byte[] GetTableJsonByteData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
    public class AxisBaseTablePalette
    {
        public class Node
        {
            private string mnodeName;
            private byte[] mnodePNG;

            private OrderedNodeType.EOrderedTypeClassify morderedTypeClassify;
            private string mnodeOrderedTypeKind;

            public Node(string nodeName, in byte[] nodePNG)
            {
                mnodeName = nodeName;
                mnodePNG = nodePNG;

                morderedTypeClassify = new OrderedNodeType.EOrderedTypeClassify();
                mnodeOrderedTypeKind = default(string);
            }

            public string NodeName
            {
                get => mnodeName;
            }
            public byte[] NodePNG
            {
                get => mnodePNG;
            }
            public OrderedNodeType.EOrderedTypeClassify OrderedTypeClassify
            {
                get => morderedTypeClassify;
            }
            public string NodeOrderedTypeKind
            {
                get => mnodeOrderedTypeKind;
            }

            internal void SetOrderedClassify(OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
            {
                morderedTypeClassify = orderedTypeClassify;
            }
            internal void SetNodeOrderedTypeKind(string kind)
            {
                mnodeOrderedTypeKind = kind;
            }
        }



        private Dictionary<int, Node> mnodeTable;
        private List<string> mnodeNames;
        private OrderedCube morderedCube;
        private Node mdefaultNodeKind;



        public AxisBaseTablePalette(in OrderedCube orderedCube)
        {
            mnodeTable = new Dictionary<int, Node>();
            mnodeNames = new List<string>();
            morderedCube = orderedCube;
            mdefaultNodeKind = null;
        }
        public AxisBaseTablePalette(in AxisBaseTablePalette axisBaseTablePalette)
        {
            mnodeTable = axisBaseTablePalette.NodeTable;
            mnodeNames = axisBaseTablePalette.NodeNames;
            morderedCube = axisBaseTablePalette.OrderedCube;
            mdefaultNodeKind = axisBaseTablePalette.DefauleNodeKind;
        }

        public Dictionary<int, Node> NodeTable
        {
            get => mnodeTable;
        }
        public List<string> NodeNames
        {
            get => mnodeNames;
        }
        public OrderedCube OrderedCube
        {
            get => morderedCube;
        }
        public Node DefauleNodeKind
        {
            get => mdefaultNodeKind;
        }

        public void AddNode(string nodeName, in byte[] nodePNG)
        {
            mnodeTable.Add(nodeName.GetHashCode(), new Node(nodeName, nodePNG));
            mnodeNames.Add(nodeName);

            if(mdefaultNodeKind == null)
            {
                mdefaultNodeKind = mnodeTable[nodeName.GetHashCode()];
            }
        }
        public void SetOrderedType(string targetNodeName, string orderedTypeName, OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
        {
            switch(orderedTypeClassify)
            {
                case OrderedNodeType.EOrderedTypeClassify.Low:
                    if(morderedCube.OrderedNodeType.LowOrderedNodeType.ContainsKey(orderedTypeName))
                    {
                        mnodeTable[targetNodeName.GetHashCode()].SetOrderedClassify(OrderedNodeType.EOrderedTypeClassify.Low);
                    }
                    else
                    {
                        return;
                    }
                    break;

                case OrderedNodeType.EOrderedTypeClassify.Middle:
                    if(morderedCube.OrderedNodeType.MiddleOrderedNodeType.ContainsKey(orderedTypeName))
                    {
                        mnodeTable[targetNodeName.GetHashCode()].SetOrderedClassify(OrderedNodeType.EOrderedTypeClassify.Middle);
                    }
                    else
                    {
                        return;
                    }
                    break;

                case OrderedNodeType.EOrderedTypeClassify.High:
                    if(morderedCube.OrderedNodeType.HighOrderedNodeType.ContainsKey(orderedTypeName))
                    {
                        mnodeTable[targetNodeName.GetHashCode()].SetOrderedClassify(OrderedNodeType.EOrderedTypeClassify.High);
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            mnodeTable[targetNodeName.GetHashCode()].SetNodeOrderedTypeKind(orderedTypeName);
        }
        public void DeleteNode(string nodeName)
        {
            mnodeTable.Remove(nodeName.GetHashCode());
            mnodeNames.Remove(nodeName);
        }
    }
    public class OrderedCube
    {
        public class Node
        {
            private string mnodeOrderedType;
            private OrderedNodeType.EOrderedTypeClassify morderedTypeClassify;

            public string NodeOrderedType
            {
                get => mnodeOrderedType;
            }
            public OrderedNodeType.EOrderedTypeClassify OrderedTypeClassify
            {
                get => morderedTypeClassify;
            }

            internal void SetNodeOrderedType(in string nodeOrderedType)
            {
                mnodeOrderedType = nodeOrderedType;
            }
            internal void SetOrderedTypeClassify(OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
            {
                morderedTypeClassify = orderedTypeClassify;
            }
        };
        public struct InputPack
        {
            private int mx, my, mz;
            private OrderedNodeType morderedNodeType;

            public InputPack(int x, int y, int z, in OrderedNodeType orderedNodeType)
            {
                mx = x;
                my = y;
                mz = z;
                morderedNodeType = orderedNodeType;
            }

            public int X
            {
                get => mx;
            }
            public int Y
            {
                get => my;
            }
            public int Z
            {
                get => mz;
            }
            public OrderedNodeType OrderedNodeType
            {
                get => morderedNodeType;
            }
        };



        private OrderedNodeType morderedNodeType;
        private Node[,,] morderedCubeTable;
        private int mxLength, myLength, mzLength;



        public OrderedCube(in InputPack inputPack)
        {
            mxLength = inputPack.X;
            myLength = inputPack.Y;
            mzLength = inputPack.Z;

            morderedNodeType = inputPack.OrderedNodeType;
            morderedCubeTable = new Node[mxLength, myLength, mzLength];
        }

        public Node[,,] OrderedCubeTable
        {
            get => morderedCubeTable;
        }
        public OrderedNodeType OrderedNodeType
        {
            get => morderedNodeType;
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

        public bool SetNode(int x, int y, int z, string orderedType, OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
        {
            if (x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                switch (orderedTypeClassify)
                {
                    case OrderedNodeType.EOrderedTypeClassify.Low:
                        if (!morderedNodeType.LowOrderedNodeType.ContainsKey(orderedType))
                        {
                            return false;
                        }
                        break;

                    case OrderedNodeType.EOrderedTypeClassify.Middle:
                        if (!morderedNodeType.MiddleOrderedNodeType.ContainsKey(orderedType))
                        {
                            return false;
                        }
                        break;

                    case OrderedNodeType.EOrderedTypeClassify.High:
                        if (!morderedNodeType.HighOrderedNodeType.ContainsKey(orderedType))
                        {
                            return false;
                        }
                        break;
                }
                morderedCubeTable[x, y, z].SetNodeOrderedType(orderedType);
                morderedCubeTable[x, y, z].SetOrderedTypeClassify(orderedTypeClassify);
                return true;
            }
            return false;
        }
        public bool RemoveNode(int x, int y, int z)
        {
            if (x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                morderedCubeTable[x, y, z].SetNodeOrderedType(default(string));
                morderedCubeTable[x, y, z].SetOrderedTypeClassify(default(OrderedNodeType.EOrderedTypeClassify));
                return true;
            }
            return false;
        }
        public void SetXYZLength(int newX, int newY, int newZ)
        {
            Node[,,] tempNodeTable = new Node[newX, newY, newZ];

            for (int coord_x = 0; coord_x < ((mxLength < newX) ? mxLength : newX); coord_x++)
            {
                for (int coord_y = 0; coord_y < ((myLength < newY) ? myLength : newY); coord_y++)
                {
                    for (int coord_z = 0; coord_z < ((mzLength < newZ) ? mzLength : newZ); coord_z++)
                    {
                        tempNodeTable[coord_x, coord_y, coord_z].SetNodeOrderedType(morderedCubeTable[coord_x, coord_y, coord_z].NodeOrderedType);
                        tempNodeTable[coord_x, coord_y, coord_z].SetOrderedTypeClassify(morderedCubeTable[coord_x, coord_y, coord_z].OrderedTypeClassify);
                    }
                }
            }

            morderedCubeTable = tempNodeTable;
            mxLength = newX;
            myLength = newY;
            mzLength = newZ;
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
            private string mnodeTypeName;
            private List<Tuple<string, EOrderedTypeClassify>> minfluencedNodeTypes;

            public Node(string nodeTypeName)
            {
                mnodeTypeName = nodeTypeName;
                minfluencedNodeTypes = new List<Tuple<string, EOrderedTypeClassify>>();
            }

            public string NodeTypeName
            {
                get => mnodeTypeName;
            }
            public List<Tuple<string, EOrderedTypeClassify>> InfluencedNodeTypes
            {
                get => minfluencedNodeTypes;
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
                        mlowOrderedNodeTypes[baseNodeTypeName].InfluencedNodeTypes.Add(new Tuple<string, EOrderedTypeClassify>(nodeTypeName, orderedTypeClassify));
                        break;

                    case EOrderedTypeClassify.Middle:
                        mmiddleOrderedNodeTypes[baseNodeTypeName].InfluencedNodeTypes.Add(new Tuple<string, EOrderedTypeClassify>(nodeTypeName, orderedTypeClassify));
                        break;
                }
            }
        }
        public void Remove(string nodeTypeName, EOrderedTypeClassify orderedTypeClassify)
        {
            switch(orderedTypeClassify)
            {
                case EOrderedTypeClassify.Low:
                    mlowOrderedNodeTypes[nodeTypeName].InfluencedNodeTypes.Clear();
                    mlowOrderedNodeTypes.Remove(nodeTypeName);
                    break;

                case EOrderedTypeClassify.Middle:
                    mmiddleOrderedNodeTypes[nodeTypeName].InfluencedNodeTypes.Clear();
                    mmiddleOrderedNodeTypes.Remove(nodeTypeName);
                    break;

                case EOrderedTypeClassify.High:
                    mhighOrderedNodeTypes.Remove(nodeTypeName);
                    break;
            }
        }
    }
}