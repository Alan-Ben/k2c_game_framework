package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_004_RetOtherGuildInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo showInfo;
private java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo> memberList;


public GS2GC_032_004_RetOtherGuildInfo() {
	showInfo = new Common.GuildObj.Guild_ShowInfo();
	memberList = new java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo>();
}

public GS2GC_032_004_RetOtherGuildInfo(
	 Common.GuildObj.Guild_ShowInfo _showInfo
	, java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo> _memberList
) {	showInfo = _showInfo;
	memberList = _memberList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)4; }

public Common.GuildObj.Guild_ShowInfo getShowInfo() { return showInfo; }
public void setShowInfo(Common.GuildObj.Guild_ShowInfo _showInfo) { showInfo = _showInfo; }
public java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo> getMemberList() { return memberList; }
public void addMemberList(Common.GuildObj.Guild_MemberBaseInfo _memberList) { memberList.add(_memberList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + showInfo.GetBufSize();
	_size += 2 + (memberList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + showInfo.GetBufSize();
	_size += 2 + (memberList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.position();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.position(_showInfoCurPos + _showInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.GuildObj.Guild_MemberBaseInfo _memberList = new Common.GuildObj.Guild_MemberBaseInfo();
		if(_buf.remaining() <= 0) return;
	int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.position();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.position(__memberListCurPos + __memberListCustLen);

		memberList.add(_memberList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)memberList.size());
	for(int _i = 0; _i < memberList.size(); _i++) { 
		_buf.putInt(memberList.get(_i).GetBufSize());
	memberList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)4);
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

