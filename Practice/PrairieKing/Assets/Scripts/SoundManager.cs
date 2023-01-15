using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip playerHurtSound;
    [SerializeField] private AudioClip monsterHurtSound;
    [SerializeField] private AudioClip monsterBoomSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private AudioClip gameCompleteSound;
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioClip buttonClickedSound;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    public void PlayPlayerHurtSound()
    {
        audioSource.PlayOneShot(playerHurtSound);
    }

    public void PlayMonsterHurtSound()
    {
        audioSource.PlayOneShot(monsterHurtSound);
    }

    public void PlayMonsterBoomSound()
    {
        audioSource.PlayOneShot(monsterBoomSound);
    }

    public void PlayGameStartSound()
    {
        audioSource.PlayOneShot(gameStartSound);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound);
    }

    public void PlayTeleportSound()
    {
        audioSource.PlayOneShot(teleportSound);
    }
    public void PlayButtonClickedSound()
    {
        audioSource.PlayOneShot(buttonClickedSound);
    }

    public void PlayGameCompleteSound()
    {
        audioSource.PlayOneShot(gameCompleteSound);
    }
    
}
