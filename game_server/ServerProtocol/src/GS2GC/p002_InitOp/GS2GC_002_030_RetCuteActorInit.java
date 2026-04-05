package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_030_RetCuteActorInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_CuteActor> cuteActorList;


public GS2GC_002_030_RetCuteActorInit() {
	cuteActorList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_CuteActor>();
}

public GS2GC_002_030_RetCuteActorInit(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_CuteActor> _cuteActorList
) {	cuteActorList = _cuteActorList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)30; }

public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_CuteActor> getCuteActorList() { return cuteActorList; }
public void addCuteActorList(Common.NpPlayerInfoObj.PlayerInfo_CuteActor _cuteActorList) { cuteActorList.add(_cuteActorList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cuteActorList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cuteActorList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cuteActorListCount = _buf.getShort();
	for(int _i = 0; _i < _cuteActorListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_CuteActor _cuteActorList = new Common.NpPlayerInfoObj.PlayerInfo_CuteActor();
		if(_buf.remaining() <= 0) return;
	int __cuteActorListCustLen = _buf.getInt();
	int __cuteActorListCurPos = _buf.position();
	_cuteActorList.ReadUnzipBuf(_buf, __cuteActorListCurPos + __cuteActorListCustLen);
	_buf.position(__cuteActorListCurPos + __cuteActorListCustLen);

		cuteActorList.add(_cuteActorList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cuteActorList.size());
	for(int _i = 0; _i < cuteActorList.size(); _i++) { 
		_buf.putInt(cuteActorList.get(_i).GetBufSize());
	cuteActorList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)30);
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

