using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityJsonDll;

namespace AxisCubeTool_TablePathManager
{
    #region Define

    #endregion

    #region MainClass
    public class TablePathManager
    {
        //  defines
        internal struct LinkTableInfo
        {
            public Dictionary<string, string> mnodeEditingManagerLinkTable;
            public Dictionary<string, string> mpaletteManagerLinkTable;

            public int mnodeEditingTableCount, mpaletteTableCount;
        };

        public enum ETableKind
        {
            NodeEditingTable = 1,
            PaletteTable = 2
        };



        //  properties
        private const string mfolderPath_NodeEditor = "NodeEditor/";
        private const string mfolderPath_NodeEditingTable = "NodeEditor/NodeEditingTable/";
        private const string mfolderPath_PaletteTable = "NodeEditor/PaletteTable/";

        private const string mnodeEditorLinkFileName = "NodeEditingLinkTable";

        [SerializeField]
        private LinkTableInfo mlinkTableInfo;


        //  methods
            //  constructor
        public TablePathManager()
        {
            mlinkTableInfo = new LinkTableInfo();

            //  NodeEditingManager Path Table Object Load or Create New One
            if(File.Exists(CustomUnityJsonuUtility.pathForDocumentsFile(mnodeEditorLinkFileName, mfolderPath_NodeEditor)))
            {
                mlinkTableInfo = CustomUnityJsonuUtility.LoadjsonFile<LinkTableInfo>(CustomUnityJsonuUtility.pathForDocumentsFolder("NodeEditor/"), "NodeEditingManagerPathTable");
            }
            else
            {
                mlinkTableInfo = new LinkTableInfo();
                mlinkTableInfo.mnodeEditingManagerLinkTable = new Dictionary<string, string>();
                mlinkTableInfo.mpaletteManagerLinkTable = new Dictionary<string, string>();
                mlinkTableInfo.mnodeEditingTableCount = 0;
                mlinkTableInfo.mpaletteTableCount = 0;
            }
        }

            //  get, set
        public string FolderPath_NodeEditor
        {
            get => mfolderPath_NodeEditor;
        }
        public string FolderPath_NodeEditingTable
        {
            get => mfolderPath_NodeEditingTable;
        }
        public string FolderPath_PaletteTable
        {
            get => mfolderPath_PaletteTable;
        }
        public string NodeEditorLinkTable
        {
            get => mnodeEditorLinkFileName;
        }

            //  behaviour
        public string TryGetNextPath(in ETableKind tableKind)
        {
            string path = null;

            switch(tableKind)
            {
                case ETableKind.NodeEditingTable:
                    path = Path.Combine(FolderPath_NodeEditingTable, mlinkTableInfo.mnodeEditingTableCount.ToString());
                    break;

                case ETableKind.PaletteTable:
                    path = Path.Combine(FolderPath_PaletteTable, mlinkTableInfo.mpaletteTableCount.ToString());
                    break;
            }

            return path;
        }
        public void SetNextTablePath(in ETableKind tableKind, string userDetermineName, string defaultName)
        {
            switch(tableKind)
            {
                case ETableKind.NodeEditingTable:
                    mlinkTableInfo.mnodeEditingManagerLinkTable.Add(userDetermineName, defaultName);
                    mlinkTableInfo.mnodeEditingTableCount++;
                    break;

                case ETableKind.PaletteTable:
                    mlinkTableInfo.mpaletteManagerLinkTable.Add(userDetermineName, defaultName);
                    mlinkTableInfo.mpaletteTableCount++;
                    break;
            }
        }
        public void SaveFile()
        {
            CustomUnityJsonuUtility.SaveToJson(FolderPath_NodeEditor, NodeEditorLinkTable, mlinkTableInfo);
        }
    }
    #endregion
}
