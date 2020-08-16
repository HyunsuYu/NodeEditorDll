using System;
using System.Collections.Generic;
using AxisBaseTableManager;
using UnityEngine;
using Utilities;

namespace DirectNodeEditor
{
    #region FullPack
    public class DirectFullPack
    {
        public struct InputPack
        {
            private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
            private List<byte[]> mfloorPNGs;

            public InputPack(DirectNodeTableCoreInfo directNodeTableCoreInfo, List<byte[]> floorPNGs)
            {
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
                mfloorPNGs = floorPNGs;
            }

            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
            public List<byte[]> FloorPNGs
            {
                get => mfloorPNGs;
            }
        }



        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
        private List<byte[]> mfloorPNGs;



        public DirectFullPack(InputPack inputPack)
        {
            mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;
            mfloorPNGs = inputPack.FloorPNGs;
        }

        public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
        {
            get => mdirectNodeTableCoreInfo;
        }
        public List<byte[]> FloorPNGs
        {
            get => mfloorPNGs;
        }
        public List<Texture2D> DisorderByteDatas()
        {
            List<Texture2D> texture2Ds = new List<Texture2D>();

            for (int index = 0; index < mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Count; index++)
            {
                texture2Ds.Add(new Texture2D(mdirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].x, mdirectNodeTableCoreInfo.FloorAbstractAxisLengths[index].y));
                texture2Ds[index].LoadImage(mfloorPNGs[index]);
            }

            return texture2Ds;
        }
        public List<FloorTable> GetFloorTable(List<Texture2D> texture2Ds, AxisBaseTablePalette.EPaletteType paletteType)
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

