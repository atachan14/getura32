using System.Collections.Generic;
using UnityEngine;

public class LegSet : MonoBehaviour
{
    public static LegSet C;
    public List<List<Sprite>> SpritesList { get; set; }
    [SerializeField] List<Sprite> sp0 = new();
    [SerializeField] List<Sprite> sp1 = new();
    [SerializeField] List<Sprite> sp2 = new();
    [SerializeField] List<Sprite> sp3 = new();

    private void Awake()
    {
        SpritesList = new List<List<Sprite>> {
            sp0 ,
            sp1 ,
            sp2 ,
            sp3
        };
        C = this;
    }
    public List<Sprite> GetSprites(int index)
    {
        return SpritesList[index];
    }

}