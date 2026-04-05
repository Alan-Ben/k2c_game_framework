using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_CasualStatInfo : ALBasicProtocolPack._IALProtocolStructure {
private long casualId;
private int win;
private int lose;
private int draw;
private int serialWin;
private int serialLose;


public WCGGS2GC_CasualStatInfo() {
	casualId = (long)0;
	win = 0;
	lose = 0;
	draw = 0;
	serialWin = 0;
	serialLose = 0;
}

public WCGGS2GC_CasualStatInfo(
	long _casualId
	, int _win
	, int _lose
	, int _draw
	, int _serialWin
	, int _serialLose
) {	casualId = _casualId;
	win = _win;
	lose = _lose;
	draw = _draw;
	serialWin = _serialWin;
	serialLose = _serialLose;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCasualId() { return casualId; }
public void setCasualId(long _casualId) { casualId = _casualId; }
public int getWin() { return win; }
public void setWin(int _win) { win = _win; }
public int getLose() { return lose; }
public void setLose(int _lose) { lose = _lose; }
public int getDraw() { return draw; }
public void setDraw(int _draw) { draw = _draw; }
public int getSerialWin() { return serialWin; }
public void setSerialWin(int _serialWin) { serialWin = _serialWin; }
public int getSerialLose() { return serialLose; }
public void setSerialLose(int _serialLose) { serialLose = _serialLose; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	casualId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	win = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lose = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	draw = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialWin = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialLose = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(casualId);
	_buf.putInt(win);
	_buf.putInt(lose);
	_buf.putInt(draw);
	_buf.putInt(serialWin);
	_buf.putInt(serialLose);
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
	builder.Append("casualId").Append(":").Append(casualId.ToString()).Append(", ");
	builder.Append("win").Append(":").Append(win.ToString()).Append(", ");
	builder.Append("lose").Append(":").Append(lose.ToString()).Append(", ");
	builder.Append("draw").Append(":").Append(draw.ToString()).Append(", ");
	builder.Append("serialWin").Append(":").Append(serialWin.ToString()).Append(", ");
	builder.Append("serialLose").Append(":").Append(serialLose.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

