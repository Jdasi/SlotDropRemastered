﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePyramidTurret : Projectile
{
    [SerializeField] AudioClip bounce_sound;
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] GameObject turret_prefab;
    [SerializeField] float dist_from_player;
    [SerializeField] float forward_throw_force;
    [SerializeField] float upward_throw_force;
    [SerializeField] float scan_radius = 30;
    [SerializeField] float laser_radius = 1.5f;
    [SerializeField] float stun_duration = 1;

    private List<USBCharacter> affected_characters = new List<USBCharacter>();

    
    void Start()
    {
        if (owner != null)
        {
            forward_throw_force += (owner.move_dir.magnitude * 2);
            transform.position = origin = owner.body_group.transform.position;

            transform.position += facing * dist_from_player;
        }

        rigid_body.AddForce(facing * forward_throw_force * 1000);
        rigid_body.AddForce(new Vector3(0, 1, 0) * (upward_throw_force * 1000));
    }


    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("KillZone"))
        {
            Destroy(this.gameObject);
            return;
        }
        
        if ((owner != null && _other.transform.GetInstanceID() == owner.transform.GetInstanceID()))
            return;

        if (_other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            SpawnTurret();
            Destroy(this.gameObject);
        }
        else if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            USBCharacter character = _other.GetComponent<USBCharacter>();

            if (!affected_characters.Contains(character))
            {
                character.Damage(damage / 5, owner);
                Bounce(400);

                affected_characters.Add(character);
            }
        }
        else if (_other.gameObject.layer != LayerMask.NameToLayer("Projectile"))
        {
            Bounce(350);
        }
    }


    void SpawnTurret()
    {
        AudioManager.PlayOneShot(hit_sound);

        GameObject clone = Instantiate(turret_prefab, transform.position, Quaternion.identity);
        clone.transform.Rotate(-90, 0, 0);

        ConfigureTurret(clone.GetComponent<Turret>());
    }


    void Bounce(float _force)
    {
        AudioManager.PlayOneShot(bounce_sound);

        rigid_body.AddForce(-rigid_body.velocity * _force);
    }


    void ConfigureTurret(Turret _turret)
    {
        _turret.owner = this.owner;
        _turret.lifetime = this.lifetime;
        _turret.damage = this.damage;
        _turret.scan_radius = this.scan_radius;
        _turret.laser_radius = this.laser_radius;
        _turret.stun_duration = this.stun_duration;
    }

}
