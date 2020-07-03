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
    public class NodeTableManager : INodeEditing
    {
        //  defines
        public struct NodeInfo
        {
            
        }

        public enum EAxisKind
        {
            lava = 1,
            Glacier = 2,
            Depth = 3
        };



        //  properties
        private GuideAreamanager mguideAreamanager;
        private NodePositionManager mnodePositionManager;

        private Dictionary<string, int> mnodeKindsTable;




        //  methods
            //  constructor
        public NodeTableManager()
        {
            mguideAreamanager = new GuideAreamanager();
            mnodePositionManager = new NodePositionManager();

        }

            //  Implements
        public void InsertNode()
        {

        }
        public void DeleteNode()
        {

        }
    }
    public class PaletteTableManager : INodeEditing
    {
        //  defines

        //  properties

        //  methods
            //  constructor
        public PaletteTableManager()
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
