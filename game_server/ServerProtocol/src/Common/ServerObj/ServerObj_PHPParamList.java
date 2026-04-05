package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 平台参数列表
 **/
public class ServerObj_PHPParamList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ServerObj.ServerObj_PHPParam> pList;


public ServerObj_PHPParamList() {
	pList = new java.util.ArrayList<Common.ServerObj.ServerObj_PHPParam>();
}

public ServerObj_PHPParamList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_PHPParam> _pList
) {	pList = _pList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.ServerObj.ServerObj_PHPParam> getPList() { return pList; }
public void addPList(Common.ServerObj.ServerObj_PHPParam _pList) { pList.add(_pList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < pList.size(); _i++) {
	_size += 4 + pList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < pList.size(); _i++) {
	_size += 4 + pList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _pListCount = _buf.getShort();
	for(int _i = 0; _i < _pListCount; _i++) { 
		Common.ServerObj.ServerObj_PHPParam _pList = new Common.ServerObj.ServerObj_PHPParam();
		if(_buf.remaining() <= 0) return;
	int __pListCustLen = _buf.getInt();
	int __pListCurPos = _buf.position();
	_pList.ReadUnzipBuf(_buf, __pListCurPos + __pListCustLen);
	_buf.position(__pListCurPos + __pListCustLen);

		pList.add(_pList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)pList.size());
	for(int _i = 0; _i < pList.size(); _i++) { 
		_buf.putInt(pList.get(_i).GetBufSize());
	pList.get(_i).PutUnzipBuf(_buf);
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

