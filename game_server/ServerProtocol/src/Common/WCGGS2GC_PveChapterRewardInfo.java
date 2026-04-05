package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_PveChapterRewardInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
private int takenRewardIndex;


public WCGGS2GC_PveChapterRewardInfo() {
	chapterId = (long)0;
	takenRewardIndex = 0;
}

public WCGGS2GC_PveChapterRewardInfo(
	 long _chapterId
	, int _takenRewardIndex
) {	chapterId = _chapterId;
	takenRewardIndex = _takenRewardIndex;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
public int getTakenRewardIndex() { return takenRewardIndex; }
public void setTakenRewardIndex(int _takenRewardIndex) { takenRewardIndex = _takenRewardIndex; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) takenRewardIndex = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(takenRewardIndex);
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

