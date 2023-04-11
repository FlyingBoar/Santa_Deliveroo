using System;
using System.Collections.Generic;
using UnityEngine;

public class Pooler<T> where T : IPoollable
{
    public string ID { get; private set; }
    List<IPoollable> avaiableObjects = new List<IPoollable>();
    List<IPoollable> unavaiableObjects = new List<IPoollable>();
    Transform parentTransform;

    PoolStruct pooledData;

    public Pooler(PoolStruct _object, Transform _parentTransform)
    {
        ID = _object.ID;
        pooledData = _object;
        parentTransform = _parentTransform;
        PopulatePool();
    }

    public void Unsetup()
    {
        for (int i = unavaiableObjects.Count - 1; i >= 0; i--)
            RetrieveCollectable(unavaiableObjects[i]);
    }

    public Type GetPoolObjType()
    {
        return pooledData.Prefab.GetType();
    }

    /// <summary>
    /// Controlla se è presente un collectable disponibile, lo sposta dalla lista dei disponibili a quella dei non disponibili,
    /// e lo restituisce. (Se la lista è a zero, crea un nuovo collectable)
    /// </summary>
    /// <returns></returns>
    public T GetFirstCollectable()
    {
        IPoollable obj = null;
        if (avaiableObjects.Count > 0)
        {
            obj = avaiableObjects[0];
            unavaiableObjects.Add(obj);
            avaiableObjects.Remove(obj);
        }
        else
            obj = InstantiateCollectables();

        obj.ToggleObject(true);
        return (T)obj;
    }

    /// <summary>
    /// Restituisce tutti gli elementi del pooler
    /// </summary>
    /// <returns></returns>
    public List<T> GetAllElements<T>()
    {
        List<T> collectables = new List<T>();
        foreach (IPoollable poollable in avaiableObjects)
            collectables.Add((T)poollable);
        foreach (IPoollable poollable in unavaiableObjects)
            collectables.Add((T)poollable);

        return collectables;
    }

    internal void RetrieveCollectable(IPoollable _collectable)
    {
        if (!avaiableObjects.Contains(_collectable))
        {
            _collectable.RetrieveObject(pooledData.ObjectStateOnRetrieve, parentTransform);
            avaiableObjects.Add(_collectable);
            unavaiableObjects.Remove(_collectable);
        }
    }

    /// <summary>
    /// Istanzia i collectable
    /// </summary>
    /// <returns></returns>
    IPoollable InstantiateCollectables()
    {
        IPoollable p = null;
        p = GameObject.Instantiate(pooledData.Prefab, parentTransform).GetComponent<IPoollable>();
        if (p != null)
            p.InternalSetup(pooledData.ID);
        else
            throw new System.Exception("**Poollable not found on the prefab**");
        return p;
    }

    /// <summary>
    /// Per ogni struttura crea i prefab degli oggetti e se li salva in una lista
    /// </summary>
    void PopulatePool()
    {
        for (int i = 0; i < pooledData.Quantity; i++)
        {
            IPoollable p = InstantiateCollectables();
            avaiableObjects.Add(p);
            p.ToggleObject(false);
        }
    }
}