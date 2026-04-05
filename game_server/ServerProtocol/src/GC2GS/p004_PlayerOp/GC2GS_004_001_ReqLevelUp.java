package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GC2GS_004_001_ReqLevelUp implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前等级，为了防止多次发送错误，这里用于校验 */
private int curLvl;


public GC2GS_004_001_ReqLevelUp() {
	curLvl = 0;
}

public GC2GS_004_001_ReqLevelUp(
	 int _curLvl
) {	curLvl = _curLvl;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)1; }

/** 当前等级，为了防止多次发送错误，这里用于校验 */
public int getCurLvl() { return curLvl; }
/** 当前等级，为了防止多次发送错误，这里用于校验 */
public void setCurLvl(int _curLvl) { curLvl = _curLvl; }


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
	if(_buf.remaining() > 0) curLvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(curLvl);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)1);
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

