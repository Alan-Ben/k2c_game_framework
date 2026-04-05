package GS2GC.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 当前穿戴称号变化
 **/
public class GS2GC_018_054_OnCurTitleChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家当前穿戴称号数据 */
private NPCommon.PlayerInfo_CurTitle curInfo;


public GS2GC_018_054_OnCurTitleChg() {
	curInfo = new NPCommon.PlayerInfo_CurTitle();
}

public GS2GC_018_054_OnCurTitleChg(
	 NPCommon.PlayerInfo_CurTitle _curInfo
) {	curInfo = _curInfo;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)54; }

/** 玩家当前穿戴称号数据 */
public NPCommon.PlayerInfo_CurTitle getCurInfo() { return curInfo; }
/** 玩家当前穿戴称号数据 */
public void setCurInfo(NPCommon.PlayerInfo_CurTitle _curInfo) { curInfo = _curInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + curInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + curInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _curInfoCustLen = _buf.getInt();
	int _curInfoCurPos = _buf.position();
	curInfo.ReadUnzipBuf(_buf, _curInfoCurPos + _curInfoCustLen);
	_buf.position(_curInfoCurPos + _curInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(curInfo.GetBufSize());
	curInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)54);
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

