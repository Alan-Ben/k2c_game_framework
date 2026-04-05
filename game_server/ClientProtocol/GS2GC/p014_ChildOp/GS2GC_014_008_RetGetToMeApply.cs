using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 获取对玩家的指定联姻请求
/// </summary>
public class GS2GC_014_008_RetGetToMeApply : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.ChildObj.Adult_ToMeApplyInfo apply;


public GS2GC_014_008_RetGetToMeApply() {
	apply = new Common.ChildObj.Adult_ToMeApplyInfo();
}

public GS2GC_014_008_RetGetToMeApply(
	Common.ChildObj.Adult_ToMeApplyInfo _apply
) {	apply = _apply;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 空
/// </summary>
public Common.ChildObj.Adult_ToMeApplyInfo getApply() { return apply; }
/// <summary>
/// 空
/// </summary>
public void setApply(Common.ChildObj.Adult_ToMeApplyInfo _apply) { apply = _apply; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + apply.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + apply.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _applyCustLen = _buf.getInt();
	int _applyCurPos = _buf.getCurPos();
	apply.ReadUnzipBuf(_buf, _applyCurPos + _applyCustLen);
	_buf.setPosition(_applyCurPos + _applyCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(apply.GetBufSize());
	apply.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)8);
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
	builder.Append("apply").Append(":").Append(apply == null ? "null" : apply.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

