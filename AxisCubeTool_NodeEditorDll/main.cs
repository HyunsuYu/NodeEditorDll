using System;
using System.Collections.Generic;
using System.IO;
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
        internal struct NodeInfo
        {
            public int mnodeHashCode;
            public float mpercent;
        }
        internal struct Node
        {
            public List<NodeInfo> mnodeInfos;
            public ENodeQualifier mnodeQualifier;
        }

        public enum EAxisKind
        {
            lava = 1,
            Glacier = 2,
            Depth = 3
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

        private Vector3Int maxisLength;
        private Node[,,] mnodeTable;



        //  methods
            //  constructor
        public TableEditingManager()
        {
            mguideAreamanager = new GuideAreamanager();
            mnodePositionManager = new NodePositionManager();

            mtableQualifier = ETableQualifier.Public;
        }

            //  Implements
        public void InsertNode()
        {

        }
        public void DeleteNode()
        {

        }
    }
    [SerializeField]
    public class PaletteManager : INodeEditing
    {
        //  defines

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
