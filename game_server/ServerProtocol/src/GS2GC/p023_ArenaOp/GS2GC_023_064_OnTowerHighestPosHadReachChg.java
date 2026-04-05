package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_064_OnTowerHighestPosHadReachChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 最高位置信息 */
private Common.TowerObj.Tower_PosInfo pos;


public GS2GC_023_064_OnTowerHighestPosHadReachChg() {
	pos = new Common.TowerObj.Tower_PosInfo();
}

public GS2GC_023_064_OnTowerHighestPosHadReachChg(
	 Common.TowerObj.Tower_PosInfo _pos
) {	pos = _pos;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)64; }

/** 最高位置信息 */
public Common.TowerObj.Tower_PosInfo getPos() { return pos; }
/** 最高位置信息 */
public void setPos(Common.TowerObj.Tower_PosInfo _pos) { pos = _pos; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)64);
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

