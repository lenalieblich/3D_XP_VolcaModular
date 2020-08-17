using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFocusManager : MonoBehaviour
{

    List<ObjectFocus> objectsInRange = new List<ObjectFocus>(); 


    # region Singleton
    private static ObjectFocusManager _instance;

    public static ObjectFocusManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ObjectFocusManager>();

                if(_instance == null)
                {
       
                    _instance = (new GameObject("ObjectFocusManager : Singleton")).AddComponent<ObjectFocusManager>(); 
                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }

    }
    #endregion

    #region Static Properties & Methods
    static public int Count { get { return Instance.objectsInRange.Count; } }

    static public void Remove (ObjectFocus objectFocus)
    {
        Instance.objectsInRange.Remove(objectFocus);
    }

    static public void Add (ObjectFocus objectFocus)
    {
        if (Instance.objectsInRange.Contains(objectFocus))
        {
            return;
        }

        Instance.objectsInRange.Add(objectFocus);
    }

    static void Sort()
    {

        if (Instance.objectsInRange.Count > 1)
            Instance.objectsInRange.Sort((a, b) => a.delta.CompareTo(b.delta));

        Instance.firstInList = Instance.objectsInRange.Count > 0 ? Instance.objectsInRange[0] : null;
        
    }
    #endregion

    #region Instance Properties & Methods
    private ObjectFocus _firstInList;
    public ObjectFocus firstInList
    {
        get { return _firstInList;  }
        private set
        {
            if (value != _firstInList)
            {
                if (_firstInList)
                    _firstInList.LostFocus();

                _firstInList = value;

                if (_firstInList)
                    _firstInList.GotFocus();                
            }
        }
    }

    public void TriggerAction()
    {
        firstInList.ActionTrigger();
    }

    #endregion
    #region Mono
    private void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);

            Instance = this;
        }
    }

    private void OnDisable()
    {
        Instance = null; 
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sort();
    }

#if DEBUG
    private void OnGUI()
    {
        GUILayout.Label("Objects in Focus : " + Count.ToString());
        if (firstInList)
            GUILayout.Label("1st : " + firstInList.name); 
    }
#endif
#endregion
}
