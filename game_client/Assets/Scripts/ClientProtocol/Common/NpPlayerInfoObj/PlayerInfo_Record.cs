using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

public class PlayerInfo_Record : ALBasicProtocolPack._IALProtocolStructure {
private NPEnum.ENPPlayerRecordParam type;
private long count;


public PlayerInfo_Record() {
	type = 0;
	count = (long)0;
}

public PlayerInfo_Record(
	NPEnum.ENPPlayerRecordParam _type
	, long _count
) {	type = _type;
	count = _count;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public NPEnum.ENPPlayerRecordParam getType() { return type; }
public void setType(NPEnum.ENPPlayerRecordParam _type) { type = _type; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (NPEnum.ENPPlayerRecordParam)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putLong(count);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

