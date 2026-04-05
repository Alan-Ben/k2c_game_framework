package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 永久加成变更
 **/
public class GS2GC_021_075_OnForeverAddChg implements ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_ForeverAddInfo add;


public GS2GC_021_075_OnForeverAddChg() {
	add = new NPCommon.NPCommon_ForeverAddInfo();
}

public GS2GC_021_075_OnForeverAddChg(
	 NPCommon.NPCommon_ForeverAddInfo _add
) {	add = _add;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)75; }

public NPCommon.NPCommon_ForeverAddInfo getAdd() { return add; }
public void setAdd(NPCommon.NPCommon_ForeverAddInfo _add) { add = _add; }


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
	int _addCustLen = _buf.getInt();
	int _addCurPos = _buf.position();
	add.ReadUnzipBuf(_buf, _addCurPos + _addCustLen);
	_buf.position(_addCurPos + _addCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(add.GetBufSize());
	add.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)75);
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

