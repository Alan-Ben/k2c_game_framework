package SSDB.Bo;
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
public class ActivityScheduleUsGroupBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_schedule_db_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "schedule_db_id", comment = "排期实例ID")
    private long schedule_db_id;

    public static final int FIELD_res_file_name =1;
    @DataBaseField(type = "varchar(500)", fieldname = "res_file_name", comment = "资源文件名")
    private String res_file_name;

    public static final int FIELD_res_file_md5 =2;
    @DataBaseField(type = "varchar(500)", fieldname = "res_file_md5", comment = "资源文件MD5")
    private String res_file_md5;

    public static final int FIELD_res_file_dir =3;
    @DataBaseField(type = "varchar(500)", fieldname = "res_file_dir", comment = "资源文件目录")
    private String res_file_dir;

    public static final int FIELD_cross_instance_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_instance_id", comment = "跨服实例ID")
    private long cross_instance_id;

    public static final int FIELD_game_logic_instance_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "game_logic_instance_id", comment = "游戏逻辑主体实例ID")
    private long game_logic_instance_id;

    public static final int FIELD_had_discarded =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_discarded", comment = "是否已完成废弃处理")
    private boolean had_discarded;

    public ActivityScheduleUsGroupBO() {
        id = 0;
        schedule_db_id = 0L;
        res_file_name = "";
        res_file_md5 = "";
        res_file_dir = "";
        cross_instance_id = 0L;
        game_logic_instance_id = 0L;
        had_discarded = false;
    }

    public ActivityScheduleUsGroupBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        schedule_db_id = rs.getLong(2);
        res_file_name = rs.getString(3);
        res_file_md5 = rs.getString(4);
        res_file_dir = rs.getString(5);
        cross_instance_id = rs.getLong(6);
        game_logic_instance_id = rs.getLong(7);
        had_discarded = rs.getBoolean(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityScheduleUsGroupBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `schedule_db_id`, `res_file_name`, `res_file_md5`, `res_file_dir`, `cross_instance_id`, `game_logic_instance_id`, `had_discarded`";
    }

    @Override
    public String getTableName() {
        return "`activity_schedule_us_group`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(schedule_db_id).append("', ");
        strBuf.append("'").append(res_file_name == null ? null : res_file_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(res_file_md5 == null ? null : res_file_md5.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(res_file_dir == null ? null : res_file_dir.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cross_instance_id).append("', ");
        strBuf.append("'").append(game_logic_instance_id).append("', ");
        strBuf.append("'").append(had_discarded ? 1 : 0).append("', ");
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

    // 排期实例ID
    public long getScheduleDbId() { return this.schedule_db_id; }
    public void setScheduleDbId(BM _bm, long schedule_db_id) {
        if(schedule_db_id==this.schedule_db_id) 
            return;
        this.schedule_db_id = schedule_db_id; 
        markField(_bm, FIELD_schedule_db_id); 
    }
    public void saveScheduleDbId(BM _bm, long schedule_db_id) {
        if(schedule_db_id==this.schedule_db_id) 
            return;
        this.schedule_db_id = schedule_db_id;
        saveField(_bm, "schedule_db_id", schedule_db_id);
    }

    // 资源文件名
    public String getResFileName() { return this.res_file_name; }
    public void setResFileName(BM _bm, String res_file_name) {
        if(res_file_name.equals(this.res_file_name)) 
            return;
        this.res_file_name = res_file_name; 
        markField(_bm, FIELD_res_file_name); 
    }
    public void saveResFileName(BM _bm, String res_file_name) {
        if(res_file_name.equals(this.res_file_name)) 
            return;
        this.res_file_name = res_file_name;
        saveField(_bm, "res_file_name", res_file_name);
    }

    // 资源文件MD5
    public String getResFileMd5() { return this.res_file_md5; }
    public void setResFileMd5(BM _bm, String res_file_md5) {
        if(res_file_md5.equals(this.res_file_md5)) 
            return;
        this.res_file_md5 = res_file_md5; 
        markField(_bm, FIELD_res_file_md5); 
    }
    public void saveResFileMd5(BM _bm, String res_file_md5) {
        if(res_file_md5.equals(this.res_file_md5)) 
            return;
        this.res_file_md5 = res_file_md5;
        saveField(_bm, "res_file_md5", res_file_md5);
    }

    // 资源文件目录
    public String getResFileDir() { return this.res_file_dir; }
    public void setResFileDir(BM _bm, String res_file_dir) {
        if(res_file_dir.equals(this.res_file_dir)) 
            return;
        this.res_file_dir = res_file_dir; 
        markField(_bm, FIELD_res_file_dir); 
    }
    public void saveResFileDir(BM _bm, String res_file_dir) {
        if(res_file_dir.equals(this.res_file_dir)) 
            return;
        this.res_file_dir = res_file_dir;
        saveField(_bm, "res_file_dir", res_file_dir);
    }

    // 跨服实例ID
    public long getCrossInstanceId() { return this.cross_instance_id; }
    public void setCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id; 
        markField(_bm, FIELD_cross_instance_id); 
    }
    public void saveCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id;
        saveField(_bm, "cross_instance_id", cross_instance_id);
    }

    // 游戏逻辑主体实例ID
    public long getGameLogicInstanceId() { return this.game_logic_instance_id; }
    public void setGameLogicInstanceId(BM _bm, long game_logic_instance_id) {
        if(game_logic_instance_id==this.game_logic_instance_id) 
            return;
        this.game_logic_instance_id = game_logic_instance_id; 
        markField(_bm, FIELD_game_logic_instance_id); 
    }
    public void saveGameLogicInstanceId(BM _bm, long game_logic_instance_id) {
        if(game_logic_instance_id==this.game_logic_instance_id) 
            return;
        this.game_logic_instance_id = game_logic_instance_id;
        saveField(_bm, "game_logic_instance_id", game_logic_instance_id);
    }

    // 是否已完成废弃处理
    public boolean getHadDiscarded() { return this.had_discarded; }
    public void setHadDiscarded(BM _bm, boolean had_discarded) {
        if(had_discarded==this.had_discarded) 
            return;
        this.had_discarded = had_discarded; 
        markField(_bm, FIELD_had_discarded); 
    }
    public void saveHadDiscarded(BM _bm, boolean had_discarded) {
        if(had_discarded==this.had_discarded) 
            return;
        this.had_discarded = had_discarded;
        saveField(_bm, "had_discarded", had_discarded ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `schedule_db_id` = '").append(schedule_db_id).append("',");
        sBuilder.append(" `res_file_name` = '").append(res_file_name == null ? null : res_file_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `res_file_md5` = '").append(res_file_md5 == null ? null : res_file_md5.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `res_file_dir` = '").append(res_file_dir == null ? null : res_file_dir.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        sBuilder.append(" `game_logic_instance_id` = '").append(game_logic_instance_id).append("',");
        sBuilder.append(" `had_discarded` = '").append(had_discarded ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_schedule_db_id)) sBuilder.append(" `schedule_db_id` = '").append(schedule_db_id).append("',");
        if(isFieldMarked(FIELD_res_file_name)) sBuilder.append(" `res_file_name` = '").append(res_file_name == null ? null : res_file_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_res_file_md5)) sBuilder.append(" `res_file_md5` = '").append(res_file_md5 == null ? null : res_file_md5.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_res_file_dir)) sBuilder.append(" `res_file_dir` = '").append(res_file_dir == null ? null : res_file_dir.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cross_instance_id)) sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        if(isFieldMarked(FIELD_game_logic_instance_id)) sBuilder.append(" `game_logic_instance_id` = '").append(game_logic_instance_id).append("',");
        if(isFieldMarked(FIELD_had_discarded)) sBuilder.append(" `had_discarded` = '").append(had_discarded ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_schedule_us_group` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`schedule_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排期实例ID',"
                + "`res_file_name` varchar(500) NOT NULL DEFAULT '' COMMENT '资源文件名',"
                + "`res_file_md5` varchar(500) NOT NULL DEFAULT '' COMMENT '资源文件MD5',"
                + "`res_file_dir` varchar(500) NOT NULL DEFAULT '' COMMENT '资源文件目录',"
                + "`cross_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服实例ID',"
                + "`game_logic_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '游戏逻辑主体实例ID',"
                + "`had_discarded` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已完成废弃处理',"
                + "KEY `schedule_db_id` (`schedule_db_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排期US组数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.ss_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//schedule_db_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(res_file_name);//res_file_name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(res_file_md5);//res_file_md5
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(res_file_dir);//res_file_dir
        _size+=8;//cross_instance_id
        _size+=8;//game_logic_instance_id
        _size+=1;//had_discarded
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(schedule_db_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, res_file_name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, res_file_md5);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, res_file_dir);
        buff.putLong(cross_instance_id);
        buff.putLong(game_logic_instance_id);
        buff.put((byte)(had_discarded?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        schedule_db_id=buff.getLong();
        res_file_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        res_file_md5=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        res_file_dir=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cross_instance_id=buff.getLong();
        game_logic_instance_id=buff.getLong();
        had_discarded=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
