package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 获取联盟内分享矿信息
 **/
public class GuildOp_041_018_RetOccupyMineInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long posId;
/** 具体矿的信息 */
private Common.ServerObj.ServerObj_MarsMine mineInfo;


public GuildOp_041_018_RetOccupyMineInfo() {
	posId = (long)0;
	mineInfo = new Common.ServerObj.ServerObj_MarsMine();
}

public GuildOp_041_018_RetOccupyMineInfo(
	 long _posId
	, Common.ServerObj.ServerObj_MarsMine _mineInfo
) {	posId = _posId;
	mineInfo = _mineInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getPosId() { return posId; }
public void setPosId(long _posId) { posId = _posId; }
/** 具体矿的信息 */
public Common.ServerObj.ServerObj_MarsMine getMineInfo() { return mineInfo; }
/** 具体矿的信息 */
public void setMineInfo(Common.ServerObj.ServerObj_MarsMine _mineInfo) { mineInfo = _mineInfo; }


public final int GetBufSize() {
	int _size = 108;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 110;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _mineInfoCustLen = _buf.getInt();
	int _mineInfoCurPos = _buf.position();
	mineInfo.ReadUnzipBuf(_buf, _mineInfoCurPos + _mineInfoCustLen);
	_buf.position(_mineInfoCurPos + _mineInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(posId);
	_buf.putInt(mineInfo.GetBufSize());
	mineInfo.PutUnzipBuf(_buf);
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

