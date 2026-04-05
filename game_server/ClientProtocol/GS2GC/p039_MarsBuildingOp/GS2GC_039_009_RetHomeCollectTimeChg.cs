using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p039_MarsBuildingOp
{

public class GS2GC_039_009_RetHomeCollectTimeChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 最后一次收集产出时间点
/// </summary>
private int lastHomeOutputCollectTimeS;


public GS2GC_039_009_RetHomeCollectTimeChg() {
	lastHomeOutputCollectTimeS = 0;
}

public GS2GC_039_009_RetHomeCollectTimeChg(
	int _lastHomeOutputCollectTimeS
) {	lastHomeOutputCollectTimeS = _lastHomeOutputCollectTimeS;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)9; }

/// <summary>
/// 最后一次收集产出时间点
/// </summary>
public int getLastHomeOutputCollectTimeS() { return lastHomeOutputCollectTimeS; }
/// <summary>
/// 最后一次收集产出时间点
/// </summary>
public void setLastHomeOutputCollectTimeS(int _lastHomeOutputCollectTimeS) { lastHomeOutputCollectTimeS = _lastHomeOutputCollectTimeS; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastHomeOutputCollectTimeS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lastHomeOutputCollectTimeS);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
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
	builder.Append("lastHomeOutputCollectTimeS").Append(":").Append(lastHomeOutputCollectTimeS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

