using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<PoolStruct> ObjectsToPool = new List<PoolStruct>();

    List<Pooler<PoolObjectBase>> poolers = new List<Pooler<PoolObjectBase>>();

    #region API

    /// <summary>
    /// Setup del manager creando i pooler
    /// </summary>
    public void Setup()
    {
        CreatePools();
    }

    ///////////////////////////////////////////////

    public void Unsetup()
    {
        foreach (Pooler<PoolObjectBase> pool in poolers)
            pool.Unsetup();
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Fra tutti i pooler Restituisce l'oggetto con l'ID specificato
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_id"></param>
    /// <returns></returns>
    public T GetFirstAvaiableObject<T>(string _id) where T : PoolObjectBase
    {
        Pooler<PoolObjectBase> pool = GetPooler<T>(_id);
        return (T)pool.GetFirstCollectable();
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Fra tutti i pooler cerca quello che corrisponde al tipo e gli chiede il primo oggetto disponibile
    /// </summary>
    /// <typeparam name="T">Il tipo di PoolObject richiesto</typeparam>
    /// <returns>Restituisce un IPoollable che può essere castato con il valore passato al posto del tipo T</returns>
    public T GetFirstAvaiableObject<T>() where T : PoolObjectBase
    {
        Pooler<PoolObjectBase> pool = GetPooler<T>();
        return (T)pool.GetFirstCollectable();
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Restotiosce il primo oggetto disponibile settandogli la posizione
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_position"></param>
    /// <returns></returns>
    public T GetFirstAvaiableObject<T>(Vector3 _position) where T : PoolObjectBase
    {
        T obj = GetFirstAvaiableObject<T>();
        SetPoolObjectPosition(obj, _position);
        return obj;
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Restituisce il primo oggetto disponibile del tipo indicato, settandogli il nuovo parent
    /// </summary>
    /// <typeparam name="T">Il tipo di elemento richiesto</typeparam>
    /// <param name="_parent">Il parent che deve avere l'oggetto</param>
    /// <returns></returns>
    public T GetFirstAvaiableObject<T>(Transform _parent) where T : PoolObjectBase
    {
        T obj = GetFirstAvaiableObject<T>();
        SetPoolObjectParent(obj, _parent);
        return obj;
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Restituisce il primo oggetto disponibile settandogli il parent e la posizione
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <param name="_position"></param>
    /// <returns></returns>
    public T GetFirstAvaiableObject<T>(Transform _parent, Vector3 _position) where T : PoolObjectBase
    {
        T obj = GetFirstAvaiableObject<T>();
        SetPoolObjectParent(obj, _parent);
        SetPoolObjectPosition(obj, _position);
        return obj;
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Ritorna l'oggetto poollato al pooler corrispondente
    /// </summary>
    /// <typeparam name="T">Il tipo dell'oggetto che viene restituito</typeparam>
    /// <param name="_poollable">L'oggetto da restituire al pooler</param>
    public void RetrievePoollable<T>(T _poollable) where T : IPoollable
    {
        GetPooler<T>().RetrieveCollectable(_poollable);
    }

    ///////////////////////////////////////////////

    public void RetrievePoollable<T>(string _id, T _poollable) where T : IPoollable
    {
        GetPooler<T>(_id).RetrieveCollectable(_poollable);
    }

    ///////////////////////////////////////////////

    public List<T> GetAllElements<T>()
    {
        Pooler<PoolObjectBase> pool = GetPooler<T>();
        return pool.GetAllElements<T>();
    }

    #endregion

    ///////////////////////////////////////////////

    /// <summary>
    /// Setta il parent dell'oggetto passato come parametro
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_object"></param>
    /// <param name="_parent"></param>
    void SetPoolObjectParent<T>(T _object, Transform _parent) where T : PoolObjectBase
    {
        _object.transform.parent = _parent;
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Setta la posizione dell'oggeto passato come parametro
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_object"></param>
    /// <param name="_position"></param>
    void SetPoolObjectPosition<T>(T _object, Vector3 _position) where T : PoolObjectBase
    {
        _object.transform.position = _position;
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Per ogni oggetto che deve essere poollato crea un gameobject figlio ed un pooler
    /// </summary>
    void CreatePools()
    {
        foreach (PoolStruct pooled in ObjectsToPool)
        {
            GameObject g = new GameObject("Pool_" + pooled.ID);
            g.transform.parent = transform;
            g.transform.position = transform.position;
            Pooler<PoolObjectBase> pool = new Pooler<PoolObjectBase>(pooled, g.transform);
            poolers.Add(pool);
        }
    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Cerca il pooler che si occupa di un determinato tipo di oggetto poollable
    /// </summary>
    /// <typeparam name="T">Il tipo di oggetto di cui si occupa il pooler</typeparam>
    /// <returns>Il pooler che si occupa dell'oggetto passato</returns>
    Pooler<PoolObjectBase> GetPooler<T>()
    {
        Pooler<PoolObjectBase> pool = null;

        foreach (var item in poolers)
        {
            if (item.GetPoolObjType() == typeof(T))
            {
                pool = item;
                break;
            }
        }
        return pool;
    }

    /// <summary>
    /// Cerca il pooler con il dato ID
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_ID"></param>
    /// <returns></returns>
    Pooler<PoolObjectBase> GetPooler<T>(string _ID)
    {
        Pooler<PoolObjectBase> pool = null;

        foreach (var item in poolers)
        {
            if (item.ID == _ID)
            {
                pool = item;
                break;
            }
        }
        return pool;
    }

}

[System.Serializable]
public class PoolStruct
{
    ///   !!! To use only for custom editor !!! \\\
    public bool inspectorExplandeToggle;
    ////////////////////////////////////////

    public string ID;
    public PoolObjectBase Prefab;
    public int Quantity;
    public bool ObjectStateOnRetrieve;
}