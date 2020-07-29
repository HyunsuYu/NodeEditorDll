using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Animations;

namespace DirectNodeTableManagerTool
{
    #region MainClass
    public class DIrectNodeTable
    {
        public enum ENodePlaneKind
        {
            Top = 1,
            FrontSide = 2,
            BackSide = 3,
            LeftSide = 4,
            RightSide = 5
        };



        private List<FloorTable> mfloors;
        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;



        public DIrectNodeTable(in DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            mfloors = new List<FloorTable>();
            mfloors.Add(new FloorTable(new FloorTable.InputPack(new Vector2Int(DirectNodeTableCoreInfo.DefaultAxisLength, DirectNodeTableCoreInfo.DefaultAxisLength), DirectNodeTableCoreInfo.BlockPerNode)));
        }

        public List<FloorTable> Floors
        {
            get => mfloors;
        }
        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }

        public void AddFloor(int floorIndex, in Vector2Int axisLength)
        {
            mfloors.Insert(floorIndex, new FloorTable(new FloorTable.InputPack(axisLength, DirectNodeTableCoreInfo.BlockPerNode)));
        }
        public bool RemoveFloor(int targetfloorIndex)
        {
            return mfloors.Remove(mfloors[targetfloorIndex]);
        }
        public void MoveFloor(int targetIndex, int destinationIndex)
        {
            FloorTable tempFloorTable = Floors[targetIndex];
            Floors.Remove(Floors[targetIndex]);
            Floors.Insert(destinationIndex, tempFloorTable);
        }
        public void SetAxisLength(int targetFloorIndex, int newX, int newY)
        {
            mfloors[targetFloorIndex].SetAxisLength(newX, newY);
        }
        public byte[] GetTableJsonByteData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
    #endregion

    #region SubClass
    public class AbstractNodeData
    {
        private AxisBaseTableManager.AxisBaseTablePalette.Node[,] mnodeTable;



        public AxisBaseTableManager.AxisBaseTablePalette.Node[,] NodeTable
        {
            get => mnodeTable;
        }
    }
    public class DetailNodeData
    {
        private AxisBaseTableManager.AxisBaseTablePalette.Node[,] mnodeTable;



        public AxisBaseTableManager.AxisBaseTablePalette.Node[,] NodeTable
        {
            get => mnodeTable;
        }
    }
    public class SubNodeData
    {
        private AxisBaseTableManager.AxisBaseTablePalette.Node[,] mnodeTable;



        public AxisBaseTableManager.AxisBaseTablePalette.Node[,] NodeTable
        {
            get => mnodeTable;
        }
    }
    public class FloorTable
    {
        public struct InputPack
        {
            private Vector2Int maxisLength;
            private int mblockPerNode;

            public InputPack(in Vector2Int axisLength, int blockPerNode)
            {
                maxisLength = axisLength;
                mblockPerNode = blockPerNode;
            }

            public Vector2Int AxisLength
            {
                get => maxisLength;
            }
            public int BlockPerNode
            {
                get => mblockPerNode;
            }
        };
        public struct Node
        {
            private Vector3Int mnodePosition;

            private AbstractNodeData mabstractNodeData;
            private DetailNodeData mdetailNodeData;
            private SubNodeData msubNodeData;

            private bool mbactive;

            public Node(in Vector3Int nodePosition)
            {
                mnodePosition = nodePosition;
                mabstractNodeData = new AbstractNodeData();
                mdetailNodeData = new DetailNodeData();
                msubNodeData = new SubNodeData();
                mbactive = false;
            }

            public Vector3Int NodePosition
            {
                get => mnodePosition;
            }
            public AbstractNodeData AbstractNodeData
            {
                get => mabstractNodeData;
            }
            public DetailNodeData DetailNodeData
            {
                get => mdetailNodeData;
            }
            public SubNodeData SubNodeData
            {
                get => msubNodeData;
            }
            public bool Active
            {
                get => mbactive;
                set => mbactive = value;
            }
        };



        private Vector2Int maxisLength;
        private Node[,] mnodeTable;

        private int mblockPerNode;
        private bool mbenforcedMode;



        public FloorTable(in InputPack inputPack)
        {
            maxisLength = inputPack.AxisLength;
            mnodeTable = new Node[AxisLength.y, AxisLength.x];

            mblockPerNode = inputPack.BlockPerNode;
            mbenforcedMode = true;
        }

        public Node[,] NodeTable
        {
            get => mnodeTable;
        }
        public Vector2Int AxisLength
        {
            get => maxisLength;
        }
        public bool EnforcedMode
        {
            get => mbenforcedMode;
            set => mbenforcedMode = value;
        }
        internal int BlockPerNode
        {
            get => mblockPerNode;
        }

        public void SetNode(AxisBaseTableManager.AxisBaseTablePalette.Node node, Vector2Int relativePosition)
        {
            if(relativePosition.x < 0 || relativePosition.x > BlockPerNode - 1 || relativePosition.y < 0 || relativePosition.y > BlockPerNode - 1)
            {
                return;
            }

            
        }
        public void SetAxisLength(int newX, int newY)
        {
            Node[,] tempTable = new Node[newX, newY];

            for(int coord_x = 0; coord_x < (AxisLength.x < newX ? AxisLength.x : newX); coord_x++)
            {
                for(int coord_y = 0; coord_y < (AxisLength.y < newY ? AxisLength.y : newY); coord_y++)
                {
                    tempTable[coord_y, coord_x] = NodeTable[coord_y, coord_x];
                }
            }

            mnodeTable = tempTable;
        }
    }
    public class DirectNodeTableCoreInfo
    {
        public struct InputPack
        {
            private AxisBaseTableManager.AxisBaseTable maxisBaseTable;
            private int mblockPerNode;

            public InputPack(in AxisBaseTableManager.AxisBaseTable axisBaseTable, int blockPerNode)
            {
                maxisBaseTable = axisBaseTable;
                mblockPerNode = blockPerNode;
            }

            public AxisBaseTableManager.AxisBaseTable AxisBaseTable
            {
                get => maxisBaseTable;
            }
            public int BlockPerNode
            {
                get => mblockPerNode;
            }
        }



        private AxisBaseTableManager.AxisBaseTable maxisBaseTable;
        private int mblockPerNode;

        public DirectNodeTableCoreInfo(in InputPack inputPack)
        {
            maxisBaseTable = inputPack.AxisBaseTable;
            mblockPerNode = inputPack.BlockPerNode;
        }

        public AxisBaseTableManager.AxisBaseTable AxisBaseTable
        {
            get => maxisBaseTable;
        }
        public int BlockPerNode
        {
            get => mblockPerNode;
        }
        public int DefaultAxisLength
        {
            get => 50;
        }
    }
    #endregion
}