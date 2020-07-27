using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using UnityEngine;

namespace DirectNodeTableManagerTool
{
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
            public AxisBaseTableManager.AxisBaseTable maxisBaseTable;
            public DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

            public InputPack(in AxisBaseTableManager.AxisBaseTable axisBaseTable, DirectNodeTableCoreInfo directNodeTableCoreInfo)
            {
                maxisBaseTable = axisBaseTable;
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            }
        };
        public struct Node
        {
            private Vector3Int mnodePosition;
            private Dictionary<ENodePlaneKind, byte[]> mnodeSideImages;
            private NodeGameData mnodeGameData;

            public Node(in Vector3Int nodePosition, Dictionary<ENodePlaneKind, byte[]> nodeSideImages, NodeGameData nodeGameData)
            {
                mnodePosition = nodePosition;
                mnodeSideImages = nodeSideImages;
                mnodeGameData = nodeGameData;
            }

            public Vector3Int NodePosition
            {
                get => mnodePosition;
            }
            public Dictionary<ENodePlaneKind, byte[]> NodeSideImages
            {
                get => mnodeSideImages;
            }
            public NodeGameData NodeGameData
            {
                get => mnodeGameData;
            }
        };



        private AxisBaseTableManager.AxisBaseTable maxisBaseTable;
        private List<Node[,]> mnodeTables;
        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;



        public DIrectNodeTable(in InputPack inputPack)
        {
            maxisBaseTable = inputPack.maxisBaseTable;
        }

        public AxisBaseTableManager.AxisBaseTable AxisBaseTable
        {
            get => maxisBaseTable;
        }
        public List<Node[,]> NodeTable
        {
            get => mnodeTables;
        }
        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }

        public byte[] GetTableJsonByteData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
    public class NodeGameData
    {

    }
    public class DirectNodeTableCoreInfo
    {

    }
}