using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p003_FristTeamActivityOp
{

public class GS2GC_003_001_RetGameLogicTest : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 测试参数1
/// </summary>
private int param1;


public GS2GC_003_001_RetGameLogicTest() {
	param1 = 0;
}

public GS2GC_003_001_RetGameLogicTest(
	int _param1
) {	param1 = _param1;
}

public byte getMainOrder() { return (byte)3; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 测试参数1
/// </summary>
public int getParam1() { return param1; }
/// <summary>
/// 测试参数1
/// </summary>
public void setParam1(int _param1) { param1 = _param1; }


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
	param1 = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(param1);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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
	builder.Append("param1").Append(":").Append(param1.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

