package GC2GS.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 处理选项事件
 **/
public class GC2GS_016_005_ReqDealChapterChoiceEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 选项ID */
private long optionId;


public GC2GS_016_005_ReqDealChapterChoiceEvent() {
	optionId = (long)0;
}

public GC2GS_016_005_ReqDealChapterChoiceEvent(
	 long _optionId
) {	optionId = _optionId;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)5; }

/** 选项ID */
public long getOptionId() { return optionId; }
/** 选项ID */
public void setOptionId(long _optionId) { optionId = _optionId; }


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
	if(_buf.remaining() > 0) optionId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(optionId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
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

