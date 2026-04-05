package GS2GC.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 关卡鼓舞信息变更
 **/
public class GS2GC_016_052_OnChapterInspireChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 鼓舞信息 */
private Common.ChapterObj.Chapter_InspireInfo inspireInfo;


public GS2GC_016_052_OnChapterInspireChg() {
	inspireInfo = new Common.ChapterObj.Chapter_InspireInfo();
}

public GS2GC_016_052_OnChapterInspireChg(
	 Common.ChapterObj.Chapter_InspireInfo _inspireInfo
) {	inspireInfo = _inspireInfo;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)52; }

/** 鼓舞信息 */
public Common.ChapterObj.Chapter_InspireInfo getInspireInfo() { return inspireInfo; }
/** 鼓舞信息 */
public void setInspireInfo(Common.ChapterObj.Chapter_InspireInfo _inspireInfo) { inspireInfo = _inspireInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + inspireInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + inspireInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _inspireInfoCustLen = _buf.getInt();
	int _inspireInfoCurPos = _buf.position();
	inspireInfo.ReadUnzipBuf(_buf, _inspireInfoCurPos + _inspireInfoCustLen);
	_buf.position(_inspireInfoCurPos + _inspireInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(inspireInfo.GetBufSize());
	inspireInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)52);
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

