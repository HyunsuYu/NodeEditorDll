using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using AxisBaseTableManager;

namespace Utilities
{
    public static class AESEncryption
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

        public static List<EncodingNodeData> GetEncodingNodeDatas(AxisBaseTable axisBaseTable)
        {
            List<EncodingNodeData> encodingNodeDatas = new List<EncodingNodeData>();
            int curIndex = 1;

            for(int index = 0; index < axisBaseTable.AxisBaseTablePalette.NodeNames.Count; index++)
            {
                if(axisBaseTable.AxisBaseTablePalette.NodeTable[axisBaseTable.AxisBaseTablePalette.NodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.Low)
                {
                    encodingNodeDatas.Add(new EncodingNodeData(axisBaseTable.AxisBaseTablePalette.NodeNames[index], GetNextPrimeNumber(curIndex)));
                    curIndex++;
                }
            }
            for(int index = 0; index < axisBaseTable.AxisBaseTablePalette.NodeNames.Count; index++)
            {
                if(axisBaseTable.AxisBaseTablePalette.NodeTable[axisBaseTable.AxisBaseTablePalette.NodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.Middle)
                {
                    encodingNodeDatas.Add(new EncodingNodeData(axisBaseTable.AxisBaseTablePalette.NodeNames[index], GetNextPrimeNumber(curIndex)));
                    curIndex++;
                }
            }
            for (int index = 0; index < axisBaseTable.AxisBaseTablePalette.NodeNames.Count; index++)
            {
                if (axisBaseTable.AxisBaseTablePalette.NodeTable[axisBaseTable.AxisBaseTablePalette.NodeNames[index].GetHashCode()].OrderedTypeClassify == OrderedNodeType.EOrderedTypeClassify.High)
                {
                    encodingNodeDatas.Add(new EncodingNodeData(axisBaseTable.AxisBaseTablePalette.NodeNames[index], GetNextPrimeNumber(curIndex)));
                    curIndex++;
                }
            }

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
