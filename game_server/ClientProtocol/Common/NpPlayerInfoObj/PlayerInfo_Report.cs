using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

/// <summary>
/// 玩家举报数据
/// </summary>
public class PlayerInfo_Report : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标玩家CID
/// </summary>
private long targetCid;
/// <summary>
/// 举报内容
/// </summary>
private string contenxt;


public PlayerInfo_Report() {
	targetCid = (long)0;
	contenxt = "";
}

public PlayerInfo_Report(
	long _targetCid
	, string _contenxt
) {	targetCid = _targetCid;
	contenxt = _contenxt;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 目标玩家CID
/// </summary>
public long getTargetCid() { return targetCid; }
/// <summary>
/// 目标玩家CID
/// </summary>
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }
/// <summary>
/// 举报内容
/// </summary>
public string getContenxt() { return contenxt; }
/// <summary>
/// 举报内容
/// </summary>
public void setContenxt(string _contenxt) { contenxt = _contenxt; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contenxt);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contenxt);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	contenxt = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(targetCid);
	_buf.putString(contenxt);
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
	builder.Append("targetCid").Append(":").Append(targetCid.ToString()).Append(", ");
	builder.Append("contenxt").Append(":").Append(contenxt.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

