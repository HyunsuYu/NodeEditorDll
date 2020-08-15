using System;
using System.Collections.Generic;
using AxisBaseTableManager;
using UnityEngine;

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
        public static List<EncodingNodeData> DisorderNumber(List<EncodingNodeData> encodingNodeDatas, int targetNumber)
        {
            List<EncodingNodeData> tempEncodingNodeDatas = new List<EncodingNodeData>();

            for(int index = 0; index < encodingNodeDatas.Count; index++)
            {
                if(targetNumber % encodingNodeDatas[index].PrimeNumber == 0)
                {
                    tempEncodingNodeDatas.Add(new EncodingNodeData(encodingNodeDatas[index].NodeName, encodingNodeDatas[index].PrimeNumber));
                }
            }

            return encodingNodeDatas;
        }
        public static float[,] CompressionNodes(int xAxisLength, int yAxisLength, int[,] nodeTable, List<EncodingNodeData> encodingNodeDatas)
        {
            if(encodingNodeDatas.Count == 0)
            {
                return null;
            }

            float[,] tempNodeTable = new float[yAxisLength, xAxisLength];
            int maxMulNumber = 0;

            //  Calculate maxMulNumber
            for(int index = 0; index < encodingNodeDatas.Count; index++)
            {
                maxMulNumber *= encodingNodeDatas[index].PrimeNumber;
            }

            for(int coord_y = 0; coord_y < yAxisLength; coord_y++)
            {
                for(int coord_x = 0; coord_x < xAxisLength; coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = (float)(nodeTable[coord_y, coord_x] / maxMulNumber);
                }
            }

            return tempNodeTable;
        }
        public static int[,] DecompressionNodes(int xAxisLength, int yAxisLength, float[,] nodeTable, List<EncodingNodeData> encodingNodeDatas)
        {
            if (encodingNodeDatas.Count == 0)
            {
                return null;
            }

            int[,] tempNodeTable = new int[yAxisLength, xAxisLength];
            int maxMulNumber = 0;

            //  Calculate maxMulNumber
            for (int index = 0; index < encodingNodeDatas.Count; index++)
            {
                maxMulNumber *= encodingNodeDatas[index].PrimeNumber;
            }

            for (int coord_y = 0; coord_y < yAxisLength; coord_y++)
            {
                for (int coord_x = 0; coord_x < xAxisLength; coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = (int)(nodeTable[coord_y, coord_x] * maxMulNumber);
                }
            }

            return tempNodeTable;
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
    public class VallyCalculate
    {
        public struct FrequencyFunction
        {
            public double mamplitude;
            public double mperiod;
        }
        private struct NodeVector
        {
            public double mcurTime;
            public Vector2 mcenter;
            public Vector2 mvector;
            public Vector2 mdot;
        }

        public struct InputPack
        {
            private Vector2Int mcoord;
            private int mmaxVectorNum;
            private int mmaxAmplitude;
            private int mmaxPeriod;
            private double mfrequency;

            public InputPack(Vector2Int coord, int maxVectorNum, int maxAmplitude, int maxPeriod, double frequency)
            {
                mcoord = coord;
                mmaxVectorNum = maxVectorNum;
                mmaxAmplitude = maxAmplitude;
                mmaxPeriod = maxPeriod;
                mfrequency = frequency;
            }

            public Vector2Int Coord
            {
                get => mcoord;
            }
            public int MaxVectorNum
            {
                get => mmaxVectorNum;
            }
            public int MaxAmplitude
            {
                get => mmaxAmplitude;
            }
            public int MaxPeriod
            {
                get => mmaxPeriod;
            }
            public double Frequency
            {
                get => mfrequency;
            }
        };



        private FrequencyFunction[] mnodeInfos;
        private NodeVector[] mnodeVectors;
        private List<Vector2Int> mresultCoords;

        private int mmaxVectorNum;
        private int mmaxAmplitude;
        private int mmaxPeriod;
        private double mfrequency;
        private int mfirstN;

        private Vector2Int mcoord;

        //  method
        public VallyCalculate(in InputPack inputPack)
        {
            mmaxVectorNum = inputPack.MaxVectorNum;
            mmaxAmplitude = inputPack.MaxAmplitude;
            mmaxPeriod = inputPack.MaxPeriod;
            mfrequency = inputPack.Frequency;
            mfirstN = mmaxVectorNum * 3 / 4 * (-1);

            mcoord = inputPack.Coord;

            mnodeInfos = new FrequencyFunction[mmaxVectorNum];
            mnodeVectors = new NodeVector[mmaxVectorNum];
            mresultCoords = new List<Vector2Int>();

            SetNodeInfos();
        }

        internal VallyCalculate(VallyCalculate vallyCalculate, double changeAmplitudeAmount, double changePeriodAmount)
        {
            mmaxVectorNum = vallyCalculate.mmaxVectorNum;
            mmaxAmplitude = vallyCalculate.mmaxAmplitude;
            mmaxPeriod = vallyCalculate.mmaxPeriod;
            mfrequency = vallyCalculate.mfrequency;
            mfirstN = vallyCalculate.mfirstN;

            mcoord = vallyCalculate.mcoord;

            mnodeInfos = new FrequencyFunction[mmaxVectorNum];
            mnodeVectors = new NodeVector[mmaxVectorNum];
            mresultCoords = new List<Vector2Int>();

            SetNextNodeInfos(vallyCalculate.mnodeInfos, changeAmplitudeAmount, changePeriodAmount);
        }

        public static int DefaultMaxVectorNum
        {
            get => 15;
        }
        public static int DefaultMaxAmplitude
        {
            get => 10;
        }
        public static int DefaultMaxPeriod
        {
            get => 7;
        }
        public static double DefaultFrequency
        {
            get => 0.01;
        }
        public static double DefaultChangeAmplitudeAmount
        {
            get => 1.0;
        }
        public static double DefaultChangePeriodAmount
        {
            get => 1.0;
        }
        public static double DefaultExtraTime
        {
            get => Math.PI;
        }

        public List<Vector2Int> CalsulateValley(double extraTime)
        {
            while(MakeFourier(extraTime))
            {
                ;
            }

            return mresultCoords;
        }
        public VallyCalculate GetNextFloorValleyCalculate(double changeAmplitudeAmount, double changePeriodAmount)
        {
            return new VallyCalculate(this, changeAmplitudeAmount, changePeriodAmount);
        }

        private bool MakeFourier(double extraTime)
        {
            mnodeVectors[0].mcenter.x = mcoord.x;
            mnodeVectors[0].mcenter.y = mcoord.y;

            for (int index = 0; index < mmaxVectorNum; index++)
            {
                mnodeVectors[index].mvector.x = (float)(GetFx(index) * GetCircle_x(index));
                if (mfirstN + index < 0)
                {
                    mnodeVectors[index].mvector.y = (float)(GetFx(index) * GetCircle_y(index, true));
                }
                else
                {
                    mnodeVectors[index].mvector.y = (float)(GetFx(index) * GetCircle_y(index, false));
                }
                mnodeVectors[index].mdot.x = mnodeVectors[index].mcenter.x + mnodeVectors[index].mvector.x;
                mnodeVectors[index].mdot.y = mnodeVectors[index].mcenter.y + mnodeVectors[index].mvector.y;
                mnodeVectors[index].mcurTime += mfrequency * Math.Abs(mfirstN + index);

                if (mfirstN + index == 0 && mnodeVectors[index].mcurTime >= Math.PI)
                {
                    mresultCoords.Add(new Vector2Int((int)mnodeVectors[mmaxVectorNum - 1].mdot.x, (int)mnodeVectors[mmaxVectorNum - 1].mdot.y));
                }
                if(mfirstN + index == 0 && mnodeVectors[index].mcurTime >= Math.PI + extraTime)
                {
                    return false;
                }
            }

            return true;
        }
        private void SetNodeInfos()
        {
            System.Random random = new System.Random();

            for (int i = 0; i < mmaxVectorNum; i++)
            {
                mnodeInfos[i].mamplitude = random.NextDouble() * mmaxAmplitude;
                mnodeInfos[i].mperiod = random.NextDouble() * mmaxPeriod;
            }

            random = null;
        }
        private void SetNextNodeInfos(FrequencyFunction[] frequencyFunctions, double changeAmplitudeAmount, double changePeriodAmount)
        {
            for(int i = 0; i < mmaxVectorNum; i++)
            {
                mnodeInfos[i].mamplitude = frequencyFunctions[i].mamplitude + changeAmplitudeAmount;
                mnodeInfos[i].mperiod = frequencyFunctions[i].mperiod + changePeriodAmount;
            }
        }
        private double GetFx(int index)
        {
            return mnodeInfos[index].mamplitude * Math.Cos(mnodeVectors[index].mcurTime * mnodeInfos[index].mperiod);
        }
        private double GetCircle_x(int index)
        {
            return Math.Cos(mnodeVectors[index].mcurTime);
        }
        private double GetCircle_y(int index, bool isClockWay)
        {
            if (isClockWay == true)
            {
                return Math.Sin(mnodeVectors[index].mcurTime);
            }
            else
            {
                return (-1) * Math.Sin(mnodeVectors[index].mcurTime);
            }
        }
        private void MoveNestVectors(int index)
        {
            for (int i = index + 1; i < mmaxVectorNum; i++)
            {
                mnodeVectors[i].mcenter.x = mnodeVectors[i - 1].mdot.x;
                mnodeVectors[i].mcenter.y = mnodeVectors[i - 1].mdot.y;
                mnodeVectors[i].mdot.x = mnodeVectors[i].mcenter.x + mnodeVectors[i].mvector.x;
                mnodeVectors[i].mdot.y = mnodeVectors[i].mcenter.y + mnodeVectors[i].mvector.y;
            }
        }
    }
}
