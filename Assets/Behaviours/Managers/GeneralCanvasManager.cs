﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralCanvasManager : MonoBehaviour
{
    public GameObject title_panel;
    public GameObject players_needed_prompt;
    public GameObject results_panel;
    public GameObject pc_explosion;
    public DebugPanelManager debug_panel_manager;
    public Text cheats_prompt;

    [SerializeField] float time_before_explosion_focus = 0.1f;
    [SerializeField] float time_before_title = 3;
    [SerializeField] float time_before_explosion_trigger = 2.5f;
    [SerializeField] float time_before_results = 3;
    [SerializeField] float explosion_zoom = 20;


    public void FlashCheatsPrompt(float _duration)
    {
        cheats_prompt.text = "Cheats " + (GameManager.cheats_enabled ? "Enabled" : "Disabled");
        cheats_prompt.GetComponent<FadableGraphic>().FadeOut(_duration);
    }


    public void TriggerEndOfRound()
    {
        StartCoroutine(EndOfRound());
    }


    IEnumerator EndOfRound()
    {
        Time.timeScale = 0.3f;
        GameManager.restarting_scene = true;

        yield return new WaitForSecondsRealtime(time_before_explosion_focus);
        GameManager.scene.focus_camera.Focus(pc_explosion.transform.position, explosion_zoom, time_before_title + time_before_results, false);

        yield return new WaitForSecondsRealtime(time_before_explosion_trigger);
        GameManager.scene.focus_camera.Focus(pc_explosion.transform.position, explosion_zoom * 1.25f, Mathf.Infinity, false);
        pc_explosion.SetActive(true);
        AudioManager.PlayOneShot("large_explosion");
        CameraShake.Shake(1.0f, 1.0f);

        yield return new WaitForSecondsRealtime(time_before_title);

        Time.timeScale = 1;
        title_panel.SetActive(true);

        yield return new WaitForSecondsRealtime(time_before_results);

        PlayerManager.IdleAllPlayers();
        GameManager.scene.respawn_manager.KillAllCharacters();
        AudioManager.StopAllSFX();

        CleanUpAbilities();

        var results_panel_manager = results_panel.GetComponent<ResultsPanelManager>();
        results_panel.SetActive(true);

        yield return new WaitUntil(() => !results_panel_manager.enabled);

        GameManager.restarting_scene = false;
        title_panel.SetActive(false);

        SceneManager.LoadScene(0);
    }


    void CleanUpAbilities()
    {
        foreach (Projectile projectile in FindObjectsOfType<Projectile>())
            Destroy(projectile.gameObject);

        foreach (Barrel barrel in FindObjectsOfType<Barrel>())
            Destroy(barrel.gameObject);

        foreach (Turret turret in FindObjectsOfType<Turret>())
            Destroy(turret.gameObject);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!debug_panel_manager.gameObject.activeSelf)
            {
                debug_panel_manager.Activate();
            }
            else
            {
                debug_panel_manager.Deactivate();
            }
        }

        if (debug_panel_manager.gameObject.activeSelf && GameManager.restarting_scene)
            debug_panel_manager.Deactivate();

        bool players_needed = GameManager.scene.respawn_manager.MorePlayersNeeded() && !GameManager.restarting_scene;
        players_needed_prompt.SetActive(players_needed);
    }

}