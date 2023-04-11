using UnityEngine;

public class PoolObjectBase : MonoBehaviour, IPoollable
{
    public string ID { get; private set; }
    public bool IsAvaiable { get; set; }
    public GameObject Prefab { get; set; }

    public void InternalSetup(string _id)
    {
        ID = _id;
        OnSetup();
    }


    /// <summary>
    /// Setta lo stato del gameobject
    /// </summary>
    /// <param name="_setActive">True per attivare il gameobject, False per disattivarlo</param>
    public void ToggleObject(bool _setActive)
    {
        gameObject.SetActive(_setActive);
        OnToggle(_setActive);
    }

    /// <summary>
    /// Funzione chiamata dal pooler quando l'oggetto viene reinserito nello stesso
    /// </summary>
    /// <param name="_objectState">Lo stato che deve avere il gameobject</param>
    /// <param name="_parent">Viene settato il parent e la posizione c</param>
    public virtual void RetrieveObject(bool _objectState, Transform _parent = null)
    {
        if (_parent != null)
        {
            gameObject.transform.parent = _parent;
            gameObject.transform.position = _parent.position;
        }
        OnRetrieve();
        ToggleObject(_objectState);
    }

    #region Overrides

    /// <summary>
    /// Called during the internal setup
    /// </summary>
    public virtual void OnSetup() { }

    /// <summary>
    /// Called when the status is the object is setted
    /// </summary>
    /// <param name="_setActive"></param>
    public virtual void OnToggle(bool _setActive) { }

    /// <summary>
    /// Called when the object is retrieved to the pool manager
    /// </summary>
    public virtual void OnRetrieve() { }

    #endregion
}
