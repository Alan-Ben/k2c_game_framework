package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_022_RetPlayerIconBgk implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家拥有的头像框信息队列 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> iconBgkInfoList;


public GS2GC_002_022_RetPlayerIconBgk() {
	iconBgkInfoList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_IconBgk>();
}

public GS2GC_002_022_RetPlayerIconBgk(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> _iconBgkInfoList
) {	iconBgkInfoList = _iconBgkInfoList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)22; }

/** 玩家拥有的头像框信息队列 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> getIconBgkInfoList() { return iconBgkInfoList; }
/** 玩家拥有的头像框信息队列 */
public void addIconBgkInfoList(Common.NpPlayerInfoObj.PlayerInfo_IconBgk _iconBgkInfoList) { iconBgkInfoList.add(_iconBgkInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (iconBgkInfoList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (iconBgkInfoList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _iconBgkInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _iconBgkInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_IconBgk _iconBgkInfoList = new Common.NpPlayerInfoObj.PlayerInfo_IconBgk();
		if(_buf.remaining() <= 0) return;
	int __iconBgkInfoListCustLen = _buf.getInt();
	int __iconBgkInfoListCurPos = _buf.position();
	_iconBgkInfoList.ReadUnzipBuf(_buf, __iconBgkInfoListCurPos + __iconBgkInfoListCustLen);
	_buf.position(__iconBgkInfoListCurPos + __iconBgkInfoListCustLen);

		iconBgkInfoList.add(_iconBgkInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)iconBgkInfoList.size());
	for(int _i = 0; _i < iconBgkInfoList.size(); _i++) { 
		_buf.putInt(iconBgkInfoList.get(_i).GetBufSize());
	iconBgkInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)22);
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

