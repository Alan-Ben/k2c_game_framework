using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_FloatVec : ALBasicProtocolPack._IALProtocolStructure {
private float x;
private float z;


public Common_FloatVec() {
	x = 0f;
	z = 0f;
}

public Common_FloatVec(
	float _x
	, float _z
) {	x = _x;
	z = _z;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public float getX() { return x; }
public void setX(float _x) { x = _x; }
public float getZ() { return z; }
public void setZ(float _z) { z = _z; }


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
	x = _buf.getFloat();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	z = _buf.getFloat();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putFloat(x);
	_buf.putFloat(z);
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
	builder.Append("x").Append(":").Append(x.ToString()).Append(", ");
	builder.Append("z").Append(":").Append(z.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

