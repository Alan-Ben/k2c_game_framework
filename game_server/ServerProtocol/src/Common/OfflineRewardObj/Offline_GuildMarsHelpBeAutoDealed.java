package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 公会-火星互助-被帮助
 **/
public class Offline_GuildMarsHelpBeAutoDealed implements ALBasicProtocolPack._IALProtocolStructure {
/** 对象类型 */
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/** 对象实例ID */
private long objId;
/** 求助实例ID */
private long helpId;
/** 求助时长（秒） */
private int helpSecs;
/** 互助玩家CID列表 */
private java.util.ArrayList<Long> dealedCidList;
/** 之前被互助次数 */
private int preDealedCount;
/** 允许被帮助的上限 */
private int dealLimit;


public Offline_GuildMarsHelpBeAutoDealed() {
	objType = Common.GuildEnum.EGuildMarsHelpObjType.values()[0];
	objId = (long)0;
	helpId = (long)0;
	helpSecs = 0;
	dealedCidList = new java.util.ArrayList<Long>();
	preDealedCount = 0;
	dealLimit = 0;
}

public Offline_GuildMarsHelpBeAutoDealed(
	 Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, long _helpId
	, int _helpSecs
	, java.util.ArrayList<Long> _dealedCidList
	, int _preDealedCount
	, int _dealLimit
) {	objType = _objType;
	objId = _objId;
	helpId = _helpId;
	helpSecs = _helpSecs;
	dealedCidList = _dealedCidList;
	preDealedCount = _preDealedCount;
	dealLimit = _dealLimit;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 对象类型 */
public Common.GuildEnum.EGuildMarsHelpObjType getObjType() { return objType; }
/** 对象类型 */
public void setObjType(Common.GuildEnum.EGuildMarsHelpObjType _objType) { objType = _objType; }
/** 对象实例ID */
public long getObjId() { return objId; }
/** 对象实例ID */
public void setObjId(long _objId) { objId = _objId; }
/** 求助实例ID */
public long getHelpId() { return helpId; }
/** 求助实例ID */
public void setHelpId(long _helpId) { helpId = _helpId; }
/** 求助时长（秒） */
public int getHelpSecs() { return helpSecs; }
/** 求助时长（秒） */
public void setHelpSecs(int _helpSecs) { helpSecs = _helpSecs; }
/** 互助玩家CID列表 */
public java.util.ArrayList<Long> getDealedCidList() { return dealedCidList; }
/** 互助玩家CID列表 */
public void addDealedCidList(long _dealedCidList) { dealedCidList.add(_dealedCidList); }
/** 之前被互助次数 */
public int getPreDealedCount() { return preDealedCount; }
/** 之前被互助次数 */
public void setPreDealedCount(int _preDealedCount) { preDealedCount = _preDealedCount; }
/** 允许被帮助的上限 */
public int getDealLimit() { return dealLimit; }
/** 允许被帮助的上限 */
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }


public final int GetBufSize() {
	int _size = 32;
	_size += 2 + (dealedCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (dealedCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objType = Common.GuildEnum.EGuildMarsHelpObjType.EGuildMarsHelpObjType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) helpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) helpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dealedCidListCount = _buf.getShort();
	for(int _i = 0; _i < _dealedCidListCount; _i++) { 
		long _dealedCidList = (long)0;
		if(_buf.remaining() > 0) _dealedCidList = _buf.getLong();
		dealedCidList.add(_dealedCidList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) preDealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealLimit = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
	_buf.putLong(helpId);
	_buf.putInt(helpSecs);
	_buf.putShort((short)dealedCidList.size());
	for(int _i = 0; _i < dealedCidList.size(); _i++) { 
		_buf.putLong(dealedCidList.get(_i));
	}
	_buf.putInt(preDealedCount);
	_buf.putInt(dealLimit);
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

