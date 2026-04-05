package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 通知角色游戏事件
 **/
public class NP2US_R_003_016_ReqNotifyRoleGameEvent implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/** 活动标识 */
private String activityCode;
/** 事件类型 */
private String type;


public NP2US_R_003_016_ReqNotifyRoleGameEvent() {
	cid = (long)0;
	activityCode = "";
	type = "";
}

public NP2US_R_003_016_ReqNotifyRoleGameEvent(
	 long _cid
	, String _activityCode
	, String _type
) {	cid = _cid;
	activityCode = _activityCode;
	type = _type;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)16; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 活动标识 */
public String getActivityCode() { return activityCode; }
/** 活动标识 */
public void setActivityCode(String _activityCode) { activityCode = _activityCode; }
/** 事件类型 */
public String getType() { return type; }
/** 事件类型 */
public void setType(String _type) { type = _type; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(activityCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(type);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(activityCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(type);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, activityCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, type);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)16);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)16);
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

