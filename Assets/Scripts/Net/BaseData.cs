using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


public abstract class BaseData 
{
    public abstract int GetBytesNum();
    //public virtual int GetBytesNum()
    //{
    //    return 0;
    //}

    /// <summary>
    /// 序列化
    /// </summary>
    /// <returns></returns>
    public abstract byte[] Writing();
    //public virtual byte[] Writing()
    //{
    //    return null;
    //}
    public void WriteShort(byte[] bytes,short shortInfo,ref int index)
    {
        BitConverter.GetBytes(shortInfo).CopyTo(bytes, index);
        index += sizeof(short);
    }
    public void WriteFloat(byte[] bytes, float floatInfo, ref int index)
    {
        BitConverter.GetBytes(floatInfo).CopyTo(bytes, index);
        index += sizeof(float);
    }
    public void WriteInt(byte[] bytes, int intInfo, ref int index)
    {
        BitConverter.GetBytes(intInfo).CopyTo(bytes, index);
        index += sizeof(int);
    }
    public void WriteVector3(byte[] bytes, Vector3 vector3Info, ref int index)
    {
        BitConverter.GetBytes(vector3Info.X).CopyTo(bytes, index);
        index += sizeof(float);
        BitConverter.GetBytes(vector3Info.Y).CopyTo(bytes, index);
        index += sizeof(float);
        BitConverter.GetBytes(vector3Info.Z).CopyTo(bytes, index);
        index += sizeof(float);
    }

    public void WriteBool(byte[] bytes, bool boolInfo, ref int index)
    {
        BitConverter.GetBytes(boolInfo).CopyTo(bytes, index);
        index += sizeof(bool);
    }
    public void WriteString(byte[] bytes, string strInfo, ref int index)
    {
        byte[] strBytes=Encoding.UTF8.GetBytes(strInfo);
        //int strLength=strBytes.Length;
        //BitConverter.GetBytes(strLength).CopyTo(bytes, index);       
        //index += sizeof(int);
        WriteInt(bytes, strBytes.Length, ref index);
        strBytes.CopyTo(bytes, index);
        index+=strBytes.Length;
    }
    public void WriteData(byte[] bytes, BaseData dataInfo, ref int index)
    {
        dataInfo.Writing().CopyTo(bytes, index);
        index += dataInfo.GetBytesNum();
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public abstract int Reading(byte[] bytes,int index);

    public short ReadShort(byte[] bytes,ref int index)
    {
        short shortInfo = BitConverter.ToInt16(bytes, index);
        index += sizeof(short);
        return shortInfo;       
    }
    public float ReadFloat(byte[] bytes,ref int index)
    {
        float floatInfo=BitConverter.ToSingle(bytes, index);
        index += sizeof(float);
        return floatInfo;
    }

    public Vector3 ReadVector3(byte[] bytes,ref int index)
    {
        float vectorXInfo=BitConverter.ToSingle(bytes, index);
        index += sizeof(float);
        float vectorYInfo = BitConverter.ToSingle(bytes, index);
        index += sizeof(float);
        float vectorZInfo = BitConverter.ToSingle(bytes, index);
        index += sizeof(float);
        Vector3 vector3Info = new Vector3(vectorXInfo,vectorYInfo,vectorZInfo);
        return vector3Info;

    }
    public int ReadInt(byte[] bytes, ref int index)
    {
        int intInfo = BitConverter.ToInt32(bytes, index);
        index += sizeof(int);
        return intInfo;
    }
    public bool ReadBool(byte[] bytes, ref int index)
    {
        bool boolInfo = BitConverter.ToBoolean(bytes, index);
        index += sizeof(bool);
        return boolInfo;
    }
    public string ReadString(byte[] bytes, ref int index)
    {
        int strLength=BitConverter.ToInt32(bytes, index);
        index += sizeof(int);
        string strInfo=Encoding.UTF8.GetString(bytes, index, strLength);
        index += strLength;
        return strInfo;
    }
    public T ReadData<T>(byte[] bytes, ref int index) where T : BaseData,new()
    {
        T tInfo = new T();
        tInfo.Reading(bytes,index);
        index += tInfo.GetBytesNum();
        return tInfo;
    }

}
