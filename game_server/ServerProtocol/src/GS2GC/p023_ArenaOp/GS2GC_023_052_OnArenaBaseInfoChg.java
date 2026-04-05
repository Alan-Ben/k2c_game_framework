package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_052_OnArenaBaseInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础数据 */
private Common.ArenaObj.Arena_BaseInfo baseInfo;


public GS2GC_023_052_OnArenaBaseInfoChg() {
	baseInfo = new Common.ArenaObj.Arena_BaseInfo();
}

public GS2GC_023_052_OnArenaBaseInfoChg(
	 Common.ArenaObj.Arena_BaseInfo _baseInfo
) {	baseInfo = _baseInfo;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)52; }

/** 基础数据 */
public Common.ArenaObj.Arena_BaseInfo getBaseInfo() { return baseInfo; }
/** 基础数据 */
public void setBaseInfo(Common.ArenaObj.Arena_BaseInfo _baseInfo) { baseInfo = _baseInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + baseInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + baseInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.position();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.position(_baseInfoCurPos + _baseInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
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

