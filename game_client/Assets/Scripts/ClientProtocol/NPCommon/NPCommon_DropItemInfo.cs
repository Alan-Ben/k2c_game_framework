using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_DropItemInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
private long dropId;
private int leftSecs;


public NPCommon_DropItemInfo() {
	id = (long)0;
	dropId = (long)0;
	leftSecs = 0;
}

public NPCommon_DropItemInfo(
	long _id
	, long _dropId
	, int _leftSecs
) {	id = _id;
	dropId = _dropId;
	leftSecs = _leftSecs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public long getDropId() { return dropId; }
public void setDropId(long _dropId) { dropId = _dropId; }
public int getLeftSecs() { return leftSecs; }
public void setLeftSecs(int _leftSecs) { leftSecs = _leftSecs; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dropId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leftSecs = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(dropId);
	_buf.putInt(leftSecs);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("dropId").Append(":").Append(dropId.ToString()).Append(", ");
	builder.Append("leftSecs").Append(":").Append(leftSecs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

