package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
public class GS2GC_042_053_OnMyMarsHelpDealed implements ALBasicProtocolPack._IALProtocolStructure {
/** 求助实例ID */
private long id;
/** 帮助玩家CID */
private long dealCid;
/** 被帮助的次数 */
private int dealedCount;
/** 允许被帮助的上限 */
private int dealLimit;
/** 对象类型 */
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/** 对象实例ID */
private long objId;
/** 是否自动帮助 */
private boolean isAuto;


public GS2GC_042_053_OnMyMarsHelpDealed() {
	id = (long)0;
	dealCid = (long)0;
	dealedCount = 0;
	dealLimit = 0;
	objType = Common.GuildEnum.EGuildMarsHelpObjType.values()[0];
	objId = (long)0;
	isAuto = false;
}

public GS2GC_042_053_OnMyMarsHelpDealed(
	 long _id
	, long _dealCid
	, int _dealedCount
	, int _dealLimit
	, Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, boolean _isAuto
) {	id = _id;
	dealCid = _dealCid;
	dealedCount = _dealedCount;
	dealLimit = _dealLimit;
	objType = _objType;
	objId = _objId;
	isAuto = _isAuto;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)53; }

/** 求助实例ID */
public long getId() { return id; }
/** 求助实例ID */
public void setId(long _id) { id = _id; }
/** 帮助玩家CID */
public long getDealCid() { return dealCid; }
/** 帮助玩家CID */
public void setDealCid(long _dealCid) { dealCid = _dealCid; }
/** 被帮助的次数 */
public int getDealedCount() { return dealedCount; }
/** 被帮助的次数 */
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }
/** 允许被帮助的上限 */
public int getDealLimit() { return dealLimit; }
/** 允许被帮助的上限 */
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }
/** 对象类型 */
public Common.GuildEnum.EGuildMarsHelpObjType getObjType() { return objType; }
/** 对象类型 */
public void setObjType(Common.GuildEnum.EGuildMarsHelpObjType _objType) { objType = _objType; }
/** 对象实例ID */
public long getObjId() { return objId; }
/** 对象实例ID */
public void setObjId(long _objId) { objId = _objId; }
/** 是否自动帮助 */
public boolean getIsAuto() { return isAuto; }
/** 是否自动帮助 */
public void setIsAuto(boolean _isAuto) { isAuto = _isAuto; }


public final int GetBufSize() {
	int _size = 37;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objType = Common.GuildEnum.EGuildMarsHelpObjType.EGuildMarsHelpObjType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAuto = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(dealCid);
	_buf.putInt(dealedCount);
	_buf.putInt(dealLimit);
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
	_buf.put(isAuto?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)53);
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

