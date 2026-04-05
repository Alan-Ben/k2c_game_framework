package ALLRPC.US.Mars;

import java.nio.ByteBuffer;
public class MarsMineOccupyResult_Req implements ALBasicProtocolPack._IALProtocolStructure {
private Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult result;


public MarsMineOccupyResult_Req() {
	result = new Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult();
}

public MarsMineOccupyResult_Req(
	 Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult _result
) {	result = _result;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult getResult() { return result; }
public void setResult(Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult _result) { result = _result; }


public final int GetBufSize() {
	int _size = 61;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 63;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _resultCustLen = _buf.getInt();
	int _resultCurPos = _buf.position();
	result.ReadUnzipBuf(_buf, _resultCurPos + _resultCustLen);
	_buf.position(_resultCurPos + _resultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(result.GetBufSize());
	result.PutUnzipBuf(_buf);
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

