using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    [SerializeField] private int selectedWeapon = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
        int previousSelectedWeapon = selectedWeapon;
        
        if (Input.GetAxis("Mouse Scrollwheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        
        if (Input.GetAxis("Mouse Scrollwheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetButtonDown("Weapon 1"))
            selectedWeapon = 0;
        
        if (Input.GetButtonDown("Weapon 2") && transform.childCount >= 2)
            selectedWeapon = 1;
        
        if (Input.GetButtonDown("Weapon 3") && transform.childCount >= 3)
            selectedWeapon = 2;
        
        if (Input.GetButtonDown("Weapon 4") && transform.childCount >= 4)
            selectedWeapon = 2;
        
        if (Input.GetButtonDown("Weapon 5") && transform.childCount >= 5)
            selectedWeapon = 2;
        
        if (Input.GetButtonDown("Weapon 6") && transform.childCount >= 6)
            selectedWeapon = 2;
        
        if (Input.GetButtonDown("Weapon 7") && transform.childCount >= 7)
            selectedWeapon = 2;
        
        if (Input.GetButtonDown("Weapon 8") && transform.childCount >= 8)
            selectedWeapon = 2;
        
        if (Input.GetButtonDown("Weapon 9") && transform.childCount >= 9)
            selectedWeapon = 2;
        
        if(previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
