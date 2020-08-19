using AxisBaseTableManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Newtonsoft.Json;

namespace DirectNodeEditor
{
    #region FullPack
    public class DirectFullPack
    {
        public struct InputPack
        {
            private readonly DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
            private readonly List<byte[]> mfloorPNGs;
            private readonly List<CaveStructure.Node[,]> mfloorCaveInfos;

            public InputPack(DirectNodeTableCoreInfo directNodeTableCoreInfo, List<byte[]> floorPNGs, List<CaveStructure.Node[,]> floorCaveInfos)
            {
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
                mfloorPNGs = floorPNGs;
                mfloorCaveInfos = floorCaveInfos;
            }

            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
            public List<byte[]> FloorPNGs
            {
                get => mfloorPNGs;
            }
            public List<CaveStructure.Node[,]> FloorCaveInfos
            {
                get => mfloorCaveInfos;
            }
        }



        private readonly DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
        private readonly List<byte[]> mfloorPNGs;
        private readonly List<CaveStructure.Node[,]> mfloorCaveInfos;



        public DirectFullPack(InputPack inputPack)
        {
            mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;
            mfloorPNGs = inputPack.FloorPNGs;
            mfloorCaveInfos = inputPack.FloorCaveInfos;
        }

        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }
        public List<byte[]> FloorPNGs
        {
            get => mfloorPNGs;
        }
        public List<CaveStructure.Node[,]> FloorCaveInfos
        {
            get => mfloorCaveInfos;
        }

        public static DirectFullPack GetObjectData(string jsonData)
        {
            return JsonConvert.DeserializeObject<DirectFullPack>(AESEncrypt.Decrypt(AESEncrypt.Decrypt(AESEncrypt.Decrypt(AESEncrypt.Decrypt(jsonData, "Idiot"), "This"), "Hack"), "Dont"));
        }

        public string GetJsonData()
        {
            return AESEncrypt.Encrypt(AESEncrypt.Encrypt(AESEncrypt.Encrypt(AESEncrypt.Encrypt(JsonConvert.SerializeObject(this), "Dont"), "Hack"), "This"), "Idiot");
        }
        public List<Texture2D> DisorderByteDatas()
        {
            List<Texture2D> texture2Ds = new List<Texture2D>();

            for (int index = 0; index < mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Count; index++)
            {
                texture2Ds.Add(new Texture2D(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x * DirectNodeTableCoreInfo.FloorDetailAxisLengths[index].x, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y * DirectNodeTableCoreInfo.FloorDetailAxisLengths[index].y));
                texture2Ds[index].LoadImage(mfloorPNGs[index]);
                texture2Ds[index].Apply();
            }

            return texture2Ds;
        }
        public List<FloorTable> GetFloorTable(List<Texture2D> texture2Ds)
        {
            List<FloorTable> tempFloorTable = new List<FloorTable>();

            for (int index = 0; index < texture2Ds.Count; index++)
            {
                int[,] geologyNodeTable = null;
                int[,] biologyNodeTable = null;
                int[,] propNodeTable = null;
                float[,] tempGeologyNodeTable = new float[DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x];
                float[,] tempBiologyNodeTable = new float[DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x];
                float[,] tempPropNodeTable = new float[DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x];
                float[,] heightTable = new float[DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x];

                for (int coord_y = 0; coord_y < DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y; coord_y++)
                {
                    for (int coord_x = 0; coord_x < DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x; coord_x++)
                    {
                        tempGeologyNodeTable[coord_y, coord_x] = texture2Ds[index].GetPixel(coord_x, coord_y).r;
                        tempBiologyNodeTable[coord_y, coord_x] = texture2Ds[index].GetPixel(coord_x, coord_y).g;
                        tempPropNodeTable[coord_y, coord_x] = texture2Ds[index].GetPixel(coord_x, coord_y).b;
                        heightTable[coord_y, coord_x] = texture2Ds[index].GetPixel(coord_x, coord_y).a;
                    }
                }

                geologyNodeTable = NodeEncoding.DecompressionNodes(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, tempBiologyNodeTable, DirectNodeTableCoreInfo.GeologyEncodingNodeDatas);
                biologyNodeTable = NodeEncoding.DecompressionNodes(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, tempBiologyNodeTable, DirectNodeTableCoreInfo.BiologyEncodingNodeDatas);
                propNodeTable = NodeEncoding.DecompressionNodes(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x, DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y, tempPropNodeTable, DirectNodeTableCoreInfo.PropEncodingNodeDatas);

                tempFloorTable.Add(new FloorTable(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index], DirectNodeTableCoreInfo.FloorDetailAxisLengths[index], DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength, index, geologyNodeTable, biologyNodeTable, propNodeTable, heightTable, FloorCaveInfos[index]));
            }

