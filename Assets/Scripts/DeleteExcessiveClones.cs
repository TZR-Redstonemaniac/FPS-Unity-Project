using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteExcessiveClones : MonoBehaviour
{

    ////////////////////////////////////////Public Variables////////////////////////////////////////

    /// <summary>
    /// Names of the cloned objects without the (Clone) suffix
    /// </summary>
    [SerializeField] private string[] clonedObjectNames;

    /// <summary>
    /// The delay before destroying the object after it has been found.
    /// </summary>
    [SerializeField] private float delay;

    ////////////////////////////////////////Private Variables////////////////////////////////////////

    private GameObject cloneObject;

    private bool objectFound = false;
    private bool deleting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //Make sure that the current object is null
        cloneObject = null;
    }
    
    void FixedUpdate()
    {
        //Search for objects with the given names and set the currentClone to that object
        Search();
        
        //Destroy the found clone after the delay
        #region Destroy

            //If the currentClone GameObject is not null, call Delete()
            if (objectFound && !deleting)
            {
                deleting = true;
                Invoke(nameof(Delete), delay);
            }

        #endregion
    }

    void Search()
    {
        if (!objectFound)
        {
            foreach (string name in clonedObjectNames)
            {
                //Set the currentClone GameObject to the cloned object if found
                if (GameObject.Find(name + "(Clone)"))
                {
                    cloneObject = GameObject.Find(name + "(Clone)");
                    objectFound = true;
                }
            }
        }
    }

    void Delete()
    {
        Destroy(cloneObject);
        objectFound = false;
        deleting = false;
    }
}
