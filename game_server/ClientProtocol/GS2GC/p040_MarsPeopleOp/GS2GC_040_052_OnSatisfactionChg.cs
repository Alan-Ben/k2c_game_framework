using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p040_MarsPeopleOp
{

/// <summary>
/// 满意度变化
/// </summary>
public class GS2GC_040_052_OnSatisfactionChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 满意度万分比
/// </summary>
private int satisfaction;


public GS2GC_040_052_OnSatisfactionChg() {
	satisfaction = 0;
}

public GS2GC_040_052_OnSatisfactionChg(
	int _satisfaction
) {	satisfaction = _satisfaction;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 满意度万分比
/// </summary>
public int getSatisfaction() { return satisfaction; }
/// <summary>
/// 满意度万分比
/// </summary>
public void setSatisfaction(int _satisfaction) { satisfaction = _satisfaction; }


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
	satisfaction = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(satisfaction);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
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
	builder.Append("satisfaction").Append(":").Append(satisfaction.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

