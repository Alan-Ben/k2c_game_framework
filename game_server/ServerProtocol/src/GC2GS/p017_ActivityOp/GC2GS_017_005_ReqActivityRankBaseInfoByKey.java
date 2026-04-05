package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 请求活动排行榜基础信息通过主体id
 **/
public class GC2GS_017_005_ReqActivityRankBaseInfoByKey implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 排行榜id */
private long rankId;
private long key;
private boolean isCross;


public GC2GS_017_005_ReqActivityRankBaseInfoByKey() {
	instanceId = (long)0;
	rankId = (long)0;
	key = (long)0;
	isCross = false;
}

public GC2GS_017_005_ReqActivityRankBaseInfoByKey(
	 long _instanceId
	, long _rankId
	, long _key
	, boolean _isCross
) {	instanceId = _instanceId;
	rankId = _rankId;
	key = _key;
	isCross = _isCross;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)5; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 排行榜id */
public long getRankId() { return rankId; }
/** 排行榜id */
public void setRankId(long _rankId) { rankId = _rankId; }
public long getKey() { return key; }
public void setKey(long _key) { key = _key; }
public boolean getIsCross() { return isCross; }
public void setIsCross(boolean _isCross) { isCross = _isCross; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) key = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCross = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(rankId);
	_buf.putLong(key);
	_buf.put(isCross?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)5);
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

