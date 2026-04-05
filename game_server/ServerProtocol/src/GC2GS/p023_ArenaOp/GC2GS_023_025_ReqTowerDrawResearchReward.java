package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 爬塔领取研究奖励
 **/
public class GC2GS_023_025_ReqTowerDrawResearchReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 章节ID */
private long chapterId;


public GC2GS_023_025_ReqTowerDrawResearchReward() {
	chapterId = (long)0;
}

public GC2GS_023_025_ReqTowerDrawResearchReward(
	 long _chapterId
) {	chapterId = _chapterId;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)25; }

/** 章节ID */
public long getChapterId() { return chapterId; }
/** 章节ID */
public void setChapterId(long _chapterId) { chapterId = _chapterId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)25);
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

