using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 对自身指定请求新增推送
/// </summary>
public class GS2GC_014_056_OnToMeApplyAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.ChildObj.Adult_ToMeApplyBaseInfo toMeApply;


public GS2GC_014_056_OnToMeApplyAdd() {
	toMeApply = new Common.ChildObj.Adult_ToMeApplyBaseInfo();
}

public GS2GC_014_056_OnToMeApplyAdd(
	Common.ChildObj.Adult_ToMeApplyBaseInfo _toMeApply
) {	toMeApply = _toMeApply;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 空
/// </summary>
public Common.ChildObj.Adult_ToMeApplyBaseInfo getToMeApply() { return toMeApply; }
/// <summary>
/// 空
/// </summary>
public void setToMeApply(Common.ChildObj.Adult_ToMeApplyBaseInfo _toMeApply) { toMeApply = _toMeApply; }


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
	int _toMeApplyCustLen = _buf.getInt();
	int _toMeApplyCurPos = _buf.getCurPos();
	toMeApply.ReadUnzipBuf(_buf, _toMeApplyCurPos + _toMeApplyCustLen);
	_buf.setPosition(_toMeApplyCurPos + _toMeApplyCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(toMeApply.GetBufSize());
	toMeApply.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)56);
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
	builder.Append("toMeApply").Append(":").Append(toMeApply == null ? "null" : toMeApply.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

