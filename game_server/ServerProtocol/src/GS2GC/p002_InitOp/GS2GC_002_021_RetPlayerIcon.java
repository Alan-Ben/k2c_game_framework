package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_021_RetPlayerIcon implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家拥有的头像信息队列 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Icon> iconInfoList;


public GS2GC_002_021_RetPlayerIcon() {
	iconInfoList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Icon>();
}

public GS2GC_002_021_RetPlayerIcon(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Icon> _iconInfoList
) {	iconInfoList = _iconInfoList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)21; }

/** 玩家拥有的头像信息队列 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Icon> getIconInfoList() { return iconInfoList; }
/** 玩家拥有的头像信息队列 */
public void addIconInfoList(Common.NpPlayerInfoObj.PlayerInfo_Icon _iconInfoList) { iconInfoList.add(_iconInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (iconInfoList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (iconInfoList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _iconInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _iconInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Icon _iconInfoList = new Common.NpPlayerInfoObj.PlayerInfo_Icon();
		if(_buf.remaining() <= 0) return;
	int __iconInfoListCustLen = _buf.getInt();
	int __iconInfoListCurPos = _buf.position();
	_iconInfoList.ReadUnzipBuf(_buf, __iconInfoListCurPos + __iconInfoListCustLen);
	_buf.position(__iconInfoListCurPos + __iconInfoListCustLen);

		iconInfoList.add(_iconInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)iconInfoList.size());
	for(int _i = 0; _i < iconInfoList.size(); _i++) { 
		_buf.putInt(iconInfoList.get(_i).GetBufSize());
	iconInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)21);
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

