using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟派遣大臣信息
/// </summary>
public class Guild_DispatchHeroInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long heroId;
/// <summary>
/// 加成万分比
/// </summary>
private int addPer;


public Guild_DispatchHeroInfo() {
	cid = (long)0;
	heroId = (long)0;
	addPer = 0;
}

public Guild_DispatchHeroInfo(
	long _cid
	, long _heroId
	, int _addPer
) {	cid = _cid;
	heroId = _heroId;
	addPer = _addPer;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 加成万分比
/// </summary>
public int getAddPer() { return addPer; }
/// <summary>
/// 加成万分比
/// </summary>
public void setAddPer(int _addPer) { addPer = _addPer; }


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
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addPer = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(heroId);
	_buf.putInt(addPer);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("addPer").Append(":").Append(addPer.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

