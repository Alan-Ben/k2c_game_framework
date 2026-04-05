using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 永久加成变更
/// </summary>
public class GS2GC_021_075_OnForeverAddChg : ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_ForeverAddInfo add;


public GS2GC_021_075_OnForeverAddChg() {
	add = new NPCommon.NPCommon_ForeverAddInfo();
}

public GS2GC_021_075_OnForeverAddChg(
	NPCommon.NPCommon_ForeverAddInfo _add
) {	add = _add;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)75; }

public NPCommon.NPCommon_ForeverAddInfo getAdd() { return add; }
public void setAdd(NPCommon.NPCommon_ForeverAddInfo _add) { add = _add; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _addCustLen = _buf.getInt();
	int _addCurPos = _buf.getCurPos();
	add.ReadUnzipBuf(_buf, _addCurPos + _addCustLen);
	_buf.setPosition(_addCurPos + _addCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(add.GetBufSize());
	add.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)75);
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
	builder.Append("add").Append(":").Append(add == null ? "null" : add.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

