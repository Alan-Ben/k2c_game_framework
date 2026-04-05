package GS2GC.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 组合称号前缀变更
 **/
public class GS2GC_018_051_OnComboTitlePreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre pre;


public GS2GC_018_051_OnComboTitlePreChg() {
	pre = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre();
}

public GS2GC_018_051_OnComboTitlePreChg(
	 Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _pre
) {	pre = _pre;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)51; }

/** 空 */
public Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre getPre() { return pre; }
/** 空 */
public void setPre(Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _pre) { pre = _pre; }


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
	int _preCustLen = _buf.getInt();
	int _preCurPos = _buf.position();
	pre.ReadUnzipBuf(_buf, _preCurPos + _preCustLen);
	_buf.position(_preCurPos + _preCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pre.GetBufSize());
	pre.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
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