            return tempFloorTable;
        }
    }
    #endregion

    #region Floors
    public class DirectNodeTable
    {
        public struct InputPack_Auto
        {
            private readonly DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
            private readonly Dictionary<AxisBaseTablePalette.EFundamentalElements, bool> mgenerateFundamentalElements;
            private readonly bool mbisGenerateValley;

            public InputPack_Auto(DirectNodeTableCoreInfo directNodeTableCoreInfo, Dictionary<AxisBaseTablePalette.EFundamentalElements, bool> generateFundamentalElements, bool bisGenerateValley)
            {
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
                mgenerateFundamentalElements = generateFundamentalElements;
                mbisGenerateValley = bisGenerateValley;
            }

            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
            public Dictionary<AxisBaseTablePalette.EFundamentalElements, bool> GenerateFundamentalElements
            {
                get => mgenerateFundamentalElements;
            }
            public bool IsGenerateValley
            {
                get => mbisGenerateValley;
            }
        }
        public struct InputPack_Manual
        {
            private readonly DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

            public InputPack_Manual(DirectNodeTableCoreInfo directNodeTableCoreInfo)
            {
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            }

            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
        }
        public struct NormalNodeKind
        {
            private bool mbgeology, mbbiology, mbprop;

            public NormalNodeKind(bool geology, bool biology, bool prop)
            {
                mbgeology = geology;
                mbbiology = biology;
                mbprop = prop;
            }

            public bool Geology
            {
                get => mbgeology;
            }
            public bool Biology
            {
                get => mbbiology;
            }
            public bool Prop
            {
                get => mbprop;
            }
        }
        public struct FundamentalNodeKind
        {
            private bool mblava, mbglacier, mbeitr;

            public FundamentalNodeKind(bool lava, bool glacier, bool eitr)
            {
                mblava = lava;
                mbglacier = glacier;
                mbeitr = eitr;
            }

            public bool Lava
            {
                get => mblava;
            }
            public bool Glacier
            {
                get => mbglacier;
            }
            public bool Eitr
            {
                get => mbeitr;
            }
        }



        private readonly List<FloorTable> mfloorTable;
        private readonly DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;



        public DirectNodeTable(InputPack_Auto inputPack)
        {
            mfloorTable = new List<FloorTable>();
            mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;

            Texture2D texture = GenerateMapBoundaryNoiseMap();

            float sum = 0;
            for (int floorIndex = 0; floorIndex < DefaultFloorDepth; floorIndex++)
            {
                sum += GetCutLine(floorIndex);
            }

            VallyCalculate vallyCalculate = new VallyCalculate(new VallyCalculate.InputPack(new Vector2Int((int)UnityEngine.Random.Range(0.0f, texture.width), (int)UnityEngine.Random.Range(0.0f, texture.height)), VallyCalculate.DefaultMaxVectorNum, VallyCalculate.DefaultMaxAmplitude, VallyCalculate.DefaultMaxPeriod, VallyCalculate.DefaultFrequency));
            for (int floorIndex = 0; floorIndex < DefaultFloorDepth; floorIndex++)
            {
                Dictionary<CaveStructure.EChanceKinds, float> chances = new Dictionary<CaveStructure.EChanceKinds, float>();
                chances.Add(CaveStructure.EChanceKinds.NodeOccurChance, GetChance(CaveStructure.EChanceKinds.NodeOccurChance, floorIndex));
                chances.Add(CaveStructure.EChanceKinds.NodeOccurWeight, GetChance(CaveStructure.EChanceKinds.NodeOccurWeight, floorIndex));
                chances.Add(CaveStructure.EChanceKinds.NodeWebBaseDetermineChance, GetChance(CaveStructure.EChanceKinds.NodeWebBaseDetermineChance, floorIndex));
                chances.Add(CaveStructure.EChanceKinds.BridgeWebDetermineChance, GetChance(CaveStructure.EChanceKinds.BridgeWebDetermineChance, floorIndex));
                chances.Add(CaveStructure.EChanceKinds.AbstractDiagonalChancePerNode, GetChance(CaveStructure.EChanceKinds.AbstractDiagonalChancePerNode, floorIndex));
                chances.Add(CaveStructure.EChanceKinds.DetailDiagonalChancePerNode, GetChance(CaveStructure.EChanceKinds.DetailDiagonalChancePerNode, floorIndex));

                mfloorTable.Add(new DirectNodeEditor.FloorTable(new DirectNodeEditor.FloorTable.InputPack_Auto(GetCutLine(floorIndex) / sum, texture, inputPack.GenerateFundamentalElements, inputPack.IsGenerateValley, floorIndex, DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength, vallyCalculate.GetNextFloorValleyCalculate(VallyCalculate.DefaultChangeAmplitudeAmount, VallyCalculate.DefaultChangePeriodAmount), chances, DirectNodeTableCoreInfo)));
            }
        }
        public DirectNodeTable(InputPack_Manual inputPack)
        {
            mfloorTable = new List<FloorTable>();
            mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;
        }

        public List<FloorTable> FloorTable
        {
            get => mfloorTable;
        }
        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }
        public int DefaultFloorDepth
        {
            get => 33;
        }

        public List<Texture2D> BakeFloorsToTextyre2D()
        {
            List<Texture2D> texture2Ds = new List<Texture2D>();

            for (int index = 0; index < mfloorTable.Count; index++)
            {
                texture2Ds.Add(mfloorTable[index].BakeToTexture2D(mdirectNodeTableCoreInfo));
            }

            return texture2Ds;
        }
        public List<byte[]> BakeFloorsToPNG()
        {
            List<byte[]> bakeList = new List<byte[]>();
            List<Texture2D> texture2Ds = BakeFloorsToTextyre2D();

            for (int index = 0; index < mfloorTable.Count; index++)
            {
                bakeList.Add(texture2Ds[index].EncodeToPNG());
            }

            return bakeList;
        }
        public bool AddFloor(int index)
        {
            if (index < 0 || index > mfloorTable.Count - 1)
            {
                return false;
            }

            mfloorTable.Insert(index, new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength), new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength), index - 1, DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength)));
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Insert(index, new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength));
            mdirectNodeTableCoreInfo.FloorDetailAxisLengths.Insert(index, new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength));

            return true;
        }
        public bool AddFloor(int index, Vector2Int abstractAxisLength, Vector2Int detailAxisLength)
        {
            if (index < 0 || index > mfloorTable.Count - 1)
            {
                return false;
            }

            mfloorTable.Insert(index, new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(abstractAxisLength, detailAxisLength, index - 1, DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength)));
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Insert(index, abstractAxisLength);
            mdirectNodeTableCoreInfo.FloorDetailAxisLengths.Insert(index, detailAxisLength);

            return true;
        }
        public bool RemoveFloor(int index)
        {
            if (index < 0 || index > mfloorTable.Count - 1)
            {
                return false;
            }

            mfloorTable.Remove(mfloorTable[index]);
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Remove(mdirectNodeTableCoreInfo.FloorAbstractAxisLengths[index]);

            return false;
        }
        public bool SwapFloor(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex > mfloorTable.Count - 1 || secondIndex < 0 || secondIndex > mfloorTable.Count - 1)
            {
                return false;
            }

            DirectNodeEditor.FloorTable floorTable_first = mfloorTable[firstIndex];
            DirectNodeEditor.FloorTable floorTable_second = mfloorTable[secondIndex];
            mfloorTable.RemoveAt(firstIndex);
            mfloorTable.RemoveAt(secondIndex);
            if (firstIndex < secondIndex)
            {
                mfloorTable.Insert(firstIndex, floorTable_second);
                mfloorTable.Insert(secondIndex, floorTable_first);
            }
            else
            {
                mfloorTable.Insert(secondIndex, floorTable_first);
                mfloorTable.Insert(firstIndex, floorTable_second);
            }

            return true;
        }

        private Texture2D GenerateMapBoundaryNoiseMap()
        {
            Texture2D texture = new Texture2D(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength);

            List<float> overlapScales = new List<float>();
            float powNum = 0;

            System.Random random = new System.Random();
            int overlapCount = random.Next(3, 5);

            for (int i = 0; i < overlapCount; i++)
            {
                overlapScales.Add(UnityEngine.Random.Range(0.1f, 1.0f));
            }
            powNum = UnityEngine.Random.Range(2.0f, 4.0f);

            for (int coord_y = 0; coord_y < DirectNodeEditor.FloorTable.DefaultAxisAbstractLength; coord_y++)
            {
                for (int coord_x = 0; coord_x < DirectNodeEditor.FloorTable.DefaultAxisAbstractLength; coord_x++)
                {
                    //  noise generating
                    float noiseValue = 0.0f;

                    for (int index = 0; index < overlapScales.Count; index++)
                    {
                        noiseValue += 1 / overlapScales[index] * Mathf.PerlinNoise(coord_x / DirectNodeEditor.FloorTable.DefaultAxisAbstractLength * overlapScales[index], coord_y / DirectNodeEditor.FloorTable.DefaultAxisAbstractLength * overlapScales[index]);
                    }
                    noiseValue = (float)Math.Pow(noiseValue, powNum);

                    if (noiseValue < 0.0f)
                    {
                        noiseValue = 0.0f;
                    }
                    else if (noiseValue > 1.0f)
                    {
                        noiseValue = 1.0f;
                    }

                    texture.SetPixel(coord_x, coord_y, new Color(noiseValue, noiseValue, noiseValue));
                }
            }


            random = null;
            return texture;
        }
        private float GetCutLine(int floorIndex)
        {
            float returnValue = 0.0f;

            if (floorIndex >= 0 && floorIndex <= 3)
            {
                returnValue = (float)((-1.0f) * ((Math.Pow((floorIndex - 1.5f), 3) / 1.4f) + 2.0f) + 11.0f);
            }
            else if (floorIndex > 3 && floorIndex == 4)
            {
                returnValue = (-1.0f) * 7.0f + 11.0f;
            }
            else if (floorIndex > 4 && floorIndex <= 6)
            {
                returnValue = (float)((-1.0f) * ((-1.0f) * Math.Pow((floorIndex - 6.0), 2.0) + 10.0f) + 11.0f);
            }
            else if (floorIndex > 6 && floorIndex >= 7)
            {
                returnValue = (-1.0f) * 7.0f + 11.0f;
            }
            else if (floorIndex > 7 && floorIndex <= 9)
            {
                returnValue = (float)(Math.Pow((floorIndex - 7.0), 2.0) / 2.0f + 7.0f + 11.0f);
            }
            else if (floorIndex > 9 && floorIndex <= 10)
            {
                returnValue = (float)((-1.0f) * ((-1.0f) * Math.Pow((floorIndex - 9.0), 2.0) + 7.0f) + 11.0f);
            }
            else if (floorIndex > 10 && floorIndex <= 13)
            {
                returnValue = (float)((-1.0f) * ((-1.0f) * Math.Pow((floorIndex - 13.0), 3.0) / 27.0f + 5.0f) + 11.0f);
            }
            else if (floorIndex > 13 && floorIndex <= 15)
            {
                returnValue = (float)((-1.0f) * ((-1.0f) * (floorIndex - 12.3f) * (Math.Pow((floorIndex - 15.0), 3.0) / 5.6f) + 4.0f) + 11.0f);
            }
            else if (floorIndex > 15 && floorIndex <= 20)
            {
                returnValue = (float)((-1.0f) * (Math.Pow((floorIndex - 20.0), 2.0) / 25.0f + 3.0f) + 11.0f);
            }
            else if (floorIndex > 20 && floorIndex <= 22)
            {
                returnValue = (float)((1.0f) * ((-1.0f) * (floorIndex - 19.33f) * (Math.Pow((floorIndex - 22.0), 3.0) / 5.4f) + 2.0f) + 11.0f);
            }
            else if (floorIndex > 22 && floorIndex <= 28)
            {
                returnValue = (float)((-1.0f) * ((-1.0f) * 0.5 / 6.0f * (floorIndex - 28.0f) + 1.5f) + 11.0f);
            }
            else if (floorIndex > 28 && floorIndex <= 30)
            {
                returnValue = (float)((-1.0f) * ((-1.0f) * Math.Pow((floorIndex - 28), 2.0) / 4.0f + 1.5f) + 11.0f);
            }
            else if (floorIndex > 30 && floorIndex <= 32)
            {
                returnValue = (-1.0f) * 0.5f + 11.0f;
            }

            return returnValue;
        }
        private float GetChance(CaveStructure.EChanceKinds chanceKind, int floorDepth)
        {
            float chance = 0.0f;

            switch (chanceKind)
            {
                case CaveStructure.EChanceKinds.NodeOccurChance:
                    chance = (-1.0f) * (0.2f / 33.0f) * floorDepth + 0.3f;
                    break;

                case CaveStructure.EChanceKinds.NodeOccurWeight:
                    chance = 0.1f;
                    break;

                case CaveStructure.EChanceKinds.NodeWebBaseDetermineChance:
                    chance = 0.01f;
                    break;

                case CaveStructure.EChanceKinds.BridgeWebDetermineChance:
                    chance = 0.2f;
                    break;

                case CaveStructure.EChanceKinds.AbstractDiagonalChancePerNode:
                    chance = 0.02f;
                    break;

                case CaveStructure.EChanceKinds.DetailDiagonalChancePerNode:
                    chance = 0.05f;
                    break;
            }

            return chance;
        }
    }
    public class DirectNodeTableCoreInfo
    {
        public struct InputPack
        {
            private readonly AxisBaseTable maxisBaseTable;

            public InputPack(AxisBaseTable axisBaseTable)
            {
                maxisBaseTable = axisBaseTable;
            }

            public AxisBaseTable AxisBaseTable
            {
                get => maxisBaseTable;
            }
        }



        private readonly AxisBaseTable maxisBaseTable;
        private readonly List<NodeEncoding.EncodingNodeData> mgeologyEncodingNodeDatas;
        private readonly List<NodeEncoding.EncodingNodeData> mbiologyEncodingNodeDatas;
        private readonly List<NodeEncoding.EncodingNodeData> mpropEncodingBideDatas;
        private readonly List<Vector2Int> mfloorAbstractAxisLengths;
        private readonly List<Vector2Int> mfloorDetailAxisLengths;




        public DirectNodeTableCoreInfo(InputPack inputPack)
        {
            maxisBaseTable = inputPack.AxisBaseTable;
            mgeologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Geology);
            mbiologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Biology);
            mpropEncodingBideDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Prop);
            mfloorAbstractAxisLengths = new List<Vector2Int>();
            mfloorDetailAxisLengths = new List<Vector2Int>();
        }

        public AxisBaseTable AxisBaseTable
        {
            get => maxisBaseTable;
        }
        public List<NodeEncoding.EncodingNodeData> GeologyEncodingNodeDatas
        {
            get => mgeologyEncodingNodeDatas;
        }
        public List<NodeEncoding.EncodingNodeData> BiologyEncodingNodeDatas
        {
            get => mbiologyEncodingNodeDatas;
        }
        public List<NodeEncoding.EncodingNodeData> PropEncodingNodeDatas
        {
            get => mpropEncodingBideDatas;
        }
        public List<Vector2Int> FloorAbstractAxisLengths
        {
            get => mfloorAbstractAxisLengths;
        }
        public List<Vector2Int> FloorDetailAxisLengths
        {
            get => mfloorDetailAxisLengths;
        }
    }
    #endregion

    #region Floor
    public class FloorTable
    {
        public enum EAnchor
        {
            TopLeft = 1,
            TopStraight = 2,
            TopRight = 3,
            MiddleLeft = 4,
            Center = 5,
            MiddleRight = 6,
            BottomLeft = 7,
            BottomStraight = 8,
            BottomRight = 9
        };

        public class Node
        {
            private readonly int[,] mgeologyNodeTable;
            private readonly int[,] mbiologyNodeTable;
            private readonly int[,] mpropNodeTable;
            private readonly float[,] mheightTable;
            private readonly int[,] mfundamentalElementsTable;
            private readonly Dictionary<AxisBaseTablePalette.EFundamentalElements, int>[,] mfundamentalElementsInflunceTable;
            private CaveStructure.Node mcaveStructure;
            private bool mbpossible;



            public Node()
            {
                mgeologyNodeTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mbiologyNodeTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mpropNodeTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mheightTable = new float[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mfundamentalElementsTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mfundamentalElementsInflunceTable = new Dictionary<AxisBaseTablePalette.EFundamentalElements, int>[DefaultAxisAbstractLength, DefaultAxisAbstractLength];
                mcaveStructure = null;
                mbpossible = false;

                for (int coord_y = 0; coord_y < DefaultAxisDetailLength; coord_y++)
                {
                    for (int coord_x = 0; coord_x < DefaultAxisDetailLength; coord_x++)
                    {
                        mgeologyNodeTable[coord_y, coord_x] = 1;
                        mbiologyNodeTable[coord_y, coord_x] = 1;
                        mpropNodeTable[coord_y, coord_x] = 1;
                        mheightTable[coord_y, coord_x] = 0.0f;
                        mfundamentalElementsTable[coord_y, coord_x] = 1;
                        mfundamentalElementsInflunceTable[coord_y, coord_x] = new Dictionary<AxisBaseTablePalette.EFundamentalElements, int>();
                    }
                }
            }
            public Node(Vector2Int detailAxisLength)
            {
                mgeologyNodeTable = new int[detailAxisLength.y, detailAxisLength.x];
                mbiologyNodeTable = new int[detailAxisLength.y, detailAxisLength.x];
                mpropNodeTable = new int[detailAxisLength.y, detailAxisLength.x];
                mheightTable = new float[detailAxisLength.y, detailAxisLength.x];
                mfundamentalElementsTable = new int[detailAxisLength.y, detailAxisLength.x];
                mfundamentalElementsInflunceTable = new Dictionary<AxisBaseTablePalette.EFundamentalElements, int>[detailAxisLength.y, detailAxisLength.x];
                mcaveStructure = null;
                mbpossible = false;

                for (int coord_y = 0; coord_y < detailAxisLength.y; coord_y++)
                {
                    for (int coord_x = 0; coord_x < detailAxisLength.x; coord_x++)
                    {
                        mgeologyNodeTable[coord_y, coord_x] = 1;
                        mbiologyNodeTable[coord_y, coord_x] = 1;
                        mpropNodeTable[coord_y, coord_x] = 1;
                        mheightTable[coord_y, coord_x] = 0.0f;
                        mfundamentalElementsTable[coord_y, coord_x] = 1;
                        mfundamentalElementsInflunceTable[coord_y, coord_x] = new Dictionary<AxisBaseTablePalette.EFundamentalElements, int>();
                    }
                }
            }

            public int[,] GeologyNodeTable
            {
                get => mgeologyNodeTable;
            }
            public int[,] BiologyNodeTable
            {
                get => mbiologyNodeTable;
            }
            public int[,] PropNodeTable
            {
                get => mpropNodeTable;
            }
            public float[,] HeightTable
            {
                get => mheightTable;
            }
            public int[,] FundamentalElementsTable
            {
                get => mfundamentalElementsTable;
            }
            public Dictionary<AxisBaseTablePalette.EFundamentalElements, int>[,] FundamentalElementsInflunceTable
            {
                get => mfundamentalElementsInflunceTable;
            }
            public CaveStructure.Node CaveStructure
            {
                get => mcaveStructure;
                set => mcaveStructure = value;
            }
            public bool Possible
            {
                get => mbpossible;
                set => mbpossible = value;
            }
        }

        public struct InputPack_Auto
        {
            private readonly float mnoiseMapCutLine;
            private readonly Texture2D mnoiseTexture;
            private readonly Dictionary<AxisBaseTablePalette.EFundamentalElements, bool> mgenerateFundamentalElements;
            private readonly bool mbisGenerateValley;
            private readonly int mfloorDepth;
            private Vector3Int mfundamentalInfluenceRadius;
            private readonly VallyCalculate mvallyCalculate;
            private readonly Dictionary<CaveStructure.EChanceKinds, float> mchances;
            private readonly DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

            public InputPack_Auto(float noiseMapCutLine, Texture2D noiseTexture, Dictionary<AxisBaseTablePalette.EFundamentalElements, bool> generateFundamenralElements, bool isGenerateValley, int floorDepth, Vector3Int fundamentalInfluenceRadius, VallyCalculate vallyCalculate, Dictionary<CaveStructure.EChanceKinds, float> chances, DirectNodeTableCoreInfo directNodeTableCoreInfo)
            {
                mnoiseMapCutLine = noiseMapCutLine;
                mnoiseTexture = noiseTexture;
                mgenerateFundamentalElements = generateFundamenralElements;
                mbisGenerateValley = isGenerateValley;
                mfloorDepth = floorDepth;
                mfundamentalInfluenceRadius = fundamentalInfluenceRadius;
                mvallyCalculate = vallyCalculate;
                mchances = chances;
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            }

            public float NoiseMapCutLine
            {
                get => mnoiseMapCutLine;
            }
            public Texture2D NoiseTexture
            {
                get => mnoiseTexture;
            }
            public Dictionary<AxisBaseTablePalette.EFundamentalElements, bool> GenerateFundatemtalElements
            {
                get => mgenerateFundamentalElements;
            }
            public bool IsGenerateValley
            {
                get => mbisGenerateValley;
            }
            public int FloorDepth
            {
                get => mfloorDepth;
            }
            public Vector3Int FundamentalInfluenceRadius
            {
                get => mfundamentalInfluenceRadius;
            }
            public VallyCalculate VallyCalculate
            {
                get => mvallyCalculate;
            }
            public Dictionary<CaveStructure.EChanceKinds, float> Chances
            {
                get => mchances;
            }
            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
        }
        public struct InputPack_Manual
        {
            private Vector2Int mabstractAxisLength;
            private Vector2Int mdetailAxisLength;
            private readonly int mfloorDepth;
            private Vector3Int mfundamentalInfluenceRadius;

            public InputPack_Manual(Vector2Int abstractAxisLength, Vector2Int detailAxisLength, int floorDepth, Vector3Int fundamentalInfluenceRadius)
            {
                mabstractAxisLength = abstractAxisLength;
                mdetailAxisLength = detailAxisLength;
                mfloorDepth = floorDepth;
                mfundamentalInfluenceRadius = fundamentalInfluenceRadius;
            }

            public Vector2Int AbstractAxisLength
            {
                get => mabstractAxisLength;
            }
            public Vector2Int DetailAxisLength
            {
                get => mdetailAxisLength;
            }
            public int FloorDepth
            {
                get => mfloorDepth;
            }
            public Vector3Int FundamentalInfluenceRadius
            {
                get => mfundamentalInfluenceRadius;
            }
        }



        private Node[,] mnodeTable;
        private Vector2Int mabstractAxisLength, mdetailAxisLength;
        private Vector3Int mfundamentalInfluenceRadius;
        private List<List<float>> mverticalBoundaryFrequency, mhorizontalBoundaryFrequency;
        private readonly int mfloorDepth;



        public FloorTable(InputPack_Auto inputPack)
        {
            mfloorDepth = inputPack.FloorDepth;
            mfundamentalInfluenceRadius = inputPack.FundamentalInfluenceRadius;

            mabstractAxisLength = new Vector2Int(DefaultAxisAbstractLength, DefaultAxisAbstractLength);
            mdetailAxisLength = new Vector2Int(DefaultAxisDetailLength, DefaultAxisDetailLength);
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node();
                }
            }

            bool[,] blankTable = new bool[AbstractAxisLength.y, AbstractAxisLength.x];
            CalculateFloorBoundary(ref blankTable, inputPack);

            DetermineTableHeight();

            if (inputPack.GenerateFundatemtalElements[AxisBaseTablePalette.EFundamentalElements.Lava])
            {
                DetermineLava();
            }
            if (inputPack.GenerateFundatemtalElements[AxisBaseTablePalette.EFundamentalElements.Glacier])
            {
                DetermineGlacier();
            }
            if (inputPack.GenerateFundatemtalElements[AxisBaseTablePalette.EFundamentalElements.Eitr])
            {
                DetermineEitr();
            }
            if (inputPack.GenerateFundatemtalElements[AxisBaseTablePalette.EFundamentalElements.SinkHole])
            {
                DetermineSinkHole();
            }
            if (inputPack.IsGenerateValley)
            {
                DetermineValley(inputPack.VallyCalculate);
            }

            GenerateCaveStructure(inputPack.Chances);

            DetermineGeologyNodes(inputPack.DirectNodeTableCoreInfo);
            DetermineBiologyNodes(inputPack.DirectNodeTableCoreInfo);
            DeterminePropNodes(inputPack.DirectNodeTableCoreInfo);
        }
        public FloorTable(InputPack_Manual inputPack)
        {
            mfloorDepth = inputPack.FloorDepth;
            mfundamentalInfluenceRadius = inputPack.FundamentalInfluenceRadius;

            mabstractAxisLength = inputPack.AbstractAxisLength;
            mdetailAxisLength = inputPack.DetailAxisLength;
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(inputPack.DetailAxisLength);
                }
            }
        }
        public FloorTable(Vector2Int abstractAxisLength, Vector2Int detailAxisLength, Vector3Int fundamentalInfluenceRadius, int floorDepth, int[,] geologyNodeTable, int[,] biologyNodeTable, int[,] propNodeTable, float[,] heightTable, CaveStructure.Node[,] caveStructure)
        {
            mfloorDepth = floorDepth;
            mfundamentalInfluenceRadius = fundamentalInfluenceRadius;

            mabstractAxisLength = abstractAxisLength;
            mdetailAxisLength = detailAxisLength;
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(new Vector2Int(AbstractAxisLength.x, AbstractAxisLength.y));
                }
            }

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                    {
                        for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                        {
                            mnodeTable[coord_y, coord_x].GeologyNodeTable[detailCoord_y, detailCoord_x] = geologyNodeTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x];
                            mnodeTable[coord_y, coord_x].BiologyNodeTable[detailCoord_y, detailCoord_x] = biologyNodeTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x];
                            mnodeTable[coord_y, coord_x].PropNodeTable[detailCoord_y, detailCoord_x] = propNodeTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * detailAxisLength.x + detailCoord_x];
                            mnodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x] = heightTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x];
                        }
                    }
                    mnodeTable[coord_y, coord_x].CaveStructure = caveStructure[coord_y, coord_x];
                }
            }
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
        public int FloorDepth
        {
            get => mfloorDepth;
        }
        public Vector3Int FundamentalInfluenceRadius
        {
            get => mfundamentalInfluenceRadius;
        }
        public List<List<float>> VerticalBoundaryFrequency
        {
            get => mverticalBoundaryFrequency;
        }
        public List<List<float>> HorizontalBoundaryFrequency
        {
            get => mhorizontalBoundaryFrequency;
        }

        internal float DownerValueBoundary
        {
            get => 0.1f;
        }
        internal float DefaultSinkHoleCutLine
        {
            get => 0.05f;
        }
        internal Vector2 LavaArea
        {
            get => new Vector2(0.0f, 0.1f);
        }
        internal Vector2 GlacierArea
        {
            get => new Vector2(0.75f, 1.0f);
        }
        internal Vector2 EitrArea
        {
            get => new Vector2(0.35f, 0.65f);
        }

        public static int DefaultAxisAbstractLength
        {
            get => 100;
        }
        public static int DefaultAxisDetailLength
        {
            get => 11;
        }

        public bool SetNormalNode(Vector2Int abstractCoord, Vector2Int detailCoord, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
        {
            if (abstractCoord.x < 0 || abstractCoord.x > AbstractAxisLength.x - 1 || abstractCoord.y < 0 || abstractCoord.y > AbstractAxisLength.y - 1)
            {
                return false;
            }
            if (detailCoord.x < 0 || detailCoord.x > DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > DetailAxisLength.y - 1)
            {
                return false;
            }

            switch (paletteType)
            {
                case AxisBaseTablePalette.EPaletteType.Geology:
                    if(NodeTable[abstractCoord.y, abstractCoord.x].GeologyNodeTable[detailCoord.y, detailCoord.x] % targetNodePrimeNumber == 0)
                    {
                        mnodeTable[abstractCoord.y, abstractCoord.x].GeologyNodeTable[detailCoord.y, detailCoord.x] *= targetNodePrimeNumber;
                    }
                    break;

                case AxisBaseTablePalette.EPaletteType.Biology:
                    if(NodeTable[abstractCoord.y, abstractCoord.x].BiologyNodeTable[detailCoord.y, detailCoord.x] % targetNodePrimeNumber == 0)
                    {
                        mnodeTable[abstractCoord.y, abstractCoord.x].BiologyNodeTable[detailCoord.y, detailCoord.x] *= targetNodePrimeNumber;
                    }
                    break;

                case AxisBaseTablePalette.EPaletteType.Prop:
                    if(NodeTable[abstractCoord.y, abstractCoord.x].PropNodeTable[detailCoord.y, detailCoord.x] % targetNodePrimeNumber == 0)
                    {
                        mnodeTable[abstractCoord.y, abstractCoord.x].PropNodeTable[detailCoord.y, detailCoord.x] *= targetNodePrimeNumber;
                    }
                    break;
            }

            return true;
        }
        public bool RemoveNormalNode(Vector2Int abstractCoord, Vector2Int detailCoord, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
        {
            if (abstractCoord.x < 0 || abstractCoord.x > AbstractAxisLength.x - 1 || abstractCoord.y < 0 || abstractCoord.y > AbstractAxisLength.y - 1)
            {
                return false;
            }
            if (detailCoord.x < 0 || detailCoord.x > DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > DetailAxisLength.y - 1)
            {
                return false;
            }

            switch (paletteType)
            {
                case AxisBaseTablePalette.EPaletteType.Geology:
                    if(NodeTable[abstractCoord.y, abstractCoord.x].GeologyNodeTable[detailCoord.y, detailCoord.x] % targetNodePrimeNumber == 0)
                    {
                        mnodeTable[abstractCoord.y, abstractCoord.x].GeologyNodeTable[detailCoord.y, detailCoord.x] /= targetNodePrimeNumber;
                    }
                    break;

                case AxisBaseTablePalette.EPaletteType.Biology:
                    if(NodeTable[abstractCoord.y, abstractCoord.x].BiologyNodeTable[detailCoord.y, detailCoord.x] % targetNodePrimeNumber == 0)
                    {
                        mnodeTable[abstractCoord.y, abstractCoord.x].BiologyNodeTable[detailCoord.y, detailCoord.x] /= targetNodePrimeNumber;
                    }
                    break;

                case AxisBaseTablePalette.EPaletteType.Prop:
                    if(NodeTable[abstractCoord.y, abstractCoord.x].PropNodeTable[detailCoord.y, detailCoord.x] % targetNodePrimeNumber == 0)
                    {
                        mnodeTable[abstractCoord.y, abstractCoord.x].PropNodeTable[detailCoord.y, detailCoord.x] /= targetNodePrimeNumber;
                    }
                    break;
            }

            return true;
        }
        public bool SetFundamentalElementsNode(Vector2Int abstractCoord, Vector2Int detailCoord, AxisBaseTablePalette.EFundamentalElements fundamentalElementKind)
        {
            if (abstractCoord.x < 0 || abstractCoord.x > AbstractAxisLength.x - 1 || abstractCoord.y < 0 || abstractCoord.y > AbstractAxisLength.y - 1)
            {
                return false;
            }
            if (detailCoord.x < 0 || detailCoord.x > DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > DetailAxisLength.y - 1)
            {
                return false;
            }

            if(NodeTable[abstractCoord.y, abstractCoord.x].FundamentalElementsTable[detailCoord.y, detailCoord.x] % (int)fundamentalElementKind == 0)
            {
                mnodeTable[abstractCoord.y, abstractCoord.x].FundamentalElementsTable[detailCoord.y, detailCoord.x] *= (int)fundamentalElementKind;
                SetFundamentalNodeInfluence(fundamentalElementKind, new Vector2Int(abstractCoord.x * DetailAxisLength.x + detailCoord.x, abstractCoord.y * DetailAxisLength.y + detailCoord.y));
            }

            return true;
        }
        public bool RemoveFundamentalElementsNode(Vector2Int abstractCoord, Vector2Int detailCoord, AxisBaseTablePalette.EFundamentalElements fundamentalElementKind)
        {
            if (abstractCoord.x < 0 || abstractCoord.x > AbstractAxisLength.x - 1 || abstractCoord.y < 0 || abstractCoord.y > AbstractAxisLength.y - 1)
            {
                return false;
            }
            if (detailCoord.x < 0 || detailCoord.x > DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > DetailAxisLength.y - 1)
            {
                return false;
            }

            if (NodeTable[abstractCoord.y, abstractCoord.x].FundamentalElementsTable[detailCoord.y, detailCoord.x] % (int)fundamentalElementKind == 0)
            {
                mnodeTable[abstractCoord.y, abstractCoord.x].FundamentalElementsTable[detailCoord.y, detailCoord.x] /= (int)fundamentalElementKind;

                for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
                {
                    for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                    {
                        for(int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                        {
                            for(int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                            {
                                mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].Clear();

                                if(NodeTable[coord_y, coord_x].FundamentalElementsTable[detailCoord_y, detailCoord_x] % (int)AxisBaseTablePalette.EFundamentalElements.Lava == 0)
                                {
                                    SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements.Lava, new Vector2Int(coord_x * DetailAxisLength.x + detailCoord_x, coord_y * DetailAxisLength.y + detailCoord_y));
                                }
                                if (NodeTable[coord_y, coord_x].FundamentalElementsTable[detailCoord_y, detailCoord_x] % (int)AxisBaseTablePalette.EFundamentalElements.Glacier == 0)
                                {
                                    SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements.Glacier, new Vector2Int(coord_x * DetailAxisLength.x + detailCoord_x, coord_y * DetailAxisLength.y + detailCoord_y));
                                }
                                if (NodeTable[coord_y, coord_x].FundamentalElementsTable[detailCoord_y, detailCoord_x] % (int)AxisBaseTablePalette.EFundamentalElements.Eitr == 0)
                                {
                                    SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements.Eitr, new Vector2Int(coord_x * DetailAxisLength.x + detailCoord_x, coord_y * DetailAxisLength.y + detailCoord_y));
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        public bool SetHeightValue(Vector2Int abstractCoord, Vector2Int detailCoord, float targetValue)
        {
            if (abstractCoord.x < 0 || abstractCoord.x > AbstractAxisLength.x - 1 || abstractCoord.y < 0 || abstractCoord.y > AbstractAxisLength.y - 1)
            {
                return false;
            }
            if (detailCoord.x < 0 || detailCoord.x > DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > DetailAxisLength.y - 1)
            {
                return false;
            }

            if (targetValue < 0.0f)
            {
                targetValue = 0.0f;
            }
            else if (targetValue > 1.0f)
            {
                targetValue = 1.0f;
            }
            mnodeTable[abstractCoord.y, abstractCoord.x].HeightTable[detailCoord.y, detailCoord.x] = targetValue;

            return true;
        }
        public void SetAbstractAxisLength(Vector2Int newAbstractLength, EAnchor anchor)
        {
            Node[,] tempNodeTable = new Node[newAbstractLength.y, newAbstractLength.x];
            for (int coord_y = 0; coord_y < newAbstractLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < newAbstractLength.x; coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = new Node(DetailAxisLength);
                }
            }

            Vector2Int startOoord = new Vector2Int(), endCoord = new Vector2Int();

            switch (anchor)
            {
                case EAnchor.TopLeft:
                    startOoord = new Vector2Int(0, 0);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : newAbstractLength.y - 1);
                    break;

                case EAnchor.TopStraight:
                    startOoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? 0 : (AbstractAxisLength.x - newAbstractLength.x) / 2 - 1, 0);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : (AbstractAxisLength.x - newAbstractLength.x) / 2 + newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : newAbstractLength.y - 1);
                    break;

                case EAnchor.TopRight:
                    startOoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? 0 : AbstractAxisLength.x - newAbstractLength.x - 1, 0);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : newAbstractLength.y - 1);
                    break;

                case EAnchor.MiddleLeft:
                    startOoord = new Vector2Int(0, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : (AbstractAxisLength.y - newAbstractLength.y) / 2 - 1);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : (AbstractAxisLength.y - newAbstractLength.y) / 2 + newAbstractLength.y - 1);
                    break;

                case EAnchor.Center:
                    startOoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? 0 : (AbstractAxisLength.x - newAbstractLength.x) / 2 - 1, AbstractAxisLength.y <= newAbstractLength.y ? 0 : (AbstractAxisLength.y - newAbstractLength.y) / 2 - 1);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : (AbstractAxisLength.x - newAbstractLength.x) / 2 + newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : (AbstractAxisLength.y - newAbstractLength.y) / 2 + newAbstractLength.y - 1);
                    break;

                case EAnchor.MiddleRight:
                    startOoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? 0 : (AbstractAxisLength.x - newAbstractLength.x) - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : (AbstractAxisLength.y - newAbstractLength.y) / 2 - 1);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : (AbstractAxisLength.y - newAbstractLength.y) / 2 + newAbstractLength.y - 1);
                    break;

                case EAnchor.BottomLeft:
                    startOoord = new Vector2Int(0, AbstractAxisLength.y <= newAbstractLength.y ? 0 : AbstractAxisLength.y - newAbstractLength.y - 1);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : newAbstractLength.y - 1);
                    break;

                case EAnchor.BottomStraight:
                    startOoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? 0 : (AbstractAxisLength.x - newAbstractLength.x) / 2 - 1, AbstractAxisLength.y <= newAbstractLength.y ? 0 : AbstractAxisLength.y - newAbstractLength.y - 1);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : (AbstractAxisLength.x - newAbstractLength.x) / 2 + newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : newAbstractLength.y - 1);
                    break;

                case EAnchor.BottomRight:
                    startOoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? 0 : (AbstractAxisLength.x - newAbstractLength.x) - 1, AbstractAxisLength.y <= newAbstractLength.y ? 0 : AbstractAxisLength.y - newAbstractLength.y - 1);
                    endCoord = new Vector2Int(AbstractAxisLength.x <= newAbstractLength.x ? AbstractAxisLength.x - 1 : newAbstractLength.x - 1, AbstractAxisLength.y <= newAbstractLength.y ? AbstractAxisLength.y - 1 : newAbstractLength.y - 1);
                    break;
            }

            for (int coord_y = startOoord.y; coord_y < endCoord.y; coord_y++)
            {
                for (int coord_x = startOoord.x; coord_x < endCoord.x; coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = NodeTable[coord_y, coord_x];
                }
            }

            mnodeTable = tempNodeTable;
            mabstractAxisLength = newAbstractLength;
        }
        public Texture2D BakeToTexture2D(DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            Texture2D texture2D = new Texture2D(AbstractAxisLength.x * DetailAxisLength.x, AbstractAxisLength.y * DetailAxisLength.y);

            int[,] geologyIntTable = new int[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];
            int[,] biologyIntTable = new int[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];
            int[,] propIntTable = new int[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];

            float[,] geologyFloatTable = new float[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];
            float[,] biologyFloatTable = new float[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];
            float[,] propFloatTable = new float[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];
            float[,] heightTable = new float[AbstractAxisLength.y * DetailAxisLength.y, AbstractAxisLength.x * DetailAxisLength.x];

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                    {
                        for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                        {
                            geologyIntTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x] = mnodeTable[coord_y, coord_x].GeologyNodeTable[detailCoord_y, detailCoord_x];
                            biologyIntTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x] = mnodeTable[coord_y, coord_x].BiologyNodeTable[detailCoord_y, detailCoord_x];
                            propIntTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x] = mnodeTable[coord_y, coord_x].PropNodeTable[detailCoord_y, detailCoord_x];
                            heightTable[coord_y * DetailAxisLength.y + detailCoord_y, coord_x * DetailAxisLength.x + detailCoord_x] = mnodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x];
                        }
                    }
                }
            }

            geologyFloatTable = NodeEncoding.CompressionNodes(AbstractAxisLength.x, AbstractAxisLength.y, geologyIntTable, directNodeTableCoreInfo.GeologyEncodingNodeDatas);
            biologyFloatTable = NodeEncoding.CompressionNodes(AbstractAxisLength.x, AbstractAxisLength.y, biologyIntTable, directNodeTableCoreInfo.BiologyEncodingNodeDatas);
            propFloatTable = NodeEncoding.CompressionNodes(AbstractAxisLength.x, AbstractAxisLength.y, propIntTable, directNodeTableCoreInfo.PropEncodingNodeDatas);

            for (int coord_y = 0; coord_y < AbstractAxisLength.y * DetailAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x * DetailAxisLength.x; coord_x++)
                {
                    texture2D.SetPixel(coord_x, coord_y, new Color(geologyFloatTable[coord_y, coord_x], biologyFloatTable[coord_y, coord_x], propFloatTable[coord_y, coord_x], heightTable[coord_y, coord_x]));
                }
            }
            texture2D.Apply();

            return texture2D;
        }
        public void DetermineTableHeight()
        {
            Texture2D texture = GenerateNoiseMapTexture();

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                    {
                        for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                        {
                            mnodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x] = texture.GetPixel(coord_x * DetailAxisLength.x + detailCoord_x, coord_y * DetailAxisLength.y + detailCoord_y).r;
                        }
                    }
                }
            }
        }        
        public void DetermineSinkHole()
        {
            System.Random random = new System.Random();

            int totalSinkHoleNumber = (-1) * (FloorDepth + 3) * (FloorDepth - 30) / 90 + random.Next(-1, 2);

            if (totalSinkHoleNumber > 0)
            {
                Vector2Int vector = new Vector2Int();

                for (int i = 0; i < totalSinkHoleNumber; i++)
                {
                    while (true)
                    {
                        vector.x = random.Next(0, AbstractAxisLength.x * DetailAxisLength.x);
                        vector.y = random.Next(0, AbstractAxisLength.y * DetailAxisLength.y);

                        if (mnodeTable[vector.y / DetailAxisLength.y, vector.x / DetailAxisLength.x].FundamentalElementsTable[vector.y % DetailAxisLength.y, vector.x % DetailAxisLength.x] == 1)
                        {
                            break;
                        }
                    }

                    SetHoleNodes(vector, 5.0f, 25.0f);
                }
            }

            random = null;
        }
        public void DetermineValley(VallyCalculate vallyCalculate)
        {
            List<Vector2Int> vectors = vallyCalculate.CalsulateValley(VallyCalculate.DefaultExtraTime);

            for (int i = 0; i < vectors.Count; i++)
            {
                SetHoleNodes(vectors[i], 10.0f, 20.0f);
            }
        }
        public void DetermineValley(VallyCalculate vallyCalculate, float minScale, float maxScale)
        {
            List<Vector2Int> vectors = vallyCalculate.CalsulateValley(VallyCalculate.DefaultExtraTime);

            for (int i = 0; i < vectors.Count; i++)
            {
                SetHoleNodes(vectors[i], minScale, maxScale);
            }
        }
        public void DetermineLava()
        {
            int lavaNum = (int)((-1) * (FloorDepth - 10) * (Math.Pow((FloorDepth - 28), 3) / 1845.2));

            System.Random random = new System.Random();
            List<Vector2Int> coordList = new List<Vector2Int>();
            for (int i = 0; i < lavaNum; i++)
            {
                coordList.Add(SearchFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements.Lava, random.Next(0, AbstractAxisLength.x * DetailAxisLength.x), random.Next(0, AbstractAxisLength.y * DetailAxisLength.y)));
            }

            for (int i = 0; i < lavaNum; i++)
            {
                SetFundamentalElementsNode(new Vector2Int(coordList[i].x / DetailAxisLength.x, coordList[i].y / DetailAxisLength.y), new Vector2Int(coordList[i].x % DetailAxisLength.x, coordList[i].y % DetailAxisLength.y), AxisBaseTablePalette.EFundamentalElements.Lava);
            }

            random = null;
        }
        public void DetermineGlacier()
        {
            int glacierNum = (int)((-1) * FloorDepth * Math.Pow((FloorDepth - 20), 3) / 4219);

            System.Random random = new System.Random();
            List<Vector2Int> coordList = new List<Vector2Int>();
            for (int i = 0; i < glacierNum; i++)
            {
                coordList.Add(SearchFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements.Glacier, random.Next(0, AbstractAxisLength.x * DetailAxisLength.x), random.Next(0, AbstractAxisLength.y * DetailAxisLength.y)));
            }

            for (int i = 0; i < glacierNum; i++)
            {
                SetFundamentalElementsNode(new Vector2Int(coordList[i].x / DetailAxisLength.x, coordList[i].y / DetailAxisLength.y), new Vector2Int(coordList[i].x % DetailAxisLength.x, coordList[i].y % DetailAxisLength.y), AxisBaseTablePalette.EFundamentalElements.Glacier);
            }

            random = null;
        }
        public void DetermineEitr()
        {
            int eitrNum = (int)((-1) * (FloorDepth + 11) * (Math.Pow((FloorDepth - 33), 33) / 197653) + 1);

            System.Random random = new System.Random();
            List<Vector2Int> coordList = new List<Vector2Int>();
            for (int i = 0; i < eitrNum; i++)
            {
                coordList.Add(SearchFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements.Eitr, random.Next(0, AbstractAxisLength.x * DetailAxisLength.x), random.Next(0, AbstractAxisLength.y * DetailAxisLength.y)));
            }

            for (int i = 0; i < eitrNum; i++)
            {
                SetFundamentalElementsNode(new Vector2Int(coordList[i].x / DetailAxisLength.x, coordList[i].y / DetailAxisLength.y), new Vector2Int(coordList[i].x % DetailAxisLength.x, coordList[i].y % DetailAxisLength.y), AxisBaseTablePalette.EFundamentalElements.Eitr);
            }

            random = null;
        }
        public void GenerateCaveStructure(Dictionary<CaveStructure.EChanceKinds, float> chances)
        {
            CaveStructure caveStructure = new CaveStructure(new CaveStructure.InputPack(GetCurrentHeightMap(), AbstractAxisLength, DetailAxisLength, chances));

            mverticalBoundaryFrequency = caveStructure.VerticalBoundaryFrequency;
            mhorizontalBoundaryFrequency = caveStructure.HorizontalBoundaryFrequency;

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x].CaveStructure = caveStructure.NodeTable[coord_y, coord_x];
                }
            }
        }
        public Texture2D GetCurrentHeightMap()
        {
            Texture2D texture = new Texture2D(AbstractAxisLength.x * DetailAxisLength.x, AbstractAxisLength.y * DetailAxisLength.y);

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                    {
                        for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                        {
                            texture.SetPixel(coord_x * DetailAxisLength.x + detailCoord_x, coord_y * DetailAxisLength.y + detailCoord_y, new Color(NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x], NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x], NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x]));
                        }
                    }
                }
            }
            texture.Apply();

            return texture;
        }
        public void DetermineGeologyNodes(DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                    {
                        for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                        {
                            int lava = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Lava) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Lava] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.x - 1;
                            int glacier = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Glacier) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Glacier] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.y - 1;
                            int eitr = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Eitr) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Eitr] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.z - 1;

                            mnodeTable[coord_y, coord_x].GeologyNodeTable[detailCoord_y, detailCoord_x] = directNodeTableCoreInfo.AxisBaseTable.GeologyNodeTable[lava, glacier, eitr];
                        }
                    }
                }
            }
        }
        public void DetermineBiologyNodes(DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if(NodeTable[coord_y, coord_x].Possible)
                    {
                        for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                        {
                            for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                            {
                                int lava = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Lava) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Lava] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.x - 1;
                                int glacier = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Glacier) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Glacier] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.y - 1;
                                int eitr = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Eitr) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Eitr] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.z - 1;

                                mnodeTable[coord_y, coord_x].BiologyNodeTable[detailCoord_y, detailCoord_x] = directNodeTableCoreInfo.AxisBaseTable.BiologyNodeTable[lava, glacier, eitr];
                            }
                        }
                    }
                }
            }
        }
        public void DeterminePropNodes(DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if(NodeTable[coord_y, coord_x].Possible)
                    {
                        for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                        {
                            for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                            {
                                int lava = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Lava) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Lava] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.x - 1;
                                int glacier = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Glacier) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Glacier] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.y - 1;
                                int eitr = mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x].ContainsKey(AxisBaseTablePalette.EFundamentalElements.Eitr) ? mnodeTable[coord_y, coord_x].FundamentalElementsInflunceTable[detailCoord_y, detailCoord_x][AxisBaseTablePalette.EFundamentalElements.Eitr] : directNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength.z - 1;

                                mnodeTable[coord_y, coord_x].PropNodeTable[detailCoord_y, detailCoord_x] = directNodeTableCoreInfo.AxisBaseTable.PropNodeTable[lava, glacier, eitr];
                            }
                        }
                    }
                }
            }
        }
        public void CalculateFloorBoundary(ref bool[,] blankTable, in InputPack_Auto inputPack)
        {
            for (int coord_y = 0; coord_y < inputPack.NoiseTexture.height; coord_y++)
            {
                for (int coord_x = 0; coord_x < inputPack.NoiseTexture.width; coord_x++)
                {
                    if (inputPack.NoiseTexture.GetPixel(coord_x, coord_y).r >= inputPack.NoiseMapCutLine && inputPack.NoiseTexture.GetPixel(coord_x, coord_y).r <= 1.0f - inputPack.NoiseMapCutLine)
                    {
                        blankTable[coord_y, coord_x] = true;
                    }
                }
            }

            SetBlankFalseEdgyNodes(ref blankTable);

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x].Possible = blankTable[coord_y, coord_x] ? true : false;
                }
            }
        }
        public bool SetFloorBoundaryNode(Vector2Int abstractCoord, bool possible)
        {
            if (abstractCoord.x < 0 || abstractCoord.x > AbstractAxisLength.x - 1 || abstractCoord.y < 0 || abstractCoord.y > AbstractAxisLength.y - 1)
            {
                return false;
            }

            mnodeTable[abstractCoord.y, abstractCoord.x].Possible = possible;

            return true;
        }
        public void SetNewBoundaryFrequency(CaveStructure.EAxisType axisType)
        {
            switch(axisType)
            {
                case CaveStructure.EAxisType.Vertical:
                    mverticalBoundaryFrequency = CaveStructure.GetNewBoundaryFrequency(AbstractAxisLength.y);
                    break;

                case CaveStructure.EAxisType.Horizontal:
                    mhorizontalBoundaryFrequency = CaveStructure.GetNewBoundaryFrequency(AbstractAxisLength.x);
                    break;
            }
        }

        private Texture2D GenerateNoiseMapTexture()
        {
            Texture2D texture = new Texture2D(AbstractAxisLength.x * DetailAxisLength.x, AbstractAxisLength.y * DetailAxisLength.y);

            List<float> overlapScales = new List<float>();
            float powNum = 0;

            System.Random random = new System.Random();
            int overlapCount = random.Next(2, 4);

            for (int i = 0; i < overlapCount; i++)
            {
                overlapScales.Add(UnityEngine.Random.Range(0.1f, 1.0f));
            }
            powNum = UnityEngine.Random.Range(2.0f, 4.0f);

            for (int coord_y = 0; coord_y < AbstractAxisLength.y * DetailAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x * DetailAxisLength.x; coord_x++)
                {
                    //  noise generating
                    float noiseValue = 0.0f;

                    for (int index = 0; index < overlapScales.Count; index++)
                    {
                        noiseValue += 1 / overlapScales[index] * Mathf.PerlinNoise(coord_x / AbstractAxisLength.x * DetailAxisLength.x * overlapScales[index], coord_y / AbstractAxisLength.y * DetailAxisLength.y * overlapScales[index]);
                    }
                    noiseValue = (float)Math.Pow(noiseValue, powNum);

                    if (noiseValue < 0.0f)
                    {
                        noiseValue = 0.0f;
                    }
                    else if (noiseValue > 1.0f)
                    {
                        noiseValue = 1.0f;
                    }

                    texture.SetPixel(coord_x, coord_y, new Color(noiseValue, noiseValue, noiseValue));
                }
            }
            texture.Apply();

            random = null;
            return texture;
        }
        private void SetBlankFalseEdgyNodes(ref bool[,] blankTable)
        {
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                if (blankTable[coord_y, 0] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, 0, coord_y);
                }
                if (blankTable[coord_y, AbstractAxisLength.x - 1] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, AbstractAxisLength.x - 1, coord_y);
                }
            }
            for (int coord_x = 1; coord_x < AbstractAxisLength.x - 1; coord_x++)
            {
                if (blankTable[0, coord_x] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, coord_x, 0);
                }
                if (blankTable[AbstractAxisLength.y - 1, coord_x] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, coord_x, AbstractAxisLength.y - 1);
                }
            }
        }
        private void SearchNodeAndSetFalse(ref bool[,] blankTable, int x, int y)
        {
            if (blankTable[y, x])
            {
                blankTable[y, x] = false;
            }
            else
            {
                return;
            }

            if (x == 0)
            {
                if (y == 0)
                {
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                }
                else if (y == AbstractAxisLength.y - 1)
                {
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y - 1);
                }
                else
                {
                    SearchNodeAndSetFalse(ref blankTable, x, y - 1);
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                }
            }
            else if (x == AbstractAxisLength.x - 1)
            {
                if (y == 0)
                {
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                }
                else if (y == AbstractAxisLength.y - 1)
                {
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y - 1);
                }
                else
                {
                    SearchNodeAndSetFalse(ref blankTable, x, y - 1);
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                }
            }
            else
            {
                if (y == 0)
                {
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                }
                else if (y == AbstractAxisLength.y - 1)
                {
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y - 1);
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                }
                else
                {
                    SearchNodeAndSetFalse(ref blankTable, x, y - 1);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                }
            }
        }
        private Vector2Int SearchFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements fundamentalElements, int realCoord_x, int realCoord_y)
        {
            Vector2Int vector2Int = new Vector2Int();
            bool triger = true;

            switch (fundamentalElements)
            {
                case AxisBaseTablePalette.EFundamentalElements.Lava:
                    if (mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] >= LavaArea.x && mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] <= LavaArea.y)
                    {
                        vector2Int.x = realCoord_x;
                        vector2Int.y = realCoord_y;
                    }
                    else
                    {
                        triger = true;
                    }
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Glacier:
                    if (mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] >= GlacierArea.x && mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] <= GlacierArea.y)
                    {
                        vector2Int.x = realCoord_x;
                        vector2Int.y = realCoord_y;
                    }
                    else
                    {
                        triger = true;
                    }
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Eitr:
                    if (mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] >= EitrArea.x && mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] <= EitrArea.y)
                    {
                        vector2Int.x = realCoord_x;
                        vector2Int.y = realCoord_y;
                    }
                    else
                    {
                        triger = true;
                    }
                    break;
            }

            if (triger)
            {
                if (realCoord_x == 0)
                {
                    if (realCoord_y == 0)
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                    else if (realCoord_y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                    }
                    else
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                }
                else if (realCoord_x == AbstractAxisLength.x * DetailAxisLength.x - 1)
                {
                    if (realCoord_y == 0)
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                    else if (realCoord_y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                    }
                    else
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                }
                else
                {
                    if (realCoord_y == 0)
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                    }
                    else if (realCoord_y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                    }
                    else
                    {
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearchFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                    }
                }
            }

            return vector2Int;
        }
        private void SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements fundamentalElements, Vector2Int realCoord)
        {
            int r = 0;

            switch (fundamentalElements)
            {
                case AxisBaseTablePalette.EFundamentalElements.Lava:
                    r = FundamentalInfluenceRadius.x;
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Glacier:
                    r = FundamentalInfluenceRadius.y;
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Eitr:
                    r = FundamentalInfluenceRadius.z;
                    break;
            }

            for (int coord_y = realCoord.y - r < 0 ? 0 : realCoord.y - r; coord_y < (realCoord.y + r + 1 > AbstractAxisLength.y * DetailAxisLength.y ? AbstractAxisLength.y * DetailAxisLength.y : realCoord.y + r + 1); coord_y++)
            {
                for (int coord_x = realCoord.x - r < 0 ? 0 : realCoord.x - r; coord_x < (realCoord.x + r + 1 > AbstractAxisLength.x * DetailAxisLength.x ? AbstractAxisLength.x * DetailAxisLength.x : realCoord.x + r + 1); coord_x++)
                {
                    if ((coord_x >= 0 && coord_x < AbstractAxisLength.x && coord_y >= 0 && coord_y < AbstractAxisLength.y) && (coord_y >= (-1) * (coord_x - realCoord.x + r) + realCoord.y - r / 2 && coord_y >= coord_x - realCoord.x - r + realCoord.y - r / 2 && coord_y <= coord_x - realCoord.x + r + realCoord.y + r / 2 && coord_y <= (-1) * (coord_x - realCoord.x - r) + realCoord.y + r / 2))
                    {
                        int value = (int)(Math.Sqrt(Math.Pow(realCoord.x - coord_x, 2.0) + Math.Pow(realCoord.y - coord_y, 2.0))) + 1;
                        if (NodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x].ContainsKey(fundamentalElements) == false)
                        {
                            mnodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x].Add(fundamentalElements, value);
                        }
                        else if (NodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x][fundamentalElements] > value)
                        {
                            mnodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x].Remove(fundamentalElements);
                            mnodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x].Add(fundamentalElements, value);
                        }
                    }
                }
            }
        }
        private void SetHoleNodes(Vector2Int realCoord, float minScale, float maxScale)
        {
            int r = (int)UnityEngine.Random.Range(minScale, maxScale);

            for (int coord_y = realCoord.y - r; coord_y < realCoord.y + r + 1; coord_y++)
            {
                for (int coord_x = realCoord.x - r; coord_x < realCoord.x + r + 1; coord_x++)
                {
                    if ((coord_x >= 0 && coord_x < AbstractAxisLength.x * DetailAxisLength.x && coord_y >= 0 && coord_y < AbstractAxisLength.y * DetailAxisLength.y) && (coord_y >= (-1) * (coord_x - realCoord.x + r) + realCoord.y - r / 2 && coord_y >= coord_x - realCoord.x - r + realCoord.y - r / 2 && coord_y <= coord_x - realCoord.x + r + realCoord.y + r / 2 && coord_y <= (-1) * (coord_x - realCoord.x - r) + realCoord.y + r / 2))
                    {
                        mnodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x] *= (int)AxisBaseTablePalette.EFundamentalElements.SinkHole;
                    }
                }
            }
        }
        private void CalculateLiquidEffect()
        {

        }
    }
    #endregion
}