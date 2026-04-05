package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索-矿产数据增加
 **/
public class GS2GC_041_059_OnMineAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 索引数据 */
private Common.MarsObj.Mars_MineIdx idx;


public GS2GC_041_059_OnMineAdd() {
	idx = new Common.MarsObj.Mars_MineIdx();
}

public GS2GC_041_059_OnMineAdd(
	 Common.MarsObj.Mars_MineIdx _idx
) {	idx = _idx;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)59; }

/** 索引数据 */
public Common.MarsObj.Mars_MineIdx getIdx() { return idx; }
/** 索引数据 */
public void setIdx(Common.MarsObj.Mars_MineIdx _idx) { idx = _idx; }


public final int GetBufSize() {
	int _size = 44;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _idxCustLen = _buf.getInt();
	int _idxCurPos = _buf.position();
	idx.ReadUnzipBuf(_buf, _idxCurPos + _idxCustLen);
	_buf.position(_idxCurPos + _idxCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(idx.GetBufSize());
	idx.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)59);
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

