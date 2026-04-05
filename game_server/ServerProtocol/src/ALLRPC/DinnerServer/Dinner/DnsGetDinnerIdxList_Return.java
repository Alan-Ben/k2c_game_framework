package ALLRPC.DinnerServer.Dinner;

import java.nio.ByteBuffer;
public class DnsGetDinnerIdxList_Return implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.DinnerObj.Dinner_Idx> idxList;
private boolean hasNext;


public DnsGetDinnerIdxList_Return() {
	idxList = new java.util.ArrayList<Common.DinnerObj.Dinner_Idx>();
	hasNext = false;
}

public DnsGetDinnerIdxList_Return(
	 java.util.ArrayList<Common.DinnerObj.Dinner_Idx> _idxList
	, boolean _hasNext
) {	idxList = _idxList;
	hasNext = _hasNext;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.DinnerObj.Dinner_Idx> getIdxList() { return idxList; }
public void addIdxList(Common.DinnerObj.Dinner_Idx _idxList) { idxList.add(_idxList); }
public boolean getHasNext() { return hasNext; }
public void setHasNext(boolean _hasNext) { hasNext = _hasNext; }


public final int GetBufSize() {
	int _size = 1;
	_size += 2 + (idxList.size() * 57);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (idxList.size() * 57);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _idxListCount = _buf.getShort();
	for(int _i = 0; _i < _idxListCount; _i++) { 
		Common.DinnerObj.Dinner_Idx _idxList = new Common.DinnerObj.Dinner_Idx();
		if(_buf.remaining() <= 0) return;
	int __idxListCustLen = _buf.getInt();
	int __idxListCurPos = _buf.position();
	_idxList.ReadUnzipBuf(_buf, __idxListCurPos + __idxListCustLen);
	_buf.position(__idxListCurPos + __idxListCustLen);

		idxList.add(_idxList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasNext = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)idxList.size());
	for(int _i = 0; _i < idxList.size(); _i++) { 
		_buf.putInt(idxList.get(_i).GetBufSize());
	idxList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(hasNext?(byte)1:(byte)0);
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

