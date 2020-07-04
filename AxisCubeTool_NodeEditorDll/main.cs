using System;
using System.Collections.Generic;
using UnityEngine;
using UnityJsonDll;

namespace AxisCubeTool_NodeEditorDll
{
    #region Defines
    internal interface INodeEditing
    {
        public void InsertNode();
        public void DeleteNode();
    }
    #endregion

    #region mainClass
    [SerializeField]
    public class TableEditingManager : INodeEditing
    {
        //  defines
        internal struct NodeDivideInfo
        {
            public int mnodeHashCode;
            public float mpercent;
        }
        internal struct Node
        {
            public List<NodeDivideInfo> mdetailModeInfos;
            public List<NodeDivideInfo> msubNodeInfos;
            public ENodeQualifier mnodeQualifier;
        }

        public enum EAxisKind
        {
            Depth = 1,
            Glacier = 2,
            Lava = 3
        };
        public enum ENodeQualifier
        {
            Mutable = 1,
            Immutable = 2
        };
        public enum ETableQualifier
        {
            Public = 1,
            Private = 2,
            Internal = 3
        };



        //  properties
        private GuideAreamanager mguideAreamanager;
        private NodePositionManager mnodePositionManager;

        private ETableQualifier mtableQualifier;

        private Vector3Int maxisLength; //  x : Lava, y : Glacier, z : Depth
        private Node[,,] mnodeTable;

        private PaletteManager mpaletteManager;



        //  methods
            //  constructor
        public TableEditingManager(in PaletteManager paletteManager)
        {
            mguideAreamanager = new GuideAreamanager();
            mnodePositionManager = new NodePositionManager();

            mtableQualifier = ETableQualifier.Public;

            maxisLength = new Vector3Int(5, 5, 5);

        }

            //  Implements
        public void InsertNode()
        {

        }
        public void DeleteNode()
        {

        }

            //  get, set
        public ETableQualifier TableQualifier
        {
            get => mtableQualifier;
            set => mtableQualifier = value;
        }

            //  behaviour
        public void SetAxisLength(in EAxisKind axisKind, int changeAxisLength)
        {

        }
    }
    [SerializeField]
    public class PaletteManager : INodeEditing
    {
        //  defines
        public struct DetailNode
        {

        }
        public struct SubNode
        {

        }

        public enum DetailNodeKind
        {
            SedimentaryRock = 1,
            IgneousRock = 2,
            MetamorphicRock = 3
        };



        //  properties



        //  methods
            //  constructor
        public PaletteManager()
        {

        }

            //  Implements
        public void InsertNode()
        {

        }
        public void DeleteNode()
        {

        }
    }
    #endregion
    #region SubClass
    internal class GuideAreamanager
    {

    }
    internal class NodePositionManager
    {

    }
    #endregion
}
