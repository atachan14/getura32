using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SNM
{
    Title,
    Menu,
    Robby,
    Opening,
    Day,
    Night
}

public static class SLD
{
    public static void ClientLoad(SNM name)
    {
        SceneManager.LoadScene(name.ToString());
    }

    public static void SingleLoad(SNM name)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Single);
    }

    public static void AdditiveLoad(SNM name)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
    }
}
