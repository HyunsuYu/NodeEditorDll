using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AxisBaseTableEditor;
using UnityEngine;
using DirectNodeEditor;

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

        public static List<EncodingNodeData> GetEncodingNodeDatas(AxisBaseTableNodePalette axisBaseTablePalette, AxisBaseTableNodePalette.ENodePaletteType paletteType)
        {
            List<EncodingNodeData> encodingNodeDatas = new List<EncodingNodeData>();
            int curIndex = 1;

            switch(paletteType)
            {
                case AxisBaseTableNodePalette.ENodePaletteType.Geology:
                    for (int index = 0; index < axisBaseTablePalette.GeologyNodeNames.Count; index++)
                    {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.GeologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                        
                    }
                    break;

                case AxisBaseTableNodePalette.ENodePaletteType.Biology:
                    for (int index = 0; index < axisBaseTablePalette.BiologyNodeNames.Count; index++)
                    {
                            encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.BiologyNodeNames[index], GetNextPrimeNumber(curIndex)));
                            curIndex++;
                    }
                    break;

                case AxisBaseTableNodePalette.ENodePaletteType.Prop:
                    for (int index = 0; index < axisBaseTablePalette.BiologyNodeNames.Count; index++)
                    {
                        encodingNodeDatas.Add(new EncodingNodeData(axisBaseTablePalette.PropNodeNames[index], GetNextPrimeNumber(curIndex)));
                        curIndex++;
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
    public static class AESEncrypt
    {
        public static string Encrypt(string textToEncrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        public static string Decrypt(string textToDecrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) { len = keyBytes.Length; }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
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
            public enum ELinkedState
            {
                North = 1,
                South = 2,
                East = 3,
                West = 4
            };



            private bool mbexist;
            private bool mbwebBase;
            private BridgeConnection mbridgeConnection;
            private List<Vector2Int> mabstractWebConnection;
            private List<ELinkedState> mlinkedState;


            public Node()
            {
                mbexist = false;
                mbwebBase = false;
                mbridgeConnection = new BridgeConnection();
                mabstractWebConnection = new List<Vector2Int>();
                mlinkedState = new List<ELinkedState>();
            }



            internal bool Exist
            {
                get => mbexist;
                set => mbexist = value;
            }
            internal bool WebBase
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
            public List<ELinkedState> LinkedState
            {
                get => mlinkedState;
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

        public enum EChanceKinds
        {
            NodeOccurChance = 1,
            NodeOccurWeight = 2,
            NodeWebBaseDetermineChance = 3,
            BridgeWebDetermineChance = 5,
            AbstractDiagonalChancePerNode = 6,
            DetailDiagonalChancePerNode = 7
        };
        public enum EAxisType
        {
            Vertical = 1,
            Horizontal = 2
        };

        public struct InputPack
        {
            private Vector2Int mabstractAxisLength, mdetailAxisLength;
            private Dictionary<EChanceKinds, float> mchances;
            private Texture2D mtexture;

            public InputPack(Texture2D texture, Vector2Int abstractAxisLength, Vector2Int detailAxisLength, Dictionary<EChanceKinds, float> chances)
            {
                mtexture = texture;
                mabstractAxisLength = abstractAxisLength;
                mdetailAxisLength = detailAxisLength;
                mchances = chances;
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
            public Dictionary<EChanceKinds, float> Chances
            {
                get => mchances;
            }
        }



        private Node[,] mnodeTable;
        private List<List<float>> mverticalBoundaryFrequency, mhorizontalBoundaryFrequency;
        private Vector2Int mabstractAxisLength, mdetailAxisLength;



        public CaveStructure(InputPack inputPack)
        {
            mabstractAxisLength = inputPack.AbstractAxisLegnth;
            mdetailAxisLength = inputPack.DetailAxisLength;

            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];
            mverticalBoundaryFrequency = new List<List<float>>();
            mhorizontalBoundaryFrequency = new List<List<float>>();

            GenerateCaveStructure(inputPack);
            DetectNodeLinkedState();

            System.Random random = new System.Random();
            float shortcutNum = (float)(Math.PI / 2.0);
            for(int index = 0; index < AbstractAxisLength.y + 1; index++)
            {
                mhorizontalBoundaryFrequency.Add(new List<float>());

                mhorizontalBoundaryFrequency[index].Add(shortcutNum * 2);
                mhorizontalBoundaryFrequency[index].Add(shortcutNum * 4);
                mhorizontalBoundaryFrequency[index].Add(shortcutNum / random.Next(3, 11));
                mhorizontalBoundaryFrequency[index].Add(shortcutNum / random.Next(20, 41));
            }
            for(int index = 0; index < AbstractAxisLength.x + 1; index++)
            {
                mverticalBoundaryFrequency.Add(new List<float>());

                mverticalBoundaryFrequency[index].Add(shortcutNum * 2);
                mverticalBoundaryFrequency[index].Add(shortcutNum * 4);
                mverticalBoundaryFrequency[index].Add(shortcutNum / random.Next(3, 11));
                mverticalBoundaryFrequency[index].Add(shortcutNum / random.Next(20, 41));
            }

            random = null;
        }

        public Node[,] NodeTable
        {
            get => mnodeTable;
        }
        public List<List<float>> VerticalBoundaryFrequency
        {
            get => mverticalBoundaryFrequency;
        }
        public List<List<float>> HorizontalBoundaryFrequency
        {
            get => mhorizontalBoundaryFrequency;
        }
        public Vector2Int AbstractAxisLength
        {
            get => mabstractAxisLength;
        }
        public Vector2Int DetailAxisLength
        {
            get => mdetailAxisLength;
        }

        public static List<List<float>> GetNewBoundaryFrequency(int axisLength)
        {
            List<List<float>> frequency = new List<List<float>>();
            float shortcutNum = (float)(Math.PI / 2.0);
            
            System.Random random = new System.Random();

            for (int index = 0; index < axisLength + 1; index++)
            {
                frequency.Add(new List<float>());

                frequency[index].Add(shortcutNum * 2);
                frequency[index].Add(shortcutNum * 4);
                frequency[index].Add(shortcutNum / random.Next(3, 11));
                frequency[index].Add(shortcutNum / random.Next(20, 41));
            }

            random = null;

            return frequency;
        }

        private void GenerateCaveStructure(InputPack inputPack)
        {
            List<Vector2Int> baseNodeCoords = new List<Vector2Int>();

            //  base abstract structure generate
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if(CalculateChance(inputPack.Chances[EChanceKinds.NodeOccurChance] + CalculateWeight(inputPack.Texture.GetPixel(coord_x, coord_y).r, inputPack.Chances[EChanceKinds.NodeOccurWeight])))
                    {
                        mnodeTable[coord_y, coord_x].Exist = true;

                        if(CalculateChance(inputPack.Chances[EChanceKinds.NodeWebBaseDetermineChance]))
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
                    if(i != j && baseNodeCoords[i].x != baseNodeCoords[j].x && baseNodeCoords[i].y != baseNodeCoords[j].y && CalculateChance(1.0f - inputPack.Chances[EChanceKinds.AbstractDiagonalChancePerNode] * (float)(Math.Sqrt(Math.Pow(baseNodeCoords[i].x - baseNodeCoords[j].x , 2.0) + Math.Pow(baseNodeCoords[i].y - baseNodeCoords[j].y, 2.0)))))
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
                        mnodeTable[coord_y, coord_x].BridgeConnection.GenerateWebConnection(DetailAxisLength, DetectCurroundBridgeGate(new Vector2Int(coord_x, coord_y)), inputPack.Chances[EChanceKinds.BridgeWebDetermineChance]);
                    }
                }
            }
        }
        private bool CalculateChance(float chance)
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
        private float CalculateWeight(float noiseValue, float weightValue)
        {
            float result = 0.0f;

            if(noiseValue < 0.0f || noiseValue > 1.0f)
            {
                result = 0.0f;
            }
            else if(noiseValue >= 0.0f && noiseValue <= 0.5f)
            {
                result = noiseValue;
            }
            else
            {
                result = (-1.0f) * noiseValue + 1.0f;
            }

            return result;
        }
        private Dictionary<BridgeConnection.EDirction, float> DetectCurroundBridgeGate(Vector2Int curCoord)
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
        private void DetectNodeLinkedState()
        {
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if(coord_x - 1 >= 0 && NodeTable[coord_y, coord_x - 1].Exist)
                    {
                        mnodeTable[coord_y, coord_x].LinkedState.Add(Node.ELinkedState.West);
                    }
                    if(coord_y - 1 >= 0 && NodeTable[coord_y - 1, coord_x].Exist)
                    {
                        mnodeTable[coord_y, coord_x].LinkedState.Add(Node.ELinkedState.North);
                    }
                    if (coord_x + 1 < AbstractAxisLength.x && NodeTable[coord_y, coord_x + 1].Exist)
                    {
                        mnodeTable[coord_y, coord_x].LinkedState.Add(Node.ELinkedState.East);
                    }
                    if(coord_y + 1 < AbstractAxisLength.y && NodeTable[coord_y + 1, coord_x].Exist)
                    {
                        mnodeTable[coord_y, coord_x].LinkedState.Add(Node.ELinkedState.South);
                    }
                }
            }
        }
    }
    public class DetermineMapRealCoord
    {
        private List<Vector3Int[,]> mnodeTable;



        public DetermineMapRealCoord(DirectNodeTable directNodeTable, int boundaryExtraSpace)
        {
            mnodeTable = new List<Vector3Int[,]>();
            for(int count = 0; count < directNodeTable.FloorTable.Count; count++)
            {
                mnodeTable.Add(CalculateFloorRealCoords(directNodeTable.FloorTable[count], boundaryExtraSpace));
            }
        }

        public List<Vector3Int[,]> NodeTable
        {
            get => mnodeTable;
        }
        public float DefaultCartoonCutUnit
        {
            get => 0.05f;
        }

        private Vector3Int[,] CalculateFloorRealCoords(FloorTable floorTable, int boundaryExtraSpace)
        {
            Vector3Int[,] table = new Vector3Int[floorTable.AbstractAxisLength.y * (floorTable.DetailAxisLength.y + 2 * boundaryExtraSpace), floorTable.AbstractAxisLength.x * (floorTable.DetailAxisLength.x + 2 * boundaryExtraSpace)];

            for(int coord_y = 0; coord_y < floorTable.AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < floorTable.AbstractAxisLength.x; coord_x++)
                {
                    if(floorTable.NodeTable[coord_y, coord_x].Possible)
                    {
                        for (int detailCoord_y = 0; detailCoord_y < floorTable.DetailAxisLength.y; detailCoord_y++)
                        {
                            for (int detailCoord_x = 0; detailCoord_x < floorTable.DetailAxisLength.x; detailCoord_x++)
                            {
                                if ((floorTable.NodeTable[coord_y, coord_x].CaveStructure.LinkedState.Contains(CaveStructure.Node.ELinkedState.North) ? 0 : CalculateBoundaryFrequency(floorTable.HorizontalBoundaryFrequency[floorTable.FloorDepth], coord_x * floorTable.DetailAxisLength.x + detailCoord_x, boundaryExtraSpace)) >= detailCoord_y && (floorTable.NodeTable[coord_y, coord_x].CaveStructure.LinkedState.Contains(CaveStructure.Node.ELinkedState.South) ? floorTable.DetailAxisLength.y - 1 : floorTable.DetailAxisLength.y + CalculateBoundaryFrequency(floorTable.HorizontalBoundaryFrequency[floorTable.FloorDepth], coord_x * floorTable.DetailAxisLength.x + detailCoord_x, boundaryExtraSpace) - 1) <= detailCoord_y)
                                {
                                    if ((floorTable.NodeTable[coord_y, coord_x].CaveStructure.LinkedState.Contains(CaveStructure.Node.ELinkedState.West) ? 0 : CalculateBoundaryFrequency(floorTable.VerticalBoundaryFrequency[floorTable.FloorDepth], coord_y * floorTable.DetailAxisLength.y + detailCoord_y, boundaryExtraSpace)) >= detailCoord_x && (floorTable.NodeTable[coord_y, coord_x].CaveStructure.LinkedState.Contains(CaveStructure.Node.ELinkedState.East) ? floorTable.DetailAxisLength.x - 1 : floorTable.DetailAxisLength.x + CalculateBoundaryFrequency(floorTable.VerticalBoundaryFrequency[floorTable.FloorDepth], coord_y * floorTable.DetailAxisLength.y + detailCoord_y, boundaryExtraSpace) - 1) <= detailCoord_x)
                                    {
                                        table[coord_y * floorTable.DetailAxisLength.y + detailCoord_y, coord_x * floorTable.DetailAxisLength.x + detailCoord_x] = new Vector3Int(coord_x * floorTable.DetailAxisLength.x + detailCoord_x, coord_x * floorTable.DetailAxisLength.y + detailCoord_y, (int)(Math.Ceiling(floorTable.NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x] * boundaryExtraSpace) / boundaryExtraSpace));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return table;
        }
        private int CalculateBoundaryFrequency(List<float> frequency, int t, int boundaryExtraSpace)
        {
            return (int)(Math.Sin(frequency[0] * t) + Math.Sin(frequency[1] * t) + Math.Sin(frequency[2] * t) + Math.Sin(frequency[3] * t)) / 4 * boundaryExtraSpace;
        }
    }
}