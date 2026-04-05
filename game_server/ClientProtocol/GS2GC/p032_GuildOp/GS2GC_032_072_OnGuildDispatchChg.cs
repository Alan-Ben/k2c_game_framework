using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 派遣信息变更
/// </summary>
public class GS2GC_032_072_OnGuildDispatchChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_AttrDispatchInfo data;


public GS2GC_032_072_OnGuildDispatchChg() {
	data = new Common.GuildObj.Guild_AttrDispatchInfo();
}

public GS2GC_032_072_OnGuildDispatchChg(
	Common.GuildObj.Guild_AttrDispatchInfo _data
) {	data = _data;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)72; }

public Common.GuildObj.Guild_AttrDispatchInfo getData() { return data; }
public void setData(Common.GuildObj.Guild_AttrDispatchInfo _data) { data = _data; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + data.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + data.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _dataCustLen = _buf.getInt();
	int _dataCurPos = _buf.getCurPos();
	data.ReadUnzipBuf(_buf, _dataCurPos + _dataCustLen);
	_buf.setPosition(_dataCurPos + _dataCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(data.GetBufSize());
	data.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)72);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)72);
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
	builder.Append("data").Append(":").Append(data == null ? "null" : data.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

