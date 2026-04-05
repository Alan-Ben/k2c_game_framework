package GS2GC.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 普通称号数据变更
 **/
public class GS2GC_018_050_OnCommTitleChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.NpPlayerInfoObj.PlayerInfo_Title info;


public GS2GC_018_050_OnCommTitleChg() {
	info = new Common.NpPlayerInfoObj.PlayerInfo_Title();
}

public GS2GC_018_050_OnCommTitleChg(
	 Common.NpPlayerInfoObj.PlayerInfo_Title _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)50; }

/** 空 */
public Common.NpPlayerInfoObj.PlayerInfo_Title getInfo() { return info; }
/** 空 */
public void setInfo(Common.NpPlayerInfoObj.PlayerInfo_Title _info) { info = _info; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)50);
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

