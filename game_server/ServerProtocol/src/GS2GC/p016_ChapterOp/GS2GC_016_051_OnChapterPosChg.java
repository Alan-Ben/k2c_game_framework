package GS2GC.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 关卡玩家进度变更
 **/
public class GS2GC_016_051_OnChapterPosChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 位置信息 */
private Common.ChapterObj.Chapter_PosInfo posInfo;


public GS2GC_016_051_OnChapterPosChg() {
	posInfo = new Common.ChapterObj.Chapter_PosInfo();
}

public GS2GC_016_051_OnChapterPosChg(
	 Common.ChapterObj.Chapter_PosInfo _posInfo
) {	posInfo = _posInfo;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)51; }

/** 位置信息 */
public Common.ChapterObj.Chapter_PosInfo getPosInfo() { return posInfo; }
/** 位置信息 */
public void setPosInfo(Common.ChapterObj.Chapter_PosInfo _posInfo) { posInfo = _posInfo; }


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
	if(_buf.remaining() <= 0) return;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.position();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.position(_posInfoCurPos + _posInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)51);
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

