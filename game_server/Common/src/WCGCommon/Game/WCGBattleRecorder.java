package WCGCommon.Game;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Security.Base64;

import java.nio.ByteBuffer;
import java.util.ArrayList;


public class WCGBattleRecorder
{
    private boolean _m_bDataCompressed = false;
    private String _m_strCompressed = "";

    public static interface _IBattleRecordCompress
    {
        public void onBattleRecoredCompressed(String _encodedStr);
    }

    private ArrayList<byte[]> _m_arrBattleRecords = new ArrayList<>();

    public void addRecord(ByteBuffer _record)
    {
        int _oldPos = _record.position();
        _record.position(0);
        int bufLength = _record.remaining();
        byte[] bytes = new byte[bufLength];
        _record.get(bytes);
        _record.position(_oldPos);
        _m_arrBattleRecords.add(bytes);
    }

    public ArrayList<byte[]> getOpts()
    {
        return _m_arrBattleRecords;
    }

    public String toString()
    {
        byte[] zipBytes = getBattleRecordZipBytes();

        if (null != zipBytes)
            return Base64.encode(zipBytes);
        else
            return null;
    }

    public byte[] getBattleRecordBytes()
    {
        ByteBuffer buf = ByteBuffer.allocate(getBufSize());
        buf.putInt(_m_arrBattleRecords.size());
        for (int i = 0; i < _m_arrBattleRecords.size(); i++)
        {
            byte[] record = _m_arrBattleRecords.get(i);
            buf.putInt(record.length);
            buf.put(record);
        }

        return CommonFunc.ByteBfferToBytes(buf);
    }

    /***************
     * 获取压缩后的字节战斗记录
     *
     * @author alzq.z
     * @time 2021年3月7日 下午5:12:35
     */
    public byte[] getBattleRecordZipBytes()
    {
        byte[] srcBytes = getBattleRecordBytes();
        try
        {
            byte[] _zippedBytes = SevenZip.Zipper.zip(srcBytes);
            return _zippedBytes;
        } catch (Exception e)
        {
            CommLog.error(" battle record encode failed", e);
            return null;
        }
    }


    public boolean fromString(String _sData)
    {
        if (_sData == null || _sData.isEmpty())
        {
            return false;
        }
        _m_arrBattleRecords.clear();
        byte[] bytes = Base64.decode(_sData);
        if (bytes.length == 0)
        {
            return false;
        }
        try
        {
            byte[] _unzippedBytes = SevenZip.Zipper.unzip(bytes);
            ByteBuffer buf = ByteBuffer.wrap(_unzippedBytes);
            int len = buf.getInt();
            for (int i = 0; i < len; i++)
            {
                int size = buf.getInt();
                if (size == 0) return false;
                byte[] opt = new byte[size];
                buf.get(opt);
                this._m_arrBattleRecords.add(opt);
            }
        } catch (Exception e)
        {
            CommLog.error("decode battle record error", e);
            return false;
        }
        return true;
    }

    /*************
     * 从压缩字节中获取对应记录信息
     *
     * @author alzq.z
     * @time 2021年3月7日 下午5:12:13
     */
    public boolean fromZipBytes(byte[] _bytes)
    {
        if (null == _bytes || _bytes.length <= 0)
        {
            return false;
        }

        _m_arrBattleRecords.clear();

        try
        {
            byte[] _unzippedBytes = SevenZip.Zipper.unzip(_bytes);
            ByteBuffer buf = ByteBuffer.wrap(_unzippedBytes);
            int len = buf.getInt();
            for (int i = 0; i < len; i++)
            {
                int size = buf.getInt();
                if (size == 0) return false;
                byte[] opt = new byte[size];
                buf.get(opt);
                this._m_arrBattleRecords.add(opt);
            }
        } catch (Exception e)
        {
            CommLog.error("decode battle record error", e);
            return false;
        }
        return true;
    }

    private int getBufSize()
    {
        int len = 4;//array len
        for (int i = 0; i < _m_arrBattleRecords.size(); i++)
        {
            len += 4;//record size
            byte[] record = _m_arrBattleRecords.get(i);
            len += record.length;
        }
        return len;
    }

    public String getCompressedStr()
    {
        if (!_m_bDataCompressed)
            CommLog.error("BattleRecord can not get while not Compressed", new Exception(""));

        return _m_strCompressed;
    }

}
