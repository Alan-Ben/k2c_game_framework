package MJLog.Bo;
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
public class BaseOnlineNumLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_online_num =0;
    @DataBaseField(type = "int(11)", fieldname = "online_num", comment = "在线人数")
    private int online_num;

    public static final int FIELD_online_android =1;
    @DataBaseField(type = "int(11)", fieldname = "online_android", comment = "安卓在线用户数（暂无=0）")
    private int online_android;

    public static final int FIELD_online_ios =2;
    @DataBaseField(type = "int(11)", fieldname = "online_ios", comment = "IOS在线用户数（暂无=0）")
    private int online_ios;

    public static final int FIELD_timestamp =3;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "在线人数时时间-时间戳（10位数）")
    private int timestamp;

    public static final int FIELD_server_id =4;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "服务器id")
    private int server_id;

    public static final int FIELD_platform =5;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "账号归属的平台id")
    private int platform;

    public static final int FIELD_region =6;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "账号归属的区域id")
    private String region;

    public static final int FIELD_ext =7;
    @DataBaseField(type = "text", fieldname = "ext", comment = "扩展字段：json格式")
    private String ext;

    public static final int FIELD_date_time =8;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public BaseOnlineNumLogBO() {
        id = 0;
        online_num = 0;
        online_android = 0;
        online_ios = 0;
        timestamp = 0;
        server_id = 0;
        platform = 0;
        region = "";
        ext = "";
        date_time = 0;
    }

    public BaseOnlineNumLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        online_num = rs.getInt(2);
        online_android = rs.getInt(3);
        online_ios = rs.getInt(4);
        timestamp = rs.getInt(5);
        server_id = rs.getInt(6);
        platform = rs.getInt(7);
        region = rs.getString(8);
        ext = rs.getString(9);
        date_time = rs.getInt(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new BaseOnlineNumLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `online_num`, `online_android`, `online_ios`, `timestamp`, `server_id`, `platform`, `region`, `ext`, `date_time`";
    }

    @Override
    public String getTableName() {
        return "`base_online_num_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(online_num).append("', ");
        strBuf.append("'").append(online_android).append("', ");
        strBuf.append("'").append(online_ios).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(date_time).append("', ");
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

    // 在线人数
    public int getOnlineNum() { return this.online_num; }
    public void setOnlineNum(BM _bm, int online_num) {
        if(online_num==this.online_num) 
            return;
        this.online_num = online_num; 
        markField(_bm, FIELD_online_num); 
    }
    public void saveOnlineNum(BM _bm, int online_num) {
        if(online_num==this.online_num) 
            return;
        this.online_num = online_num;
        saveField(_bm, "online_num", online_num);
    }

    // 安卓在线用户数（暂无=0）
    public int getOnlineAndroid() { return this.online_android; }
    public void setOnlineAndroid(BM _bm, int online_android) {
        if(online_android==this.online_android) 
            return;
        this.online_android = online_android; 
        markField(_bm, FIELD_online_android); 
    }
    public void saveOnlineAndroid(BM _bm, int online_android) {
        if(online_android==this.online_android) 
            return;
        this.online_android = online_android;
        saveField(_bm, "online_android", online_android);
    }

    // IOS在线用户数（暂无=0）
    public int getOnlineIos() { return this.online_ios; }
    public void setOnlineIos(BM _bm, int online_ios) {
        if(online_ios==this.online_ios) 
            return;
        this.online_ios = online_ios; 
        markField(_bm, FIELD_online_ios); 
    }
    public void saveOnlineIos(BM _bm, int online_ios) {
        if(online_ios==this.online_ios) 
            return;
        this.online_ios = online_ios;
        saveField(_bm, "online_ios", online_ios);
    }

    // 在线人数时时间-时间戳（10位数）
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }

    // 服务器id
    public int getServerId() { return this.server_id; }
    public void setServerId(BM _bm, int server_id) {
        if(server_id==this.server_id) 
            return;
        this.server_id = server_id; 
        markField(_bm, FIELD_server_id); 
    }
    public void saveServerId(BM _bm, int server_id) {
        if(server_id==this.server_id) 
            return;
        this.server_id = server_id;
        saveField(_bm, "server_id", server_id);
    }

    // 账号归属的平台id
    public int getPlatform() { return this.platform; }
    public void setPlatform(BM _bm, int platform) {
        if(platform==this.platform) 
            return;
        this.platform = platform; 
        markField(_bm, FIELD_platform); 
    }
    public void savePlatform(BM _bm, int platform) {
        if(platform==this.platform) 
            return;
        this.platform = platform;
        saveField(_bm, "platform", platform);
    }

    // 账号归属的区域id
    public String getRegion() { return this.region; }
    public void setRegion(BM _bm, String region) {
        if(region.equals(this.region)) 
            return;
        this.region = region; 
        markField(_bm, FIELD_region); 
    }
    public void saveRegion(BM _bm, String region) {
        if(region.equals(this.region)) 
            return;
        this.region = region;
        saveField(_bm, "region", region);
    }

    // 扩展字段：json格式
    public String getExt() { return this.ext; }
    public void setExt(BM _bm, String ext) {
        if(ext.equals(this.ext)) 
            return;
        this.ext = ext; 
        markField(_bm, FIELD_ext); 
    }
    public void saveExt(BM _bm, String ext) {
        if(ext.equals(this.ext)) 
            return;
        this.ext = ext;
        saveField(_bm, "ext", ext);
    }

    // 日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `online_num` = '").append(online_num).append("',");
        sBuilder.append(" `online_android` = '").append(online_android).append("',");
        sBuilder.append(" `online_ios` = '").append(online_ios).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_online_num)) sBuilder.append(" `online_num` = '").append(online_num).append("',");
        if(isFieldMarked(FIELD_online_android)) sBuilder.append(" `online_android` = '").append(online_android).append("',");
        if(isFieldMarked(FIELD_online_ios)) sBuilder.append(" `online_ios` = '").append(online_ios).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `base_online_num_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`online_num` int(11) NOT NULL DEFAULT '0' COMMENT '在线人数',"
                + "`online_android` int(11) NOT NULL DEFAULT '0' COMMENT '安卓在线用户数（暂无=0）',"
                + "`online_ios` int(11) NOT NULL DEFAULT '0' COMMENT 'IOS在线用户数（暂无=0）',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '在线人数时时间-时间戳（10位数）',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '账号归属的平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '账号归属的区域id',"
                + "`ext` text NULL COMMENT '扩展字段：json格式',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-在线人数表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//online_num
        _size+=4;//online_android
        _size+=4;//online_ios
        _size+=4;//timestamp
        _size+=4;//server_id
        _size+=4;//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(region);//region
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);//ext
        _size+=4;//date_time
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(online_num);
        buff.putInt(online_android);
        buff.putInt(online_ios);
        buff.putInt(timestamp);
        buff.putInt(server_id);
        buff.putInt(platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, region);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ext);
        buff.putInt(date_time);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        online_num=buff.getInt();
        online_android=buff.getInt();
        online_ios=buff.getInt();
        timestamp=buff.getInt();
        server_id=buff.getInt();
        platform=buff.getInt();
        region=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        date_time=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
