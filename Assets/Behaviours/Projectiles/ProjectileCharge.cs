﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCharge : Projectile
{
    public float charge_speed;
    public float dir_threshold = 0.1f;

    [SerializeField] List<AudioClip> hit_sounds = new List<AudioClip>();

    private Transform torso_mount;
    private Rigidbody rigid_body;
    private Vector3 absolute_direction;


	void Start()
    {
        if (owner)
        {
            torso_mount = owner.body_group.transform;
            rigid_body = owner.GetComponent<Rigidbody>();
        }

        TrackPlayer();
    }


    void Update()
    {
        if (owner != null && owner.controls_disabled)
        {
            owner = null;
            Destroy(this.gameObject);

            return;
        }

        CalculateRawDirection();
        TrackPlayer();

        if (owner == null)
            Destroy(this.gameObject);
    }


    void FixedUpdate()
    {
        if (owner == null)
            return;

        rigid_body.MovePosition(owner.transform.position + 
            (absolute_direction * Time.fixedDeltaTime * charge_speed));
    }


    void OnTriggerEnter(Collider _other)
    {
        // Only collide with players.
        if (_other.tag != "USBCharacter")
            return;

        USBCharacter character = _other.GetComponent<USBCharacter>();

        // Don't collide with self.
        if (owner)
        {
            if (character == owner)
                return;
        }

        AudioManager.PlayOneShot(hit_sounds[Random.Range(0, hit_sounds.Count - 1)]);
        character.Damage(damage, _other.transform.position + (absolute_direction * 5), owner);
    }


    void CalculateRawDirection()
    {
        if (owner.last_facing.x > dir_threshold)
        {
            absolute_direction = Vector3.right;
        }
        else if (owner.last_facing.x < -dir_threshold)
        {
            absolute_direction = Vector3.left;
        }
        else if (owner.last_facing.z > dir_threshold)
        {
            absolute_direction = Vector3.forward;
        }
        else if (owner.last_facing.z < -dir_threshold)
        {
            absolute_direction = Vector3.back;
        }
    }

    
    void TrackPlayer()
    {
        if (torso_mount)
    	    transform.position = torso_mount.position;
    }

}
