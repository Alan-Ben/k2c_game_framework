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
public class GuildCooperateMainBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "公会ID")
    private long guild_id;

    public static final int FIELD_next_refresh_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "next_refresh_time_ms", comment = "下次刷新时间毫秒")
    private long next_refresh_time_ms;

    public static final int FIELD_reset_count =2;
    @DataBaseField(type = "int(11)", fieldname = "reset_count", comment = "已重置次数")
    private int reset_count;

    public static final int FIELD_recommend_area_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "recommend_area_id", comment = "推荐区域ID")
    private long recommend_area_id;

    public static final int FIELD_recommend_index =4;
    @DataBaseField(type = "int(11)", fieldname = "recommend_index", comment = "推荐据点索引")
    private int recommend_index;

    public static final int FIELD_point_layout_data =5;
    @DataBaseField(type = "blob", fieldname = "point_layout_data", comment = "据点布局数据（二进制序列化）")
    private byte[] point_layout_data;

    public GuildCooperateMainBO() {
        id = 0;
        guild_id = 0L;
        next_refresh_time_ms = 0L;
        reset_count = 0;
        recommend_area_id = 0L;
        recommend_index = 0;
        point_layout_data = null;
    }

    public GuildCooperateMainBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        next_refresh_time_ms = rs.getLong(3);
        reset_count = rs.getInt(4);
        recommend_area_id = rs.getLong(5);
        recommend_index = rs.getInt(6);
        point_layout_data = rs.getBytes(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildCooperateMainBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `next_refresh_time_ms`, `reset_count`, `recommend_area_id`, `recommend_index`, `point_layout_data`";
    }

    @Override
    public String getTableName() {
        return "`guild_cooperate_main`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(next_refresh_time_ms).append("', ");
        strBuf.append("'").append(reset_count).append("', ");
        strBuf.append("'").append(recommend_area_id).append("', ");
        strBuf.append("'").append(recommend_index).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(point_layout_data);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_point_layout_data)) ret.add(point_layout_data);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 公会ID
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 下次刷新时间毫秒
    public long getNextRefreshTimeMs() { return this.next_refresh_time_ms; }
    public void setNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms; 
        markField(_bm, FIELD_next_refresh_time_ms); 
    }
    public void saveNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms;
        saveField(_bm, "next_refresh_time_ms", next_refresh_time_ms);
    }

    // 已重置次数
    public int getResetCount() { return this.reset_count; }
    public void setResetCount(BM _bm, int reset_count) {
        if(reset_count==this.reset_count) 
            return;
        this.reset_count = reset_count; 
        markField(_bm, FIELD_reset_count); 
    }
    public void saveResetCount(BM _bm, int reset_count) {
        if(reset_count==this.reset_count) 
            return;
        this.reset_count = reset_count;
        saveField(_bm, "reset_count", reset_count);
    }

    // 推荐区域ID
    public long getRecommendAreaId() { return this.recommend_area_id; }
    public void setRecommendAreaId(BM _bm, long recommend_area_id) {
        if(recommend_area_id==this.recommend_area_id) 
            return;
        this.recommend_area_id = recommend_area_id; 
        markField(_bm, FIELD_recommend_area_id); 
    }
    public void saveRecommendAreaId(BM _bm, long recommend_area_id) {
        if(recommend_area_id==this.recommend_area_id) 
            return;
        this.recommend_area_id = recommend_area_id;
        saveField(_bm, "recommend_area_id", recommend_area_id);
    }

    // 推荐据点索引
    public int getRecommendIndex() { return this.recommend_index; }
    public void setRecommendIndex(BM _bm, int recommend_index) {
        if(recommend_index==this.recommend_index) 
            return;
        this.recommend_index = recommend_index; 
        markField(_bm, FIELD_recommend_index); 
    }
    public void saveRecommendIndex(BM _bm, int recommend_index) {
        if(recommend_index==this.recommend_index) 
            return;
        this.recommend_index = recommend_index;
        saveField(_bm, "recommend_index", recommend_index);
    }

    // 据点布局数据（二进制序列化）
    public byte[] getPointLayoutData() { return this.point_layout_data; }
    public void setPointLayoutData(BM _bm, byte[] point_layout_data) {
        if(point_layout_data==this.point_layout_data) 
            return;
        this.point_layout_data = point_layout_data; 
        markField(_bm, FIELD_point_layout_data); 
    }
    public void savePointLayoutData(BM _bm, byte[] point_layout_data) {
        if(point_layout_data==this.point_layout_data) 
            return;
        this.point_layout_data = point_layout_data;
        saveFieldBytes(_bm, "point_layout_data", point_layout_data);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        sBuilder.append(" `reset_count` = '").append(reset_count).append("',");
        sBuilder.append(" `recommend_area_id` = '").append(recommend_area_id).append("',");
        sBuilder.append(" `recommend_index` = '").append(recommend_index).append("',");
        sBuilder.append(" `point_layout_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_next_refresh_time_ms)) sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        if(isFieldMarked(FIELD_reset_count)) sBuilder.append(" `reset_count` = '").append(reset_count).append("',");
        if(isFieldMarked(FIELD_recommend_area_id)) sBuilder.append(" `recommend_area_id` = '").append(recommend_area_id).append("',");
        if(isFieldMarked(FIELD_recommend_index)) sBuilder.append(" `recommend_index` = '").append(recommend_index).append("',");
        if(isFieldMarked(FIELD_point_layout_data)) sBuilder.append(" `point_layout_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_cooperate_main` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会ID',"
                + "`next_refresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下次刷新时间毫秒',"
                + "`reset_count` int(11) NOT NULL DEFAULT '0' COMMENT '已重置次数',"
                + "`recommend_area_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '推荐区域ID',"
                + "`recommend_index` int(11) NOT NULL DEFAULT '0' COMMENT '推荐据点索引',"
                + "`point_layout_data` blob NULL COMMENT '据点布局数据（二进制序列化）',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='公会协作主表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=8;//next_refresh_time_ms
        _size+=4;//reset_count
        _size+=8;//recommend_area_id
        _size+=4;//recommend_index
        _size+=2;_size+=point_layout_data.length;//point_layout_data
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(next_refresh_time_ms);
        buff.putInt(reset_count);
        buff.putLong(recommend_area_id);
        buff.putInt(recommend_index);
        buff.putShort((short)(point_layout_data == null ? 0 : point_layout_data.length));if(null != point_layout_data){buff.put(point_layout_data);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        next_refresh_time_ms=buff.getLong();
        reset_count=buff.getInt();
        recommend_area_id=buff.getLong();
        recommend_index=buff.getInt();
        int point_layout_data_count = buff.getShort();if(point_layout_data_count>0){point_layout_data = new byte[point_layout_data_count];buff.get(point_layout_data);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
