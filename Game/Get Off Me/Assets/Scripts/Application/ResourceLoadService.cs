using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadService {
    public static readonly string VIAL_CONTEXT_PATH = "Config/VailContext";

    private static ResourceLoadService _instance;
    public static ResourceLoadService Instance {
        get
        {
            if (_instance == null)
                _instance = new ResourceLoadService();
            return _instance;
        }
    }

    private ResourceLoadService() { }

    public T Load<T>(string path) where T:Object
    {
        return Resources.Load<T>(path);
    }
}
