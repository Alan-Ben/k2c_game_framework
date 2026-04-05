package GS2GC.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 组合称号后缀变更
 **/
public class GS2GC_018_052_OnComboTitleSfxChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx sfx;


public GS2GC_018_052_OnComboTitleSfxChg() {
	sfx = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx();
}

public GS2GC_018_052_OnComboTitleSfxChg(
	 Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _sfx
) {	sfx = _sfx;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)52; }

/** 空 */
public Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx getSfx() { return sfx; }
/** 空 */
public void setSfx(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _sfx) { sfx = _sfx; }


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
	int _sfxCustLen = _buf.getInt();
	int _sfxCurPos = _buf.position();
	sfx.ReadUnzipBuf(_buf, _sfxCurPos + _sfxCustLen);
	_buf.position(_sfxCurPos + _sfxCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(sfx.GetBufSize());
	sfx.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
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

