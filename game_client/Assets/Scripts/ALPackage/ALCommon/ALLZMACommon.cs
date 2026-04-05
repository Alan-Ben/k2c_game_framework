using System;
using System.Collections.Generic;

#if AL_SEVEN_ZIP
using SevenZip.Compression.LZMA;
#endif

using System.IO;

using UnityEngine;

#if AL_SEVEN_ZIP
/******************
 * 压缩操作的通用函数处理对象
 **/
namespace ALPackage
{
    public class ALLZMACommon
    {
        /**************
         * 将文件使用lzma解压
         **/
        public static void DecompressFileLZMA(string _inFile, string _outFile)
        {
            DecompressFileLZMA(_inFile, _outFile, null);
        }
        public static void DecompressFileLZMA(string _inFile, string _outFile, SevenZip.ICodeProgress _progress)
        {
            string exctractionPath = System.IO.Path.GetDirectoryName(_outFile);
            int ret = lzma.doDecompress7zip(_inFile, exctractionPath, false, true, null, null);
            if (ret != 1)
            {
                Debug.LogErrorFormat("{0} 解压失败", _inFile);
            }
            else
            {
                //todo 解压成功后，设置进度
            }
        }
        public static void DecompressFileLZMA(byte[] _bytes, string _outFile)
        {
            DecompressFileLZMA(_bytes, _outFile, null);
        }
        public static void DecompressFileLZMA(byte[] _bytes, string _outFile, SevenZip.ICodeProgress _progress)
        {
            string exctractionPath = System.IO.Path.GetDirectoryName(_outFile);
            string fileName = Path.GetFileName(_outFile);
            int ret = 0;
#if(UNITY_IPHONE || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_EDITOR) && !UNITY_EDITOR_WIN
            ret = lzma.doDecompress7zip(null, exctractionPath, false, true, null, _bytes);
#else
            //windows平台不能用Buffer->文件解压,先把bytes存为文件
            string tempFile = Path.Combine(exctractionPath, fileName + ".tmp");
            File.WriteAllBytes(tempFile, _bytes);
            ret = lzma.doDecompress7zip(tempFile, exctractionPath, false, true, null, null);
            ALFile.Delete(tempFile);
#endif
            if (ret != 1)
            {
                Debug.LogErrorFormat("{0} 解压失败", fileName);
            }
            else
            {
                //todo 解压成功后，设置进度
            }
        }


        /////////////////////////////////////////////
        /////////////// LZMA旧算法 //////////////////
        ////////////////////////////////////////////
        /**************
         * 将文件进行lzma压缩
         **/
        /**
       public static void CompressFileLZMA(string _inFile, string _outFile)
       {
           //压缩对象
           SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
           FileStream input = new FileStream(_inFile, FileMode.Open);
           FileStream output = new FileStream(_outFile, FileMode.Create);

           //写入编码属性
           coder.WriteCoderProperties(output);

           //写入压缩文件大小
           output.Write(BitConverter.GetBytes(input.Length), 0, 8);

           //加密文件
           coder.Code(input, output, input.Length, -1, null);
           output.Flush();
           output.Close();
           input.Close();
       }
       */

        /**************
         * 将文件使用lzma解压
         **/
        /**
       public static void DecompressFileLZMA(string _inFile, string _outFile)
       {
           DecompressFileLZMA(_inFile, _outFile, null);
       }
       public static void DecompressFileLZMA(string _inFile, string _outFile, SevenZip.ICodeProgress _progress)
       {
           SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
           FileStream input = File.OpenRead(_inFile);
           FileStream output = new FileStream(_outFile, FileMode.Create);
           

           //读取解压信息
           byte[] properties = new byte[5];
           input.Read(properties, 0, 5);

           //读取解压文件长度
           byte[] fileLengthBytes = new byte[8];
           input.Read(fileLengthBytes, 0, 8);
           long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

           //解压文件
           coder.SetDecoderProperties(properties);
           coder.Code(input, output, input.Length, fileLength, _progress);
           output.Flush();
           output.Close();
           input.Close();
       }
       public static void DecompressFileLZMA(byte[] _bytes, string _outFile)
       {
           DecompressFileLZMA(_bytes, _outFile, null);
       }
       public static void DecompressFileLZMA(byte[] _bytes, string _outFile, SevenZip.ICodeProgress _progress)
       {
           SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
           MemoryStream input = new MemoryStream(_bytes);
           FileStream output = new FileStream(_outFile, FileMode.Create);

           //读取解压信息
           byte[] properties = new byte[5];
           input.Read(properties, 0, 5);

           //读取解压文件长度
           byte[] fileLengthBytes = new byte[8];
           input.Read(fileLengthBytes, 0, 8);
           long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

           //解压文件
           coder.SetDecoderProperties(properties);
           coder.Code(input, output, input.Length, fileLength, _progress);
           output.Flush();
           output.Close();
           input.Close();
       }
       */
    }
}
#endif
