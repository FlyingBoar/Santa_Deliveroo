using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoollable
{
    string ID { get; }
    bool IsAvaiable { get; set; }
    GameObject Prefab { get; set; }

    void InternalSetup(string _id);
    void ToggleObject(bool _setActive);
    void RetrieveObject(bool _objectState ,Transform _parent = null);
}
