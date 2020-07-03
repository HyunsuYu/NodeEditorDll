using System;
using System.Collections.Generic;
using UnityEngine;
using AxisCubeTool_CameraManagerDll;
using AxisCubeTool_UIManagerDll;
using AxisCubeTool_NodeEditorDll;

namespace AxisCubeToolManagerDll
{
    #region Defines

    #endregion

    #region MainClass
    public class AxisCubeToolManager
    {
        //  defines
        public struct InputPack
        {
            public GameObject mtargetObject;

            public InputPack(in GameObject targetObject)
            {
                mtargetObject = targetObject;
            }
        };



        //  properties
        private GameObject mtargetObject;



        //  methods
            //  constructor
        public AxisCubeToolManager(in InputPack inputPack)
        {
            mtargetObject = inputPack.mtargetObject;
        }
    }
    #endregion

    #region SubClass
    internal class TablePathManager
    {

    }
    #endregion
}
