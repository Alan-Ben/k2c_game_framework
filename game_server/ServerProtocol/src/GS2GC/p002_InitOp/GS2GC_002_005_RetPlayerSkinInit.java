package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_005_RetPlayerSkinInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Skin> skinList;


public GS2GC_002_005_RetPlayerSkinInit() {
	skinList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Skin>();
}

public GS2GC_002_005_RetPlayerSkinInit(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Skin> _skinList
) {	skinList = _skinList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)5; }

public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Skin> getSkinList() { return skinList; }
public void addSkinList(Common.NpPlayerInfoObj.PlayerInfo_Skin _skinList) { skinList.add(_skinList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (skinList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (skinList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _skinListCount = _buf.getShort();
	for(int _i = 0; _i < _skinListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Skin _skinList = new Common.NpPlayerInfoObj.PlayerInfo_Skin();
		if(_buf.remaining() <= 0) return;
	int __skinListCustLen = _buf.getInt();
	int __skinListCurPos = _buf.position();
	_skinList.ReadUnzipBuf(_buf, __skinListCurPos + __skinListCustLen);
	_buf.position(__skinListCurPos + __skinListCustLen);

		skinList.add(_skinList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)skinList.size());
	for(int _i = 0; _i < skinList.size(); _i++) { 
		_buf.putInt(skinList.get(_i).GetBufSize());
	skinList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)5);
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

