using System.Collections.Generic;
using UnityEngine;

public class EyeSet : MonoBehaviour
{
    public static EyeSet C;
    [SerializeField] public List<List<Sprite>> SpritesList { get; set; }
    [SerializeField] List<Sprite> sp0 = new();
    [SerializeField] List<Sprite> sp1 = new();
    [SerializeField] List<Sprite> sp2 = new();

    private void Awake()
    {
        SpritesList = new List<List<Sprite>> {
            sp0 ,
            sp1 ,
            sp2
        };
        C = this;
    }
    public List<Sprite> getSprites(int index)
    {
        return SpritesList[index];
    }
}