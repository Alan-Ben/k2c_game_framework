using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p040_MarsPeopleOp
{

/// <summary>
/// 决策数据变化
/// </summary>
public class GS2GC_040_051_OnIntelligentChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 决策数据
/// </summary>
private Common.MarsObj.Mars_Intelligent intelligent;


public GS2GC_040_051_OnIntelligentChg() {
	intelligent = new Common.MarsObj.Mars_Intelligent();
}

public GS2GC_040_051_OnIntelligentChg(
	Common.MarsObj.Mars_Intelligent _intelligent
) {	intelligent = _intelligent;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 决策数据
/// </summary>
public Common.MarsObj.Mars_Intelligent getIntelligent() { return intelligent; }
/// <summary>
/// 决策数据
/// </summary>
public void setIntelligent(Common.MarsObj.Mars_Intelligent _intelligent) { intelligent = _intelligent; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _intelligentCustLen = _buf.getInt();
	int _intelligentCurPos = _buf.getCurPos();
	intelligent.ReadUnzipBuf(_buf, _intelligentCurPos + _intelligentCustLen);
	_buf.setPosition(_intelligentCurPos + _intelligentCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(intelligent.GetBufSize());
	intelligent.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)51);
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
	builder.Append("intelligent").Append(":").Append(intelligent == null ? "null" : intelligent.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

