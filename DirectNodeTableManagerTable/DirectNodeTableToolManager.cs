using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AxisBaseTableManager;
using UnityEngine;
using UnityEngine.UIElements;
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

                tempFloorTable.Add(new FloorTable(DirectNodeTableCoreInfo.FloorAbstractAxisLengths[index], DirectNodeTableCoreInfo.FloorDetailAxisLengths[index], geologyNodeTable, biologyNodeTable, propNodeTable, heightTable));
            }

            return tempFloorTable;
        }
    }
    #endregion

    #region Floors
    public class DirectNodeTable
    {
        public struct InputPack
        {
            private DirectNodeTableCoreInfo mdirectNodeTableCoreInfo;

            public InputPack(DirectNodeTableCoreInfo directNodeTableCoreInfo)
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



        public DirectNodeTable(InputPack inputPack, bool isAuto)
        {
            mfloorTable = new List<FloorTable>();
            mdirectNodeTableCoreInfo = inputPack.DirectNodeTableCoreInfo;

            if (isAuto)
            {
                Texture2D texture = GenerateMapBoundaryNoiseMap();

                float sum = 0;
                for (int floorIndex = 0; floorIndex < DefaultFloorDepth; floorIndex++)
                {
                    sum += GetCutLine(floorIndex);
                }

                for (int floorIndex = 0; floorIndex < DefaultFloorDepth; floorIndex++)
                {
                    mfloorTable.Add(new DirectNodeEditor.FloorTable(new DirectNodeEditor.FloorTable.InputPack_Auto(GetCutLine(floorIndex) / sum, texture)));
                }
            }
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
        public bool SetFloorAxisLength(int floorIndex, int x, int y)
        {
            if (floorIndex < 0 || floorIndex >= mfloorTable.Count)
            {
                return false;
            }

            mfloorTable[floorIndex].SetAxisLength(x, y);

            return true;
        }
        public void AddFloor()
        {
            mfloorTable.Add(new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength), new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength))));
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Add(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength));
            mdirectNodeTableCoreInfo.FloorDetailAxisLengths.Add(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength));
        }
        public void AddFloor(Vector2Int abstractAxisLength, Vector2Int detailAxisLength)
        {
            mfloorTable.Add(new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(abstractAxisLength, detailAxisLength)));
            mdirectNodeTableCoreInfo.FloorAbstractAxisLengths.Add(abstractAxisLength);
            mdirectNodeTableCoreInfo.FloorDetailAxisLengths.Add(detailAxisLength);
        }
        public bool AddFloor(int index)
        {
            if (index < 0 || index > mfloorTable.Count - 1)
            {
                return false;
            }

            mfloorTable.Insert(index, new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisAbstractLength, DirectNodeEditor.FloorTable.DefaultAxisAbstractLength), new Vector2Int(DirectNodeEditor.FloorTable.DefaultAxisDetailLength, DirectNodeEditor.FloorTable.DefaultAxisDetailLength))));
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

            mfloorTable.Insert(index, new FloorTable(new DirectNodeEditor.FloorTable.InputPack_Manual(abstractAxisLength, detailAxisLength)));
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

                    texture.SetPixel(coord_x, coord_y, new Color(noiseValue, 0.0f, 0.0f));
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
        public class Node
        {
            private int[,] mgeologyNodeTable;
            private int[,] mbiologyNodeTable;
            private int[,] mpropNodeTable;
            private float[,] mheightTable;
            private Vector2Int mdetailAxisLength;



            public Node()
            {
                mgeologyNodeTable = new int[FloorTable.DefaultAxisDetailLength, FloorTable.DefaultAxisDetailLength];
                mbiologyNodeTable = new int[FloorTable.DefaultAxisDetailLength, FloorTable.DefaultAxisDetailLength];
                mpropNodeTable = new int[FloorTable.DefaultAxisDetailLength, FloorTable.DefaultAxisDetailLength];
                mheightTable = new float[FloorTable.DefaultAxisDetailLength, FloorTable.DefaultAxisDetailLength];
                mdetailAxisLength = new Vector2Int(FloorTable.DefaultAxisDetailLength, FloorTable.DefaultAxisDetailLength);
            }
            public Node(Vector2Int detailAxisLength)
            {
                mgeologyNodeTable = new int[detailAxisLength.y, detailAxisLength.x];
                mbiologyNodeTable = new int[detailAxisLength.y, detailAxisLength.x];
                mpropNodeTable = new int[detailAxisLength.y, detailAxisLength.x];
                mheightTable = new float[detailAxisLength.y, detailAxisLength.x];
                mdetailAxisLength = detailAxisLength;
            }

            public Vector2Int DetailAxisLength
            {
                get => mdetailAxisLength;
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
        }

        public struct InputPack_Auto
        {
            private float mnoiseMapCutLine;
            private Texture2D mnoiseTexture;

            public InputPack_Auto(float noiseMapCutLine, Texture2D noiseTexture)
            {
                mnoiseMapCutLine = noiseMapCutLine;
                mnoiseTexture = noiseTexture;
            }

            public float NoiseMapCutLine
            {
                get => mnoiseMapCutLine;
            }
            public Texture2D NoiseTexture
            {
                get => mnoiseTexture;
            }
        }
        public struct InputPack_Manual
        {
            private Vector2Int mabstractAxisLength;
            private Vector2Int mdetailAxisLength;

            public InputPack_Manual(Vector2Int abstractAxisLength, Vector2Int detailAxisLength)
            {
                mabstractAxisLength = abstractAxisLength;
                mdetailAxisLength = detailAxisLength;
            }

            public Vector2Int AbstractAxisLength
            {
                get => mabstractAxisLength;
            }
            public Vector2Int DetailAxisLength
            {
                get => mdetailAxisLength;
            }
        }



        private Node[,] mnodeTable;
        private Vector2Int mabstractAxisLength;



        public FloorTable(InputPack_Auto inputPack)
        {
            mabstractAxisLength = new Vector2Int(inputPack.NoiseTexture.width, inputPack.NoiseTexture.height);
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            bool[,] blankTable = new bool[AbstractAxisLength.y, AbstractAxisLength.x];

            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(new Vector2Int(DefaultAxisDetailLength, DefaultAxisDetailLength));
                }
            }

            CalculateFloorBoundary(ref blankTable, inputPack);

            //  do something

        }
        public FloorTable(InputPack_Manual inputPack)
        {
            mabstractAxisLength = inputPack.AbstractAxisLength;
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];

            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(inputPack.DetailAxisLength);
                }
            }
        }
        public FloorTable(Vector2Int abstractAxisLength, Vector2Int detailAxisLength, int[,] geologyNodeTable, int[,] biologyNodeTable, int[,] propNodeTable, float[,] heightTable)
        {
            mabstractAxisLength = abstractAxisLength;
            mnodeTable = new Node[AbstractAxisLength.y, AbstractAxisLength.x];
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    mnodeTable[coord_y, coord_x] = new Node(new Vector2Int(AbstractAxisLength.x, AbstractAxisLength.y));
                }
            }

            int detailCoord_x = 0, detailCoord_y = 0;
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for(int i = 0; i < detailAxisLength.y; i++, detailCoord_y++)
                    {
                        for(int j = 0; j < detailAxisLength.x; j++, detailCoord_x++)
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

        internal static float DownerValueBoundary
        {
            get => 0.1f;
        }
        internal static float UpperValueBoundary
        {
            get => 0.9f;
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
            if(detailCoord.x < 0 || detailCoord.x > mnodeTable[abstractCoord.y, abstractCoord.x].DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > mnodeTable[abstractCoord.y, abstractCoord.x].DetailAxisLength.y - 1)
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
            if (detailCoord.x < 0 || detailCoord.x > mnodeTable[abstractCoord.y, abstractCoord.x].DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > mnodeTable[abstractCoord.y, abstractCoord.x].DetailAxisLength.y - 1)
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
            if (detailCoord.x < 0 || detailCoord.x > mnodeTable[abstractCoord.y, abstractCoord.x].DetailAxisLength.x - 1 || detailCoord.y < 0 || detailCoord.y > mnodeTable[abstractCoord.y, abstractCoord.x].DetailAxisLength.y - 1)
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
        internal void SetAxisLength(int x, int y)
        {
            Node[,] tempNodeTable = new Node[y, x];

            for (int coord_y = 0; coord_y < (AbstractAxisLength.y < y ? AbstractAxisLength.y : y); coord_y++)
            {
                for (int coord_x = 0; coord_x < (AbstractAxisLength.x < x ? AbstractAxisLength.x : x); coord_x++)
                {
                    tempNodeTable[coord_y, coord_x] = NodeTable[coord_y, coord_x];
                }
            }

            mnodeTable = tempNodeTable;
            mabstractAxisLength = new Vector2Int(x, y);
        }
        internal void GenerateHeightTable(List<float> overlapScales, float powNum)
        {
            for (int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    //  noise generating
                    float noiseValue = 0.0f;

                    for (int index = 0; index < overlapScales.Count; index++)
                    {
                        noiseValue += 1 / overlapScales[index] * Mathf.PerlinNoise(coord_x / AbstractAxisLength.x * overlapScales[index], coord_y / AbstractAxisLength.y * overlapScales[index]);
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

                }
            }
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
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < AbstractAxisLength.x; coord_x++)
                {
                    for(int detailCoord_y = 0; detailCoord_y < mnodeTable[coord_y, coord_x].DetailAxisLength.y; detailCoord_y++, realCoord_y++)
                    {
                        for(int detailCoord_x = 0; detailCoord_x < mnodeTable[coord_y, coord_x].DetailAxisLength.x; detailCoord_x++, realCoord_x++)
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
            for(int coord_y = 0; coord_y < AbstractAxisLength.y; coord_y++)
            {
                if(blankTable[coord_y, 0] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, 0, coord_y);
                }
                if(blankTable[coord_y, AbstractAxisLength.x - 1] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, AbstractAxisLength.x - 1, coord_y);
                }
            }
            for(int coord_x = 1; coord_x < AbstractAxisLength.x - 1; coord_x++)
            {
                if(blankTable[0, coord_x] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, coord_x, 0);
                }
                if(blankTable[AbstractAxisLength.y - 1, coord_x] == true)
                {
                    SearchNodeAndSetFalse(ref blankTable, coord_x, AbstractAxisLength.y - 1);
                }
            }
        }
        internal void SearchNodeAndSetFalse(ref bool[,] blankTable, int x, int y)
        {
            if(blankTable[y, x])
            {
                return;
            }
            else
            {
                blankTable[y, x] = true;
            }

            if(x == 0)
            {
                if(y == 0)
                {
                    SearchNodeAndSetFalse(ref blankTable, x + 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                }
                else if(y == AbstractAxisLength.y - 1)
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
            else if(x == AbstractAxisLength.x - 1)
            {
                if(y == 0)
                {
                    SearchNodeAndSetFalse(ref blankTable, x - 1, y);
                    SearchNodeAndSetFalse(ref blankTable, x, y + 1);
                }
                else if(y == AbstractAxisLength.y - 1)
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
    }
    #endregion
}