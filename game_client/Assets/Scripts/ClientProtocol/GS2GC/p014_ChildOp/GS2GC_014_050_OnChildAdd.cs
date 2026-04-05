using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 子嗣（未成年）新增推送
/// </summary>
public class GS2GC_014_050_OnChildAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.ChildObj.Child_Info child;


public GS2GC_014_050_OnChildAdd() {
	child = new Common.ChildObj.Child_Info();
}

public GS2GC_014_050_OnChildAdd(
	Common.ChildObj.Child_Info _child
) {	child = _child;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 空
/// </summary>
public Common.ChildObj.Child_Info getChild() { return child; }
/// <summary>
/// 空
/// </summary>
public void setChild(Common.ChildObj.Child_Info _child) { child = _child; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + child.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + child.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _childCustLen = _buf.getInt();
	int _childCurPos = _buf.getCurPos();
	child.ReadUnzipBuf(_buf, _childCurPos + _childCustLen);
	_buf.setPosition(_childCurPos + _childCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(child.GetBufSize());
	child.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)50);
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
	builder.Append("child").Append(":").Append(child == null ? "null" : child.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

