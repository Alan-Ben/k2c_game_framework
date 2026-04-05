using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_054_OnGuildMemberAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成员基础信息
/// </summary>
private Common.GuildObj.Guild_MemberBaseInfo baseInfo;


public GS2GC_032_054_OnGuildMemberAdd() {
	baseInfo = new Common.GuildObj.Guild_MemberBaseInfo();
}

public GS2GC_032_054_OnGuildMemberAdd(
	Common.GuildObj.Guild_MemberBaseInfo _baseInfo
) {	baseInfo = _baseInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 成员基础信息
/// </summary>
public Common.GuildObj.Guild_MemberBaseInfo getBaseInfo() { return baseInfo; }
/// <summary>
/// 成员基础信息
/// </summary>
public void setBaseInfo(Common.GuildObj.Guild_MemberBaseInfo _baseInfo) { baseInfo = _baseInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.getCurPos();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.setPosition(_baseInfoCurPos + _baseInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)54);
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
	builder.Append("baseInfo").Append(":").Append(baseInfo == null ? "null" : baseInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

