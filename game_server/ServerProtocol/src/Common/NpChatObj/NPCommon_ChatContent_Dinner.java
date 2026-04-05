package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 宴会邀请
 **/
public class NPCommon_ChatContent_Dinner implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会实例ID */
private long instanceId;
/** 宴会配置ID */
private long dinnerId;
/** 开宴玩家CID */
private long ownerCid;
/** 参与宴会玩家数量 */
private int joinerCount;


public NPCommon_ChatContent_Dinner() {
	instanceId = (long)0;
	dinnerId = (long)0;
	ownerCid = (long)0;
	joinerCount = 0;
}

public NPCommon_ChatContent_Dinner(
	 long _instanceId
	, long _dinnerId
	, long _ownerCid
	, int _joinerCount
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	ownerCid = _ownerCid;
	joinerCount = _joinerCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宴会实例ID */
public long getInstanceId() { return instanceId; }
/** 宴会实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 宴会配置ID */
public long getDinnerId() { return dinnerId; }
/** 宴会配置ID */
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }
/** 开宴玩家CID */
public long getOwnerCid() { return ownerCid; }
/** 开宴玩家CID */
public void setOwnerCid(long _ownerCid) { ownerCid = _ownerCid; }
/** 参与宴会玩家数量 */
public int getJoinerCount() { return joinerCount; }
/** 参与宴会玩家数量 */
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ownerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(ownerCid);
	_buf.putInt(joinerCount);
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

