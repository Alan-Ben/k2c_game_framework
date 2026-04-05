package GC2GS.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 打Boss鼓舞
 **/
public class GC2GS_016_003_ReqChapterFightBossInspire implements ALBasicProtocolPack._IALProtocolStructure {
/** 鼓舞类型 */
private Common.ChapterEnum.EChapterInspireType type;


public GC2GS_016_003_ReqChapterFightBossInspire() {
	type = Common.ChapterEnum.EChapterInspireType.values()[0];
}

public GC2GS_016_003_ReqChapterFightBossInspire(
	 Common.ChapterEnum.EChapterInspireType _type
) {	type = _type;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)3; }

/** 鼓舞类型 */
public Common.ChapterEnum.EChapterInspireType getType() { return type; }
/** 鼓舞类型 */
public void setType(Common.ChapterEnum.EChapterInspireType _type) { type = _type; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.ChapterEnum.EChapterInspireType.EChapterInspireType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)3);
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

