using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 联盟建造次数信息变更
/// </summary>
public class GS2GC_032_067_OnGuildConstructListChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ConstructList constructList;


public GS2GC_032_067_OnGuildConstructListChg() {
	constructList = new Common.GuildObj.Guild_ConstructList();
}

public GS2GC_032_067_OnGuildConstructListChg(
	Common.GuildObj.Guild_ConstructList _constructList
) {	constructList = _constructList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)67; }

public Common.GuildObj.Guild_ConstructList getConstructList() { return constructList; }
public void setConstructList(Common.GuildObj.Guild_ConstructList _constructList) { constructList = _constructList; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + constructList.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + constructList.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _constructListCustLen = _buf.getInt();
	int _constructListCurPos = _buf.getCurPos();
	constructList.ReadUnzipBuf(_buf, _constructListCurPos + _constructListCustLen);
	_buf.setPosition(_constructListCurPos + _constructListCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(constructList.GetBufSize());
	constructList.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)67);
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
	builder.Append("constructList").Append(":").Append(constructList == null ? "null" : constructList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

