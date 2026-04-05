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
public class PlayerInnDishBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_dish_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "dish_id", comment = "菜品ID")
    private long dish_id;

    public static final int FIELD_level =2;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_finesse =3;
    @DataBaseField(type = "bigint(20)", fieldname = "finesse", comment = "熟练度")
    private long finesse;

    public static final int FIELD_had_gain_recipe =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_gain_recipe", comment = "是否获得过菜谱")
    private boolean had_gain_recipe;

    public static final int FIELD_start_line_up_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "start_line_up_id", comment = "开始排队的id")
    private long start_line_up_id;

    public PlayerInnDishBO() {
        id = 0;
        cid = 0L;
        dish_id = 0L;
        level = 0;
        finesse = 0L;
        had_gain_recipe = false;
        start_line_up_id = 0L;
    }

    public PlayerInnDishBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        dish_id = rs.getLong(3);
        level = rs.getInt(4);
        finesse = rs.getLong(5);
        had_gain_recipe = rs.getBoolean(6);
        start_line_up_id = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerInnDishBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `dish_id`, `level`, `finesse`, `had_gain_recipe`, `start_line_up_id`";
    }

    @Override
    public String getTableName() {
        return "`player_inn_dish`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(dish_id).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(finesse).append("', ");
        strBuf.append("'").append(had_gain_recipe ? 1 : 0).append("', ");
        strBuf.append("'").append(start_line_up_id).append("', ");
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

    // 菜品ID
    public long getDishId() { return this.dish_id; }
    public void setDishId(BM _bm, long dish_id) {
        if(dish_id==this.dish_id) 
            return;
        this.dish_id = dish_id; 
        markField(_bm, FIELD_dish_id); 
    }
    public void saveDishId(BM _bm, long dish_id) {
        if(dish_id==this.dish_id) 
            return;
        this.dish_id = dish_id;
        saveField(_bm, "dish_id", dish_id);
    }

    // 等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 熟练度
    public long getFinesse() { return this.finesse; }
    public void setFinesse(BM _bm, long finesse) {
        if(finesse==this.finesse) 
            return;
        this.finesse = finesse; 
        markField(_bm, FIELD_finesse); 
    }
    public void saveFinesse(BM _bm, long finesse) {
        if(finesse==this.finesse) 
            return;
        this.finesse = finesse;
        saveField(_bm, "finesse", finesse);
    }

    // 是否获得过菜谱
    public boolean getHadGainRecipe() { return this.had_gain_recipe; }
    public void setHadGainRecipe(BM _bm, boolean had_gain_recipe) {
        if(had_gain_recipe==this.had_gain_recipe) 
            return;
        this.had_gain_recipe = had_gain_recipe; 
        markField(_bm, FIELD_had_gain_recipe); 
    }
    public void saveHadGainRecipe(BM _bm, boolean had_gain_recipe) {
        if(had_gain_recipe==this.had_gain_recipe) 
            return;
        this.had_gain_recipe = had_gain_recipe;
        saveField(_bm, "had_gain_recipe", had_gain_recipe ? 1 : 0);
    }

    // 开始排队的id
    public long getStartLineUpId() { return this.start_line_up_id; }
    public void setStartLineUpId(BM _bm, long start_line_up_id) {
        if(start_line_up_id==this.start_line_up_id) 
            return;
        this.start_line_up_id = start_line_up_id; 
        markField(_bm, FIELD_start_line_up_id); 
    }
    public void saveStartLineUpId(BM _bm, long start_line_up_id) {
        if(start_line_up_id==this.start_line_up_id) 
            return;
        this.start_line_up_id = start_line_up_id;
        saveField(_bm, "start_line_up_id", start_line_up_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `dish_id` = '").append(dish_id).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `finesse` = '").append(finesse).append("',");
        sBuilder.append(" `had_gain_recipe` = '").append(had_gain_recipe ? 1 : 0).append("',");
        sBuilder.append(" `start_line_up_id` = '").append(start_line_up_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_dish_id)) sBuilder.append(" `dish_id` = '").append(dish_id).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_finesse)) sBuilder.append(" `finesse` = '").append(finesse).append("',");
        if(isFieldMarked(FIELD_had_gain_recipe)) sBuilder.append(" `had_gain_recipe` = '").append(had_gain_recipe ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_start_line_up_id)) sBuilder.append(" `start_line_up_id` = '").append(start_line_up_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_inn_dish` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`dish_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '菜品ID',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`finesse` bigint(20) NOT NULL DEFAULT '0' COMMENT '熟练度',"
                + "`had_gain_recipe` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否获得过菜谱',"
                + "`start_line_up_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始排队的id',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家旅店 菜品数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//dish_id
        _size+=4;//level
        _size+=8;//finesse
        _size+=1;//had_gain_recipe
        _size+=8;//start_line_up_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(dish_id);
        buff.putInt(level);
        buff.putLong(finesse);
        buff.put((byte)(had_gain_recipe?1:0));
        buff.putLong(start_line_up_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        dish_id=buff.getLong();
        level=buff.getInt();
        finesse=buff.getLong();
        had_gain_recipe=(buff.get()==1);
        start_line_up_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
