using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        Loading,
        Level0,
        Level1,
    }

    private static Action onLoaderCallback;

    public static void Load(string scene)
    {
        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene);
        };
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
