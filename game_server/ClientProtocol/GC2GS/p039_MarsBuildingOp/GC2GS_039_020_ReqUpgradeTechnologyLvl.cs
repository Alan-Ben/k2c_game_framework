using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p039_MarsBuildingOp
{

/// <summary>
/// 火星科技-科技升级
/// </summary>
public class GC2GS_039_020_ReqUpgradeTechnologyLvl : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 科技ID
/// </summary>
private long technologyId;


public GC2GS_039_020_ReqUpgradeTechnologyLvl() {
	technologyId = (long)0;
}

public GC2GS_039_020_ReqUpgradeTechnologyLvl(
	long _technologyId
) {	technologyId = _technologyId;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)20; }

/// <summary>
/// 科技ID
/// </summary>
public long getTechnologyId() { return technologyId; }
/// <summary>
/// 科技ID
/// </summary>
public void setTechnologyId(long _technologyId) { technologyId = _technologyId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	technologyId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(technologyId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)20);
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
	builder.Append("technologyId").Append(":").Append(technologyId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

