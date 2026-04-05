using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p040_MarsPeopleOp
{

/// <summary>
/// 火星居民-处理选择帮助
/// </summary>
public class GC2GS_040_005_ReqDealChoiceHelp : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 选择
/// </summary>
private int choice;


public GC2GS_040_005_ReqDealChoiceHelp() {
	id = (long)0;
	choice = 0;
}

public GC2GS_040_005_ReqDealChoiceHelp(
	long _id
	, int _choice
) {	id = _id;
	choice = _choice;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 选择
/// </summary>
public int getChoice() { return choice; }
/// <summary>
/// 选择
/// </summary>
public void setChoice(int _choice) { choice = _choice; }


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
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	choice = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(choice);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)5);
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
	builder.Append("choice").Append(":").Append(choice.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

