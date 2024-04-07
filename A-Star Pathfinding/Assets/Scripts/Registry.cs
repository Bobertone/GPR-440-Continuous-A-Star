using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registry
{

    private static Registry _registry=null;
    public static Registry GetInstance() {
        if (_registry == null)
            _registry = new Registry();
        return _registry;
    }

    private Registry() { }
}
