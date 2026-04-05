package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作攻击日志
 **/
public class GuildCooperate_AttackLog implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据ID */
private long dbId;
/** 攻击时间 ms */
private long timeMs;
/** 攻击者名称 */
private String name;
/** 奖励据点ID */
private long posId;
/** 属性 */
private CommonEnum.ESpecAttrType attr;
/** 攻击血量 */
private long attackHp;


public GuildCooperate_AttackLog() {
	dbId = (long)0;
	timeMs = (long)0;
	name = "";
	posId = (long)0;
	attr = CommonEnum.ESpecAttrType.values()[0];
	attackHp = (long)0;
}

public GuildCooperate_AttackLog(
	 long _dbId
	, long _timeMs
	, String _name
	, long _posId
	, CommonEnum.ESpecAttrType _attr
	, long _attackHp
) {	dbId = _dbId;
	timeMs = _timeMs;
	name = _name;
	posId = _posId;
	attr = _attr;
	attackHp = _attackHp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据ID */
public long getDbId() { return dbId; }
/** 数据ID */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 攻击时间 ms */
public long getTimeMs() { return timeMs; }
/** 攻击时间 ms */
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }
/** 攻击者名称 */
public String getName() { return name; }
/** 攻击者名称 */
public void setName(String _name) { name = _name; }
/** 奖励据点ID */
public long getPosId() { return posId; }
/** 奖励据点ID */
public void setPosId(long _posId) { posId = _posId; }
/** 属性 */
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/** 属性 */
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/** 攻击血量 */
public long getAttackHp() { return attackHp; }
/** 攻击血量 */
public void setAttackHp(long _attackHp) { attackHp = _attackHp; }


public final int GetBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attr = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackHp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(timeMs);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putLong(posId);
	_buf.putInt(attr.ordinal());

	_buf.putLong(attackHp);
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

