using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_UniformItemFix : ALBasicProtocolPack._IALProtocolStructure {
private int modifierId;
private int fixType;
private long value;


public WCGGS2GC_UniformItemFix() {
	modifierId = 0;
	fixType = 0;
	value = (long)0;
}

public WCGGS2GC_UniformItemFix(
	int _modifierId
	, int _fixType
	, long _value
) {	modifierId = _modifierId;
	fixType = _fixType;
	value = _value;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getModifierId() { return modifierId; }
public void setModifierId(int _modifierId) { modifierId = _modifierId; }
public int getFixType() { return fixType; }
public void setFixType(int _fixType) { fixType = _fixType; }
public long getValue() { return value; }
public void setValue(long _value) { value = _value; }


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
	modifierId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fixType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	value = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(modifierId);
	_buf.putInt(fixType);
	_buf.putLong(value);
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
	builder.Append("modifierId").Append(":").Append(modifierId.ToString()).Append(", ");
	builder.Append("fixType").Append(":").Append(fixType.ToString()).Append(", ");
	builder.Append("value").Append(":").Append(value.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

