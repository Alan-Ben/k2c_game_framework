using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟火星求助展示信息-火星科技升级
/// </summary>
public class Guild_MarsHelp_TechUp : ALBasicProtocolPack._IALProtocolStructure {
private long techId;
private int lvl;


public Guild_MarsHelp_TechUp() {
	techId = (long)0;
	lvl = 0;
}

public Guild_MarsHelp_TechUp(
	long _techId
	, int _lvl
) {	techId = _techId;
	lvl = _lvl;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getTechId() { return techId; }
public void setTechId(long _techId) { techId = _techId; }
public int getLvl() { return lvl; }
public void setLvl(int _lvl) { lvl = _lvl; }


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
	techId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(techId);
	_buf.putInt(lvl);
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
	builder.Append("techId").Append(":").Append(techId.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

