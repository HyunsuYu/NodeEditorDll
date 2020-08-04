using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AxisBaseTableManager
{
    public class AxisBaseTable
    {
        private int[,,] mgeologyNodeTable;
        private int[,,] mbiologyNodeTable;
        private AxisBaseTablePalette maxisBaseTablePalette;
        private int mxLength, myLength, mzLength;



        public AxisBaseTable(in AxisBaseTablePalette axisBaseTablePalette)
        {
            maxisBaseTablePalette = axisBaseTablePalette;
            mxLength = maxisBaseTablePalette.OrderedCube.X;
            myLength = maxisBaseTablePalette.OrderedCube.Y;
            mzLength = maxisBaseTablePalette.OrderedCube.Z;
            mgeologyNodeTable = new int[X, Y, Z];
            mbiologyNodeTable = new int[X, Y, Z];
            for(int coord_x = 0; coord_x < mxLength; coord_x++)
            {
                for(int coord_y = 0; coord_y < myLength; coord_y++)
                {
                    for(int coord_z = 0; coord_z < mzLength; coord_z++)
                    {
                        mgeologyNodeTable[coord_x, coord_y, coord_z] = -1;
                        mbiologyNodeTable[coord_x, coord_y, coord_z] = -1;
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
            mgeologyNodeTable = new int[X, Y, Z];
            mbiologyNodeTable = new int[X, Y, Z];
            for (int coord_x = 0; coord_x < mxLength; coord_x++)
            {
                for (int coord_y = 0; coord_y < myLength; coord_y++)
                {
                    for (int coord_z = 0; coord_z < mzLength; coord_z++)
                    {
                        mgeologyNodeTable[coord_x, coord_y, coord_z] = -1;
                        mbiologyNodeTable[coord_x, coord_y, coord_z] = -1;
                    }
                }
            }
        }
        public AxisBaseTable(in AxisBaseTable axisBaseTableManager)
        {
            mgeologyNodeTable = axisBaseTableManager.GeologyNodeTable;
            mbiologyNodeTable = axisBaseTableManager.BiologyNodeTable;
            maxisBaseTablePalette = axisBaseTableManager.AxisBaseTablePalette;
            mxLength = axisBaseTableManager.X;
            myLength = axisBaseTableManager.Y;
            mzLength = axisBaseTableManager.Z;
        }

        public int[,,] GeologyNodeTable
        {
            get => mgeologyNodeTable;
        }
        public int[,,] BiologyNodeTable
        {
            get => mbiologyNodeTable;
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

        public bool SetNode(int x, int y, int z, string nodename, AxisBaseTablePalette.EPaletteType paletteType)
        {
            if(x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                switch(paletteType)
                {
                    case AxisBaseTablePalette.EPaletteType.Geology:
                        if (maxisBaseTablePalette.GeologyNodeTable[nodename.GetHashCode()].OrderedTypeClassify == maxisBaseTablePalette.OrderedCube.OrderedCubeTable[(x > maxisBaseTablePalette.OrderedCube.X - 1) ? maxisBaseTablePalette.OrderedCube.X - 1 : x, (y > maxisBaseTablePalette.OrderedCube.Y - 1) ? maxisBaseTablePalette.OrderedCube.Y - 1 : y, (z > maxisBaseTablePalette.OrderedCube.Z - 1) ? maxisBaseTablePalette.OrderedCube.Z - 1 : z].OrderedTypeClassify)
                        {
                            mgeologyNodeTable[x, y, z] = nodename.GetHashCode();
                            return true;
                        }
                        return true;

                    case AxisBaseTablePalette.EPaletteType.Biology:
                        if (maxisBaseTablePalette.BiologyNodeTable[nodename.GetHashCode()].OrderedTypeClassify == maxisBaseTablePalette.OrderedCube.OrderedCubeTable[(x > maxisBaseTablePalette.OrderedCube.X - 1) ? maxisBaseTablePalette.OrderedCube.X - 1 : x, (y > maxisBaseTablePalette.OrderedCube.Y - 1) ? maxisBaseTablePalette.OrderedCube.Y - 1 : y, (z > maxisBaseTablePalette.OrderedCube.Z - 1) ? maxisBaseTablePalette.OrderedCube.Z - 1 : z].OrderedTypeClassify)
                        {
                            mbiologyNodeTable[x, y, z] = nodename.GetHashCode();
                            return true;
                        }
                        return true;
                }
            }

            return false;
        }
        public bool RemoveNode(int x, int y, int z, AxisBaseTablePalette.EPaletteType paletteType)
        {
            if(x >= 0 && x < mxLength && y >= 0 && y < myLength && z >= 0 && z < mzLength)
            {
                switch(paletteType)
                {
                    case AxisBaseTablePalette.EPaletteType.Geology:
                        mgeologyNodeTable[x, y, z] = -1;
                        return true;

                    case AxisBaseTablePalette.EPaletteType.Biology:
                        mbiologyNodeTable[x, y, z] = -1;
                        return true;
                }
            }
            return false;
        }
        public void SetAxisLength(int newX, int newY, int newZ)
        {
            int[,,] tempGeologyNodeTable = new int[newX, newY, newZ];
            int[,,] tempBiologyNodeTable = new int[newX, newY, newZ];

            for (int coord_x = 0; coord_x < ((mxLength < newX) ? mxLength : newX); coord_x++)
            {
                for (int coord_y = 0; coord_y < ((myLength < newY) ? myLength : newY); coord_y++)
                {
                    for (int coord_z = 0; coord_z < ((mzLength < newZ) ? mzLength : newZ); coord_z++)
                    {
                        tempGeologyNodeTable[coord_x, coord_y, coord_z] = GeologyNodeTable[coord_x, coord_y, coord_z];
                        tempBiologyNodeTable[coord_x, coord_y, coord_z] = BiologyNodeTable[coord_x, coord_y, coord_z];
                    }
                }
            }

            mgeologyNodeTable = tempGeologyNodeTable;
            mbiologyNodeTable = tempBiologyNodeTable;
            mxLength = newX;
            myLength = newY;
            mzLength = newZ;
        }
        public byte[] GetJsonByteData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
    public class AxisBaseTablePalette
    {
        public enum EPaletteType
        {
            Geology = 1,
            Biology = 2
        };

        public class GeologyNode
        {
            public enum ENodeSideType
            {
                TopSide = 1,
                FrontSide = 2,
                BehindSide = 3,
                LeftSide = 4,
                RightSide = 5
            };

            private string mnodeName;
            private Dictionary<ENodeSideType, byte[]> mnodePNGs;

            private OrderedNodeType.EOrderedTypeClassify morderedTypeClassify;
            private string mnodeOrderedTypeKind;

            public GeologyNode(string nodeName)
            {
                mnodeName = nodeName;
                mnodePNGs = new Dictionary<ENodeSideType, byte[]>();

                morderedTypeClassify = new OrderedNodeType.EOrderedTypeClassify();
                mnodeOrderedTypeKind = default(string);
            }

            public string NodeName
            {
                get => mnodeName;
            }
            public Dictionary<ENodeSideType, byte[]> NodePNGs
            {
                get => mnodePNGs;
            }
            public OrderedNodeType.EOrderedTypeClassify OrderedTypeClassify
            {
                get => morderedTypeClassify;
            }
            public string NodeOrderedTypeKind
            {
                get => mnodeOrderedTypeKind;
            }

            public void SetNodeSidePNG(byte[] sidePNGBytes, ENodeSideType nodeSideType)
            {
                if(mnodePNGs.ContainsKey(nodeSideType))
                {
                    mnodePNGs.Remove(nodeSideType);
                }
                mnodePNGs.Add(nodeSideType, sidePNGBytes);
            }
            public void SetOrderedClassify(OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
            {
                morderedTypeClassify = orderedTypeClassify;
            }
            public void SetNodeOrderedTypeKind(string kind)
            {
                mnodeOrderedTypeKind = kind;
            }
        }
        public class BiologyNode
        {
            public enum EBiologyType
            {
                Dynamic = 1,
                Static = 2
            };
            public enum EMovementTendencyType
            {
                Offensive = 1,
                Defensive = 2,
                NoReaction = 3,
                Symbiosis = 4
            };
            public enum ENodeSideType
            {
                N = 1,
                S = 2,
                W = 3,
                E = 4,
                NW = 5,
                NE = 6,
                SW = 7,
                SE = 8
            };

            private string mnodeName;
            private Dictionary<ENodeSideType, byte[]> mnodePNGs;
            private EBiologyType mbiologyType;
            private Dictionary<BiologyNode, EMovementTendencyType> mmovementTendencyTypes;
            private EMovementTendencyType mdefaultMovementTendencyType;

            private OrderedNodeType.EOrderedTypeClassify morderedTypeClassify;
            private string mnodeOrderedTypeKind;

            public BiologyNode(string nodeName)
            {
                mnodeName = nodeName;
                mnodePNGs = new Dictionary<ENodeSideType, byte[]>();

                mbiologyType = new EBiologyType();
                mmovementTendencyTypes = new Dictionary<BiologyNode, EMovementTendencyType>();
                mdefaultMovementTendencyType = new EMovementTendencyType();

                morderedTypeClassify = new OrderedNodeType.EOrderedTypeClassify();
                mnodeOrderedTypeKind = default(string);
            }

            public string NodeName
            {
                get => mnodeName;
            }
            public Dictionary<ENodeSideType, byte[]> NodePNGs
            {
                get => mnodePNGs;
            }
            public EBiologyType BiologyType
            {
                get => mbiologyType;
            }
            public Dictionary<BiologyNode, EMovementTendencyType> MovementTendencyType
            {
                get => mmovementTendencyTypes;
            }
            public EMovementTendencyType DefaultMovementTendencyType
            {
                get => mdefaultMovementTendencyType;
            }
            public OrderedNodeType.EOrderedTypeClassify OrderedTypeClassify
            {
                get => morderedTypeClassify;
            }
            public string NodeOrderedTypeKind
            {
                get => mnodeOrderedTypeKind;
            }

            public void SetNodeSidePNG(byte[] sidePNGBytes, ENodeSideType nodeSideType)
            {
                if(mnodePNGs.ContainsKey(nodeSideType))
                {
                    mnodePNGs.Remove(nodeSideType);
                }

                mnodePNGs.Add(nodeSideType, sidePNGBytes);
            }
            public void SetBiologyType(EBiologyType biologyType)
            {
                mbiologyType = biologyType;
            }
            public void SetOrderedClassify(OrderedNodeType.EOrderedTypeClassify orderedTypeClassify)
            {
                morderedTypeClassify = orderedTypeClassify;
            }
            public void SetNodeOrderedTypeKind(string kind)
            {
                mnodeOrderedTypeKind = kind;
            }
            public void SetMovementTendencyTypeTarget(BiologyNode targetBiologyNode, EMovementTendencyType movementTendencyType)
            {
                if(mmovementTendencyTypes.ContainsKey(targetBiologyNode))
                {
                    mmovementTendencyTypes[targetBiologyNode] = movementTendencyType;
                }
                else
                {
                    mmovementTendencyTypes.Add(targetBiologyNode, movementTendencyType);
                }
            }
            public void SetDefaultMovementTendencyType(EMovementTendencyType movementTendencyType)
            {
                mdefaultMovementTendencyType = movementTendencyType;
            }
        }



        private Dictionary<int, GeologyNode> mgeologyNodeTable;
        private Dictionary<int, BiologyNode> mbiologyNodeTable;
        private List<string> mgeologyNodeNames;
        private List<string> mbiologyNodeNames;
        private OrderedCube morderedCube;



        public AxisBaseTablePalette(in OrderedCube orderedCube)
        {
            mgeologyNodeTable = new Dictionary<int, GeologyNode>();
            mbiologyNodeTable = new Dictionary<int, BiologyNode>();
            mgeologyNodeNames = new List<string>();
            mbiologyNodeNames = new List<string>();
            morderedCube = orderedCube;
        }
        public AxisBaseTablePalette(in AxisBaseTablePalette axisBaseTablePalette)
        {
            mgeologyNodeTable = axisBaseTablePalette.GeologyNodeTable;
            mbiologyNodeTable = axisBaseTablePalette.BiologyNodeTable;
            mgeologyNodeNames = axisBaseTablePalette.GeologyNodeNames;
            mbiologyNodeNames = axisBaseTablePalette.BiologyNodeNames;
            morderedCube = axisBaseTablePalette.OrderedCube;
        }

        public Dictionary<int, GeologyNode> GeologyNodeTable
        {
            get => mgeologyNodeTable;
        }
        public Dictionary<int, BiologyNode> BiologyNodeTable
        {
            get => mbiologyNodeTable;
        }
        public List<string> GeologyNodeNames
        {
            get => mgeologyNodeNames;
        }
        public List<string> BiologyNodeNames
        {
            get => mbiologyNodeNames;
        }
        public OrderedCube OrderedCube
        {
            get => morderedCube;
        }

        public void AddNode(string nodeName, EPaletteType paletteType)
        {
            switch(paletteType)
            {
                case EPaletteType.Geology:
                    mgeologyNodeTable.Add(nodeName.GetHashCode(), new GeologyNode(nodeName));
                    mgeologyNodeNames.Add(nodeName);
                    break;

                case EPaletteType.Biology:
                    mbiologyNodeTable.Add(nodeName.GetHashCode(), new BiologyNode(nodeName));
                    mbiologyNodeNames.Add(nodeName);
                    break;
            }
        }
        public bool RemoveNode(string nodeName, EPaletteType paletteType)
        {
            switch(paletteType)
            {
                case EPaletteType.Geology:
                    if(!mgeologyNodeTable.ContainsKey(nodeName.GetHashCode()))
                    {
                        return false;
                    }
                    else
                    {
                        mgeologyNodeTable.Remove(nodeName.GetHashCode());
                        mgeologyNodeNames.Remove(nodeName);
                    }
                    break;

                case EPaletteType.Biology:
                    if(!mbiologyNodeTable.ContainsKey(nodeName.GetHashCode()))
                    {
                        return false;
                    }
                    else
                    {
                        mbiologyNodeTable.Remove(nodeName.GetHashCode());
                        mbiologyNodeNames.Remove(nodeName);
                    }
                    break;
            }

            return true;
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
        public OrderedCube(OrderedCube orderedCube)
        {
            mxLength = orderedCube.X;
            myLength = orderedCube.Y;
            mzLength = orderedCube.Z;

            morderedNodeType = orderedCube.OrderedNodeType;
            morderedCubeTable = orderedCube.OrderedCubeTable;
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



        public OrderedNodeType(OrderedNodeType orderedNodeType)
        {
            mlowOrderedNodeTypes = orderedNodeType.LowOrderedNodeType;
            mmiddleOrderedNodeTypes = orderedNodeType.MiddleOrderedNodeType;
            mhighOrderedNodeTypes = orderedNodeType.HighOrderedNodeType;
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