package NPCommon.Util;

import NPCommon.Log.CommLog;
import SevenZip.ZipDecoder;
import SevenZip.ZipEncoder;

public class GlobalZip
{
    public static GlobalZip getInstance()
    {
        return _instance;
    }

    private static GlobalZip _instance = new GlobalZip();

    private ZipEncoder _m_encoder = new SevenZip.ZipEncoder();
    private ZipDecoder _m_decoder = new SevenZip.ZipDecoder();

    public byte[] zip(byte[] _bytes)
    {
        synchronized (_m_encoder)
        {
            try
            {
                return _m_encoder.zip(_bytes);
            } catch (Exception e)
            {
                CommLog.error("zip failed", e);
                return null;
            }
        }
    }

    public byte[] unZip(byte[] _bytes)
    {
        synchronized (_m_decoder)
        {
            try
            {
                return _m_decoder.unzip(_bytes);
            } catch (Exception e)
            {
                CommLog.error("zip failed", e);
                return null;
            }
        }
    }

    public void preCreate()
    {
        byte[] dummy = new byte[10];
        zip(dummy);

    }
}
