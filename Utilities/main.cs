using System;
using System.Collections.Generic;
using System.IO;
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
    public class CaveStructure
    {
        public class Node
        {
            private bool mbexist;
            private bool mbwebBase;
            private BridgeConnection mbridgeConnection;
            private List<Vector2Int> mabstractWebConnection;


            public Node()
            {
                mbexist = false;
                mbwebBase = false;
                mbridgeConnection = new BridgeConnection();
                mabstractWebConnection = new List<Vector2Int>();
            }



            public bool Exist
            {
                get => mbexist;
                set => mbexist = value;
            }
            public bool WebBase
            {
                get => mbwebBase;
                set => mbwebBase = value;
            }
            public BridgeConnection BridgeConnection
            {
                get => mbridgeConnection;
            }
            public List<Vector2Int> AbstractWebConnection
            {
                get => mabstractWebConnection;
            }
        }
        public class BridgeConnection
        {
            public enum EDirction
            {
                North = 1,
                South = 2,
                East = 3,
                West = 4
            };



            private Dictionary<EDirction, float> mgates;
            private Vector2Int mcrossGate;
            private List<Vector2Int> mextraCoords;



            public BridgeConnection()
            {
                mgates = new Dictionary<EDirction, float>();
                mextraCoords = new List<Vector2Int>();
            }

            public Dictionary<EDirction, float> Gates
            {
                get => mgates;
            }
            public Vector2Int CrossGate
            {
                get => mcrossGate;
            }
            public List<Vector2Int> ExtraCoords
            {
                get => mextraCoords;
            }

            internal void GenerateWebConnection(Vector2Int detailAxisLength, Dictionary<EDirction, float> surroundGates, float detailDiagonalChancePerNode)
            {
                if(surroundGates.ContainsKey(EDirction.West))
                {
                    mgates.Add(EDirction.West, surroundGates[EDirction.West]);
                }
                if(surroundGates.ContainsKey(EDirction.North))
                {
                    mgates.Add(EDirction.North, surroundGates[EDirction.North]);
                }

                System.Random random = new System.Random();
                if(random.Next(0, 11) <= 5 ? true : false)
                {
                    mgates.Add(EDirction.East, random.Next(0, detailAxisLength.y));
                }
                if (random.Next(0, 11) <= 5 ? true : false)
                {
                    mgates.Add(EDirction.South, random.Next(0, detailAxisLength.x));
                }

                mcrossGate = new Vector2Int(random.Next(1, detailAxisLength.x - 1), random.Next(1, detailAxisLength.y - 1));

                int extraDotCount = random.Next(0, 3);
                for(int count = 0; count < extraDotCount; count++)
                {
                    while(true)
                    {
                        Vector2Int tempExtraCoord = new Vector2Int(random.Next(1, detailAxisLength.x - 1), random.Next(1, detailAxisLength.y - 1));

                        if (CrossGate != tempExtraCoord)
                        {
                            mextraCoords.Add(tempExtraCoord);
                            break;
                        }
                    }
                }

                random = null;
            }
        }

        public struct InputPack
        {
            private Vector2Int mabstractAxisLength, mdetailAxisLength;
            private float mnodeOccurChance, mbridgeOccurChance, mnodeOccurWeight, mnodeWebBaseDetermineChance, mnodeWebConnectionChance;
            private float mabstractDiagonalChancePerNode, mdetailDiagonalChancePerNode;
            private Texture2D mtexture;

            public InputPack(Texture2D texture, Vector2Int abstractAxisLength, Vector2Int detailAxisLength, float nodeOccurChance, float bridgeOccurChance, float nodeOccurWeight, float nodeWebBaseDetermineChance, float nodeWebConnectionChance, float abstractDiagonalChancePerNode, float detailDiagonalChancePerNode)
            {
                mtexture = texture;
                mabstractAxisLength = abstractAxisLength;
                mdetailAxisLength = detailAxisLength;
                mnodeOccurChance = nodeOccurChance;
                mbridgeOccurChance = bridgeOccurChance;
                mnodeOccurWeight = nodeOccurWeight;
                mnodeWebBaseDetermineChance = nodeWebBaseDetermineChance;
                mnodeWebConnectionChance = nodeWebConnectionChance;
                mabstractDiagonalChancePerNode = abstractDiagonalChancePerNode;
                mdetailDiagonalChancePerNode = detailDiagonalChancePerNode;
            }

            public Texture2D Texture
            {
                get => mtexture;
            }
            public Vector2Int AbstractAxisLegnth
            {
                get => mabstractAxisLength;
            }
            public Vector2Int DetailAxisLength
            {
                get => mdetailAxisLength;
            }
            public float NodeOccurChance
            {
                get => mnodeOccurChance;
            }
            public float BridgeOccurChance
            {
                get => mbridgeOccurChance;
            }
            public float NodeOccurWeight
            {
                get => mnodeOccurWeight;
            }
            public float NodeWebBaseDetermineChance
            {
                get => mnodeWebBaseDetermineChance;
            }
            public float NodeWebConnectionChance
            {
                get => mnodeWebConnectionChance;
            }
            public float AbstractDiagonalChancePerNode
            {
                get => mabstractDiagonalChancePerNode;
            }
            public float DetailDiagonalChancePerNode
            {
                get => mdetailDiagonalChancePerNode;
            }
        }



        private Node[,] mnodeTable;
        private Vector2Int mabstractAxisLength, mdetailAxisLength;



        public CaveStructure(InputPack inputPack)
        {
            mabstractAxisLength = inputPack.AbstractAxisLegnth;
            mdetailAxisLength = inputPack.DetailAxisLength;

            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            GenerateCaveStructure(inputPack);
        }

        public Node[,] NodeTable
        {
            get => mnodeTable;
        }
        public Vector2Int AbstractAxisLength
        {
            get => mabstractAxisLength;
        }
        public Vector2Int DetailAxisLength
        {
            get => mdetailAxisLength;
        }

        public void GenerateCaveStructure(InputPack inputPack)
        {
            List<Vector2Int> baseNodeCoords = new List<Vector2Int>();

            //  base abstract structure generate
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if(CalculateChance(inputPack.NodeOccurChance + inputPack.Texture.GetPixel(coord_x, coord_y).r * inputPack.NodeOccurWeight))
                    {
                        mnodeTable[coord_y, coord_x].Exist = true;

                        if(CalculateChance(inputPack.NodeWebBaseDetermineChance))
                        {
                            mnodeTable[coord_y, coord_x].WebBase = true;
                            baseNodeCoords.Add(new Vector2Int(coord_x, coord_y));
                        }
                    }
                }
            }

            //  diagonal abstract structure generate
            for(int i = 0; i < baseNodeCoords.Count; i++)
            {
                for(int j = 0; j < baseNodeCoords.Count; j++)
                {
                    if(i != j && baseNodeCoords[i].x != baseNodeCoords[j].x && baseNodeCoords[i].y != baseNodeCoords[j].y && CalculateChance(1.0f - inputPack.AbstractDiagonalChancePerNode * (float)(Math.Sqrt(Math.Pow(baseNodeCoords[i].x - baseNodeCoords[j].x , 2.0) + Math.Pow(baseNodeCoords[i].y - baseNodeCoords[j].y, 2.0)))))
                    {
                        mnodeTable[baseNodeCoords[i].y, baseNodeCoords[i].x].AbstractWebConnection.Add(baseNodeCoords[j]);
                    }
                }
            }

            //  detail bridge structure generate
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if(mnodeTable[coord_y, coord_x].Exist == false)
                    {
                        mnodeTable[coord_y, coord_x].BridgeConnection.GenerateWebConnection(DetailAxisLength, DetectCurroundBridgeGate(new Vector2Int(coord_x, coord_y)), inputPack.NodeWebBaseDetermineChance);
                    }
                }
            }
        }
        public bool CalculateChance(float chance)
        {
            if(chance >= 1.0f)
            {
                return true;
            }
            else if(chance <= 0.0f)
            {
                return false;
            }

            return UnityEngine.Random.Range(0.0f, 1.0f) <= chance ? true : false;
        }
        public Dictionary<BridgeConnection.EDirction, float> DetectCurroundBridgeGate(Vector2Int curCoord)
        {
            Dictionary<BridgeConnection.EDirction, float> bridgeGates = new Dictionary<BridgeConnection.EDirction, float>();

            if (curCoord.x - 1 >= 0 && NodeTable[curCoord.y, curCoord.x - 1].Exist == false)
            {
                bridgeGates.Add(BridgeConnection.EDirction.West, NodeTable[curCoord.y, curCoord.x - 1].BridgeConnection.Gates[BridgeConnection.EDirction.East]);
            }
            if (curCoord.y - 1 >= 0 && NodeTable[curCoord.y - 1, curCoord.x].Exist == false)
            {
                bridgeGates.Add(BridgeConnection.EDirction.North, NodeTable[curCoord.y - 1, curCoord.x].BridgeConnection.Gates[BridgeConnection.EDirction.South]);
            }

            return bridgeGates;
        }
    }
}