                tempFloorTable.Add(new FloorTable(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index], DirectNodeTableCoreInfo.FloorDetailAxisLengths[index], DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength, index, geologyNodeTable, biologyNodeTable, propNodeTable, heightTable));
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
            private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;
            private bool mbisGenerateSinkHole;
            private bool mbisGenerateLava;
            private bool mbisGenerateGlacier;
            private bool mbisGenerateEitr;
            private bool mbisGenerateValley;

            public InputPack_Auto(DirectNodeTableCoreInfo directNodeTableCoreInfo, bool bisGenerateSinkHole, bool bisGenerateLava, bool bisGenerateGlacier, bool bisGenerateEitr, bool bisGenerateValley)
            {
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
                mbisGenerateSinkHole = bisGenerateSinkHole;
                mbisGenerateLava = bisGenerateLava;
                mbisGenerateGlacier = bisGenerateGlacier;
                mbisGenerateEitr = bisGenerateEitr;
                mbisGenerateValley = bisGenerateValley;
            }

            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
            public bool IsGenerateSinkHole
            {
                get => mbisGenerateSinkHole;
            }
            public bool IsGenerateLava
            {
                get => mbisGenerateLava;
            }
            public bool IsGenerateGlacier
            {
                get => mbisGenerateGlacier;
            }
            public bool IsGenerateEitr
            {
                get => mbisGenerateEitr;
            }
            public bool IsGenerateValley
            {
                get => mbisGenerateValley;
            }
        }
        public struct InputPack_Manual
        {
            private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

            public InputPack_Manual(DirectNodeTableCoreInfo directNodeTableCoreInfo)
            {
                mdirectNodeTableCoreInfo = directNodeTableCoreInfo;
            }

            public DirectNodeTableCoreInfo DirectNodeTableCoreInfo
            {
                get => mdirectNodeTableCoreInfo;
            }
        }



        private List<FloorTable> mfloorTable;
        private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;



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
                mfloorTable.Add(new DirectNodeEditor.FloorTable(new DirectNodeEditor.FloorTable.InputPack_Auto(GetCutLine(floorIndex) / sum, texture, inputPack.IsGenerateSinkHole, inputPack.IsGenerateLava, inputPack.IsGenerateGlacier, inputPack.IsGenerateEitr, inputPack.IsGenerateValley, floorIndex, DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength, vallyCalculate.GetNextFloorValleyCalculate(VallyCalculate.DefaultChangeAmplitudeAmount, VallyCalculate.DefaultChangePeriodAmount))));
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
        public bool SetNodeInFloor(int floorIndex, Vector2Int abstractCoord, Vector2Int detailCoord, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
        {
            if (floorIndex < 0 || floorIndex >= mfloorTable.Count)
            {
                return false;
            }

            return mfloorTable[floorIndex].SetNode(abstractCoord, detailCoord, targetNodePrimeNumber, paletteType);
        }
        public bool RemoveNodeInFloor(int floorIndex, Vector2Int abstractCoord, Vector2Int detailCoord, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
        {
            if (floorIndex < 0 || floorIndex >= mfloorTable.Count)
            {
                return false;
            }

            return mfloorTable[floorIndex].RemoveNode(abstractCoord, detailCoord, targetNodePrimeNumber, paletteType);
        }
        public void AddFloor()
        {
            mfloorTable.Add(new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength), new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength), mfloorTable.Count, DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength)));
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Add(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength));
            mdirectNodeTableCoreInfo.FloorDetailAxisLengths.Add(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength));
        }
        public void AddFloor(Vector2Int abstractAxisLength, Vector2Int detailAxisLength)
        {
            mfloorTable.Add(new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(abstractAxisLength, detailAxisLength, mfloorTable.Count, DirectNodeTableCoreInfo.AxisBaseTable.FundamentalAxisLength)));
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Add(abstractAxisLength);
            mdirectNodeTableCoreInfo.FloorDetailAxisLengths.Add(detailAxisLength);
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
        public bool SetFloorAbstractAxisLength(int index, Vector2Int newAbstractLength, FloorTable.EAnchor anchor)
        {
            if (index < 0 || index > mfloorTable.Count - 1)
            {
                return false;
            }

            FloorTable[index].SetAbstractAxisLength(newAbstractLength, anchor);

            return true;
        }

        internal Texture2D GenerateMapBoundaryNoiseMap()
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
        internal float GetCutLine(int floorIndex)
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
    }
    public class DirectNodeTableCoreInfo
    {
        public struct InputPack
        {
            AxisBaseTable maxisBaseTable;

            public InputPack(AxisBaseTable axisBaseTable)
            {
                maxisBaseTable = axisBaseTable;
            }

            public AxisBaseTable AxisBaseTable
            {
                get => maxisBaseTable;
            }
        }



        private AxisBaseTable maxisBaseTable;
        private List<NodeEncoding.EncodingNodeData> mgeologyEncodingNodeDatas;
        private List<NodeEncoding.EncodingNodeData> mbiologyEncodingNodeDatas;
        private List<NodeEncoding.EncodingNodeData> mpropEncodingBideDatas;
        private List<Vector2Int> mfloorAbstractAxisLengths;
        private List<Vector2Int> mfloorDetailAxisLengths;




        public DirectNodeTableCoreInfo(InputPack inputPack)
        {
            maxisBaseTable = inputPack.AxisBaseTable;
            mgeologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Geology);
            mbiologyEncodingNodeDatas = NodeEncoding.GetEncodingNodeDatas(AxisBaseTable.AxisBaseTablePalette, AxisBaseTablePalette.EPaletteType.Biology);
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
            private int[,] mgeologyNodeTable;
            private int[,] mbiologyNodeTable;
            private int[,] mpropNodeTable;
            private float[,] mheightTable;
            private int[,] mfundamentalElementsTable;
            private int[,] mfundamentalElementsInflunceTable;



            public Node()
            {
                mgeologyNodeTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mbiologyNodeTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mpropNodeTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mheightTable = new float[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mfundamentalElementsTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];
                mfundamentalElementsInflunceTable = new int[DefaultAxisDetailLength, DefaultAxisDetailLength];

                for(int coord_y = 0; coord_y < DefaultAxisDetailLength; coord_y++)
                {
                    for(int coord_x = 0; coord_x < DefaultAxisDetailLength; coord_x++)
                    {
                        mgeologyNodeTable[coord_y, coord_x] = 1;
                        mbiologyNodeTable[coord_y, coord_x] = 1;
                        mpropNodeTable[coord_y, coord_x] = 1;
                        mheightTable[coord_y, coord_x] = 0.0f;
                        mfundamentalElementsTable[coord_y, coord_x] = 1;
                        mfundamentalElementsInflunceTable[coord_y, coord_x] = 1;
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
                mfundamentalElementsInflunceTable = new int[detailAxisLength.y, detailAxisLength.x];

                for (int coord_y = 0; coord_y < detailAxisLength.y; coord_y++)
                {
                    for (int coord_x = 0; coord_x < detailAxisLength.x; coord_x++)
                    {
                        mgeologyNodeTable[coord_y, coord_x] = 1;
                        mbiologyNodeTable[coord_y, coord_x] = 1;
                        mpropNodeTable[coord_y, coord_x] = 1;
                        mheightTable[coord_y, coord_x] = 0.0f;
                        mfundamentalElementsTable[coord_y, coord_x] = 1;
                        mfundamentalElementsInflunceTable[coord_y, coord_x] = 1;
                    }
                }
            }

            internal int[,] GeologyNodeTable
            {
                get => mgeologyNodeTable;
                set => mgeologyNodeTable = value;
            }
            internal int[,] BiologyNodeTable
            {
                get => mbiologyNodeTable;
                set => mbiologyNodeTable = value;
            }
            internal int[,] PropNodeTable
            {
                get => mpropNodeTable;
                set => mpropNodeTable = value;
            }
            internal float[,] HeightTable
            {
                get => mheightTable;
                set => mheightTable = value;
            }
            internal int[,] FundamentalElementsTable
            {
                get => mfundamentalElementsTable;
                set => mfundamentalElementsTable = value;
            }
            internal int[,] FundamentalElementsInflunceTable
            {
                get => mfundamentalElementsInflunceTable;
                set => mfundamentalElementsInflunceTable = value;
            }
        }

        public struct InputPack_Auto
        {
            private float mnoiseMapCutLine;
            private Texture2D mnoiseTexture;
            private bool mbisGenerateSinkHole;
            private bool mbisGenerateLava;
            private bool mbisGenerateGlacier;
            private bool mbisGenerateEitr;
            private bool mbisGenerateValley;
            private int mfloorDepth;
            private Vector3Int mfundamentalInfluenceRadius;
            private VallyCalculate mvallyCalculate;
            private CaveStructureChancePack mcaveStructureChancePack;

            public InputPack_Auto(float noiseMapCutLine, Texture2D noiseTexture, bool bisGenerateSinkHole, bool bisGenerateLava, bool bisGenerateGlacier, bool bisGenerateEitr, bool bisGenerateValley, int floorDepth, Vector3Int fundamentalInfluenceRadius, VallyCalculate vallyCalculate, CaveStructureChancePack caveStructureChancePack)
            {
                mnoiseMapCutLine = noiseMapCutLine;
                mnoiseTexture = noiseTexture;
                mbisGenerateSinkHole = bisGenerateSinkHole;
                mbisGenerateLava = bisGenerateLava;
                mbisGenerateGlacier = bisGenerateGlacier;
                mbisGenerateEitr = bisGenerateEitr;
                mbisGenerateValley = bisGenerateValley;
                mfloorDepth = floorDepth;
                mfundamentalInfluenceRadius = fundamentalInfluenceRadius;
                mvallyCalculate = vallyCalculate;
                mcaveStructureChancePack = caveStructureChancePack;
            }

            public float NoiseMapCutLine
            {
                get => mnoiseMapCutLine;
            }
            public Texture2D NoiseTexture
            {
                get => mnoiseTexture;
            }
            public bool IsGenerateSinkHole
            {
                get => mbisGenerateSinkHole;
            }
            public bool IsGenerateLava
            {
                get => mbisGenerateLava;
            }
            public bool IsGenerateGlacier
            {
                get => mbisGenerateGlacier;
            }
            public bool IsGenerateEitr
            {
                get => mbisGenerateEitr;
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
            public CaveStructureChancePack CaveStructureChancePack
            {
                get => mcaveStructureChancePack;
            }
        }
        public struct InputPack_Manual
        {
            private Vector2Int mabstractAxisLength;
            private Vector2Int mdetailAxisLength;
            private int mfloorDepth;
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
        public struct CaveStructureChancePack
        {
            private float mnodeOccurChance, mbridgeOccurChance, mnodeOccurWeight, mnodeWebBaseDetermineChance, mnodeWebConnectionChance;
            private float mabstractDiagonalChancePerNode, mdetailDiagonalChancePerNode;

            public CaveStructureChancePack(float nodeOccurChance, float bridgeOccurChance, float nodeOccurWeight, float nodeWebBaseDetermineChance, float nodeWebConnectionChance, float abstractDiagonalChancePerNode, float detailDiagonalChancePerNode)
            {
                mnodeOccurChance = nodeOccurChance;
                mbridgeOccurChance = bridgeOccurChance;
                mnodeOccurWeight = nodeOccurWeight;
                mnodeWebBaseDetermineChance = nodeWebBaseDetermineChance;
                mnodeWebConnectionChance = nodeWebConnectionChance;
                mabstractDiagonalChancePerNode = abstractDiagonalChancePerNode;
                mdetailDiagonalChancePerNode = detailDiagonalChancePerNode;
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
        private Vector2Int mabstractAxisLength;
        private Vector2Int mdetailAxisLength;
        private int mfloorDepth;
        private Vector3Int mfundamentalInfluenceRadius;



        public FloorTable(InputPack_Auto inputPack)
        {
            mfloorDepth = inputPack.FloorDepth;
            mfundamentalInfluenceRadius = inputPack.FundamentalInfluenceRadius;

            mabstractAxisLength = new Vector2Int(inputPack.NoiseTexture.width, inputPack.NoiseTexture.height);
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            bool[,] blankTable = new bool[AbstractAxisLength.y, AbstractAxisLength.x];

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node();
                }
            }

            CalculateFloorBoundary(ref blankTable, inputPack);

            DetermineTableheight();

            if(inputPack.IsGenerateLava)
            {
                DetermineLava();
            }
            if(inputPack.IsGenerateGlacier)
            {
                DetermineGlacier();
            }
            if(inputPack.IsGenerateEitr)
            {
                DetermineEitr();
            }
            if(inputPack.IsGenerateSinkHole)
            {
                DetermineSinkHole();
            }
            if (inputPack.IsGenerateValley)
            {
                DetermineValley(inputPack.VallyCalculate);
            }

            GenerateCaveStructure(inputPack.CaveStructureChancePack);

            DetermineGeologyNodes();
            DetermineBiologyNodes();
            DeterminePropNodes();
        }
        public FloorTable(InputPack_Manual inputPack)
        {
            mfloorDepth = inputPack.FloorDepth;
            mfundamentalInfluenceRadius = inputPack.FundamentalInfluenceRadius;

            mabstractAxisLength = inputPack.AbstractAxisLength;
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(inputPack.DetailAxisLength);
                }
            }
        }
        public FloorTable(Vector2Int abstractAxisLength, Vector2Int detailAxisLength, Vector3Int fundamentalInfluenceRadius, int floorDepth, int[,] geologyNodeTable, int[,] biologyNodeTable, int[,] propNodeTable, float[,] heightTable)
        {
            mfloorDepth = floorDepth;
            mfundamentalInfluenceRadius = fundamentalInfluenceRadius;

            mabstractAxisLength = abstractAxisLength;
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(new Vector2Int(AbstractAxisLength.x, AbstractAxisLength.y));
                }
            }

            int detailCoord_x = 0, detailCoord_y = 0;
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int i = 0; i < detailAxisLength.y; i++, detailCoord_y++)
                    {
                        for (int j = 0; j < detailAxisLength.x; j++, detailCoord_x++)
                        {
                            mnodeTable[coord_y, coord_x].GeologyNodeTable[i, j] = geologyNodeTable[detailCoord_y, detailCoord_x];
                            mnodeTable[coord_y, coord_x].BiologyNodeTable[i, j] = biologyNodeTable[detailCoord_y, detailCoord_x];
                            mnodeTable[coord_y, coord_x].PropNodeTable[i, j] = propNodeTable[detailCoord_y, detailCoord_x];
                            mnodeTable[coord_y, coord_x].HeightTable[i, j] = heightTable[detailCoord_y, detailCoord_x];
                        }
                    }
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

        internal float DownerValueBoundary
        {
            get => 0.1f;
        }
        internal float UpperValueBoundary
        {
            get => 0.9f;
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

        internal bool SetNode(Vector2Int abstractCoord, Vector2Int detailCoord, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
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
                    mnodeTable[abstractCoord.y, abstractCoord.x].GeologyNodeTable[detailCoord.y, detailCoord.x] *= targetNodePrimeNumber;
                    break;

                case AxisBaseTablePalette.EPaletteType.Biology:
                    mnodeTable[abstractCoord.y, abstractCoord.x].BiologyNodeTable[detailCoord.y, detailCoord.x] *= targetNodePrimeNumber;
                    break;

                case AxisBaseTablePalette.EPaletteType.Prop:
                    mnodeTable[abstractCoord.y, abstractCoord.x].PropNodeTable[detailCoord.y, detailCoord.x] *= targetNodePrimeNumber;
                    break;
            }

            return true;
        }
        internal bool SetHeightValue(Vector2Int abstractCoord, Vector2Int detailCoord, float targetValue)
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
        internal bool RemoveNode(Vector2Int abstractCoord, Vector2Int detailCoord, int targetNodePrimeNumber, AxisBaseTablePalette.EPaletteType paletteType)
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
                    mnodeTable[abstractCoord.y, abstractCoord.x].GeologyNodeTable[detailCoord.y, detailCoord.x] /= targetNodePrimeNumber;
                    break;

                case AxisBaseTablePalette.EPaletteType.Biology:
                    mnodeTable[abstractCoord.y, abstractCoord.x].BiologyNodeTable[detailCoord.y, detailCoord.x] /= targetNodePrimeNumber;
                    break;

                case AxisBaseTablePalette.EPaletteType.Prop:
                    mnodeTable[abstractCoord.y, abstractCoord.x].PropNodeTable[detailCoord.y, detailCoord.x] /= targetNodePrimeNumber;
                    break;
            }

            return true;
        }
        internal void SetAbstractAxisLength(Vector2Int newAbstractLength, EAnchor anchor)
        {
            Node[,] tempNodeTable = new Node[newAbstractLength.y, newAbstractLength.x];
            for(int coord_y = 0; coord_y < newAbstractLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < newAbstractLength.x; coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = new Node(DetailAxisLength);
                }
            }

            Vector2Int startOoord = new Vector2Int(), endCoord = new Vector2Int();

            switch(anchor)
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

            for(int coord_y = startOoord.y; coord_y < endCoord.y; coord_y++)
            {
                for(int coord_x =startOoord.x; coord_x < endCoord.x; coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = NodeTable[coord_y, coord_x];
                }
            }

            mnodeTable = tempNodeTable;
            mabstractAxisLength = newAbstractLength;
        }
        internal Texture2D BakeToTexture2D(DirectNodeTableCoreInfo directNodeTableCoreInfo)
        {
            Texture2D texture2D = new Texture2D(AbstractAxisLength.y, AbstractAxisLength.x);

            int[,] geologyIntTable = new int[AbstractAxisLength.y, AbstractAxisLength.x];
            int[,] biologyIntTable = new int[AbstractAxisLength.y, AbstractAxisLength.x];
            int[,] propIntTable = new int[AbstractAxisLength.y, AbstractAxisLength.x];

            float[,] geologyFloatTable = new float[AbstractAxisLength.y, AbstractAxisLength.x];
            float[,] biologyFloatTable = new float[AbstractAxisLength.y, AbstractAxisLength.x];
            float[,] propFloatTable = new float[AbstractAxisLength.y, AbstractAxisLength.x];
            float[,] heightTable = new float[AbstractAxisLength.y, AbstractAxisLength.x];

            int realCoord_x = 0, realCoord_y = 0;
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for (int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++, realCoord_y++)
                    {
                        for (int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++, realCoord_x++)
                        {
                            geologyIntTable[realCoord_y, realCoord_x] = mnodeTable[coord_y, coord_x].GeologyNodeTable[detailCoord_y, detailCoord_x];
                            biologyIntTable[realCoord_y, realCoord_x] = mnodeTable[coord_y, coord_x].BiologyNodeTable[detailCoord_y, detailCoord_x];
                            propIntTable[realCoord_y, realCoord_x] = mnodeTable[coord_y, coord_x].PropNodeTable[detailCoord_y, detailCoord_x];
                            heightTable[realCoord_y, realCoord_x] = mnodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x];
                        }
                    }
                }
            }

            geologyFloatTable = NodeEncoding.CompressionNodes(AbstractAxisLength.x, AbstractAxisLength.y, geologyIntTable, directNodeTableCoreInfo.GeologyEncodingNodeDatas);
            biologyFloatTable = NodeEncoding.CompressionNodes(AbstractAxisLength.x, AbstractAxisLength.y, biologyIntTable, directNodeTableCoreInfo.BiologyEncodingNodeDatas);
            propFloatTable = NodeEncoding.CompressionNodes(AbstractAxisLength.x, AbstractAxisLength.y, propIntTable, directNodeTableCoreInfo.PropEncodingNodeDatas);

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    texture2D.SetPixel(coord_x, coord_y, new Color(geologyFloatTable[coord_y, coord_x], biologyFloatTable[coord_y, coord_x], propFloatTable[coord_y, coord_x], heightTable[coord_y, coord_x]));
                }
            }
            texture2D.Apply();

            return texture2D;
        }
        internal void CalculateFloorBoundary(ref bool[,] blankTable, in InputPack_Auto inputPack)
        {
            float curRealCutLine = inputPack.NoiseMapCutLine * (UpperValueBoundary - DownerValueBoundary);

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    if (inputPack.NoiseTexture.GetPixel(coord_x, coord_y).r >= curRealCutLine)
                    {
                        blankTable[coord_y, coord_x] = true;
                    }
                }
            }

            SetBlankFalseEdgyNodes(ref blankTable);
        }
        internal void SetBlankFalseEdgyNodes(ref bool[,] blankTable)
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
        internal void SearchNodeAndSetFalse(ref bool[,] blankTable, int x, int y)
        {
            if (blankTable[y, x])
            {
                return;
            }
            else
            {
                blankTable[y, x] = true;
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
        internal void DetermineTableheight()
        {
            Texture2D texture = GenerateNoiseMapTexture();

            int realCoord_x = 0, realCoord_y = 0;
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for(int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++, realCoord_y++)
                    {
                        for(int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++, realCoord_x++)
                        {
                            mnodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x] = texture.GetPixel(realCoord_x, realCoord_y).r;
                        }
                    }
                }
            }
        }
        internal Texture2D GenerateNoiseMapTexture()
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
        internal void DetermineSinkHole()
        {
            System.Random random = new System.Random();

            int totalSinkHoleNumber = (-1) * (FloorDepth + 3) * (FloorDepth - 30) / 90 + random.Next(-1, 2);

            if(totalSinkHoleNumber > 0)
            {
                Vector2Int vector = new Vector2Int();

                for(int i = 0; i < totalSinkHoleNumber; i++)
                {
                    while(true)
                    {
                        vector.x = random.Next(0, AbstractAxisLength.x);
                        vector.y = random.Next(0, AbstractAxisLength.y);

                        if(mnodeTable[vector.y / DetailAxisLength.y, vector.x / DetailAxisLength.x].FundamentalElementsTable[vector.y % DetailAxisLength.y, vector.x % DetailAxisLength.x] == 1)
                        {
                            break;
                        }
                    }

                    SetHoleNodes(vector, 5.0f, 25.0f);
                }
            }

            random = null;
        }
        internal void DetermineValley(VallyCalculate vallyCalculate)
        {
            List<Vector2Int> vectors = vallyCalculate.CalsulateValley(VallyCalculate.DefaultExtraTime);

            for(int i = 0; i < vectors.Count; i++)
            {
                SetHoleNodes(vectors[i], 10.0f, 20.0f);
            }
        }
        public void DetermineLava()
        {
            int lavaNum = (int)((-1) * (FloorDepth - 10) * (Math.Pow((FloorDepth - 28), 3) / 1845.2));

            System.Random random = new System.Random();
            List<Vector2Int> coordList = new List<Vector2Int>();
            for(int i = 0; i < lavaNum; i++)
            {
                coordList.Add(SearckFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements.Lava, random.Next(0, AbstractAxisLength.x * DetailAxisLength.x), random.Next(0, AbstractAxisLength.y * DetailAxisLength.y)));
            }

            for(int i = 0; i < lavaNum; i++)
            {
                SetFundamentalNode(AxisBaseTablePalette.EFundamentalElements.Lava, coordList[i]);
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
                coordList.Add(SearckFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements.Glacier, random.Next(0, AbstractAxisLength.x * DetailAxisLength.x), random.Next(0, AbstractAxisLength.y * DetailAxisLength.y)));
            }

            for (int i = 0; i < glacierNum; i++)
            {
                SetFundamentalNode(AxisBaseTablePalette.EFundamentalElements.Glacier, coordList[i]);
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
                coordList.Add(SearckFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements.Eitr, random.Next(0, AbstractAxisLength.x * DetailAxisLength.x), random.Next(0, AbstractAxisLength.y * DetailAxisLength.y)));
            }

            for (int i = 0; i < eitrNum; i++)
            {
                SetFundamentalNode(AxisBaseTablePalette.EFundamentalElements.Eitr, coordList[i]);
            }

            random = null;
        }
        internal Vector2Int SearckFundamentalNodeCoord(AxisBaseTablePalette.EFundamentalElements fundamentalElements, int realCoord_x, int realCoord_y)
        {
            Vector2Int vector2Int = new Vector2Int();
            bool triger = true;

            switch(fundamentalElements)
            {
                case AxisBaseTablePalette.EFundamentalElements.Lava:
                    if(mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] >= LavaArea.x && mnodeTable[realCoord_y / DetailAxisLength.y, realCoord_x / DetailAxisLength.x].HeightTable[realCoord_y % DetailAxisLength.y, realCoord_x % DetailAxisLength.x] <= LavaArea.y)
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

            if(triger)
            {
                if (realCoord_x == 0)
                {
                    if (realCoord_y == 0)
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                    else if (realCoord_y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                    }
                    else
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                }
                else if (realCoord_x == AbstractAxisLength.x * DetailAxisLength.x - 1)
                {
                    if (realCoord_y == 0)
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                    else if (realCoord_y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                    }
                    else
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                    }
                }
                else
                {
                    if (realCoord_y == 0)
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                    }
                    else if (realCoord_y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                    }
                    else
                    {
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y - 1);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x, realCoord_y + 1);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x - 1, realCoord_y);
                        vector2Int = SearckFundamentalNodeCoord(fundamentalElements, realCoord_x + 1, realCoord_y);
                    }
                }
            }

            return vector2Int;
        }
        internal void SetFundamentalNode(AxisBaseTablePalette.EFundamentalElements fundamentalElements, Vector2Int realCoord)
        {
            switch(fundamentalElements)
            {
                case AxisBaseTablePalette.EFundamentalElements.Lava:
                    if(mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].HeightTable[realCoord.y % DetailAxisLength.y, realCoord.x % DetailAxisLength.x] >= LavaArea.x && mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].HeightTable[realCoord.y % DetailAxisLength.y, realCoord.x % DetailAxisLength.x] <= LavaArea.y)
                    {
                        if(mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].FundamentalElementsTable[realCoord.y % DetailAxisLength.y, realCoord.x / DetailAxisLength.x] % (int)(AxisBaseTablePalette.EFundamentalElements.Lava) != 0)
                        {
                            mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].FundamentalElementsTable[realCoord.y % DetailAxisLength.y, realCoord.x / DetailAxisLength.x] *= (int)(AxisBaseTablePalette.EFundamentalElements.Lava);

                            SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements.Lava, realCoord);
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Glacier:
                    if (mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].HeightTable[realCoord.y % DetailAxisLength.y, realCoord.x % DetailAxisLength.x] >= GlacierArea.x && mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].HeightTable[realCoord.y % DetailAxisLength.y, realCoord.x % DetailAxisLength.x] <= GlacierArea.y)
                    {
                        if (mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].FundamentalElementsTable[realCoord.y % DetailAxisLength.y, realCoord.x / DetailAxisLength.x] % (int)(AxisBaseTablePalette.EFundamentalElements.Glacier) != 0)
                        {
                            mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].FundamentalElementsTable[realCoord.y % DetailAxisLength.y, realCoord.x / DetailAxisLength.x] *= (int)(AxisBaseTablePalette.EFundamentalElements.Glacier);

                            SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements.Glacier, realCoord);
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Eitr:
                    if (mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].HeightTable[realCoord.y % DetailAxisLength.y, realCoord.x % DetailAxisLength.x] >= EitrArea.x && mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].HeightTable[realCoord.y % DetailAxisLength.y, realCoord.x % DetailAxisLength.x] <= EitrArea.y)
                    {
                        if (mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].FundamentalElementsTable[realCoord.y % DetailAxisLength.y, realCoord.x / DetailAxisLength.x] % (int)(AxisBaseTablePalette.EFundamentalElements.Eitr) != 0)
                        {
                            mnodeTable[realCoord.y / DetailAxisLength.y, realCoord.x / DetailAxisLength.x].FundamentalElementsTable[realCoord.y % DetailAxisLength.y, realCoord.x / DetailAxisLength.x] *= (int)(AxisBaseTablePalette.EFundamentalElements.Eitr);

                            SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements.Eitr, realCoord);
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;
            }

            if (realCoord.x == 0)
            {
                if (realCoord.y == 0)
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x + 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y + 1));
                }
                else if (realCoord.y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x + 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y - 1));
                }
                else
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y - 1));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x + 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y + 1));
                }
            }
            else if (realCoord.x == AbstractAxisLength.x * DetailAxisLength.x - 1)
            {
                if (realCoord.y == 0)
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x - 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y + 1));
                }
                else if (realCoord.y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x - 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y - 1));
                }
                else
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y - 1));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x - 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y + 1));
                }
            }
            else
            {
                if (realCoord.y == 0)
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x - 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y + 1));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x + 1, realCoord.y));
                }
                else if (realCoord.y == AbstractAxisLength.y * DetailAxisLength.y - 1)
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x - 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y - 1));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x + 1, realCoord.y));
                }
                else
                {
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y + 1));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x, realCoord.y - 1));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x + 1, realCoord.y));
                    SetFundamentalNode(fundamentalElements, new Vector2Int(realCoord.x - 1, realCoord.y));

                }
            }
        }
        internal void SetFundamentalNodeInfluence(AxisBaseTablePalette.EFundamentalElements fundamentalElements, Vector2Int realCoord)
        {
            int r = 0;
            int targetFundamentalElement = 0;

            switch(fundamentalElements)
            {
                case AxisBaseTablePalette.EFundamentalElements.Lava:
                    r = FundamentalInfluenceRadius.x;
                    targetFundamentalElement = (int)(AxisBaseTablePalette.EFundamentalElements.Lava);
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Glacier:
                    r = FundamentalInfluenceRadius.y;
                    targetFundamentalElement = (int)(AxisBaseTablePalette.EFundamentalElements.Glacier);
                    break;

                case AxisBaseTablePalette.EFundamentalElements.Eitr:
                    r = FundamentalInfluenceRadius.z;
                    targetFundamentalElement = (int)(AxisBaseTablePalette.EFundamentalElements.Eitr);
                    break;
            }

            for (int coord_y = realCoord.y - r; coord_y < realCoord.y + r + 1; coord_y++)
            {
                for (int coord_x = realCoord.x - r; coord_x < realCoord.x + r + 1; coord_x++)
                {
                    if ((coord_x >= 0 && coord_x < AbstractAxisLength.x * DetailAxisLength.x && coord_y >= 0 && coord_y < AbstractAxisLength.y * DetailAxisLength.y) && (coord_y >= (-1) * (coord_x - realCoord.x + r) + realCoord.y - r / 2 && coord_y >= coord_x - realCoord.x - r + realCoord.y - r / 2 && coord_y <= coord_x - realCoord.x + r + realCoord.y + r / 2 && coord_y <= (-1) * (coord_x - realCoord.x - r) + realCoord.y + r / 2))
                    {
                        mnodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x] *= targetFundamentalElement;
                    }
                }
            }
        }
        internal void SetHoleNodes(Vector2Int realCoord, float minScale, float maxScale)
        {
            int r = (int)UnityEngine.Random.Range(minScale, maxScale);

            for (int coord_y = realCoord.y - r; coord_y < realCoord.y + r + 1; coord_y++)
            {
                for (int coord_x = realCoord.x - r; coord_x < realCoord.x + r + 1; coord_x++)
                {
                    if ((coord_x >= 0 && coord_x < AbstractAxisLength.x * DetailAxisLength.x && coord_y >= 0 && coord_y < AbstractAxisLength.y * DetailAxisLength.y) && (coord_y >= (-1) * (coord_x - realCoord.x + r) + realCoord.y - r / 2 && coord_y >= coord_x - realCoord.x - r + realCoord.y - r / 2 && coord_y <= coord_x - realCoord.x + r + realCoord.y + r / 2 && coord_y <= (-1) * (coord_x - realCoord.x - r) + realCoord.y + r / 2))
                    {
                        mnodeTable[coord_y / DetailAxisLength.y, coord_x / DetailAxisLength.x].FundamentalElementsInflunceTable[coord_y % DetailAxisLength.y, coord_x % DetailAxisLength.x] *= (int)AxisBaseTablePalette.EFundamentalElements.SinkHole;
                    }
                }
            }
        }
        internal void GenerateCaveStructure(CaveStructureChancePack caveStructureChancePack)
        {
            CaveStructure caveStructure = new CaveStructure(new CaveStructure.InputPack(GetCurrentHeightMap(), AbstractAxisLength, DetailAxisLength, caveStructureChancePack.NodeOccurChance, caveStructureChancePack.BridgeOccurChance, caveStructureChancePack.NodeOccurWeight, caveStructureChancePack.NodeWebBaseDetermineChance, caveStructureChancePack.NodeWebConnectionChance, caveStructureChancePack.AbstractDiagonalChancePerNode, caveStructureChancePack.DetailDiagonalChancePerNode));
        }
        internal Texture2D GetCurrentHeightMap()
        {
            Texture2D texture = new Texture2D(AbstractAxisLength.x * DetailAxisLength.x, AbstractAxisLength.y * DetailAxisLength.y);

            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for(int detailCoord_y = 0; detailCoord_y < DetailAxisLength.y; detailCoord_y++)
                    {
                        for(int detailCoord_x = 0; detailCoord_x < DetailAxisLength.x; detailCoord_x++)
                        {
                            texture.SetPixel(coord_x + detailCoord_x, coord_y + detailCoord_y, new Color(NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x], NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x], NodeTable[coord_y, coord_x].HeightTable[detailCoord_y, detailCoord_x]));
                        }
                    }
                }
            }
            texture.Apply();

            return texture;
        }
        internal void DetermineGeologyNodes()
        {

        }
        internal void DetermineBiologyNodes()
        {

        }
        internal void DeterminePropNodes()
        {

        }
        internal void CalculateLiquidEffect()
        {

        }
    }
    #endregion
}