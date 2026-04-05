package ALLRPC.US.Grave;

import java.nio.ByteBuffer;
public class AddGraveNewInfo_Req implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.GraveObj.GraveObj_NewInfo> newInfoList;


public AddGraveNewInfo_Req() {
	newInfoList = new java.util.ArrayList<Common.GraveObj.GraveObj_NewInfo>();
}

public AddGraveNewInfo_Req(
	 java.util.ArrayList<Common.GraveObj.GraveObj_NewInfo> _newInfoList
) {	newInfoList = _newInfoList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.GraveObj.GraveObj_NewInfo> getNewInfoList() { return newInfoList; }
public void addNewInfoList(Common.GraveObj.GraveObj_NewInfo _newInfoList) { newInfoList.add(_newInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (newInfoList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (newInfoList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _newInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _newInfoListCount; _i++) { 
		Common.GraveObj.GraveObj_NewInfo _newInfoList = new Common.GraveObj.GraveObj_NewInfo();
		if(_buf.remaining() <= 0) return;
	int __newInfoListCustLen = _buf.getInt();
	int __newInfoListCurPos = _buf.position();
	_newInfoList.ReadUnzipBuf(_buf, __newInfoListCurPos + __newInfoListCustLen);
	_buf.position(__newInfoListCurPos + __newInfoListCustLen);

		newInfoList.add(_newInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)newInfoList.size());
	for(int _i = 0; _i < newInfoList.size(); _i++) { 
		_buf.putInt(newInfoList.get(_i).GetBufSize());
	newInfoList.get(_i).PutUnzipBuf(_buf);
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

