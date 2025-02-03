using System.Collections.Generic;
using UnityEngine;

public class BallSet : MonoBehaviour
{
    public static BallSet C;
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
    public List<Sprite> GetSprites(int index)
    {
        return SpritesList[index];
    }
 
}