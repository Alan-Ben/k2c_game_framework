package ALLRPC.US.Dinner;

import java.nio.ByteBuffer;
public class UsGetDinnerIdx_Return implements ALBasicProtocolPack._IALProtocolStructure {
private Common.DinnerObj.Dinner_Idx idx;


public UsGetDinnerIdx_Return() {
	idx = new Common.DinnerObj.Dinner_Idx();
}

public UsGetDinnerIdx_Return(
	 Common.DinnerObj.Dinner_Idx _idx
) {	idx = _idx;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.DinnerObj.Dinner_Idx getIdx() { return idx; }
public void setIdx(Common.DinnerObj.Dinner_Idx _idx) { idx = _idx; }


public final int GetBufSize() {
	int _size = 57;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 59;

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

