using SevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SevenZipTransition
{
    private static SevenZipTransition _g_instance = new SevenZipTransition();
    public static SevenZipTransition instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new SevenZipTransition();

            return _g_instance;
        }
    }

    private CoderPropID[] propIDs = {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker };

    private object[] properties = { 1 << 10, 2, 3, 0, 2, 128, "bt4", true };

    public byte[] Encoder(byte[] _buff)
    {
        System.IO.MemoryStream inStream = new System.IO.MemoryStream(_buff);
        System.IO.MemoryStream outStream = new System.IO.MemoryStream();

        SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
        encoder.SetCoderProperties(propIDs, properties);
        encoder.WriteCoderProperties(outStream);

        for (int i = 0; i < 8; i++)
            outStream.WriteByte((Byte)(-1 >> (8 * i)));

        encoder.Code(inStream, outStream, -1, -1, null);
        byte[] dst = outStream.ToArray();

        //手动释放申请的Stream
        inStream.Close();
        outStream.Close();

        return dst;
    }

    public byte[] Decoder(byte[] _buff)
    {
        System.IO.MemoryStream ins = new System.IO.MemoryStream(_buff);
        System.IO.MemoryStream outs = new System.IO.MemoryStream();

        byte[] newProperties = new byte[5];
        if (ins.Read(newProperties, 0, 5) != 5)
            throw (new Exception("input .lzma is too short"));
        SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
        decoder.SetDecoderProperties(newProperties);
        long outSize = 0;
        for (int i = 0; i < 8; i++)
        {
            int v = ins.ReadByte();
            if (v < 0)
                throw (new Exception("Can't Read 1"));
            outSize |= ((long)(byte)v) << (8 * i);
        }
        long compressedSize = ins.Length - ins.Position;
        decoder.Code(ins, outs, compressedSize, outSize, null);
        byte[] dst = outs.ToArray();

        //手动释放申请的Stream
        ins.Close();
        outs.Close();
        
        return dst;
    }

    public void Decoder(byte[] _buff, System.IO.Stream _outStream)
    {
        if(null == _outStream)
            return;

        using (System.IO.MemoryStream ins = new System.IO.MemoryStream(_buff))
        {
            byte[] newProperties = new byte[5];
            if(ins.Read(newProperties, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            decoder.SetDecoderProperties(newProperties);
            long outSize = 0;
            for(int i = 0; i < 8; i++)
            {
                int v = ins.ReadByte();
                if(v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            long compressedSize = ins.Length - ins.Position;
            decoder.Code(ins, _outStream, compressedSize, outSize, null);
        }
    }
}
