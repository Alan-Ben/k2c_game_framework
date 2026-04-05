package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_019_RetMemberContribute implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_MemberContributeInfo member;


public GS2GC_032_019_RetMemberContribute() {
	member = new Common.GuildObj.Guild_MemberContributeInfo();
}

public GS2GC_032_019_RetMemberContribute(
	 Common.GuildObj.Guild_MemberContributeInfo _member
) {	member = _member;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)19; }

public Common.GuildObj.Guild_MemberContributeInfo getMember() { return member; }
public void setMember(Common.GuildObj.Guild_MemberContributeInfo _member) { member = _member; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _memberCustLen = _buf.getInt();
	int _memberCurPos = _buf.position();
	member.ReadUnzipBuf(_buf, _memberCurPos + _memberCustLen);
	_buf.position(_memberCurPos + _memberCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(member.GetBufSize());
	member.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)19);
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

