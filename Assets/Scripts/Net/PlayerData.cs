using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

public class PlayerData : BaseData
{
    public Vector3 position;
    public float moveDirectionX;
    public float velocityX;
    public float velocityY;
    public int comboCounter;
    public bool isJump;
    public bool isAttack;
    public bool isDash;
    public bool isHurt;
    public bool isDead;
    public float currentHp;

    public PlayerData() { }
   public PlayerData(Vector3 position,float moveDirectionX, float velocityX, float velocityY,int comboCounter, bool isJump, bool isAttack, bool isDash, bool isHurt, bool isDead,float currentHp)
    {
        this.position = position;
        this.moveDirectionX = moveDirectionX;
        this.velocityX = velocityX;
        this.velocityY = velocityY;
        this.comboCounter = comboCounter;
        this.isJump = isJump;
        this.isAttack = isAttack;
        this.isDash = isDash;
        this.isHurt = isHurt;
        this.isDead = isDead;
        this.currentHp = currentHp;
    }

    public override int GetBytesNum()
    {
        return sizeof(float)*7+sizeof(int)+sizeof(bool)*5;
    }

    public override int Reading(byte[] bytes, int index=0)
    {
        this.moveDirectionX=ReadFloat(bytes, ref index);
        this.position=ReadVector3(bytes, ref index);
        this.velocityX=ReadFloat(bytes,ref index);
        this.velocityY=ReadFloat(bytes,ref index);
        this.comboCounter=ReadInt(bytes,ref index);
        this.isJump=ReadBool(bytes, ref index);
        this.isAttack=ReadBool(bytes, ref index);
        this.isDash=ReadBool(bytes, ref index);
        this.isHurt=ReadBool(bytes, ref index);
        this.isDead=ReadBool(bytes, ref index);
        this.currentHp=ReadFloat(bytes,ref index);
        return index;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetBytesNum()];
        int index = 0;
        WriteFloat(bytes, moveDirectionX, ref index);
        WriteVector3(bytes,position,ref index);
        WriteFloat(bytes, velocityX, ref index);
        WriteFloat(bytes, velocityY, ref index);
        WriteInt(bytes, comboCounter, ref index);
        WriteBool(bytes, isJump, ref index);
        WriteBool(bytes,isAttack, ref index);
        WriteBool(bytes,isDash, ref index);
        WriteBool(bytes,isHurt, ref index);
        WriteBool(bytes,isDead, ref index);
        WriteFloat(bytes,currentHp, ref index);
        return bytes;
    }
}
