using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_058_OnJoinGuildCdChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 加入联盟CD信息
/// </summary>
private Common.GuildObj.Guild_JoinCdInfo joinCdInfo;


public GS2GC_032_058_OnJoinGuildCdChg() {
	joinCdInfo = new Common.GuildObj.Guild_JoinCdInfo();
}

public GS2GC_032_058_OnJoinGuildCdChg(
	Common.GuildObj.Guild_JoinCdInfo _joinCdInfo
) {	joinCdInfo = _joinCdInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 加入联盟CD信息
/// </summary>
public Common.GuildObj.Guild_JoinCdInfo getJoinCdInfo() { return joinCdInfo; }
/// <summary>
/// 加入联盟CD信息
/// </summary>
public void setJoinCdInfo(Common.GuildObj.Guild_JoinCdInfo _joinCdInfo) { joinCdInfo = _joinCdInfo; }


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
	int _joinCdInfoCustLen = _buf.getInt();
	int _joinCdInfoCurPos = _buf.getCurPos();
	joinCdInfo.ReadUnzipBuf(_buf, _joinCdInfoCurPos + _joinCdInfoCustLen);
	_buf.setPosition(_joinCdInfoCurPos + _joinCdInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(joinCdInfo.GetBufSize());
	joinCdInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)58);
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
	builder.Append("joinCdInfo").Append(":").Append(joinCdInfo == null ? "null" : joinCdInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

