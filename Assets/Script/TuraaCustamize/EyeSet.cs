using System.Collections.Generic;
using UnityEngine;

public class EyeSet : MonoBehaviour
{
    public static EyeSet C;
    [SerializeField] List<List<Sprite>> spritesList;
    [SerializeField] List<Sprite> sp0 = new();
    [SerializeField] List<Sprite> sp1 = new();
    [SerializeField] List<Sprite> sp2 = new();

    private void Awake()
    {
        spritesList = new List<List<Sprite>> {
            sp0 ,
            sp1 ,
            sp2
        };
        C = this;
    }
    public List<Sprite> getSprites(int index)
    {
        return spritesList[index];
    }
}