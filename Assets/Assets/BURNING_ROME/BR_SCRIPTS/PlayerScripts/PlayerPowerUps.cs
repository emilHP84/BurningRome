using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    PlayerBombing bombSystem => GetComponent<PlayerBombing>();
    PlayerManager manager => GetComponent<PlayerManager>();

    public AudioClip powerupclip;

    public GameObject vfx;
    public GameObject vfxinvincibility;

    public void BombUp(int amount)
    {
        bombSystem.ChangeMaxBomb(bombSystem.MaxBomb + amount);
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
    }

    public void RangeUp(int amount)
    {
        bombSystem.ChangeRange(bombSystem.ExplosionRange + amount);
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
    }

    public void BombDown(int amount)
    {
        bombSystem.ChangeMaxBomb(bombSystem.MaxBomb - amount);
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
    }

    public void RedButton()
    {
        bombSystem.AddManualDetonation();
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
    }

    public void Invincibility()
    {
        manager.InvincibilityFor(10f);
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
        Destroy(Instantiate(vfx,transform.localPosition, Quaternion.identity),10f);
    }

    public void AdesFire()
    {
        bombSystem.Switchdelay();
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
    }

    public void Perçing()
    {
        bombSystem.BombPercing();
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        aus.clip = powerupclip;
        aus.Play();
        Destroy(Go, aus.clip.length);
        Instantiate(vfx, transform.localPosition, Quaternion.identity);
    }



} // FIN DU SCRIPT
