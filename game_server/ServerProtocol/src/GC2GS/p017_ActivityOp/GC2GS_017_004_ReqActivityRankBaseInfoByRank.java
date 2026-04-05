package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 请求活动排行榜基础信息通过排名
 **/
public class GC2GS_017_004_ReqActivityRankBaseInfoByRank implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 排行榜id */
private long rankId;
private int rank;
private boolean isCross;


public GC2GS_017_004_ReqActivityRankBaseInfoByRank() {
	instanceId = (long)0;
	rankId = (long)0;
	rank = 0;
	isCross = false;
}

public GC2GS_017_004_ReqActivityRankBaseInfoByRank(
	 long _instanceId
	, long _rankId
	, int _rank
	, boolean _isCross
) {	instanceId = _instanceId;
	rankId = _rankId;
	rank = _rank;
	isCross = _isCross;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)4; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 排行榜id */
public long getRankId() { return rankId; }
/** 排行榜id */
public void setRankId(long _rankId) { rankId = _rankId; }
public int getRank() { return rank; }
public void setRank(int _rank) { rank = _rank; }
public boolean getIsCross() { return isCross; }
public void setIsCross(boolean _isCross) { isCross = _isCross; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCross = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(rankId);
	_buf.putInt(rank);
	_buf.put(isCross?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)4);
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

