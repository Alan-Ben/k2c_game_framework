using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_019_RetMemberContribute : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_MemberContributeInfo member;


public GS2GC_032_019_RetMemberContribute() {
	member = new Common.GuildObj.Guild_MemberContributeInfo();
}

public GS2GC_032_019_RetMemberContribute(
	Common.GuildObj.Guild_MemberContributeInfo _member
) {	member = _member;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)19; }

public Common.GuildObj.Guild_MemberContributeInfo getMember() { return member; }
public void setMember(Common.GuildObj.Guild_MemberContributeInfo _member) { member = _member; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _memberCustLen = _buf.getInt();
	int _memberCurPos = _buf.getCurPos();
	member.ReadUnzipBuf(_buf, _memberCurPos + _memberCustLen);
	_buf.setPosition(_memberCurPos + _memberCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(member.GetBufSize());
	member.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)19);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("member").Append(":").Append(member == null ? "null" : member.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

