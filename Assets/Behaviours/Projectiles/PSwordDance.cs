﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSwordDance : Projectile
{
    public float orbit_distance = 5.0f;
    public float rotate_speed = 10.0f;
    public float speed_modifier = 0.5f;

    private GameObject orbit_axis;


    void Start()
    {
        if (owner)
        {
            orbit_axis = owner.body_group;
            transform.position = orbit_axis.transform.position;

            owner.SetMoveSpeedModifier(speed_modifier);
        }

        GetComponent<SphereCollider>().center = new Vector3(orbit_distance, 0, 0);
        transform.FindChild("SwordDanceParticle").transform.localPosition = new Vector3(orbit_distance, 0, 0);

        Destroy(this.gameObject, lifetime);
    }


    void Update()
    {
        if (!owner)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = orbit_axis.transform.position;
        transform.Rotate(Vector3.up * Time.deltaTime * rotate_speed);
    }


    void OnTriggerEnter(Collider other)
    {
        // Only collide with players.
        if (other.tag != "USBCharacter")
            return;

        USBCharacter character = other.GetComponent<USBCharacter>();

        // Don't collide with self.
        if (owner)
        {
            if (character == owner)
                return;
        }

        GameObject.FindObjectOfType<AudioManager>().PlayOneShot("sword_dance_hit");
        character.Damage(damage);
    }


    private void OnDestroy()
    {
        // Reset player speed.
        if (owner)
            owner.SetMoveSpeedModifier(1);
    }
}