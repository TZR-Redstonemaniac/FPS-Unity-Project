using UnityEngine;
using UnityEngine.InputSystem;

public class Rifle : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    public MyPlayerMovement PlayerMovement;
    
    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.fireButtonPressed)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            
        }
    }
}
