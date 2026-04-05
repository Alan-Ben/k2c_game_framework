package NPCommon.ShareCode;

public class ShareCodeData
{
    private long serialNum;
    private int type;

    public ShareCodeData(int type, long serialNum)
    {
        this.serialNum = serialNum;
        this.type = type;
    }

    public int getType()
    {
        return type;
    }

    public long getSerialNum()
    {
        return serialNum;
    }

    @Override
    public String toString()
    {
        return "ShareCodeData{" +
                "serialNum=" + serialNum +
                ", type=" + type +
                '}';
    }
}
