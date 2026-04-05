using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_077_OnStageGoalBigStepFirstReachAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大阶段ID
/// </summary>
private long bigStepId;


public GS2GC_007_077_OnStageGoalBigStepFirstReachAdd() {
	bigStepId = (long)0;
}

public GS2GC_007_077_OnStageGoalBigStepFirstReachAdd(
	long _bigStepId
) {	bigStepId = _bigStepId;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)77; }

/// <summary>
/// 大阶段ID
/// </summary>
public long getBigStepId() { return bigStepId; }
/// <summary>
/// 大阶段ID
/// </summary>
public void setBigStepId(long _bigStepId) { bigStepId = _bigStepId; }


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
	bigStepId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bigStepId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)77);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)77);
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
	builder.Append("bigStepId").Append(":").Append(bigStepId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

