using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip playerProjParede;
    public AudioClip enemyDamage;
    public AudioClip itemPickupSound; 
    public AudioClip itemHeal;
    public AudioClip iceProjectile;
    public AudioClip fireProjectile;
    public AudioClip teleportSound;
    public AudioClip freezeProjectile;
    public AudioClip portalProjectile;
    public AudioClip boss4Projectile;
    public AudioClip boss1ProjectileLaser;
    public AudioClip boss1ProjectileMetal;
    public AudioClip playerSonicSpeed;
    public AudioClip playerTeleport;
    public AudioClip playerDano;
    public AudioClip ficaFrioAi;
    public AudioClip zaWarudo;
    public AudioClip playerQueimando;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySoundBasedOnCollision(string collisionPair)
    {
        switch (collisionPair)
        {
            case "PlayerProjParede":
                if (playerProjParede != null)
                {
                    audioSource.PlayOneShot(playerProjParede);
                }
                break;

            case "DanoInimigo":
                if (enemyDamage != null)
                {
                    audioSource.PlayOneShot(enemyDamage);
                }
                break;

            case "iceProjectile":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(fireProjectile);
                }
                break;
            case "fireProjectile":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(iceProjectile);
                }
                break;
            case "teleportSound":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(teleportSound);
                }
                break;
            case "freezeProjectile":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(freezeProjectile);
                }
                break;
            case "portalProjectile":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(portalProjectile);
                }
                break;
            case "Boss4Projectile":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(boss4Projectile);
                }
                break;
            case "Boss1ProjectileLaser":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(boss1ProjectileLaser);
                }
                break;
            case "Boss1ProjectileMetal":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(boss1ProjectileMetal);
                }
                break;
            case "PlayerSonicSpeed":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(playerSonicSpeed);
                }
                break;
            case "PlayerTeleport":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(playerTeleport);
                }
                break;
            case "PlayerDano":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(playerDano);
                }
                break;
            case "FicaFrioAi":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(ficaFrioAi);
                }
                break;
            case "zaWarudo":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(zaWarudo);
                }
                break;
            case "PlayerQueimando":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(playerQueimando);
                }
                break;

            default:
                Debug.LogWarning("Par não reconhecido: " + collisionPair);
                break;
        }
    }

    public void PlayItemHealPickupSound()
    {
        if (itemPickupSound != null)
        {
            audioSource.PlayOneShot(itemHeal);
        }
        else
        {
            Debug.LogWarning("Som de itemPickupSound não configurado no SoundManager!");
        }
    }

    public void PlayItemPickupSound()
    {
        if (itemPickupSound != null)
        {
            audioSource.PlayOneShot(itemPickupSound);
        }
        else
        {
            Debug.LogWarning("Som de itemPickupSound não configurado no SoundManager!");
        }
    }
}
