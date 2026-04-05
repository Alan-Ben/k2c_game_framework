package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_069_RetForeverAddInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<NPCommon.NPCommon_ForeverAddInfo> addList;


public GS2GC_002_069_RetForeverAddInit() {
	addList = new java.util.ArrayList<NPCommon.NPCommon_ForeverAddInfo>();
}

public GS2GC_002_069_RetForeverAddInit(
	 java.util.ArrayList<NPCommon.NPCommon_ForeverAddInfo> _addList
) {	addList = _addList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)69; }

public java.util.ArrayList<NPCommon.NPCommon_ForeverAddInfo> getAddList() { return addList; }
public void addAddList(NPCommon.NPCommon_ForeverAddInfo _addList) { addList.add(_addList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (addList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (addList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _addListCount = _buf.getShort();
	for(int _i = 0; _i < _addListCount; _i++) { 
		NPCommon.NPCommon_ForeverAddInfo _addList = new NPCommon.NPCommon_ForeverAddInfo();
		if(_buf.remaining() <= 0) return;
	int __addListCustLen = _buf.getInt();
	int __addListCurPos = _buf.position();
	_addList.ReadUnzipBuf(_buf, __addListCurPos + __addListCustLen);
	_buf.position(__addListCurPos + __addListCustLen);

		addList.add(_addList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)addList.size());
	for(int _i = 0; _i < addList.size(); _i++) { 
		_buf.putInt(addList.get(_i).GetBufSize());
	addList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)69);
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

