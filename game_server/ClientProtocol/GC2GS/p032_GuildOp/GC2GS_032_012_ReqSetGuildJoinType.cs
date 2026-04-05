using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 切换联盟加入类型
/// </summary>
public class GC2GS_032_012_ReqSetGuildJoinType : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildEnum.EGuildJoinType type;
private List<Common.GuildObj.Guild_JoinLimitInfo> joinLimitInfo;


public GC2GS_032_012_ReqSetGuildJoinType() {
	type = 0;
	joinLimitInfo = new List<Common.GuildObj.Guild_JoinLimitInfo>();
}

public GC2GS_032_012_ReqSetGuildJoinType(
	Common.GuildEnum.EGuildJoinType _type
	, List<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfo
) {	type = _type;
	joinLimitInfo = _joinLimitInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)12; }

public Common.GuildEnum.EGuildJoinType getType() { return type; }
public void setType(Common.GuildEnum.EGuildJoinType _type) { type = _type; }
public List<Common.GuildObj.Guild_JoinLimitInfo> getJoinLimitInfo() { return joinLimitInfo; }
public void addJoinLimitInfo(Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo) { joinLimitInfo.Add(_joinLimitInfo); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (joinLimitInfo.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (joinLimitInfo.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.GuildEnum.EGuildJoinType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _joinLimitInfoCount = _buf.getShort();
	for(int _i = 0; _i < _joinLimitInfoCount; _i++) { 
		Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo = new Common.GuildObj.Guild_JoinLimitInfo();
		int __joinLimitInfoCustLen = _buf.getInt();
	int __joinLimitInfoCurPos = _buf.getCurPos();
	_joinLimitInfo.ReadUnzipBuf(_buf, __joinLimitInfoCurPos + __joinLimitInfoCustLen);
	_buf.setPosition(__joinLimitInfoCurPos + __joinLimitInfoCustLen);

		joinLimitInfo.Add(_joinLimitInfo);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putShort((short)joinLimitInfo.Count);
	for(int _i = 0; _i < joinLimitInfo.Count; _i++) { 
		_buf.putInt(joinLimitInfo[_i].GetBufSize());
	joinLimitInfo[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)12);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("joinLimitInfo").Append(":").Append(joinLimitInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

