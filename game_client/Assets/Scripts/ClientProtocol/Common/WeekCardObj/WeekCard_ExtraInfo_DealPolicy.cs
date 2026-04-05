using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-额外设置-懒汉策略
/// </summary>
public class WeekCard_ExtraInfo_DealPolicy : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否懒汉 即只处理超过上限的
/// </summary>
private bool isLazy;


public WeekCard_ExtraInfo_DealPolicy() {
	isLazy = false;
}

public WeekCard_ExtraInfo_DealPolicy(
	bool _isLazy
) {	isLazy = _isLazy;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 是否懒汉 即只处理超过上限的
/// </summary>
public bool getIsLazy() { return isLazy; }
/// <summary>
/// 是否懒汉 即只处理超过上限的
/// </summary>
public void setIsLazy(bool _isLazy) { isLazy = _isLazy; }


public int GetBufSize() {
	int _size = 1;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isLazy = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isLazy?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("isLazy").Append(":").Append(isLazy.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

