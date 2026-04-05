package ALLRPC.Common;

import java.nio.ByteBuffer;
public class AllExecGmCommand_Return implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isSucc;
private java.util.ArrayList<String> resultList;


public AllExecGmCommand_Return() {
	isSucc = false;
	resultList = new java.util.ArrayList<String>();
}

public AllExecGmCommand_Return(
	 boolean _isSucc
	, java.util.ArrayList<String> _resultList
) {	isSucc = _isSucc;
	resultList = _resultList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public boolean getIsSucc() { return isSucc; }
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }
public java.util.ArrayList<String> getResultList() { return resultList; }
public void addResultList(String _resultList) { resultList.add(_resultList); }


public final int GetBufSize() {
	int _size = 1;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resultList.get(_i));
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resultList.get(_i));
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _resultListCount = _buf.getShort();
	for(int _i = 0; _i < _resultListCount; _i++) { 
		String _resultList = "";
		if(_buf.remaining() > 0) _resultList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		resultList.add(_resultList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putShort((short)resultList.size());
	for(int _i = 0; _i < resultList.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resultList.get(_i));
	}
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

