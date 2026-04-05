using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 盟主转让
/// </summary>
public class GuildLog_LeaderTransfer : ALBasicProtocolPack._IALProtocolStructure {
private string oriLeaderName;
private string newLeaderName;


public GuildLog_LeaderTransfer() {
	oriLeaderName = "";
	newLeaderName = "";
}

public GuildLog_LeaderTransfer(
	string _oriLeaderName
	, string _newLeaderName
) {	oriLeaderName = _oriLeaderName;
	newLeaderName = _newLeaderName;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public string getOriLeaderName() { return oriLeaderName; }
public void setOriLeaderName(string _oriLeaderName) { oriLeaderName = _oriLeaderName; }
public string getNewLeaderName() { return newLeaderName; }
public void setNewLeaderName(string _newLeaderName) { newLeaderName = _newLeaderName; }


public int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriLeaderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newLeaderName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriLeaderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newLeaderName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oriLeaderName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	newLeaderName = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(oriLeaderName);
	_buf.putString(newLeaderName);
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
	builder.Append("oriLeaderName").Append(":").Append(oriLeaderName.ToString()).Append(", ");
	builder.Append("newLeaderName").Append(":").Append(newLeaderName.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

