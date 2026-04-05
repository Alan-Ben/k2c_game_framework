using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 请求变更联盟名称
/// </summary>
public class GC2GS_032_009_ReqChgGuildName : ALBasicProtocolPack._IALProtocolStructure {
private string name;
private string simpleName;


public GC2GS_032_009_ReqChgGuildName() {
	name = "";
	simpleName = "";
}

public GC2GS_032_009_ReqChgGuildName(
	string _name
	, string _simpleName
) {	name = _name;
	simpleName = _simpleName;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)9; }

public string getName() { return name; }
public void setName(string _name) { name = _name; }
public string getSimpleName() { return simpleName; }
public void setSimpleName(string _simpleName) { simpleName = _simpleName; }


public int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	simpleName = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(name);
	_buf.putString(simpleName);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)9);
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
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("simpleName").Append(":").Append(simpleName.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

