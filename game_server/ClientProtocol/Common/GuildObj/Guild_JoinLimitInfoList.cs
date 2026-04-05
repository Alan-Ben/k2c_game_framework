using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 加入限制信息列表
/// </summary>
public class Guild_JoinLimitInfoList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 加入限制信息
/// </summary>
private List<Common.GuildObj.Guild_JoinLimitInfo> joinLimitInfo;


public Guild_JoinLimitInfoList() {
	joinLimitInfo = new List<Common.GuildObj.Guild_JoinLimitInfo>();
}

public Guild_JoinLimitInfoList(
	List<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfo
) {	joinLimitInfo = _joinLimitInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 加入限制信息
/// </summary>
public List<Common.GuildObj.Guild_JoinLimitInfo> getJoinLimitInfo() { return joinLimitInfo; }
/// <summary>
/// 加入限制信息
/// </summary>
public void addJoinLimitInfo(Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo) { joinLimitInfo.Add(_joinLimitInfo); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (joinLimitInfo.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (joinLimitInfo.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
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
	_buf.putShort((short)joinLimitInfo.Count);
	for(int _i = 0; _i < joinLimitInfo.Count; _i++) { 
		_buf.putInt(joinLimitInfo[_i].GetBufSize());
	joinLimitInfo[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("joinLimitInfo").Append(":").Append(joinLimitInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

