using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 子嗣（未成年）等级变动推送
/// </summary>
public class GS2GC_014_052_OnChildLvlChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣实例ID
/// </summary>
private long id;
/// <summary>
/// 子嗣等级
/// </summary>
private int lvl;


public GS2GC_014_052_OnChildLvlChg() {
	id = (long)0;
	lvl = 0;
}

public GS2GC_014_052_OnChildLvlChg(
	long _id
	, int _lvl
) {	id = _id;
	lvl = _lvl;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 子嗣实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 子嗣实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 子嗣等级
/// </summary>
public int getLvl() { return lvl; }
/// <summary>
/// 子嗣等级
/// </summary>
public void setLvl(int _lvl) { lvl = _lvl; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(lvl);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)52);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

