package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 检查宴会邀请信息
 **/
public class GS2GC_019_014_RetCheckDinnerInvite implements ALBasicProtocolPack._IALProtocolStructure {
/** 参与宴会玩家数量 */
private int joinerCount;


public GS2GC_019_014_RetCheckDinnerInvite() {
	joinerCount = 0;
}

public GS2GC_019_014_RetCheckDinnerInvite(
	 int _joinerCount
) {	joinerCount = _joinerCount;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)14; }

/** 参与宴会玩家数量 */
public int getJoinerCount() { return joinerCount; }
/** 参与宴会玩家数量 */
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }


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
	if(_buf.remaining() > 0) joinerCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(joinerCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)14);
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

