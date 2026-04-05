package GS2GC.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 组合称号底色变更
 **/
public class GS2GC_018_053_OnComboTitleBgChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg bg;


public GS2GC_018_053_OnComboTitleBgChg() {
	bg = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg();
}

public GS2GC_018_053_OnComboTitleBgChg(
	 Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _bg
) {	bg = _bg;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)53; }

/** 空 */
public Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg getBg() { return bg; }
/** 空 */
public void setBg(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _bg) { bg = _bg; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _bgCustLen = _buf.getInt();
	int _bgCurPos = _buf.position();
	bg.ReadUnzipBuf(_buf, _bgCurPos + _bgCustLen);
	_buf.position(_bgCurPos + _bgCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(bg.GetBufSize());
	bg.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)53);
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

