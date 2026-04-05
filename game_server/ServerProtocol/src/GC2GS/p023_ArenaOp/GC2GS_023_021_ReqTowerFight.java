package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 爬塔攻击
 **/
public class GC2GS_023_021_ReqTowerFight implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标位置信息 */
private Common.TowerObj.Tower_PosInfo targetPos;


public GC2GS_023_021_ReqTowerFight() {
	targetPos = new Common.TowerObj.Tower_PosInfo();
}

public GC2GS_023_021_ReqTowerFight(
	 Common.TowerObj.Tower_PosInfo _targetPos
) {	targetPos = _targetPos;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)21; }

/** 目标位置信息 */
public Common.TowerObj.Tower_PosInfo getTargetPos() { return targetPos; }
/** 目标位置信息 */
public void setTargetPos(Common.TowerObj.Tower_PosInfo _targetPos) { targetPos = _targetPos; }


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
	int _targetPosCustLen = _buf.getInt();
	int _targetPosCurPos = _buf.position();
	targetPos.ReadUnzipBuf(_buf, _targetPosCurPos + _targetPosCustLen);
	_buf.position(_targetPosCurPos + _targetPosCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(targetPos.GetBufSize());
	targetPos.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)21);
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

