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
public class PlayerEquipSkillBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_equip_db_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "equip_db_id", comment = "藏品数据id")
    private long equip_db_id;

    public static final int FIELD_index =2;
    @DataBaseField(type = "int(11)", fieldname = "index", comment = "索引")
    private int index;

    public static final int FIELD_value =3;
    @DataBaseField(type = "int(11)", fieldname = "value", comment = "属性值")
    private int value;

    public static final int FIELD_pending_value =4;
    @DataBaseField(type = "int(11)", fieldname = "pending_value", comment = "上次重塑的值")
    private int pending_value;

    public static final int FIELD_normal_reshape_num =5;
    @DataBaseField(type = "int(11)", fieldname = "normal_reshape_num", comment = "普通重塑次数")
    private int normal_reshape_num;

    public PlayerEquipSkillBO() {
        id = 0;
        cid = 0L;
        equip_db_id = 0L;
        index = 0;
        value = 0;
        pending_value = 0;
        normal_reshape_num = 0;
    }

    public PlayerEquipSkillBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        equip_db_id = rs.getLong(3);
        index = rs.getInt(4);
        value = rs.getInt(5);
        pending_value = rs.getInt(6);
        normal_reshape_num = rs.getInt(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerEquipSkillBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `equip_db_id`, `index`, `value`, `pending_value`, `normal_reshape_num`";
    }

    @Override
    public String getTableName() {
        return "`player_equip_skill`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(equip_db_id).append("', ");
        strBuf.append("'").append(index).append("', ");
        strBuf.append("'").append(value).append("', ");
        strBuf.append("'").append(pending_value).append("', ");
        strBuf.append("'").append(normal_reshape_num).append("', ");
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

    // 藏品数据id
    public long getEquipDbId() { return this.equip_db_id; }
    public void setEquipDbId(BM _bm, long equip_db_id) {
        if(equip_db_id==this.equip_db_id) 
            return;
        this.equip_db_id = equip_db_id; 
        markField(_bm, FIELD_equip_db_id); 
    }
    public void saveEquipDbId(BM _bm, long equip_db_id) {
        if(equip_db_id==this.equip_db_id) 
            return;
        this.equip_db_id = equip_db_id;
        saveField(_bm, "equip_db_id", equip_db_id);
    }

    // 索引
    public int getIndex() { return this.index; }
    public void setIndex(BM _bm, int index) {
        if(index==this.index) 
            return;
        this.index = index; 
        markField(_bm, FIELD_index); 
    }
    public void saveIndex(BM _bm, int index) {
        if(index==this.index) 
            return;
        this.index = index;
        saveField(_bm, "index", index);
    }

    // 属性值
    public int getValue() { return this.value; }
    public void setValue(BM _bm, int value) {
        if(value==this.value) 
            return;
        this.value = value; 
        markField(_bm, FIELD_value); 
    }
    public void saveValue(BM _bm, int value) {
        if(value==this.value) 
            return;
        this.value = value;
        saveField(_bm, "value", value);
    }

    // 上次重塑的值
    public int getPendingValue() { return this.pending_value; }
    public void setPendingValue(BM _bm, int pending_value) {
        if(pending_value==this.pending_value) 
            return;
        this.pending_value = pending_value; 
        markField(_bm, FIELD_pending_value); 
    }
    public void savePendingValue(BM _bm, int pending_value) {
        if(pending_value==this.pending_value) 
            return;
        this.pending_value = pending_value;
        saveField(_bm, "pending_value", pending_value);
    }

    // 普通重塑次数
    public int getNormalReshapeNum() { return this.normal_reshape_num; }
    public void setNormalReshapeNum(BM _bm, int normal_reshape_num) {
        if(normal_reshape_num==this.normal_reshape_num) 
            return;
        this.normal_reshape_num = normal_reshape_num; 
        markField(_bm, FIELD_normal_reshape_num); 
    }
    public void saveNormalReshapeNum(BM _bm, int normal_reshape_num) {
        if(normal_reshape_num==this.normal_reshape_num) 
            return;
        this.normal_reshape_num = normal_reshape_num;
        saveField(_bm, "normal_reshape_num", normal_reshape_num);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `equip_db_id` = '").append(equip_db_id).append("',");
        sBuilder.append(" `index` = '").append(index).append("',");
        sBuilder.append(" `value` = '").append(value).append("',");
        sBuilder.append(" `pending_value` = '").append(pending_value).append("',");
        sBuilder.append(" `normal_reshape_num` = '").append(normal_reshape_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_equip_db_id)) sBuilder.append(" `equip_db_id` = '").append(equip_db_id).append("',");
        if(isFieldMarked(FIELD_index)) sBuilder.append(" `index` = '").append(index).append("',");
        if(isFieldMarked(FIELD_value)) sBuilder.append(" `value` = '").append(value).append("',");
        if(isFieldMarked(FIELD_pending_value)) sBuilder.append(" `pending_value` = '").append(pending_value).append("',");
        if(isFieldMarked(FIELD_normal_reshape_num)) sBuilder.append(" `normal_reshape_num` = '").append(normal_reshape_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_equip_skill` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`equip_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '藏品数据id',"
                + "`index` int(11) NOT NULL DEFAULT '0' COMMENT '索引',"
                + "`value` int(11) NOT NULL DEFAULT '0' COMMENT '属性值',"
                + "`pending_value` int(11) NOT NULL DEFAULT '0' COMMENT '上次重塑的值',"
                + "`normal_reshape_num` int(11) NOT NULL DEFAULT '0' COMMENT '普通重塑次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家藏品技能数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//equip_db_id
        _size+=4;//index
        _size+=4;//value
        _size+=4;//pending_value
        _size+=4;//normal_reshape_num
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(equip_db_id);
        buff.putInt(index);
        buff.putInt(value);
        buff.putInt(pending_value);
        buff.putInt(normal_reshape_num);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        equip_db_id=buff.getLong();
        index=buff.getInt();
        value=buff.getInt();
        pending_value=buff.getInt();
        normal_reshape_num=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
