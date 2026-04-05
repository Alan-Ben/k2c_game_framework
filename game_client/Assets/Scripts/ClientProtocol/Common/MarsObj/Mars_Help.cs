using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星居民-求助数据
/// </summary>
public class Mars_Help : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 信件ID
/// </summary>
private long helpId;
private long npcId;
private int chooseIdx;
private bool isFinish;


public Mars_Help() {
	id = (long)0;
	helpId = (long)0;
	npcId = (long)0;
	chooseIdx = 0;
	isFinish = false;
}

public Mars_Help(
	long _id
	, long _helpId
	, long _npcId
	, int _chooseIdx
	, bool _isFinish
) {	id = _id;
	helpId = _helpId;
	npcId = _npcId;
	chooseIdx = _chooseIdx;
	isFinish = _isFinish;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 信件ID
/// </summary>
public long getHelpId() { return helpId; }
/// <summary>
/// 信件ID
/// </summary>
public void setHelpId(long _helpId) { helpId = _helpId; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }
public int getChooseIdx() { return chooseIdx; }
public void setChooseIdx(int _chooseIdx) { chooseIdx = _chooseIdx; }
public bool getIsFinish() { return isFinish; }
public void setIsFinish(bool _isFinish) { isFinish = _isFinish; }


public int GetBufSize() {
	int _size = 29;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	helpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chooseIdx = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFinish = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(helpId);
	_buf.putLong(npcId);
	_buf.putInt(chooseIdx);
	_buf.put(isFinish?(byte)1:(byte)0);
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
	builder.Append("helpId").Append(":").Append(helpId.ToString()).Append(", ");
	builder.Append("npcId").Append(":").Append(npcId.ToString()).Append(", ");
	builder.Append("chooseIdx").Append(":").Append(chooseIdx.ToString()).Append(", ");
	builder.Append("isFinish").Append(":").Append(isFinish.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

