package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_035_RetGuildAllMemberEntrustInfo implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.GuildObj.Guild_MemberEntrustInfo> infoList;


public GS2GC_032_035_RetGuildAllMemberEntrustInfo() {
	infoList = new java.util.ArrayList<Common.GuildObj.Guild_MemberEntrustInfo>();
}

public GS2GC_032_035_RetGuildAllMemberEntrustInfo(
	 java.util.ArrayList<Common.GuildObj.Guild_MemberEntrustInfo> _infoList
) {	infoList = _infoList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)35; }

public java.util.ArrayList<Common.GuildObj.Guild_MemberEntrustInfo> getInfoList() { return infoList; }
public void addInfoList(Common.GuildObj.Guild_MemberEntrustInfo _infoList) { infoList.add(_infoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (infoList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (infoList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.GuildObj.Guild_MemberEntrustInfo _infoList = new Common.GuildObj.Guild_MemberEntrustInfo();
		if(_buf.remaining() <= 0) return;
	int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.position();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.position(__infoListCurPos + __infoListCustLen);

		infoList.add(_infoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)infoList.size());
	for(int _i = 0; _i < infoList.size(); _i++) { 
		_buf.putInt(infoList.get(_i).GetBufSize());
	infoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)35);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)35);
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

