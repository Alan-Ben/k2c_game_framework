package WCGCommon;

import java.nio.ByteBuffer;

public class ALProtocolBuf
{
    private ByteBuffer _m_buf;

    private ALProtocolBuf(int _len)
    {
        _m_buf = ByteBuffer.allocate(_len);
    }

    public ALProtocolBuf(byte[] _buff)
    {
        _m_buf = ByteBuffer.wrap(_buff);
    }

    public static ALProtocolBuf allocate(int _len)
    {
        return new ALProtocolBuf(_len);
    }

    /**
     * 输出本数据存储的实际数据
     * @return
     */
    public byte[] exportBuffer()
    {
        byte[] bytes = new byte[_m_buf.capacity()];
        int oldPos = _m_buf.position();
        _m_buf.position(0);
        _m_buf.get(bytes);
        _m_buf.position(oldPos);
        return bytes;

    }

    public void putString(String _str)
    {
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_m_buf, _str);

    }

    public void putInt(int _i)
    {
        _m_buf.putInt(_i);
    }

    public void putLong(long _l)
    {
        _m_buf.putLong(_l);
    }

    public void putShort(short _s)
    {
        _m_buf.putShort(_s);
    }

    public long getLong()
    {
        return _m_buf.getLong();
    }

    public String getString()
    {
        return ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_m_buf);
    }

    public int getInt()
    {
        return _m_buf.getInt();
    }

    public short getShort()
    {
        return _m_buf.getShort();
    }

    public void putByteBuffer(byte[] _bytes)
    {
        _m_buf.putInt(_bytes.length);
        _m_buf.put(_bytes);

    }

    public byte[] getByteBuffer()
    {
        int len = _m_buf.getInt();
        byte[] ret = new byte[len];
        _m_buf.get(ret);
        return ret;
    }

    public void putFloat(float x)
    {
        _m_buf.putFloat(x);
    }

    public float getFloat()
    {
        return _m_buf.getFloat();
    }

    public void putByteBuffer(ByteBuffer _src)
    {
        int len = _src.remaining();
        _m_buf.putInt(len);
        _m_buf.put(_src);

    }

    public byte get()
    {
        return _m_buf.get();
    }

    public void put(byte b)
    {
        _m_buf.put(b);
    }

}
