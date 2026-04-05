package Common.NpChatObj;

import java.nio.ByteBuffer;
public class NPCommon_ChatContent_ActivityRankBox implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long instanceId;
/** 宝箱配表ID */
private long refId;
/** cid或团体id */
private long key;
/** 活动冲榜ID */
private long activityRankRushId;


public NPCommon_ChatContent_ActivityRankBox() {
	instanceId = (long)0;
	refId = (long)0;
	key = (long)0;
	activityRankRushId = (long)0;
}

public NPCommon_ChatContent_ActivityRankBox(
	 long _instanceId
	, long _refId
	, long _key
	, long _activityRankRushId
) {	instanceId = _instanceId;
	refId = _refId;
	key = _key;
	activityRankRushId = _activityRankRushId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getInstanceId() { return instanceId; }
/** 实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 宝箱配表ID */
public long getRefId() { return refId; }
/** 宝箱配表ID */
public void setRefId(long _refId) { refId = _refId; }
/** cid或团体id */
public long getKey() { return key; }
/** cid或团体id */
public void setKey(long _key) { key = _key; }
/** 活动冲榜ID */
public long getActivityRankRushId() { return activityRankRushId; }
/** 活动冲榜ID */
public void setActivityRankRushId(long _activityRankRushId) { activityRankRushId = _activityRankRushId; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) key = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityRankRushId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(refId);
	_buf.putLong(key);
	_buf.putLong(activityRankRushId);
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

