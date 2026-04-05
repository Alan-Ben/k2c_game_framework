package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 请求章节列表
 **/
public class GC2GS_023_023_ReqTowerChapterList implements ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
/** 开始查询的位置 */
private int level;
private int num;


public GC2GS_023_023_ReqTowerChapterList() {
	chapterId = (long)0;
	level = 0;
	num = 0;
}

public GC2GS_023_023_ReqTowerChapterList(
	 long _chapterId
	, int _level
	, int _num
) {	chapterId = _chapterId;
	level = _level;
	num = _num;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)23; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
/** 开始查询的位置 */
public int getLevel() { return level; }
/** 开始查询的位置 */
public void setLevel(int _level) { level = _level; }
public int getNum() { return num; }
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(level);
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
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

