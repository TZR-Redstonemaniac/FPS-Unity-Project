using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileGun : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    
    [Header("Bullet force")]
    [SerializeField] private float shootForce;
    [SerializeField] private float upwardForce;
    
    [Header("Gun stats")]
    [SerializeField] private float spread;
    [SerializeField] private float reloadTime;
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private int bulletsPerTap;
    [SerializeField] private int magSize;
    [SerializeField] private bool allowButtonHold;
    
    private int bulletsLeft, bulletsShot;

    //Booleans
    private bool shooting, readyToShoot, reloading;

    [Header("References")]
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform attackPoint;

    [Header("Graphics")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private TextMeshProUGUI ammoDisplay;
    
    [Header("Bug Fixing")]
    [SerializeField] private bool allowInvoke = true;

    private RaycastHit hit;

    [HideInInspector] public Vector3 directionWithSpread;

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
        
        //Set ammo display if it exists
        if (ammoDisplay != null)
            ammoDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magSize / bulletsPerTap);
    }

    private void MyInput()
    {
        //Check if the button can be held down
        if (allowButtonHold) shooting = Input.GetButton("Fire1");
        else shooting = Input.GetButtonDown("Fire1");
        
        //Reloading
        if(Input.GetButtonDown("Reload") && bulletsLeft < magSize && !reloading) Reload();
        
        //Reload if shooting without ammo
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
        
        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        //Calculate new direction with spread
        directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;
        
        //Invoke resetShot function
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }
        
        //If more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadingFinished), reloadTime);
    }

    private void ReloadingFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }
}
