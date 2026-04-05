package USDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class PlayerPrivilegeCardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_cardType =1;
    @DataBaseField(type = "int(11)", fieldname = "cardType", comment = "权益卡类型")
    private int cardType;

    public static final int FIELD_startS =2;
    @DataBaseField(type = "bigint(20)", fieldname = "startS", comment = "生效开始时间戳（秒）")
    private long startS;

    public static final int FIELD_endS =3;
    @DataBaseField(type = "bigint(20)", fieldname = "endS", comment = "生效结束时间戳（秒）")
    private long endS;

    public static final int FIELD_lastGainDailyRewardS =4;
    @DataBaseField(type = "bigint(20)", fieldname = "lastGainDailyRewardS", comment = "最后一次领取每日奖励的时间戳（秒）")
    private long lastGainDailyRewardS;

    public PlayerPrivilegeCardBO() {
        id = 0;
        cid = 0L;
        cardType = 0;
        startS = 0L;
        endS = 0L;
        lastGainDailyRewardS = 0L;
    }

    public PlayerPrivilegeCardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        cardType = rs.getInt(3);
        startS = rs.getLong(4);
        endS = rs.getLong(5);
        lastGainDailyRewardS = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerPrivilegeCardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `cardType`, `startS`, `endS`, `lastGainDailyRewardS`";
    }

    @Override
    public String getTableName() {
        return "`player_privilege_card`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(cardType).append("', ");
        strBuf.append("'").append(startS).append("', ");
        strBuf.append("'").append(endS).append("', ");
        strBuf.append("'").append(lastGainDailyRewardS).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家CID
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 权益卡类型
    public int getCardType() { return this.cardType; }
    public void setCardType(BM _bm, int cardType) {
        if(cardType==this.cardType) 
            return;
        this.cardType = cardType; 
        markField(_bm, FIELD_cardType); 
    }
    public void saveCardType(BM _bm, int cardType) {
        if(cardType==this.cardType) 
            return;
        this.cardType = cardType;
        saveField(_bm, "cardType", cardType);
    }

    // 生效开始时间戳（秒）
    public long getStartS() { return this.startS; }
    public void setStartS(BM _bm, long startS) {
        if(startS==this.startS) 
            return;
        this.startS = startS; 
        markField(_bm, FIELD_startS); 
    }
    public void saveStartS(BM _bm, long startS) {
        if(startS==this.startS) 
            return;
        this.startS = startS;
        saveField(_bm, "startS", startS);
    }

    // 生效结束时间戳（秒）
    public long getEndS() { return this.endS; }
    public void setEndS(BM _bm, long endS) {
        if(endS==this.endS) 
            return;
        this.endS = endS; 
        markField(_bm, FIELD_endS); 
    }
    public void saveEndS(BM _bm, long endS) {
        if(endS==this.endS) 
            return;
        this.endS = endS;
        saveField(_bm, "endS", endS);
    }

    // 最后一次领取每日奖励的时间戳（秒）
    public long getLastGainDailyRewardS() { return this.lastGainDailyRewardS; }
    public void setLastGainDailyRewardS(BM _bm, long lastGainDailyRewardS) {
        if(lastGainDailyRewardS==this.lastGainDailyRewardS) 
            return;
        this.lastGainDailyRewardS = lastGainDailyRewardS; 
        markField(_bm, FIELD_lastGainDailyRewardS); 
    }
    public void saveLastGainDailyRewardS(BM _bm, long lastGainDailyRewardS) {
        if(lastGainDailyRewardS==this.lastGainDailyRewardS) 
            return;
        this.lastGainDailyRewardS = lastGainDailyRewardS;
        saveField(_bm, "lastGainDailyRewardS", lastGainDailyRewardS);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `cardType` = '").append(cardType).append("',");
        sBuilder.append(" `startS` = '").append(startS).append("',");
        sBuilder.append(" `endS` = '").append(endS).append("',");
        sBuilder.append(" `lastGainDailyRewardS` = '").append(lastGainDailyRewardS).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_cardType)) sBuilder.append(" `cardType` = '").append(cardType).append("',");
        if(isFieldMarked(FIELD_startS)) sBuilder.append(" `startS` = '").append(startS).append("',");
        if(isFieldMarked(FIELD_endS)) sBuilder.append(" `endS` = '").append(endS).append("',");
        if(isFieldMarked(FIELD_lastGainDailyRewardS)) sBuilder.append(" `lastGainDailyRewardS` = '").append(lastGainDailyRewardS).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_privilege_card` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`cardType` int(11) NOT NULL DEFAULT '0' COMMENT '权益卡类型',"
                + "`startS` bigint(20) NOT NULL DEFAULT '0' COMMENT '生效开始时间戳（秒）',"
                + "`endS` bigint(20) NOT NULL DEFAULT '0' COMMENT '生效结束时间戳（秒）',"
                + "`lastGainDailyRewardS` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一次领取每日奖励的时间戳（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家权益卡数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cid
        _size+=4;//cardType
        _size+=8;//startS
        _size+=8;//endS
        _size+=8;//lastGainDailyRewardS
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(cardType);
        buff.putLong(startS);
        buff.putLong(endS);
        buff.putLong(lastGainDailyRewardS);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        cardType=buff.getInt();
        startS=buff.getLong();
        endS=buff.getLong();
        lastGainDailyRewardS=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
