package NP2CRS_R.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_R_001_024_DumpCrossRankListByUs implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服分组ID */
private long crossInstanceId;
/** 排行Id */
private long rankId;
/** 限制排名 */
private int limitRank;
/** 指定us */
private int usId;


public NP2CRS_R_001_024_DumpCrossRankListByUs() {
	crossInstanceId = (long)0;
	rankId = (long)0;
	limitRank = 0;
	usId = 0;
}

public NP2CRS_R_001_024_DumpCrossRankListByUs(
	 long _crossInstanceId
	, long _rankId
	, int _limitRank
	, int _usId
) {	crossInstanceId = _crossInstanceId;
	rankId = _rankId;
	limitRank = _limitRank;
	usId = _usId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)24; }

/** 跨服分组ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服分组ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** 排行Id */
public long getRankId() { return rankId; }
/** 排行Id */
public void setRankId(long _rankId) { rankId = _rankId; }
/** 限制排名 */
public int getLimitRank() { return limitRank; }
/** 限制排名 */
public void setLimitRank(int _limitRank) { limitRank = _limitRank; }
/** 指定us */
public int getUsId() { return usId; }
/** 指定us */
public void setUsId(int _usId) { usId = _usId; }


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
	if(_buf.remaining() > 0) limitRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(crossInstanceId);
	_buf.putLong(rankId);
	_buf.putInt(limitRank);
	_buf.putInt(usId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

