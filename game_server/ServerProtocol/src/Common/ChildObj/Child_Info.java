package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣数据
 **/
public class Child_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣ID */
private long id;
/** 关联家人ID */
private long consortId;
/** 子嗣初始形象配置ID */
private long initResId;
/** 子嗣资质 */
private long quality;
/** 子嗣相性 */
private CommonEnum.ESpecAttrType attrType;
/** 子嗣职业配置ID */
private long careerId;
/** 训练位ID */
private long seatId;
/** 是否卷王 */
private boolean isGiftde;
/** 初始亲密度 */
private long initIntimacy;
/** 子嗣名称 */
private String name;
/** 子嗣等级 */
private int lvl;
/** 子嗣基础收益（用于计算上课收益） */
private long baseBonus;
/** 子嗣收益（上课收益） */
private long bonus;
/** 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus */
private int initStudyBonus;


public Child_Info() {
	id = (long)0;
	consortId = (long)0;
	initResId = (long)0;
	quality = (long)0;
	attrType = CommonEnum.ESpecAttrType.values()[0];
	careerId = (long)0;
	seatId = (long)0;
	isGiftde = false;
	initIntimacy = (long)0;
	name = "";
	lvl = 0;
	baseBonus = (long)0;
	bonus = (long)0;
	initStudyBonus = 0;
}

public Child_Info(
	 long _id
	, long _consortId
	, long _initResId
	, long _quality
	, CommonEnum.ESpecAttrType _attrType
	, long _careerId
	, long _seatId
	, boolean _isGiftde
	, long _initIntimacy
	, String _name
	, int _lvl
	, long _baseBonus
	, long _bonus
	, int _initStudyBonus
) {	id = _id;
	consortId = _consortId;
	initResId = _initResId;
	quality = _quality;
	attrType = _attrType;
	careerId = _careerId;
	seatId = _seatId;
	isGiftde = _isGiftde;
	initIntimacy = _initIntimacy;
	name = _name;
	lvl = _lvl;
	baseBonus = _baseBonus;
	bonus = _bonus;
	initStudyBonus = _initStudyBonus;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子嗣ID */
public long getId() { return id; }
/** 子嗣ID */
public void setId(long _id) { id = _id; }
/** 关联家人ID */
public long getConsortId() { return consortId; }
/** 关联家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 子嗣初始形象配置ID */
public long getInitResId() { return initResId; }
/** 子嗣初始形象配置ID */
public void setInitResId(long _initResId) { initResId = _initResId; }
/** 子嗣资质 */
public long getQuality() { return quality; }
/** 子嗣资质 */
public void setQuality(long _quality) { quality = _quality; }
/** 子嗣相性 */
public CommonEnum.ESpecAttrType getAttrType() { return attrType; }
/** 子嗣相性 */
public void setAttrType(CommonEnum.ESpecAttrType _attrType) { attrType = _attrType; }
/** 子嗣职业配置ID */
public long getCareerId() { return careerId; }
/** 子嗣职业配置ID */
public void setCareerId(long _careerId) { careerId = _careerId; }
/** 训练位ID */
public long getSeatId() { return seatId; }
/** 训练位ID */
public void setSeatId(long _seatId) { seatId = _seatId; }
/** 是否卷王 */
public boolean getIsGiftde() { return isGiftde; }
/** 是否卷王 */
public void setIsGiftde(boolean _isGiftde) { isGiftde = _isGiftde; }
/** 初始亲密度 */
public long getInitIntimacy() { return initIntimacy; }
/** 初始亲密度 */
public void setInitIntimacy(long _initIntimacy) { initIntimacy = _initIntimacy; }
/** 子嗣名称 */
public String getName() { return name; }
/** 子嗣名称 */
public void setName(String _name) { name = _name; }
/** 子嗣等级 */
public int getLvl() { return lvl; }
/** 子嗣等级 */
public void setLvl(int _lvl) { lvl = _lvl; }
/** 子嗣基础收益（用于计算上课收益） */
public long getBaseBonus() { return baseBonus; }
/** 子嗣基础收益（用于计算上课收益） */
public void setBaseBonus(long _baseBonus) { baseBonus = _baseBonus; }
/** 子嗣收益（上课收益） */
public long getBonus() { return bonus; }
/** 子嗣收益（上课收益） */
public void setBonus(long _bonus) { bonus = _bonus; }
/** 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus */
public int getInitStudyBonus() { return initStudyBonus; }
/** 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus */
public void setInitStudyBonus(int _initStudyBonus) { initStudyBonus = _initStudyBonus; }


public final int GetBufSize() {
	int _size = 85;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 87;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) initResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) quality = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attrType = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) careerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) seatId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isGiftde = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) initIntimacy = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) baseBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) initStudyBonus = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(consortId);
	_buf.putLong(initResId);
	_buf.putLong(quality);
	_buf.putInt(attrType.ordinal());

	_buf.putLong(careerId);
	_buf.putLong(seatId);
	_buf.put(isGiftde?(byte)1:(byte)0);
	_buf.putLong(initIntimacy);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putInt(lvl);
	_buf.putLong(baseBonus);
	_buf.putLong(bonus);
	_buf.putInt(initStudyBonus);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

