using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 请求群发消息
/// </summary>
public class GC2GS_032_015_ReqGuildBroadcastMessage : ALBasicProtocolPack._IALProtocolStructure {
private List<long> cidList;
private string message;


public GC2GS_032_015_ReqGuildBroadcastMessage() {
	cidList = new List<long>();
	message = "";
}

public GC2GS_032_015_ReqGuildBroadcastMessage(
	List<long> _cidList
	, string _message
) {	cidList = _cidList;
	message = _message;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)15; }

public List<long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.Add(_cidList); }
public string getMessage() { return message; }
public void setMessage(string _message) { message = _message; }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (cidList.Count * 8);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(message);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cidList.Count * 8);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(message);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		_cidList = _buf.getLong();
		cidList.Add(_cidList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	message = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)cidList.Count);
	for(int _i = 0; _i < cidList.Count; _i++) { 
		_buf.putLong(cidList[_i]);
	}
	_buf.putString(message);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)15);
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
	builder.Append("cidList").Append(":").Append(cidList.ToString()).Append(", ");
	builder.Append("message").Append(":").Append(message.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

