package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_002_RetGmCommand implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isSucc;
private String result;
private byte[] zipedResult;


public GS2GC_004_002_RetGmCommand() {
	isSucc = false;
	result = "";
	zipedResult = null;
}

public GS2GC_004_002_RetGmCommand(
	 boolean _isSucc
	, String _result
	, byte[] _zipedResult
) {	isSucc = _isSucc;
	result = _result;
	zipedResult = _zipedResult;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)2; }

public boolean getIsSucc() { return isSucc; }
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }
public String getResult() { return result; }
public void setResult(String _result) { result = _result; }
public byte[] getZipedResult() { return zipedResult; }
public java.nio.ByteBuffer get_buffer_ZipedResult() { if(null == zipedResult)return null; else return ByteBuffer.wrap(zipedResult); }

public void setZipedResult(byte[] _zipedResult) { zipedResult = _zipedResult; }
public void setZipedResult(java.nio.ByteBuffer _zipedResult) 
{
	if(null == _zipedResult){return;}
	int _oldPos = _zipedResult.position();
	int _bufLength = _zipedResult.remaining();
	zipedResult = new byte[_bufLength];
	_zipedResult.get(zipedResult);
	_zipedResult.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 1;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result);
	_size += 4 + (zipedResult == null ? 0 : zipedResult.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result);
	_size += 4 + (zipedResult == null ? 0 : zipedResult.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) result = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _zipedResultCount = _buf.getInt();
	if(0 < _zipedResultCount){
		zipedResult = new byte[_zipedResultCount];
		_buf.get(zipedResult);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, result);
	_buf.putInt((zipedResult == null ? 0 : zipedResult.length));
	if(null != zipedResult){_buf.put(zipedResult);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)2);
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

