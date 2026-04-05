package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
public class GS2GC_041_014_RetGetCollectPVPLogIdxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 索引数据列表 */
private java.util.ArrayList<Common.MarsObj.Mars_ExplorePVPLogIdx> idxList;


public GS2GC_041_014_RetGetCollectPVPLogIdxList() {
	idxList = new java.util.ArrayList<Common.MarsObj.Mars_ExplorePVPLogIdx>();
}

public GS2GC_041_014_RetGetCollectPVPLogIdxList(
	 java.util.ArrayList<Common.MarsObj.Mars_ExplorePVPLogIdx> _idxList
) {	idxList = _idxList;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)14; }

/** 索引数据列表 */
public java.util.ArrayList<Common.MarsObj.Mars_ExplorePVPLogIdx> getIdxList() { return idxList; }
/** 索引数据列表 */
public void addIdxList(Common.MarsObj.Mars_ExplorePVPLogIdx _idxList) { idxList.add(_idxList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < idxList.size(); _i++) {
	_size += 4 + idxList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < idxList.size(); _i++) {
	_size += 4 + idxList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _idxListCount = _buf.getShort();
	for(int _i = 0; _i < _idxListCount; _i++) { 
		Common.MarsObj.Mars_ExplorePVPLogIdx _idxList = new Common.MarsObj.Mars_ExplorePVPLogIdx();
		if(_buf.remaining() <= 0) return;
	int __idxListCustLen = _buf.getInt();
	int __idxListCurPos = _buf.position();
	_idxList.ReadUnzipBuf(_buf, __idxListCurPos + __idxListCustLen);
	_buf.position(__idxListCurPos + __idxListCustLen);

		idxList.add(_idxList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)idxList.size());
	for(int _i = 0; _i < idxList.size(); _i++) { 
		_buf.putInt(idxList.get(_i).GetBufSize());
	idxList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)14);
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

