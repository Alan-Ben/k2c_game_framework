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
public class PlayerChildSeatBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_seatId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "seatId", comment = "训练房ID")
    private long seatId;

    public static final int FIELD_energy =2;
    @DataBaseField(type = "int(11)", fieldname = "energy", comment = "当前脑力值")
    private int energy;

    public static final int FIELD_lastCalMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "lastCalMs", comment = "上次计算时间（毫秒）")
    private long lastCalMs;

    public static final int FIELD_fullGetNextRemainMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "fullGetNextRemainMs", comment = "脑力值已满时，获得下一点脑力值所需时间（毫秒）")
    private long fullGetNextRemainMs;

    public PlayerChildSeatBO() {
        id = 0;
        cid = 0L;
        seatId = 0L;
        energy = 0;
        lastCalMs = 0L;
        fullGetNextRemainMs = 0L;
    }

    public PlayerChildSeatBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        seatId = rs.getLong(3);
        energy = rs.getInt(4);
        lastCalMs = rs.getLong(5);
        fullGetNextRemainMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerChildSeatBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `seatId`, `energy`, `lastCalMs`, `fullGetNextRemainMs`";
    }

    @Override
    public String getTableName() {
        return "`player_child_seat`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(seatId).append("', ");
        strBuf.append("'").append(energy).append("', ");
        strBuf.append("'").append(lastCalMs).append("', ");
        strBuf.append("'").append(fullGetNextRemainMs).append("', ");
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

    // 训练房ID
    public long getSeatId() { return this.seatId; }
    public void setSeatId(BM _bm, long seatId) {
        if(seatId==this.seatId) 
            return;
        this.seatId = seatId; 
        markField(_bm, FIELD_seatId); 
    }
    public void saveSeatId(BM _bm, long seatId) {
        if(seatId==this.seatId) 
            return;
        this.seatId = seatId;
        saveField(_bm, "seatId", seatId);
    }

    // 当前脑力值
    public int getEnergy() { return this.energy; }
    public void setEnergy(BM _bm, int energy) {
        if(energy==this.energy) 
            return;
        this.energy = energy; 
        markField(_bm, FIELD_energy); 
    }
    public void saveEnergy(BM _bm, int energy) {
        if(energy==this.energy) 
            return;
        this.energy = energy;
        saveField(_bm, "energy", energy);
    }

    // 上次计算时间（毫秒）
    public long getLastCalMs() { return this.lastCalMs; }
    public void setLastCalMs(BM _bm, long lastCalMs) {
        if(lastCalMs==this.lastCalMs) 
            return;
        this.lastCalMs = lastCalMs; 
        markField(_bm, FIELD_lastCalMs); 
    }
    public void saveLastCalMs(BM _bm, long lastCalMs) {
        if(lastCalMs==this.lastCalMs) 
            return;
        this.lastCalMs = lastCalMs;
        saveField(_bm, "lastCalMs", lastCalMs);
    }

    // 脑力值已满时，获得下一点脑力值所需时间（毫秒）
    public long getFullGetNextRemainMs() { return this.fullGetNextRemainMs; }
    public void setFullGetNextRemainMs(BM _bm, long fullGetNextRemainMs) {
        if(fullGetNextRemainMs==this.fullGetNextRemainMs) 
            return;
        this.fullGetNextRemainMs = fullGetNextRemainMs; 
        markField(_bm, FIELD_fullGetNextRemainMs); 
    }
    public void saveFullGetNextRemainMs(BM _bm, long fullGetNextRemainMs) {
        if(fullGetNextRemainMs==this.fullGetNextRemainMs) 
            return;
        this.fullGetNextRemainMs = fullGetNextRemainMs;
        saveField(_bm, "fullGetNextRemainMs", fullGetNextRemainMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `seatId` = '").append(seatId).append("',");
        sBuilder.append(" `energy` = '").append(energy).append("',");
        sBuilder.append(" `lastCalMs` = '").append(lastCalMs).append("',");
        sBuilder.append(" `fullGetNextRemainMs` = '").append(fullGetNextRemainMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_seatId)) sBuilder.append(" `seatId` = '").append(seatId).append("',");
        if(isFieldMarked(FIELD_energy)) sBuilder.append(" `energy` = '").append(energy).append("',");
        if(isFieldMarked(FIELD_lastCalMs)) sBuilder.append(" `lastCalMs` = '").append(lastCalMs).append("',");
        if(isFieldMarked(FIELD_fullGetNextRemainMs)) sBuilder.append(" `fullGetNextRemainMs` = '").append(fullGetNextRemainMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_child_seat` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`seatId` bigint(20) NOT NULL DEFAULT '0' COMMENT '训练房ID',"
                + "`energy` int(11) NOT NULL DEFAULT '0' COMMENT '当前脑力值',"
                + "`lastCalMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次计算时间（毫秒）',"
                + "`fullGetNextRemainMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '脑力值已满时，获得下一点脑力值所需时间（毫秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家子嗣席位数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//seatId
        _size+=4;//energy
        _size+=8;//lastCalMs
        _size+=8;//fullGetNextRemainMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(seatId);
        buff.putInt(energy);
        buff.putLong(lastCalMs);
        buff.putLong(fullGetNextRemainMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        seatId=buff.getLong();
        energy=buff.getInt();
        lastCalMs=buff.getLong();
        fullGetNextRemainMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
