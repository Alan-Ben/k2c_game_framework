package NPCommon.Util;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;

public class ByteStream extends ByteArrayOutputStream
{
    private static Charset _g_CharSet = Charset.forName("UTF-8");
    private ByteBuffer _m_tempBuffer = ByteBuffer.allocate(8);
    private byte[] _b1 = new byte[1];
    private byte[] _b2 = new byte[2];
    private byte[] _b4 = new byte[4];
    private byte[] _b8 = new byte[8];

    public void putInt(int _v) throws IOException
    {
        _m_tempBuffer.clear();
        _m_tempBuffer.putInt(_v);
        _m_tempBuffer.flip();
        _m_tempBuffer.get(_b4);
        this.write(_b4);
    }

    public void putLong(long _v) throws IOException
    {
        _m_tempBuffer.clear();
        _m_tempBuffer.putLong(_v);
        _m_tempBuffer.flip();
        _m_tempBuffer.get(_b8);
        this.write(_b8);
    }

    public void putFloat(float _v) throws IOException
    {
        _m_tempBuffer.clear();
        _m_tempBuffer.putFloat(_v);
        _m_tempBuffer.flip();
        _m_tempBuffer.get(_b4);
        this.write(_b4);
    }

    public void putDouble(double _v) throws IOException
    {
        _m_tempBuffer.clear();
        _m_tempBuffer.putDouble(_v);
        _m_tempBuffer.flip();
        _m_tempBuffer.get(_b8);
        this.write(_b8);
    }

    public void putShort(short _v) throws IOException
    {
        _m_tempBuffer.clear();
        _m_tempBuffer.putShort(_v);
        _m_tempBuffer.flip();
        _m_tempBuffer.get(_b2);
        this.write(_b2);
    }

    public void putByte(byte _v) throws IOException
    {
        _b1[0] = _v;
        this.write(_b1);
    }

    public void putBoolean(boolean _v) throws IOException
    {
        putByte((byte) (_v ? 1 : 0));
    }

    public void putString(String str) throws IOException
    {
        byte[] bytes;
        try
        {
            bytes = str.getBytes(_g_CharSet);
            putInt(bytes.length);
            this.write(bytes);
        } catch (UnsupportedEncodingException e)
        {

        }
    }
}
