using System.Collections.Generic;
using UnityEngine;

public class BallSet : MonoBehaviour
{
    public static BallSet C;
    [SerializeField] List<List<Sprite>> spritesList=new();

    private void Awake()
    {
        C = this;
    }
    public List<Sprite> getSprites(int index)
    {
        return spritesList[index];
    }
}