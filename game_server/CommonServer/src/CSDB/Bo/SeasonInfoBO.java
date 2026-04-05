package CSDB.Bo;
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
public class SeasonInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_lastSeasonDesc =0;
    @DataBaseField(type = "int(11)", fieldname = "lastSeasonDesc", comment = "上赛季描述yyyymm")
    private int lastSeasonDesc;

    public static final int FIELD_currentSeason =1;
    @DataBaseField(type = "int(11)", fieldname = "currentSeason", comment = "赛季ID")
    private int currentSeason;

    public static final int FIELD_seasonStartTime =2;
    @DataBaseField(type = "int(11)", fieldname = "seasonStartTime", comment = "赛季开始日期")
    private int seasonStartTime;

    public static final int FIELD_seasonEndTime =3;
    @DataBaseField(type = "int(11)", fieldname = "seasonEndTime", comment = "赛季结束日期")
    private int seasonEndTime;

    public SeasonInfoBO() {
        id = 0;
        lastSeasonDesc = 0;
        currentSeason = 0;
        seasonStartTime = 0;
        seasonEndTime = 0;
    }

    public SeasonInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        lastSeasonDesc = rs.getInt(2);
        currentSeason = rs.getInt(3);
        seasonStartTime = rs.getInt(4);
        seasonEndTime = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new SeasonInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `lastSeasonDesc`, `currentSeason`, `seasonStartTime`, `seasonEndTime`";
    }

    @Override
    public String getTableName() {
        return "`seasonInfo`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(lastSeasonDesc).append("', ");
        strBuf.append("'").append(currentSeason).append("', ");
        strBuf.append("'").append(seasonStartTime).append("', ");
        strBuf.append("'").append(seasonEndTime).append("', ");
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

    // 上赛季描述yyyymm
    public int getLastSeasonDesc() { return this.lastSeasonDesc; }
    public void setLastSeasonDesc(BM _bm, int lastSeasonDesc) {
        if(lastSeasonDesc==this.lastSeasonDesc) 
            return;
        this.lastSeasonDesc = lastSeasonDesc; 
        markField(_bm, FIELD_lastSeasonDesc); 
    }
    public void saveLastSeasonDesc(BM _bm, int lastSeasonDesc) {
        if(lastSeasonDesc==this.lastSeasonDesc) 
            return;
        this.lastSeasonDesc = lastSeasonDesc;
        saveField(_bm, "lastSeasonDesc", lastSeasonDesc);
    }

    // 赛季ID
    public int getCurrentSeason() { return this.currentSeason; }
    public void setCurrentSeason(BM _bm, int currentSeason) {
        if(currentSeason==this.currentSeason) 
            return;
        this.currentSeason = currentSeason; 
        markField(_bm, FIELD_currentSeason); 
    }
    public void saveCurrentSeason(BM _bm, int currentSeason) {
        if(currentSeason==this.currentSeason) 
            return;
        this.currentSeason = currentSeason;
        saveField(_bm, "currentSeason", currentSeason);
    }

    // 赛季开始日期
    public int getSeasonStartTime() { return this.seasonStartTime; }
    public void setSeasonStartTime(BM _bm, int seasonStartTime) {
        if(seasonStartTime==this.seasonStartTime) 
            return;
        this.seasonStartTime = seasonStartTime; 
        markField(_bm, FIELD_seasonStartTime); 
    }
    public void saveSeasonStartTime(BM _bm, int seasonStartTime) {
        if(seasonStartTime==this.seasonStartTime) 
            return;
        this.seasonStartTime = seasonStartTime;
        saveField(_bm, "seasonStartTime", seasonStartTime);
    }

    // 赛季结束日期
    public int getSeasonEndTime() { return this.seasonEndTime; }
    public void setSeasonEndTime(BM _bm, int seasonEndTime) {
        if(seasonEndTime==this.seasonEndTime) 
            return;
        this.seasonEndTime = seasonEndTime; 
        markField(_bm, FIELD_seasonEndTime); 
    }
    public void saveSeasonEndTime(BM _bm, int seasonEndTime) {
        if(seasonEndTime==this.seasonEndTime) 
            return;
        this.seasonEndTime = seasonEndTime;
        saveField(_bm, "seasonEndTime", seasonEndTime);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `lastSeasonDesc` = '").append(lastSeasonDesc).append("',");
        sBuilder.append(" `currentSeason` = '").append(currentSeason).append("',");
        sBuilder.append(" `seasonStartTime` = '").append(seasonStartTime).append("',");
        sBuilder.append(" `seasonEndTime` = '").append(seasonEndTime).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_lastSeasonDesc)) sBuilder.append(" `lastSeasonDesc` = '").append(lastSeasonDesc).append("',");
        if(isFieldMarked(FIELD_currentSeason)) sBuilder.append(" `currentSeason` = '").append(currentSeason).append("',");
        if(isFieldMarked(FIELD_seasonStartTime)) sBuilder.append(" `seasonStartTime` = '").append(seasonStartTime).append("',");
        if(isFieldMarked(FIELD_seasonEndTime)) sBuilder.append(" `seasonEndTime` = '").append(seasonEndTime).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `seasonInfo` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`lastSeasonDesc` int(11) NOT NULL DEFAULT '0' COMMENT '上赛季描述yyyymm',"
                + "`currentSeason` int(11) NOT NULL DEFAULT '0' COMMENT '赛季ID',"
                + "`seasonStartTime` int(11) NOT NULL DEFAULT '0' COMMENT '赛季开始日期',"
                + "`seasonEndTime` int(11) NOT NULL DEFAULT '0' COMMENT '赛季结束日期',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='排位赛赛季信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//lastSeasonDesc
        _size+=4;//currentSeason
        _size+=4;//seasonStartTime
        _size+=4;//seasonEndTime
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(lastSeasonDesc);
        buff.putInt(currentSeason);
        buff.putInt(seasonStartTime);
        buff.putInt(seasonEndTime);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        lastSeasonDesc=buff.getInt();
        currentSeason=buff.getInt();
        seasonStartTime=buff.getInt();
        seasonEndTime=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
