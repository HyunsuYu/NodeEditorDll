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

        public struct InputPack
        {
            private AxisBaseTableManager.AxisBaseTable maxisBaseTable;
            private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

            public InputPack(in AxisBaseTableManager.AxisBaseTable axisBaseTable, DirectNodeTableCoreInfo directNodeTableCoreInfo)
            {
                maxisBaseTable = axisBaseTable;
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            }

            public AxisBaseTableManager.AxisBaseTable AxisBaseTable
            {
                get => maxisBaseTable;
            }
            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
        };



        private AxisBaseTableManager.AxisBaseTable maxisBaseTable;
        private List<FloorTable> mfloors;
        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
        private const int mdefaultAxisLength = 50;



        public DIrectNodeTable(in InputPack inputPack)
        {
            maxisBaseTable = inputPack.AxisBaseTable;
            mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;
            mfloors = new List<FloorTable>();
            mfloors.Add(new FloorTable(new FloorTable.InputPack(new Vector2Int(DefaultAxisLength, DefaultAxisLength))));
        }

        public AxisBaseTableManager.AxisBaseTable AxisBaseTable
        {
            get => maxisBaseTable;
        }
        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }
        internal int DefaultAxisLength
        {
            get => mdefaultAxisLength;
        }

        public void AddFloor(int floorIndex, in Vector2Int axisLength)
        {
            mfloors.Insert(floorIndex, new FloorTable(new FloorTable.InputPack(axisLength)));
        }
        public bool RemoveFloor(int targetfloorIndex)
        {
            return mfloors.Remove(mfloors[targetfloorIndex]);
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

    }
    public class DetailNodeData
    {

    }
    public class SubNodeData
    {

    }
    internal class FloorTable
    {
        public struct InputPack
        {
            private Vector2Int maxisLength;

            public InputPack(in Vector2Int axisLength)
            {
                maxisLength = axisLength;
            }

            public Vector2Int AxisLength
            {
                get => maxisLength;
            }
        };
        public struct Node
        {
            private Vector3Int mnodePosition;
            private AbstractNodeData mabstractNodeData;
            private DetailNodeData mdetailNodeData;
            private SubNodeData msubNodeData;

            public Node(in Vector3Int nodePosition)
            {
                mnodePosition = nodePosition;
                mabstractNodeData = new AbstractNodeData();
                mdetailNodeData = new DetailNodeData();
                msubNodeData = new SubNodeData();
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
        };



        private Vector2Int maxisLength;
        private Node[,] mnodeTable;



        public FloorTable(in InputPack inputPack)
        {
            maxisLength = inputPack.AxisLength;
            mnodeTable = new Node[AxisLength.y, AxisLength.x];
        }

        public Node[,] NodeTable
        {
            get => mnodeTable;
        }
        public Vector2Int AxisLength
        {
            get => maxisLength;
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

    }
    #endregion
}