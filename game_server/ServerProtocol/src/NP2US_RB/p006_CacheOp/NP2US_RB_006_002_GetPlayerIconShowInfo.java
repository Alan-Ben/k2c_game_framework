package NP2US_RB.p006_CacheOp;

import java.nio.ByteBuffer;
public class NP2US_RB_006_002_GetPlayerIconShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.PlayerInfo_IconShow showInfo;


public NP2US_RB_006_002_GetPlayerIconShowInfo() {
	showInfo = new NPCommon.PlayerInfo_IconShow();
}

public NP2US_RB_006_002_GetPlayerIconShowInfo(
	 NPCommon.PlayerInfo_IconShow _showInfo
) {	showInfo = _showInfo;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)2; }

public NPCommon.PlayerInfo_IconShow getShowInfo() { return showInfo; }
public void setShowInfo(NPCommon.PlayerInfo_IconShow _showInfo) { showInfo = _showInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.position();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.position(_showInfoCurPos + _showInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)2);
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

