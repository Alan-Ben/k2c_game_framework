using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_004_RetOtherGuildInfo : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo showInfo;
private List<Common.GuildObj.Guild_MemberBaseInfo> memberList;


public GS2GC_032_004_RetOtherGuildInfo() {
	showInfo = new Common.GuildObj.Guild_ShowInfo();
	memberList = new List<Common.GuildObj.Guild_MemberBaseInfo>();
}

public GS2GC_032_004_RetOtherGuildInfo(
	Common.GuildObj.Guild_ShowInfo _showInfo
	, List<Common.GuildObj.Guild_MemberBaseInfo> _memberList
) {	showInfo = _showInfo;
	memberList = _memberList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)4; }

public Common.GuildObj.Guild_ShowInfo getShowInfo() { return showInfo; }
public void setShowInfo(Common.GuildObj.Guild_ShowInfo _showInfo) { showInfo = _showInfo; }
public List<Common.GuildObj.Guild_MemberBaseInfo> getMemberList() { return memberList; }
public void addMemberList(Common.GuildObj.Guild_MemberBaseInfo _memberList) { memberList.Add(_memberList); }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + showInfo.GetBufSize();
	_size += 2 + (memberList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + showInfo.GetBufSize();
	_size += 2 + (memberList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.getCurPos();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.setPosition(_showInfoCurPos + _showInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.GuildObj.Guild_MemberBaseInfo _memberList = new Common.GuildObj.Guild_MemberBaseInfo();
		int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.getCurPos();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.setPosition(__memberListCurPos + __memberListCustLen);

		memberList.Add(_memberList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)memberList.Count);
	for(int _i = 0; _i < memberList.Count; _i++) { 
		_buf.putInt(memberList[_i].GetBufSize());
	memberList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)4);
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
	builder.Append("showInfo").Append(":").Append(showInfo == null ? "null" : showInfo.ToString()).Append(", ");
	builder.Append("memberList").Append(":").Append(memberList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

