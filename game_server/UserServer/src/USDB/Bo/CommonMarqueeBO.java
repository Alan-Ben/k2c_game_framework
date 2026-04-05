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
public class CommonMarqueeBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_ref_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "跑马灯配置id")
    private long ref_id;

    public static final int FIELD_php_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "php_id", comment = "跑马灯后台id")
    private long php_id;

    public static final int FIELD_create_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "create_time_ms", comment = "生成时间")
    private long create_time_ms;

    public static final int FIELD_expired_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "expired_time_ms", comment = "过期时间")
    private long expired_time_ms;

    public static final int FIELD_param_list =4;
    @DataBaseField(type = "blob", fieldname = "param_list", comment = "参数列表")
    private byte[] param_list;

    public static final int FIELD_show_pos_id =5;
    @DataBaseField(type = "int(11)", fieldname = "show_pos_id", comment = "窗口展示队列")
    private int show_pos_id;

    public static final int FIELD_priority_id =6;
    @DataBaseField(type = "int(11)", fieldname = "priority_id", comment = "优先级")
    private int priority_id;

    public static final int FIELD_duration_sec =7;
    @DataBaseField(type = "int(11)", fieldname = "duration_sec", comment = "循环播放时长秒")
    private int duration_sec;

    public static final int FIELD_duration_count =8;
    @DataBaseField(type = "int(11)", fieldname = "duration_count", comment = "循环播放次数")
    private int duration_count;

    public static final int FIELD_ui_res_id =9;
    @DataBaseField(type = "bigint(20)", fieldname = "ui_res_id", comment = "预制体ID")
    private long ui_res_id;

    public static final int FIELD_content =10;
    @DataBaseField(type = "varchar(2048)", fieldname = "content", comment = "内容")
    private String content;

    public static final int FIELD_can_del_type =11;
    @DataBaseField(type = "int(11)", fieldname = "can_del_type", comment = "是否可删除类型")
    private int can_del_type;

    public static final int FIELD_offline_need_show =12;
    @DataBaseField(type = "tinyint(1)", fieldname = "offline_need_show", comment = "玩家离线期间是否需要展示")
    private boolean offline_need_show;

    public static final int FIELD_defaultLang =13;
    @DataBaseField(type = "varchar(500)", fieldname = "defaultLang", comment = "默认语言")
    private String defaultLang;

    public static final int FIELD_channelList =14;
    @DataBaseField(type = "blob", fieldname = "channelList", comment = "渠道列表")
    private byte[] channelList;

    public static final int FIELD_showCondition =15;
    @DataBaseField(type = "varchar(2048)", fieldname = "showCondition", comment = "展示条件")
    private String showCondition;

    public CommonMarqueeBO() {
        id = 0;
        ref_id = 0L;
        php_id = 0L;
        create_time_ms = 0L;
        expired_time_ms = 0L;
        param_list = null;
        show_pos_id = 0;
        priority_id = 0;
        duration_sec = 0;
        duration_count = 0;
        ui_res_id = 0L;
        content = "";
        can_del_type = 0;
        offline_need_show = false;
        defaultLang = "";
        channelList = null;
        showCondition = "";
    }

    public CommonMarqueeBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        ref_id = rs.getLong(2);
        php_id = rs.getLong(3);
        create_time_ms = rs.getLong(4);
        expired_time_ms = rs.getLong(5);
        param_list = rs.getBytes(6);
        show_pos_id = rs.getInt(7);
        priority_id = rs.getInt(8);
        duration_sec = rs.getInt(9);
        duration_count = rs.getInt(10);
        ui_res_id = rs.getLong(11);
        content = rs.getString(12);
        can_del_type = rs.getInt(13);
        offline_need_show = rs.getBoolean(14);
        defaultLang = rs.getString(15);
        channelList = rs.getBytes(16);
        showCondition = rs.getString(17);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CommonMarqueeBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `ref_id`, `php_id`, `create_time_ms`, `expired_time_ms`, `param_list`, `show_pos_id`, `priority_id`, `duration_sec`, `duration_count`, `ui_res_id`, `content`, `can_del_type`, `offline_need_show`, `defaultLang`, `channelList`, `showCondition`";
    }

    @Override
    public String getTableName() {
        return "`common_marquee`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(php_id).append("', ");
        strBuf.append("'").append(create_time_ms).append("', ");
        strBuf.append("'").append(expired_time_ms).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(show_pos_id).append("', ");
        strBuf.append("'").append(priority_id).append("', ");
        strBuf.append("'").append(duration_sec).append("', ");
        strBuf.append("'").append(duration_count).append("', ");
        strBuf.append("'").append(ui_res_id).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(can_del_type).append("', ");
        strBuf.append("'").append(offline_need_show ? 1 : 0).append("', ");
        strBuf.append("'").append(defaultLang == null ? null : defaultLang.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(showCondition == null ? null : showCondition.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(param_list); 
        ret.add(channelList);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_param_list)) ret.add(param_list); 
        if(isFieldMarked(FIELD_channelList)) ret.add(channelList);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 跑马灯配置id
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 跑马灯后台id
    public long getPhpId() { return this.php_id; }
    public void setPhpId(BM _bm, long php_id) {
        if(php_id==this.php_id) 
            return;
        this.php_id = php_id; 
        markField(_bm, FIELD_php_id); 
    }
    public void savePhpId(BM _bm, long php_id) {
        if(php_id==this.php_id) 
            return;
        this.php_id = php_id;
        saveField(_bm, "php_id", php_id);
    }

    // 生成时间
    public long getCreateTimeMs() { return this.create_time_ms; }
    public void setCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms; 
        markField(_bm, FIELD_create_time_ms); 
    }
    public void saveCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms;
        saveField(_bm, "create_time_ms", create_time_ms);
    }

    // 过期时间
    public long getExpiredTimeMs() { return this.expired_time_ms; }
    public void setExpiredTimeMs(BM _bm, long expired_time_ms) {
        if(expired_time_ms==this.expired_time_ms) 
            return;
        this.expired_time_ms = expired_time_ms; 
        markField(_bm, FIELD_expired_time_ms); 
    }
    public void saveExpiredTimeMs(BM _bm, long expired_time_ms) {
        if(expired_time_ms==this.expired_time_ms) 
            return;
        this.expired_time_ms = expired_time_ms;
        saveField(_bm, "expired_time_ms", expired_time_ms);
    }

    // 参数列表
    public byte[] getParamList() { return this.param_list; }
    public void setParamList(BM _bm, byte[] param_list) {
        if(param_list==this.param_list) 
            return;
        this.param_list = param_list; 
        markField(_bm, FIELD_param_list); 
    }
    public void saveParamList(BM _bm, byte[] param_list) {
        if(param_list==this.param_list) 
            return;
        this.param_list = param_list;
        saveFieldBytes(_bm, "param_list", param_list);
    }

    // 窗口展示队列
    public int getShowPosId() { return this.show_pos_id; }
    public void setShowPosId(BM _bm, int show_pos_id) {
        if(show_pos_id==this.show_pos_id) 
            return;
        this.show_pos_id = show_pos_id; 
        markField(_bm, FIELD_show_pos_id); 
    }
    public void saveShowPosId(BM _bm, int show_pos_id) {
        if(show_pos_id==this.show_pos_id) 
            return;
        this.show_pos_id = show_pos_id;
        saveField(_bm, "show_pos_id", show_pos_id);
    }

    // 优先级
    public int getPriorityId() { return this.priority_id; }
    public void setPriorityId(BM _bm, int priority_id) {
        if(priority_id==this.priority_id) 
            return;
        this.priority_id = priority_id; 
        markField(_bm, FIELD_priority_id); 
    }
    public void savePriorityId(BM _bm, int priority_id) {
        if(priority_id==this.priority_id) 
            return;
        this.priority_id = priority_id;
        saveField(_bm, "priority_id", priority_id);
    }

    // 循环播放时长秒
    public int getDurationSec() { return this.duration_sec; }
    public void setDurationSec(BM _bm, int duration_sec) {
        if(duration_sec==this.duration_sec) 
            return;
        this.duration_sec = duration_sec; 
        markField(_bm, FIELD_duration_sec); 
    }
    public void saveDurationSec(BM _bm, int duration_sec) {
        if(duration_sec==this.duration_sec) 
            return;
        this.duration_sec = duration_sec;
        saveField(_bm, "duration_sec", duration_sec);
    }

    // 循环播放次数
    public int getDurationCount() { return this.duration_count; }
    public void setDurationCount(BM _bm, int duration_count) {
        if(duration_count==this.duration_count) 
            return;
        this.duration_count = duration_count; 
        markField(_bm, FIELD_duration_count); 
    }
    public void saveDurationCount(BM _bm, int duration_count) {
        if(duration_count==this.duration_count) 
            return;
        this.duration_count = duration_count;
        saveField(_bm, "duration_count", duration_count);
    }

    // 预制体ID
    public long getUiResId() { return this.ui_res_id; }
    public void setUiResId(BM _bm, long ui_res_id) {
        if(ui_res_id==this.ui_res_id) 
            return;
        this.ui_res_id = ui_res_id; 
        markField(_bm, FIELD_ui_res_id); 
    }
    public void saveUiResId(BM _bm, long ui_res_id) {
        if(ui_res_id==this.ui_res_id) 
            return;
        this.ui_res_id = ui_res_id;
        saveField(_bm, "ui_res_id", ui_res_id);
    }

    // 内容
    public String getContent() { return this.content; }
    public void setContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content; 
        markField(_bm, FIELD_content); 
    }
    public void saveContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content;
        saveField(_bm, "content", content);
    }

    // 是否可删除类型
    public int getCanDelType() { return this.can_del_type; }
    public void setCanDelType(BM _bm, int can_del_type) {
        if(can_del_type==this.can_del_type) 
            return;
        this.can_del_type = can_del_type; 
        markField(_bm, FIELD_can_del_type); 
    }
    public void saveCanDelType(BM _bm, int can_del_type) {
        if(can_del_type==this.can_del_type) 
            return;
        this.can_del_type = can_del_type;
        saveField(_bm, "can_del_type", can_del_type);
    }

    // 玩家离线期间是否需要展示
    public boolean getOfflineNeedShow() { return this.offline_need_show; }
    public void setOfflineNeedShow(BM _bm, boolean offline_need_show) {
        if(offline_need_show==this.offline_need_show) 
            return;
        this.offline_need_show = offline_need_show; 
        markField(_bm, FIELD_offline_need_show); 
    }
    public void saveOfflineNeedShow(BM _bm, boolean offline_need_show) {
        if(offline_need_show==this.offline_need_show) 
            return;
        this.offline_need_show = offline_need_show;
        saveField(_bm, "offline_need_show", offline_need_show ? 1 : 0);
    }

    // 默认语言
    public String getDefaultLang() { return this.defaultLang; }
    public void setDefaultLang(BM _bm, String defaultLang) {
        if(defaultLang.equals(this.defaultLang)) 
            return;
        this.defaultLang = defaultLang; 
        markField(_bm, FIELD_defaultLang); 
    }
    public void saveDefaultLang(BM _bm, String defaultLang) {
        if(defaultLang.equals(this.defaultLang)) 
            return;
        this.defaultLang = defaultLang;
        saveField(_bm, "defaultLang", defaultLang);
    }

    // 渠道列表
    public byte[] getChannelList() { return this.channelList; }
    public void setChannelList(BM _bm, byte[] channelList) {
        if(channelList==this.channelList) 
            return;
        this.channelList = channelList; 
        markField(_bm, FIELD_channelList); 
    }
    public void saveChannelList(BM _bm, byte[] channelList) {
        if(channelList==this.channelList) 
            return;
        this.channelList = channelList;
        saveFieldBytes(_bm, "channelList", channelList);
    }

    // 展示条件
    public String getShowCondition() { return this.showCondition; }
    public void setShowCondition(BM _bm, String showCondition) {
        if(showCondition.equals(this.showCondition)) 
            return;
        this.showCondition = showCondition; 
        markField(_bm, FIELD_showCondition); 
    }
    public void saveShowCondition(BM _bm, String showCondition) {
        if(showCondition.equals(this.showCondition)) 
            return;
        this.showCondition = showCondition;
        saveField(_bm, "showCondition", showCondition);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `php_id` = '").append(php_id).append("',");
        sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        sBuilder.append(" `expired_time_ms` = '").append(expired_time_ms).append("',");
        sBuilder.append(" `param_list` = ?,");
        sBuilder.append(" `show_pos_id` = '").append(show_pos_id).append("',");
        sBuilder.append(" `priority_id` = '").append(priority_id).append("',");
        sBuilder.append(" `duration_sec` = '").append(duration_sec).append("',");
        sBuilder.append(" `duration_count` = '").append(duration_count).append("',");
        sBuilder.append(" `ui_res_id` = '").append(ui_res_id).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `can_del_type` = '").append(can_del_type).append("',");
        sBuilder.append(" `offline_need_show` = '").append(offline_need_show ? 1 : 0).append("',");
        sBuilder.append(" `defaultLang` = '").append(defaultLang == null ? null : defaultLang.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `channelList` = ?,");
        sBuilder.append(" `showCondition` = '").append(showCondition == null ? null : showCondition.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_php_id)) sBuilder.append(" `php_id` = '").append(php_id).append("',");
        if(isFieldMarked(FIELD_create_time_ms)) sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        if(isFieldMarked(FIELD_expired_time_ms)) sBuilder.append(" `expired_time_ms` = '").append(expired_time_ms).append("',");
        if(isFieldMarked(FIELD_param_list)) sBuilder.append(" `param_list` = ?,");
        if(isFieldMarked(FIELD_show_pos_id)) sBuilder.append(" `show_pos_id` = '").append(show_pos_id).append("',");
        if(isFieldMarked(FIELD_priority_id)) sBuilder.append(" `priority_id` = '").append(priority_id).append("',");
        if(isFieldMarked(FIELD_duration_sec)) sBuilder.append(" `duration_sec` = '").append(duration_sec).append("',");
        if(isFieldMarked(FIELD_duration_count)) sBuilder.append(" `duration_count` = '").append(duration_count).append("',");
        if(isFieldMarked(FIELD_ui_res_id)) sBuilder.append(" `ui_res_id` = '").append(ui_res_id).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_can_del_type)) sBuilder.append(" `can_del_type` = '").append(can_del_type).append("',");
        if(isFieldMarked(FIELD_offline_need_show)) sBuilder.append(" `offline_need_show` = '").append(offline_need_show ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_defaultLang)) sBuilder.append(" `defaultLang` = '").append(defaultLang == null ? null : defaultLang.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_channelList)) sBuilder.append(" `channelList` = ?,");
        if(isFieldMarked(FIELD_showCondition)) sBuilder.append(" `showCondition` = '").append(showCondition == null ? null : showCondition.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `common_marquee` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跑马灯配置id',"
                + "`php_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跑马灯后台id',"
                + "`create_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '生成时间',"
                + "`expired_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '过期时间',"
                + "`param_list` blob NULL COMMENT '参数列表',"
                + "`show_pos_id` int(11) NOT NULL DEFAULT '0' COMMENT '窗口展示队列',"
                + "`priority_id` int(11) NOT NULL DEFAULT '0' COMMENT '优先级',"
                + "`duration_sec` int(11) NOT NULL DEFAULT '0' COMMENT '循环播放时长秒',"
                + "`duration_count` int(11) NOT NULL DEFAULT '0' COMMENT '循环播放次数',"
                + "`ui_res_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '预制体ID',"
                + "`content` varchar(2048) NOT NULL DEFAULT '' COMMENT '内容',"
                + "`can_del_type` int(11) NOT NULL DEFAULT '0' COMMENT '是否可删除类型',"
                + "`offline_need_show` tinyint(1) NOT NULL DEFAULT '0' COMMENT '玩家离线期间是否需要展示',"
                + "`defaultLang` varchar(500) NOT NULL DEFAULT '' COMMENT '默认语言',"
                + "`channelList` blob NULL COMMENT '渠道列表',"
                + "`showCondition` varchar(2048) NOT NULL DEFAULT '' COMMENT '展示条件',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跑马灯信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//ref_id
        _size+=8;//php_id
        _size+=8;//create_time_ms
        _size+=8;//expired_time_ms
        _size+=2;_size+=param_list.length;//param_list
        _size+=4;//show_pos_id
        _size+=4;//priority_id
        _size+=4;//duration_sec
        _size+=4;//duration_count
        _size+=8;//ui_res_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
        _size+=4;//can_del_type
        _size+=1;//offline_need_show
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defaultLang);//defaultLang
        _size+=2;_size+=channelList.length;//channelList
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);//showCondition
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(ref_id);
        buff.putLong(php_id);
        buff.putLong(create_time_ms);
        buff.putLong(expired_time_ms);
        buff.putShort((short)(param_list == null ? 0 : param_list.length));if(null != param_list){buff.put(param_list);}
        buff.putInt(show_pos_id);
        buff.putInt(priority_id);
        buff.putInt(duration_sec);
        buff.putInt(duration_count);
        buff.putLong(ui_res_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);
        buff.putInt(can_del_type);
        buff.put((byte)(offline_need_show?1:0));
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, defaultLang);
        buff.putShort((short)(channelList == null ? 0 : channelList.length));if(null != channelList){buff.put(channelList);}
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, showCondition);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        ref_id=buff.getLong();
        php_id=buff.getLong();
        create_time_ms=buff.getLong();
        expired_time_ms=buff.getLong();
        int param_list_count = buff.getShort();if(param_list_count>0){param_list = new byte[param_list_count];buff.get(param_list);}
        show_pos_id=buff.getInt();
        priority_id=buff.getInt();
        duration_sec=buff.getInt();
        duration_count=buff.getInt();
        ui_res_id=buff.getLong();
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        can_del_type=buff.getInt();
        offline_need_show=(buff.get()==1);
        defaultLang=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        int channelList_count = buff.getShort();if(channelList_count>0){channelList = new byte[channelList_count];buff.get(channelList);}
        showCondition=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
