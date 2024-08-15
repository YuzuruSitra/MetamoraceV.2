using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockStack : MonoBehaviour
{
private string[] _stackBlocks = new string[3];
    public string[] _StackBlocks => _stackBlocks;

   public bool FullStack { get; private set; } = false;

   private void Update()
   {
       Debug.Log(_stackBlocks[0] + " " + _stackBlocks[1] + " " + _stackBlocks[2]);
   }
   //破壊したブロックをスタック
    public void StackBlock(string _objName)
    {      
        if(_objName == "NULL") return;
       //保持しているブロックUI表示
        for(int i = 0; i < _stackBlocks.Length; i++) 
        {       
            //配列のi番目が空（０）だったら処理実行
            if(_stackBlocks[i] == null)
            {
                _stackBlocks[i] = _objName;
                Debug.Log("stack");
                //Debug.Log(_stackBlocks[0] + " " + _stackBlocks[1] + " " + _stackBlocks[2]);
                if (_stackBlocks[2] != null) FullStack = true;
                break;
            }
        }  
    }

    public void ResetBlock()
    {
        for(int i = 0; i < _stackBlocks.Length; i++)
        {
            //配列のi番目が空（０）だったら処理実行
            _stackBlocks[i] = null;      
        }
        FullStack = false;
    }
    //自ブロックの数をカウント
    public string CreateItemType()
    {
        int _myBrockNum = 0;
        for(int i = 0; i < _stackBlocks.Length; i++)
        {
            //配列のi番目がAmbrassだったら処理実行
            if(_stackBlocks[i] == "Ambras")
            {
               _myBrockNum += 1;
               //Debug.Log(_stackBlocks[0] + " " + _stackBlocks[1] + " " + _stackBlocks[2]);
            }
            
        }  
        //Debug.Log(_myBrockNum);
        if(_myBrockNum == 0) return "ItemA";
       else if(_myBrockNum == 3) return "ItemB";
        else return "ItemC";
    }
}
