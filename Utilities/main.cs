using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using AxisBaseTableManager;

namespace Utilities
{
    public static class NodeEncoding
    {
        public struct EncodingNodeData
        {
            private string mnodeName;
            private int mprimeNumber;

            public EncodingNodeData(string nodeName, int primeNumber)
            {
                mnodeName = nodeName;
                mprimeNumber = primeNumber;
            }

            public string NodeName
            {
                get => mnodeName;
            }
            public int PrimeNumber
            {
                get => mprimeNumber;
            }
        }

        public static List<EncodingNodeData> GetEncodingNodeDatas(AxisBaseTablePalette axisBaseTablePalette, AxisBaseTablePalette.EPaletteType paletteType)
        {
            List<EncodingNodeData> encodingNodeDatas = new List<EncodingNodeData>();
            int curIndex = 1;

            switch(paletteType)
            {
                case AxisBaseTablePalette.EPaletteType.Geology:
                    for (int index = 0; index < axisBaseTablePalette.GeologyNodeNames.Count; index++)
                    {
                        if (axisBaseTablePalette.GeologyNodeTable[axisBaseTablePalette.GeologyNodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.Low)
                        {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.GeologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        }
                    }
                    for (int index = 0; index < axisBaseTablePalette.GeologyNodeNames.Count; index++)
                    {
                        if (axisBaseTablePalette.GeologyNodeTable[axisBaseTablePalette.GeologyNodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.Middle)
                        {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.GeologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        }
                    }
                    for (int index = 0; index < axisBaseTablePalette.GeologyNodeNames.Count; index++)
                    {
                        if (axisBaseTablePalette.GeologyNodeTable[axisBaseTablePalette.GeologyNodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.High)
                        {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.GeologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        }
                    }
                    break;

                case AxisBaseTablePalette.EPaletteType.Biology:
                    for (int index = 0; index < axisBaseTablePalette.BiologyNodeNames.Count; index++)
                    {
                        if (axisBaseTablePalette.BiologyNodeTable[axisBaseTablePalette.BiologyNodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.Low)
                        {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.BiologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        }
                    }
                    for (int index = 0; index < axisBaseTablePalette.BiologyNodeNames.Count; index++)
                    {
                        if (axisBaseTablePalette.BiologyNodeTable[axisBaseTablePalette.BiologyNodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.Middle)
                        {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.BiologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        }
                    }
                    for (int index = 0; index < axisBaseTablePalette.BiologyNodeNames.Count; index++)
                    {
                        if (axisBaseTablePalette.BiologyNodeTable[axisBaseTablePalette.BiologyNodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.High)
                        {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.BiologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        }
                    }
                    break;
            }

            return encodingNodeDatas;
        }
        public static int GetNodePrimeNumber(List<EncodingNodeData> encodingNodeDatas, string nodeName)
        {
            for(int index = 0; index < encodingNodeDatas.Count; index++)
            {
                if(encodingNodeDatas[index].NodeName == nodeName)
                {
                    return encodingNodeDatas[index].PrimeNumber;
                }
            }

            return -1;
        }
        public static List<EncodingNodeData> DisorderNumber(int targetNumber)
        {
            List<EncodingNodeData> encodingNodeDatas = new List<EncodingNodeData>();



            return encodingNodeDatas;
        }
        internal static int GetNextPrimeNumber(int index)
        {
            List<int> primeStorage = new List<int>();
            primeStorage.Add(2);

            int curNumber = 3;
            bool triger = false;

            while(true)
            {
                for(int i = 0; i < primeStorage.Count; i++)
                {
                    if(curNumber % primeStorage[i] == 0)
                    {
                        triger = true;
                        break;
                    }
                }

                if(triger == true)
                {
                    triger = false;
                }
                else
                {
                    primeStorage.Add(curNumber);
                    triger = false;
                }

                if(primeStorage.Count == index)
                {
                    break;
                }
            }

            return primeStorage[primeStorage.Count - 1];
        }
    }
}
