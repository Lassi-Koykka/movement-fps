﻿using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update

    // --SAA PYSYÄ WEAPONIN SISÄLLÄ
    public bool automatic = false;
    public float damage = 25f;
    public float fireRate = 5f;
    [Range(0f, 100f)]
    public float sideRecoil = 1f;
    [Range(0f, 100f)]
    public float upRecoil = 1f;

    public WeaponAudioManager weaponAudioManager;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject characterImpactEffect;
    public Transform leftHandGrip;
    public Transform rightHandGrip;

    public GameObject RecoilHandlerObject;

    public string soundName;
    Animator anim;
    // --SAA PYSYÄ WEAPONIN SISÄLLÄ

    // --ASETETAAN WEAPONCONTROLLERISSA
    [HideInInspector]
    public bool holdingFire;
    [HideInInspector]
    public Transform leftArmTarget;
    [HideInInspector]
    public Transform rightArmTarget;

    [HideInInspector]
    public Transform raycastOrigin;
    [HideInInspector]
    public IRecoilHandler recoilHandler;
    // --ASETETAAN WEAPONCONTROLLERISSA

    private float nextTimeToFire = 0f;
    private bool triggerPulled = false;

    void Start()
    {
        recoilHandler = RecoilHandlerObject.GetComponent<IRecoilHandler>();
        anim = GetComponent<Animator>();
        //PlaceHands();
    }
    //Once Object is Enabled
    private void OnEnable()
    {
        PlaceHands();
    }
    // Update is called once per frame
    void Update()
    {
        PlaceHands();
        if (holdingFire && !triggerPulled)
        {
            FireWeapon();
        }
        else if(!holdingFire)
        {
            triggerPulled = false;
            recoilHandler.ResetRecoil();
        }
        else if (triggerPulled)
        {
            recoilHandler.ResetRecoil();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        anim.SetTrigger("FiringTrigger");
        weaponAudioManager.Play(soundName);

        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit))
        {
            GameObject hitImpactEffect;
            IDamageable target;

            if(hit.transform.TryGetComponent<IDamageable>(out target))
            {
                target.Damage(damage);
                Debug.Log(hit.transform.name + " damaged for: " + damage);
            }

            // Different impact effect for enemy and environment;
            if(hit.transform.tag == "Enemy" || hit.transform.tag == "Player")
            {
                hitImpactEffect = characterImpactEffect;
            }
            else
            {
                hitImpactEffect = impactEffect;
            }
            GameObject impactGO = Instantiate(hitImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }

    public void FireWeapon()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            float side = UnityEngine.Random.Range(-sideRecoil, sideRecoil);
            recoilHandler.AddRecoil(upRecoil / 1000, side / 1000, automatic);
            if (!automatic)
            {
                triggerPulled = true;
            }
        }
    }

    void PlaceHands()
    {
        //Place hands correctly
        leftArmTarget.position = leftHandGrip.position;
        leftArmTarget.rotation = leftHandGrip.rotation;
        rightArmTarget.position = rightHandGrip.position;
        rightArmTarget.rotation = rightHandGrip.rotation;
    }

}