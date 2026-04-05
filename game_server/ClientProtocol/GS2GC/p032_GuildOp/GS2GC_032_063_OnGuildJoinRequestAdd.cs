using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_063_OnGuildJoinRequestAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 入盟请求
/// </summary>
private Common.GuildObj.Guild_JoinRequestInfo joinRequestList;


public GS2GC_032_063_OnGuildJoinRequestAdd() {
	joinRequestList = new Common.GuildObj.Guild_JoinRequestInfo();
}

public GS2GC_032_063_OnGuildJoinRequestAdd(
	Common.GuildObj.Guild_JoinRequestInfo _joinRequestList
) {	joinRequestList = _joinRequestList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)63; }

/// <summary>
/// 入盟请求
/// </summary>
public Common.GuildObj.Guild_JoinRequestInfo getJoinRequestList() { return joinRequestList; }
/// <summary>
/// 入盟请求
/// </summary>
public void setJoinRequestList(Common.GuildObj.Guild_JoinRequestInfo _joinRequestList) { joinRequestList = _joinRequestList; }


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
	int _joinRequestListCustLen = _buf.getInt();
	int _joinRequestListCurPos = _buf.getCurPos();
	joinRequestList.ReadUnzipBuf(_buf, _joinRequestListCurPos + _joinRequestListCustLen);
	_buf.setPosition(_joinRequestListCurPos + _joinRequestListCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(joinRequestList.GetBufSize());
	joinRequestList.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)63);
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
	builder.Append("joinRequestList").Append(":").Append(joinRequestList == null ? "null" : joinRequestList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

