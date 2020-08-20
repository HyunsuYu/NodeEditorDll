using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;

namespace AxisBaseTableEditor
{
    public class AxisBaseTable
    {
        private int[,,] mgeologyNodeTable;
        private int[,,] mbiologyNodeTable;
        private int[,,] mpropNodeTable;
        private AxisBaseTableNodePalette maxisBaseTableNodePalette;
        private List<NodeEncoding.EncodingNodeData> mgeologyEncodingNodeDatas;
        private List<NodeEncoding.EncodingNodeData> mbiologyEncodingNodeDatas;
        private List<NodeEncoding.EncodingNodeData> mpropEncodingNodeDatas;
        private Vector3Int mfundamentalAxisLength;  //  x = lava, y = glacier, z = eitr



        public AxisBaseTable(in AxisBaseTableNodePalette axisBaseTablePalette)
        {
            maxisBaseTableNodePalette = axisBaseTablePalette;
            mfundamentalAxisLength = AxisBaseTable.DefaultFundamentalAxisLength;
            mgeologyNodeTable = new int[FundamentalAxisLength.x, FundamentalAxisLength.y, FundamentalAxisLength.z];
            mbiologyNodeTable = new int[FundamentalAxisLength.x, FundamentalAxisLength.y, FundamentalAxisLength.z];
            mpropNodeTable = new int[FundamentalAxisLength.x, FundamentalAxisLength.y, FundamentalAxisLength.z];
            mgeologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTableNodePalette, AxisBaseTableNodePalette.ENodePaletteType.Geology);
            mbiologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTableNodePalette, AxisBaseTableNodePalette.ENodePaletteType.Biology);
            mpropEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTableNodePalette, AxisBaseTableNodePalette.ENodePaletteType.Prop);
            for(int coord_x = 0; coord_x < FundamentalAxisLength.x; coord_x++)
            {
                for(int coord_y = 0; coord_y < FundamentalAxisLength.y; coord_y++)
                {
                    for(int coord_z = 0; coord_z < FundamentalAxisLength.z; coord_z++)
                    {
                        mgeologyNodeTable[coord_x, coord_y, coord_z] = 1;
                        mbiologyNodeTable[coord_x, coord_y, coord_z] = 1;
                        mpropNodeTable[coord_x, coord_y, coord_z] = 1;
                    }
                }
            }
        }
        public AxisBaseTable(in AxisBaseTableNodePalette axisBaseTablePalette, int x, int y, int z)
        {
            maxisBaseTableNodePalette = axisBaseTablePalette;
            mfundamentalAxisLength = new Vector3Int(x, y, z);
            mgeologyNodeTable = new int[FundamentalAxisLength.x, FundamentalAxisLength.y, FundamentalAxisLength.z];
            mbiologyNodeTable = new int[FundamentalAxisLength.x, FundamentalAxisLength.y, FundamentalAxisLength.z];
            mpropNodeTable = new int[FundamentalAxisLength.x, FundamentalAxisLength.y, FundamentalAxisLength.z];
            mgeologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTableNodePalette, AxisBaseTableNodePalette.ENodePaletteType.Geology);
            mbiologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTableNodePalette, AxisBaseTableNodePalette.ENodePaletteType.Biology);
            mpropEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTableNodePalette, AxisBaseTableNodePalette.ENodePaletteType.Prop);
            for (int coord_x = 0; coord_x < FundamentalAxisLength.x; coord_x++)
            {
                for (int coord_y = 0; coord_y < FundamentalAxisLength.y; coord_y++)
                {
                    for (int coord_z = 0; coord_z < FundamentalAxisLength.z; coord_z++)
                    {
                        mgeologyNodeTable[coord_x, coord_y, coord_z] = 1;
                        mbiologyNodeTable[coord_x, coord_y, coord_z] = 1;
                        mpropNodeTable[coord_x, coord_y, coord_z] = 1;
                    }
                }
            }
        }
        public AxisBaseTable(in AxisBaseTable axisBaseTableManager)
        {
            mgeologyNodeTable = axisBaseTableManager.GeologyNodeTable;
            mbiologyNodeTable = axisBaseTableManager.BiologyNodeTable;
            mpropNodeTable = axisBaseTableManager.PropNodeTable;
            maxisBaseTableNodePalette = axisBaseTableManager.AxisBaseTableNodePalette;
            mgeologyEncodingNodeDatas = axisBaseTableManager.GeologyEncodingNodeDatas;
            mbiologyEncodingNodeDatas = axisBaseTableManager.BiologyEncodingNodeDatas;
            mpropEncodingNodeDatas = axisBaseTableManager.PropEncodingNOdeDatas;
            mfundamentalAxisLength = axisBaseTableManager.FundamentalAxisLength;
        }

        public int[,,] GeologyNodeTable
        {
            get => mgeologyNodeTable;
        }
        public int[,,] BiologyNodeTable
        {
            get => mbiologyNodeTable;
        }
        public int[,,] PropNodeTable
        {
            get => mpropNodeTable;
        }
        public List<NodeEncoding.EncodingNodeData> GeologyEncodingNodeDatas
        {
            get => mgeologyEncodingNodeDatas;
        }
        public List<NodeEncoding.EncodingNodeData> BiologyEncodingNodeDatas
        {
            get => mbiologyEncodingNodeDatas;
        }
        public List<NodeEncoding.EncodingNodeData> PropEncodingNOdeDatas
        {
            get => mpropEncodingNodeDatas;
        }
        public AxisBaseTableNodePalette AxisBaseTableNodePalette
        {
            get => maxisBaseTableNodePalette;
        }
        public Vector3Int FundamentalAxisLength
        {
            get => mfundamentalAxisLength;
        }
        public static Vector3Int DefaultFundamentalAxisLength
        {
            get => new Vector3Int(30, 15, 33);
        }

        public bool SetNode(int x, int y, int z, NodeEncoding.EncodingNodeData targetNode, AxisBaseTableNodePalette.ENodePaletteType paletteType)
        {
            if(x >= 0 && x < FundamentalAxisLength.x && y >= 0 && y < FundamentalAxisLength.y && z >= 0 && z < FundamentalAxisLength.z)
            {
                switch (paletteType)
                {
                    case AxisBaseTableNodePalette.ENodePaletteType.Geology:
                        if (GeologyNodeTable[x, y, z] % targetNode.PrimeNumber != 0)
                        {
                            mgeologyNodeTable[x, y, z] *= targetNode.PrimeNumber;
                        }
                        return true;

                    case AxisBaseTableNodePalette.ENodePaletteType.Biology:
                        if(BiologyNodeTable[x,y,z] % targetNode.PrimeNumber != 0)
                        {
                            mbiologyNodeTable[x, y, z] *= targetNode.PrimeNumber;
                        }
                        return true;
                    case AxisBaseTableNodePalette.ENodePaletteType.Prop:
                        if(PropNodeTable[x,y,z] % targetNode.PrimeNumber != 0)
                        {
                            mpropNodeTable[x, y, z] *= targetNode.PrimeNumber;
                        }
                        return true;
                }
            }

            return false;
        }
        public bool RemoveNode(int x, int y, int z, NodeEncoding.EncodingNodeData targetNode, AxisBaseTableNodePalette.ENodePaletteType paletteType)
        {
            if(x >= 0 && x < FundamentalAxisLength.x && y >= 0 && y < FundamentalAxisLength.y && z >= 0 && z < FundamentalAxisLength.z)
            {
                switch(paletteType)
                {
                    case AxisBaseTableNodePalette.ENodePaletteType.Geology:
                        if(GeologyNodeTable[x,y,z] % targetNode.PrimeNumber == 0)
                        {
                            mgeologyNodeTable[x, y, z] /= targetNode.PrimeNumber;
                        }
                        break;

                    case AxisBaseTableNodePalette.ENodePaletteType.Biology:
                        if(BiologyNodeTable[x,y,z] % targetNode.PrimeNumber == 0)
                        {
                            mbiologyNodeTable[x, y, z] /= targetNode.PrimeNumber;
                        }
                        break;

                    case AxisBaseTableNodePalette.ENodePaletteType.Prop:
                        if(PropNodeTable[x,y,z] % targetNode.PrimeNumber == 0)
                        {
                            mpropNodeTable[x, y, z] /= targetNode.PrimeNumber;
                        }
                        break;
                }
                return true;
            }
            return false;
        }
        public void SetAxisLength(int newX, int newY, int newZ)
        {
            int[,,] tempGeologyNodeTable = new int[newX, newY, newZ];
            int[,,] tempBiologyNodeTable = new int[newX, newY, newZ];
            int[,,] tempPropNodeTable = new int[newX, newY, newZ];

            for (int coord_x = 0; coord_x < ((FundamentalAxisLength.x < newX) ? FundamentalAxisLength.x : newX); coord_x++)
            {
                for (int coord_y = 0; coord_y < ((FundamentalAxisLength.y < newY) ? FundamentalAxisLength.y : newY); coord_y++)
                {
                    for (int coord_z = 0; coord_z < ((FundamentalAxisLength.z < newZ) ? FundamentalAxisLength.z : newZ); coord_z++)
                    {
                        tempGeologyNodeTable[coord_x, coord_y, coord_z] = GeologyNodeTable[coord_x, coord_y, coord_z];
                        tempBiologyNodeTable[coord_x, coord_y, coord_z] = BiologyNodeTable[coord_x, coord_y, coord_z];
                        tempPropNodeTable[coord_x, coord_y, coord_z] = PropNodeTable[coord_x, coord_y, coord_z];
                    }
                }
            }

            mgeologyNodeTable = tempGeologyNodeTable;
            mbiologyNodeTable = tempBiologyNodeTable;
            mpropNodeTable = tempPropNodeTable;
            mfundamentalAxisLength = new Vector3Int(newX, newY, newZ);
        }
        public bool CheckComplete()
        {
            bool flag = true;

            for(int coord_x = 0; coord_x < FundamentalAxisLength.x; coord_x++)
            {
                for(int coord_y = 0; coord_y < FundamentalAxisLength.y; coord_y++)
                {
                    for(int coord_z = 0; coord_z < FundamentalAxisLength.z; coord_z++)
                    {
                        if(GeologyNodeTable[coord_x, coord_y, coord_z] == 1 || BiologyNodeTable[coord_x, coord_y, coord_z] == 1 || PropNodeTable[coord_x, coord_y, coord_z] == 1)
                        {
                            flag = false;
                            goto ESC;
                        }
                    }
                }
            }

            ESC:

            return flag;
        }
        public string GetJsonByteData()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class AxisBaseTableNodePalette
    {
        public enum ENodePaletteType
        {
            Geology = 1,
            Biology = 2,
            Prop = 3
        };
        public enum EFundamentalElements
        {
            Lava = 2,
            Glacier = 3,
            Eitr = 5,
            SinkHole = 7,
            Cave = 11
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
            public enum ENodeStateType
            {
                Solid = 1,
                Liquid = 2,
                Gas = 3
            };

            private string mnodeName;
            private Dictionary<ENodeSideType, byte[]> mnodePNGs;

            private ENodeStateType mnodeStateType;
            private float mnodeMovementSpeed;

            public GeologyNode(string nodeName)
            {
                mnodeName = nodeName;
                mnodePNGs = new Dictionary<ENodeSideType, byte[]>();

                mnodeStateType = ENodeStateType.Solid;
                mnodeMovementSpeed = 0.0f;
            }

            public string NodeName
            {
                get => mnodeName;
            }
            public Dictionary<ENodeSideType, byte[]> NodePNGs
            {
                get => mnodePNGs;
            }
            public ENodeStateType NodeStateType
            {
                get => mnodeStateType;
                set => mnodeStateType = value;
            }
            public float NodeMovementSpeed
            {
                get => mnodeMovementSpeed;
                set => mnodeMovementSpeed = value;
            }

            public void SetNodeSidePNG(byte[] sidePNGBytes, ENodeSideType nodeSideType)
            {
                if(mnodePNGs.ContainsKey(nodeSideType))
                {
                    mnodePNGs.Remove(nodeSideType);
                }
                mnodePNGs.Add(nodeSideType, sidePNGBytes);
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

            public BiologyNode(string nodeName)
            {
                mnodeName = nodeName;
                mnodePNGs = new Dictionary<ENodeSideType, byte[]>();

                mbiologyType = new EBiologyType();
                mmovementTendencyTypes = new Dictionary<BiologyNode, EMovementTendencyType>();
                mdefaultMovementTendencyType = new EMovementTendencyType();
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
        public class PropNode
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

            public PropNode(string nodeName)
            {
                mnodeName = nodeName;
                mnodePNGs = new Dictionary<ENodeSideType, byte[]>();
            }

            public string NodeName
            {
                get => mnodeName;
            }
            public Dictionary<ENodeSideType, byte[]> NodePNGs
            {
                get => mnodePNGs;
            }

            public void SetNodeSidePNG(byte[] sidePNGBytes, ENodeSideType nodeSideType)
            {
                if (mnodePNGs.ContainsKey(nodeSideType))
                {
                    mnodePNGs.Remove(nodeSideType);
                }
                mnodePNGs.Add(nodeSideType, sidePNGBytes);
            }
        }



        private Dictionary<int, GeologyNode> mgeologyNodeTable;
        private Dictionary<int, BiologyNode> mbiologyNodeTable;
        private Dictionary<int, PropNode> mpropNodeTable;
        private List<string> mgeologyNodeNames;
        private List<string> mbiologyNodeNames;
        private List<string> mpropNodeNames;



        public AxisBaseTableNodePalette(in AxisBaseTableNodePalette axisBaseTablePalette)
        {
            mgeologyNodeTable = axisBaseTablePalette.GeologyNodeTable;
            mbiologyNodeTable = axisBaseTablePalette.BiologyNodeTable;
            mpropNodeTable = axisBaseTablePalette.PropNodeTable;
            mgeologyNodeNames = axisBaseTablePalette.GeologyNodeNames;
            mbiologyNodeNames = axisBaseTablePalette.BiologyNodeNames;
            mpropNodeNames = axisBaseTablePalette.PropNodeNames;
        }

        public Dictionary<int, GeologyNode> GeologyNodeTable
        {
            get => mgeologyNodeTable;
        }
        public Dictionary<int, BiologyNode> BiologyNodeTable
        {
            get => mbiologyNodeTable;
        }
        public Dictionary<int, PropNode> PropNodeTable
        {
            get => mpropNodeTable;
        }
        public List<string> GeologyNodeNames
        {
            get => mgeologyNodeNames;
        }
        public List<string> BiologyNodeNames
        {
            get => mbiologyNodeNames;
        }
        public List<string> PropNodeNames
        {
            get => mpropNodeNames;
        }

        public void AddNode(string nodeName, ENodePaletteType paletteType)
        {
            switch(paletteType)
            {
                case ENodePaletteType.Geology:
                    mgeologyNodeTable.Add(nodeName.GetHashCode(), new GeologyNode(nodeName));
                    mgeologyNodeNames.Add(nodeName);
                    break;

                case ENodePaletteType.Biology:
                    mbiologyNodeTable.Add(nodeName.GetHashCode(), new BiologyNode(nodeName));
                    mbiologyNodeNames.Add(nodeName);
                    break;

                case ENodePaletteType.Prop:
                    mpropNodeTable.Add(nodeName.GetHashCode(), new PropNode(nodeName));
                    mpropNodeNames.Add(nodeName);
                    break;
            }
        }
        public bool RemoveNode(string nodeName, ENodePaletteType paletteType)
        {
            switch(paletteType)
            {
                case ENodePaletteType.Geology:
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

                case ENodePaletteType.Biology:
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

                case ENodePaletteType.Prop:
                    if (!mpropNodeTable.ContainsKey(nodeName.GetHashCode()))
                    {
                        return false;
                    }
                    else
                    {
                        mpropNodeTable.Remove(nodeName.GetHashCode());
                        mpropNodeNames.Remove(nodeName);
                    }
                    break;
            }

            return true;
        }
    }
}