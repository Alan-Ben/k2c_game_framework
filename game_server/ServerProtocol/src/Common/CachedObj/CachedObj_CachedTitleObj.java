package Common.CachedObj;

import java.nio.ByteBuffer;
public class CachedObj_CachedTitleObj implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家当前穿戴称号数据 */
private NPCommon.PlayerInfo_CurTitle curInfo;
/** 是否展示称号 */
private boolean isShow;


public CachedObj_CachedTitleObj() {
	curInfo = new NPCommon.PlayerInfo_CurTitle();
	isShow = false;
}

public CachedObj_CachedTitleObj(
	 NPCommon.PlayerInfo_CurTitle _curInfo
	, boolean _isShow
) {	curInfo = _curInfo;
	isShow = _isShow;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家当前穿戴称号数据 */
public NPCommon.PlayerInfo_CurTitle getCurInfo() { return curInfo; }
/** 玩家当前穿戴称号数据 */
public void setCurInfo(NPCommon.PlayerInfo_CurTitle _curInfo) { curInfo = _curInfo; }
/** 是否展示称号 */
public boolean getIsShow() { return isShow; }
/** 是否展示称号 */
public void setIsShow(boolean _isShow) { isShow = _isShow; }


public final int GetBufSize() {
	int _size = 1;
	_size += 4 + curInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
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

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isShow = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(curInfo.GetBufSize());
	curInfo.PutUnzipBuf(_buf);
	_buf.put(isShow?(byte)1:(byte)0);
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

