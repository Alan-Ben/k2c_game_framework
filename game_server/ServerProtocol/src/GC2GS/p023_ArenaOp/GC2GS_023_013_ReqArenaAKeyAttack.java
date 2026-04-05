package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 一键攻击
 **/
public class GC2GS_023_013_ReqArenaAKeyAttack implements ALBasicProtocolPack._IALProtocolStructure {
private Common.ArenaEnum.EArenaBuffType buffType;


public GC2GS_023_013_ReqArenaAKeyAttack() {
	buffType = Common.ArenaEnum.EArenaBuffType.values()[0];
}

public GC2GS_023_013_ReqArenaAKeyAttack(
	 Common.ArenaEnum.EArenaBuffType _buffType
) {	buffType = _buffType;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)13; }

public Common.ArenaEnum.EArenaBuffType getBuffType() { return buffType; }
public void setBuffType(Common.ArenaEnum.EArenaBuffType _buffType) { buffType = _buffType; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buffType = Common.ArenaEnum.EArenaBuffType.EArenaBuffType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(buffType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)13);
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

