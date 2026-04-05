package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_024_RetRefreshChatBoxStatus implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱实例ID */
private long instanceId;
/** 宝箱状态 */
private NPEnum.ENPBoxChatStatus boxStatus;
/** 已经领取的玩家CID列表 */
private java.util.ArrayList<Long> gainedCidList;


public GS2GC_004_024_RetRefreshChatBoxStatus() {
	instanceId = (long)0;
	boxStatus = NPEnum.ENPBoxChatStatus.values()[0];
	gainedCidList = new java.util.ArrayList<Long>();
}

public GS2GC_004_024_RetRefreshChatBoxStatus(
	 long _instanceId
	, NPEnum.ENPBoxChatStatus _boxStatus
	, java.util.ArrayList<Long> _gainedCidList
) {	instanceId = _instanceId;
	boxStatus = _boxStatus;
	gainedCidList = _gainedCidList;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)24; }

/** 宝箱实例ID */
public long getInstanceId() { return instanceId; }
/** 宝箱实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 宝箱状态 */
public NPEnum.ENPBoxChatStatus getBoxStatus() { return boxStatus; }
/** 宝箱状态 */
public void setBoxStatus(NPEnum.ENPBoxChatStatus _boxStatus) { boxStatus = _boxStatus; }
/** 已经领取的玩家CID列表 */
public java.util.ArrayList<Long> getGainedCidList() { return gainedCidList; }
/** 已经领取的玩家CID列表 */
public void addGainedCidList(long _gainedCidList) { gainedCidList.add(_gainedCidList); }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (gainedCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (gainedCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxStatus = NPEnum.ENPBoxChatStatus.ENPBoxChatStatus_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainedCidListCount = _buf.getShort();
	for(int _i = 0; _i < _gainedCidListCount; _i++) { 
		long _gainedCidList = (long)0;
		if(_buf.remaining() > 0) _gainedCidList = _buf.getLong();
		gainedCidList.add(_gainedCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(boxStatus.ordinal());

	_buf.putShort((short)gainedCidList.size());
	for(int _i = 0; _i < gainedCidList.size(); _i++) { 
		_buf.putLong(gainedCidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)24);
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

