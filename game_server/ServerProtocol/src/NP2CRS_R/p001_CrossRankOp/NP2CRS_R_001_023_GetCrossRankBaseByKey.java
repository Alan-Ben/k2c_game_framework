package NP2CRS_R.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_R_001_023_GetCrossRankBaseByKey implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服分组ID */
private long crossInstanceId;
/** 排行Id */
private long rankId;
private long key;


public NP2CRS_R_001_023_GetCrossRankBaseByKey() {
	crossInstanceId = (long)0;
	rankId = (long)0;
	key = (long)0;
}

public NP2CRS_R_001_023_GetCrossRankBaseByKey(
	 long _crossInstanceId
	, long _rankId
	, long _key
) {	crossInstanceId = _crossInstanceId;
	rankId = _rankId;
	key = _key;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)23; }

/** 跨服分组ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服分组ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** 排行Id */
public long getRankId() { return rankId; }
/** 排行Id */
public void setRankId(long _rankId) { rankId = _rankId; }
public long getKey() { return key; }
public void setKey(long _key) { key = _key; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) key = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(crossInstanceId);
	_buf.putLong(rankId);
	_buf.putLong(key);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)23);
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

