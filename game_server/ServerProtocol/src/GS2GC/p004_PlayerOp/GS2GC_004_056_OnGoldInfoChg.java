package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_056_OnGoldInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 金币信息 */
private Common.PlayerObj.Player_GoldInfo goldInfo;


public GS2GC_004_056_OnGoldInfoChg() {
	goldInfo = new Common.PlayerObj.Player_GoldInfo();
}

public GS2GC_004_056_OnGoldInfoChg(
	 Common.PlayerObj.Player_GoldInfo _goldInfo
) {	goldInfo = _goldInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)56; }

/** 金币信息 */
public Common.PlayerObj.Player_GoldInfo getGoldInfo() { return goldInfo; }
/** 金币信息 */
public void setGoldInfo(Common.PlayerObj.Player_GoldInfo _goldInfo) { goldInfo = _goldInfo; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _goldInfoCustLen = _buf.getInt();
	int _goldInfoCurPos = _buf.position();
	goldInfo.ReadUnzipBuf(_buf, _goldInfoCurPos + _goldInfoCustLen);
	_buf.position(_goldInfoCurPos + _goldInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(goldInfo.GetBufSize());
	goldInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)56);
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

