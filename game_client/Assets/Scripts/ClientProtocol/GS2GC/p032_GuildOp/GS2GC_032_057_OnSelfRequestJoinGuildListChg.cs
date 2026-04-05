using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_057_OnSelfRequestJoinGuildListChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 自己请求加入联盟列表
/// </summary>
private List<long> selfRequestJoinGuildList;


public GS2GC_032_057_OnSelfRequestJoinGuildListChg() {
	selfRequestJoinGuildList = new List<long>();
}

public GS2GC_032_057_OnSelfRequestJoinGuildListChg(
	List<long> _selfRequestJoinGuildList
) {	selfRequestJoinGuildList = _selfRequestJoinGuildList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 自己请求加入联盟列表
/// </summary>
public List<long> getSelfRequestJoinGuildList() { return selfRequestJoinGuildList; }
/// <summary>
/// 自己请求加入联盟列表
/// </summary>
public void addSelfRequestJoinGuildList(long _selfRequestJoinGuildList) { selfRequestJoinGuildList.Add(_selfRequestJoinGuildList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (selfRequestJoinGuildList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (selfRequestJoinGuildList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _selfRequestJoinGuildListCount = _buf.getShort();
	for(int _i = 0; _i < _selfRequestJoinGuildListCount; _i++) { 
		long _selfRequestJoinGuildList = (long)0;
		_selfRequestJoinGuildList = _buf.getLong();
		selfRequestJoinGuildList.Add(_selfRequestJoinGuildList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)selfRequestJoinGuildList.Count);
	for(int _i = 0; _i < selfRequestJoinGuildList.Count; _i++) { 
		_buf.putLong(selfRequestJoinGuildList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)57);
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
	builder.Append("selfRequestJoinGuildList").Append(":").Append(selfRequestJoinGuildList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

