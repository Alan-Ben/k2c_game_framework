package Common.ChapterObj;

import java.nio.ByteBuffer;
/*********
 * 关卡单一鼓舞信息
 **/
public class Chapter_SingleInspireInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 鼓舞类型 */
private Common.ChapterEnum.EChapterInspireType type;
/** 鼓舞次数 */
private int inspireTimes;


public Chapter_SingleInspireInfo() {
	type = Common.ChapterEnum.EChapterInspireType.values()[0];
	inspireTimes = 0;
}

public Chapter_SingleInspireInfo(
	 Common.ChapterEnum.EChapterInspireType _type
	, int _inspireTimes
) {	type = _type;
	inspireTimes = _inspireTimes;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 鼓舞类型 */
public Common.ChapterEnum.EChapterInspireType getType() { return type; }
/** 鼓舞类型 */
public void setType(Common.ChapterEnum.EChapterInspireType _type) { type = _type; }
/** 鼓舞次数 */
public int getInspireTimes() { return inspireTimes; }
/** 鼓舞次数 */
public void setInspireTimes(int _inspireTimes) { inspireTimes = _inspireTimes; }


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
	if(_buf.remaining() > 0) type = Common.ChapterEnum.EChapterInspireType.EChapterInspireType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) inspireTimes = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt(inspireTimes);
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

