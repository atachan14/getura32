using System.Collections.Generic;
using UnityEngine;

public class EyeSet : MonoBehaviour
{
    public static EyeSet C;
    [SerializeField] List<List<Sprite>> spritesList = new();

    private void Awake()
    {
        C = this;
    }
    public List<Sprite> getSprites(int index)
    {
        return spritesList[index];
    }
}