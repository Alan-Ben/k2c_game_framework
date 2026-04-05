using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_CheckReadyPlayerInfo : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private long icon;
private string name;
private bool isReady;
private long iconBgk;


public WCGGS2GC_CheckReadyPlayerInfo() {
	uid = (long)0;
	icon = (long)0;
	name = "";
	isReady = false;
	iconBgk = (long)0;
}

public WCGGS2GC_CheckReadyPlayerInfo(
	long _uid
	, long _icon
	, string _name
	, bool _isReady
	, long _iconBgk
) {	uid = _uid;
	icon = _icon;
	name = _name;
	isReady = _isReady;
	iconBgk = _iconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public string getName() { return name; }
public void setName(string _name) { name = _name; }
public bool getIsReady() { return isReady; }
public void setIsReady(bool _isReady) { isReady = _isReady; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public int GetBufSize() {
	int _size = 25;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putLong(icon);
	_buf.putString(name);
	_buf.put(isReady?(byte)1:(byte)0);
	_buf.putLong(iconBgk);
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
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("isReady").Append(":").Append(isReady.ToString()).Append(", ");
	builder.Append("iconBgk").Append(":").Append(iconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

