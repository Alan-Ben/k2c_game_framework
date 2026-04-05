package NP2HS_RB.p002_HsClientOp;

import java.nio.ByteBuffer;
public class NP2HS_RB_002_001_RetExecGmSuper implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<NP2HS_RB.p002_HsClientOp.ExeGmSuperResult> resultList;


public NP2HS_RB_002_001_RetExecGmSuper() {
	resultList = new java.util.ArrayList<NP2HS_RB.p002_HsClientOp.ExeGmSuperResult>();
}

public NP2HS_RB_002_001_RetExecGmSuper(
	 java.util.ArrayList<NP2HS_RB.p002_HsClientOp.ExeGmSuperResult> _resultList
) {	resultList = _resultList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public java.util.ArrayList<NP2HS_RB.p002_HsClientOp.ExeGmSuperResult> getResultList() { return resultList; }
public void addResultList(NP2HS_RB.p002_HsClientOp.ExeGmSuperResult _resultList) { resultList.add(_resultList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += 4 + resultList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += 4 + resultList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _resultListCount = _buf.getShort();
	for(int _i = 0; _i < _resultListCount; _i++) { 
		NP2HS_RB.p002_HsClientOp.ExeGmSuperResult _resultList = new NP2HS_RB.p002_HsClientOp.ExeGmSuperResult();
		if(_buf.remaining() <= 0) return;
	int __resultListCustLen = _buf.getInt();
	int __resultListCurPos = _buf.position();
	_resultList.ReadUnzipBuf(_buf, __resultListCurPos + __resultListCustLen);
	_buf.position(__resultListCurPos + __resultListCustLen);

		resultList.add(_resultList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)resultList.size());
	for(int _i = 0; _i < resultList.size(); _i++) { 
		_buf.putInt(resultList.get(_i).GetBufSize());
	resultList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

