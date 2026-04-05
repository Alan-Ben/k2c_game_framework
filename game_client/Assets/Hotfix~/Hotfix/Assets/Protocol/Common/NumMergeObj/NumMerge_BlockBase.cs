using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.NumMergeObj
{

/// <summary>
/// 数字合并-棋子基础信息
/// </summary>
public class NumMerge_BlockBase : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 棋子等级 0=空 1-10=等级
/// </summary>
private int level;
/// <summary>
/// buff还有多少步消失 0=无buff
/// </summary>
private int buffStep;


public NumMerge_BlockBase() {
	level = 0;
	buffStep = 0;
}

public NumMerge_BlockBase(
	int _level
	, int _buffStep
) {	level = _level;
	buffStep = _buffStep;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 棋子等级 0=空 1-10=等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 棋子等级 0=空 1-10=等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// buff还有多少步消失 0=无buff
/// </summary>
public int getBuffStep() { return buffStep; }
/// <summary>
/// buff还有多少步消失 0=无buff
/// </summary>
public void setBuffStep(int _buffStep) { buffStep = _buffStep; }


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
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buffStep = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(level);
	_buf.putInt(buffStep);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("buffStep").Append(":").Append(buffStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

