using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SNM
{
    Title,
    Menu,
    Robby,
    GameStart,
    Day,
    Night
}

public static class SLD
{
    public static void ClientLoad(SNM name)
    {
        SceneManager.LoadScene(name.ToString());
    }

    public static void NetworkSingleLoad(SNM name)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Single);
    }

    public static void NetworkAdditiveLoad(SNM name)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
    }
}
